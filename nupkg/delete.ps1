. ".\delete_common.ps1"

$apiKey = $args[0]

foreach($package in $packages) {
	Write-Host("Delete package:" + $package)
    dotnet nuget delete $package 0.0.6 --non-interactive -s http://192.168.0.22:2500/v3/index.json --api-key "$apiKey"
}
