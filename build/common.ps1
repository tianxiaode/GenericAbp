# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
    "../src/modules/helper",
    "../src/modules/extmenu",
    "../src/modules/extresource",
    "../src/modules/texttemplate",
    "../src/modules/enumeration",
    "../src/modules/themes",
    "../src/modules/account"
)