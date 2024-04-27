$awsProfile = "bookapplatest"

function Get-HighestVersion {
    $filenames = Invoke-Expression "aws s3 ls s3://book-prices-app --profile $awsProfile"

    $highestMajor = 0
    $highestMinor = 0
    $highestPatch = 0

    foreach ($filename in $filenames) {
        if ($filename -match '(\d+)\.(\d+)\.(\d+)\.zip') {
            $nextMajor = [int]$matches[1]
            $nextMinor = [int]$matches[2]
            $nextPatch = [int]$matches[3]


            $nextValue = ($nextMajor * 1000) + ($nextMinor * 100) + $nextPatch
            $currentValue = ($highestMajor * 1000) + ($highestMinor * 100) + $highestPatch

            if ($nextValue -gt $currentValue) {
                $highestMajor = $nextMajor
                $highestMinor = $nextMinor
                $highestPatch = $nextPatch
            }
        }
    }

    return "$highestMajor.$highestMinor.$highestPatch" + ".zip"
}

function Download-Version {
    param(
        [string]$hv
    )
    $desktopPath = [Environment]::GetFolderPath("Desktop")
    $bucketPath = "aws s3 cp s3://book-prices-app/$hv"
    Invoke-Expression "$bucketPath $desktopPath\$hv --profile $awsProfile"
}

function Main {
    $highestVersion = Get-HighestVersion
    echo "`nHighest version: $highestVersion"
    Download-Version $highestVersion
}

Main