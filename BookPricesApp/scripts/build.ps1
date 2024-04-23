param(
    [string]$v
)

if ($v -eq $null) {
    Write-Host "Version is not provided."
    exit 1
} else {
    Write-Host "building $v"
}

function DeleteBuildDirectory {
    $directoryPath = ".\bin\Release\net6.0-windows\win-x64"

	if (Test-Path -Path $directoryPath -PathType Container) {
        Remove-Item -Path $directoryPath -Recurse -Force
        Write-Host "win-64 dir deleted"
    } else {
        Write-Host "win-64 dir does not exist."
    }
}

function CreateAppDirectory {
    $containerPath = ".\bin\App"
    $appPath = ".\bin\App\$v"

	if (Test-Path -Path $containerPath -PathType Container) {
        Remove-Item -Path $containerPath -Recurse -Force
        Write-Host "app directory deleted"
    } else {
        Write-Host "app directory does not exist."
    }
    Write-Host "creating $appPath"
    mkdir $appPath
}

function MoveContentToAppFolder {
    $sourceDir = ".\bin\Release\net6.0-windows\win-x64\publish"
    $destinationDir = ".\bin\App\$v"

    $items = Get-ChildItem -Path $sourceDir

    foreach ($item in $items) {
        Move-Item -Path $item.FullName -Destination $destinationDir
    }
}

function ZipVersion {
    Write-Host "zipping folder $v"
    Compress-Archive -Path ".\bin\App\$v" -DestinationPath ".\bin\App\$v.zip"
}

function UploadToS3 {
    Write-Host "uploading to s3"
    & aws s3 cp ".\bin\App\$v.zip" "s3://book-prices-app/$v.zip" --profile bookapp
}

DeleteBuildDirectory
CreateAppDirectory

Write-Host "building app"
dotnet publish -r win-x64 -c Release

MoveContentToAppFolder
ZipVersion
UploadToS3