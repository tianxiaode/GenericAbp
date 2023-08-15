# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
	"../src/modules/w2ui",
    "../src/modules/jspreadsheet",
	"../src/modules/businessexception",
   	"../src/modules/helper",
    # "../src/modules/extmenu",
    "../src/modules/extresource",
    # "../src/modules/texttemplate",
    "../src/modules/enumeration",
    "../src/modules/ddd",
    "../src/modules/application",
    "../src/modules/identity",
    "../src/modules/IdentityServer",
    "../src/modules/FileManagement",
    "../src/modules/openiddict",
    "../src/modules/phonelogin",
    "../src/modules/metroui"
    # "../src/modules/themes",
    # "../src/modules/account"
)