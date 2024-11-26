. ".\copy-common.ps1"

$destiontionRoot = $args[0]
# Create all packages
foreach($project in $projects) {
    
    $projectFolder = Join-Path $rootFolder $project 
    $projectFolder = Join-Path $projectFolder "bin/Release" 

	$framework = "netstandard2.0"
	$dllFolder = Join-Path $projectFolder $framework
	if(!(Test-Path $dllFolder))
		{
			$framework = "netstandard2.1"
			$dllFolder = Join-Path $projectFolder $framework
		} 
	if(!(Test-Path $dllFolder))
		{
			$framework = "netcoreapp3.1"
			$dllFolder = Join-Path $projectFolder $framework
		} 
	if(!(Test-Path $dllFolder))
		{
			$framework = "net8.0"
			$dllFolder = Join-Path $projectFolder $framework
		} 
	if(!(Test-Path $dllFolder))
		{
			$framework = "net9.0"
			$dllFolder = Join-Path $projectFolder $framework
		} 
	$projectName = $project.Substring($project.LastIndexOf("/") + 1)
	$dllFile = Join-Path $dllFolder ($projectName + ".dll")
	$pdbFile = Join-Path $dllFolder ($projectName + ".pdb")
	$destination = Join-Path $destiontionRoot ($projectName + "/1.0.6/lib/" + $framework)
	Write-Host("Copy file:" + $dllFile)
	Copy-Item -Path $dllFile  -Destination $destination 
	Copy-Item -Path $pdbFile  -Destination $destination 

}

# Go back to the pack folder
Set-Location $packFolder