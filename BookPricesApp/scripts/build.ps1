function DeleteBuildDirectory {
    $directoryPath = ".\bin\Release\net6.0-windows\win-x64"

	if (Test-Path -Path $directoryPath -PathType Container) {
        Remove-Item -Path $directoryPath -Recurse -Force
        Write-Host "win-64 deleted"
    } else {
        Write-Host "win-64 does not exist."
    }
}

function SetShortcut {
    param ( [string]$SourceExe, [string]$DestinationPath )
    $WshShell = New-Object -comObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut($DestinationPath)
    $Shortcut.TargetPath = $SourceExe
    $Shortcut.Save()
}

function CreateShortcut {
    $currentDirectory = Get-Location
    $exePath = "$currentDirectory\bin\App\BookPrices\BookPricesApp.GUI.exe"
    $shortcutPath = "$currentDirectory\bin\App\BookPrices.lnk"

    $WshShell = New-Object -comObject WScript.Shell
    $Shortcut = $WshShell.CreateShortcut($shortcutPath)
    $Shortcut.TargetPath = $exePath
    $Shortcut.Save()

    Write-Host "Created Shortcut"
}

function MoveContentToAppFolder {
    $sourceDir = ".\bin\Release\net6.0-windows\win-x64\publish"
    $destinationDir = ".\bin\App\BookPrices"

    $items = Get-ChildItem -Path $sourceDir

    foreach ($item in $items) {
        Move-Item -Path $item.FullName -Destination $destinationDir
    }
}

function CreateAppDirectory {
    $containerPath = ".\bin\App"
    $appPath = ".\bin\App\BookPrices"

	if (Test-Path -Path $containerPath -PathType Container) {
        Remove-Item -Path $containerPath -Recurse -Force
        Write-Host "app directory deleted"
    } else {
        Write-Host "app directory does not exist."
    }

    mkdir $appPath
}

DeleteBuildDirectory
CreateAppDirectory


Write-Host "buildign app"
dotnet publish -r win-x64 -c Release

# CreateShortcut
MoveContentToAppFolder