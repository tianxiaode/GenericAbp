# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../src/"

# List of solutions
$solutions = (
    "modules/helper",
    "modules/extmenu",
    "modules/extresource",
    "modules/texttemplate",
    "modules/enumeration",
    "modules/themes",
    "modules/account"
)

# List of projects
$projects = (

    # modules/helper
    "modules/helper/src/Generic.Abp.Helper.File",

    # modules/extmenu
    "modules/extmenu/src/Generic.Abp.ExtMenu.Application.Contracts",
    "modules/extmenu/src/Generic.Abp.ExtMenu.Application",
    "modules/extmenu/src/Generic.Abp.ExtMenu.HttpApi",

    # modules/extresource
    "modules/extresource/src/Generic.Abp.ExtResource.Application.Contracts",

    # modules/texttemplate
    "modules/texttemplate/src/Generic.Abp.TextTemplate",

    # modules/enumeration
    "modules/enumeration/src/Generic.Abp.Enumeration.Domain.Shared",
    "modules/enumeration/src/Generic.Abp.Enumeration.Application.Contracts",
    "modules/enumeration/src/Generic.Abp.Enumeration.Application",
    "modules/enumeration/src/Generic.Abp.Enumeration.HttpApi",

    # modules/themes
    "modules/themes/src/Generic.Abp.Themes.Shared",
    "modules/themes/src/Generic.Abp.Themes",

    # modules/account
    "modules/account/src/Generic.Abp.Account.Application.Contracts",
    "modules/account/src/Generic.Abp.Account.Application",
    "modules/account/src/Generic.Abp.Account.HttpApi",
    "modules/account/src/Generic.Abp.Account.Identity.Web",
    "modules/account/src/Generic.Abp.Account.Web",
    "modules/account/src/Generic.Abp.Account.IdentityServer.Web"
        
)
