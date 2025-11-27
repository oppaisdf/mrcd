if (-not (Test-Path $PROFILE)) { New-Item -Path $PROFILE -ItemType File -Force | Out-Null }
Add-Content -Path $PROFILE -Value 'Set-PSReadLineOption -HistorySaveStyle SaveNothing'
Add-Content -Path $PROFILE -Value 'Set-PSReadLineOption -MaximumHistoryCount 5'
. $PROFILE
