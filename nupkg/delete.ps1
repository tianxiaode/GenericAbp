. ".\delete_common.ps1"

$version = $args[0]
$apiKey = $args[1]

# Get the version

# Publish all packages
foreach($project in $projects) {
    $projectName = $project.ToLower()
    & dotnet nuget delete $projectName "$version" -s http://localhost:1234/v3/index.json --api-key "$apiKey" --non-interactive
}

