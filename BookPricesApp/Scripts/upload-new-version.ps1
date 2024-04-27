param(
    [string]$v,
    [string]$dropTables = "notset"
)

$newVersion = $v
$sourceDir = ".\bin\Release\net6.0-windows\win-x64\publish"
$appSettingsPath = "$sourceDir\appsettings.json"

if ($newVersion -eq $null) {
    echo "Version is not provided."
    exit 1
} else {
    echo "building $newVersion"
}

function DeleteBuildDirectory {
    $directoryPath = ".\bin\Release\net6.0-windows\win-x64"

	if (Test-Path -Path $directoryPath -PathType Container) {
        Remove-Item -Path $directoryPath -Recurse -Force
        echo "win-64 dir deleted"
    } else {
        echo "win-64 dir does not exist."
    }
}

function CreateAppDirectory {
    $containerPath = ".\bin\App"
    $appPath = ".\bin\App\$newVersion"

	if (Test-Path -Path $containerPath -PathType Container) {
        Remove-Item -Path $containerPath -Recurse -Force
        echo "app directory deleted"
    } else {
        echo "app directory does not exist."
    }
    echo "creating $appPath"
    mkdir $appPath
}

function MoveContentToAppFolder {
    $destinationDir = ".\bin\App\$newVersion"

    $items = Get-ChildItem -Path $sourceDir

    foreach ($item in $items) {
        Move-Item -Path $item.FullName -Destination $destinationDir
    }
}

function ZipVersion {
    echo "zipping folder $newVersion"
    $path = ".\bin\App\$newVersion"
    $destPath = ".\bin\App\$newVersion.zip"
    Compress-Archive -Path $path -DestinationPath $destPath
}

function UploadToS3 {
    echo "uploading to s3"
    & aws s3 cp ".\bin\App\$newVersion.zip" "s3://book-prices-app/$newVersion.zip" --profile bookapp
}

function ValidateVersion {
    param(
        [string]$newVersion
    )

    if (-not $newVersion) {
        echo "Version is not provided."
        exit 1
    }

    $versionPattern = '^\d+\.\d+\.\d+$'

    if ($newVersion -notmatch $versionPattern) {
        echo "Invalid version format. Version should be in the format x.y.z (e.g., 1.2.3)"
        exit 1
    }
}

function ValidateVersionUpdate {
    param(
        [string]$oldVersion,
        [string]$newVersion
    )

    $oldComponents = $oldVersion -split '\.'
    $newComponents = $newVersion -split '\.'

    $oldMajor = [int]$oldComponents[0]
    $oldMinor = [int]$oldComponents[1]
    $oldPatch = [int]$oldComponents[2]
    $oldVersionValue = $oldMajor + $oldMinor + $oldPatch;

    $newMajor = [int]$newComponents[0]
    $newMinor = [int]$newComponents[1]
    $newPatch = [int]$newComponents[2]
    $newVersionValue = $newMajor + $newMinor + $newPatch
    
    if ($newVersionValue -gt $oldVersionValue) {
        echo "New version looks good: $newVersion"
    } else {
        echo "Invalid version update. New version should be an increase of one in major, minor, or patch."
        exit 1
    }
}

function GetOldVersion {
    $appsettingsString = Get-Content -Path $appSettingsPath -Raw
    $appsettings = $appsettingsString | ConvertFrom-Json
    return $appsettings.version
}

function UpdateAppSettingsVersion {
    param([string]$newVersion)

    $appsettingsString = Get-Content -Path $appSettingsPath -Raw
    $appsettings = $appsettingsString | ConvertFrom-Json
    $appsettings.version = $newVersion

    $csCurrentKey = $appsettings.ConnectionStrings.sqlExpress
    $csPlaceHolder = $appsettings.ConnectionStrings.placeHolder

    $appsettings.ConnectionStrings.sqlExpress = $csPlaceHolder
    $appsettings.ConnectionStrings.placeHolder = $csCurrentKey

    $appsettings.version = $newVersion

    if ($dropTables -ne "notset") {
    echo "TABLES WILL BE DROPPED!!!"
    $appsettings.dropTableSchemas = $true
    } else {
        echo "tables will not be dropped"
    }

    $updatedAppsettings = $appsettings | ConvertTo-Json
    $updatedAppsettings | Set-Content -Path $appSettingsPath
}

function Main {
    DeleteBuildDirectory
    CreateAppDirectory

    echo "building app"
    dotnet publish -r win-x64 -c Release

    $oldVersion = GetOldVersion
    ValidateVersion -newVersion $newVersion
    ValidateVersionUpdate -oldVersion $oldVersion -newVersion $newVersion
    UpdateAppSettingsVersion -newVersion $newVersion
    echo "appsettings updated to version $newVersion"

    MoveContentToAppFolder
    ZipVersion
    UploadToS3
}

Main