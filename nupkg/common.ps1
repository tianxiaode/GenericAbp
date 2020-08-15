# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../src/"

# List of solutions
$solutions = (
    "modules/themes",
    "modules/account"
)

# List of projects
$projects = (

    # modules/themes
    "modules/themes/src/Generic.Abp.Themes",

    # modules/account
    "modules/account/src/Generic.Abp.Account.Application.Contracts",
    "modules/account/src/Generic.Abp.Account.Application",
    "modules/account/src/Generic.Abp.Account.HttpApi",
    "modules/account/src/Generic.Abp.Account.Identity.Web",
    "modules/account/src/Generic.Abp.Account.Web",
    "modules/account/src/Generic.Abp.Account.IdentityServer.Web"
        
)
