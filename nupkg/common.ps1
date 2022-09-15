# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../src/"

# List of solutions
$solutions = (
    "modules/businessexception",
    "modules/helper",
    # "modules/extmenu",
    "modules/extresource",
    # "modules/texttemplate",
    "modules/enumeration",
    "modules/ddd",
    "modules/application",
    "modules/identity",
    "modules/IdentityServer",
    "modules/FileManagement"
    # "modules/themes",
    # "modules/account"
)

# List of projects
$projects = (

    # modules/businessexception
    "modules/businessexception/src/Generic.Abp.BusinessException",

    # modules/helper
    "modules/helper/src/Generic.Abp.Helper.Common",
    "modules/helper/src/Generic.Abp.Helper.File",

    # modules/extmenu
    # "modules/extmenu/src/Generic.Abp.ExtMenu.Application.Contracts",
    # "modules/extmenu/src/Generic.Abp.ExtMenu.Application",
    # "modules/extmenu/src/Generic.Abp.ExtMenu.HttpApi",

    # modules/extresource
    "modules/extresource/src/Generic.Abp.ExtResource.Application.Contracts",
    "modules/extresource/src/Generic.Abp.ExtResource.Application",
    "modules/extresource/src/Generic.Abp.ExtResource.HttpApi",

    # modules/texttemplate
    # "modules/texttemplate/src/Generic.Abp.TextTemplate",

    # modules/enumeration
    "modules/enumeration/src/Generic.Abp.Enumeration.Domain.Shared",
    "modules/enumeration/src/Generic.Abp.Enumeration.Application.Contracts",
    "modules/enumeration/src/Generic.Abp.Enumeration.Application",
    "modules/enumeration/src/Generic.Abp.Enumeration.HttpApi",
    
    # modules/ddd
    "modules/ddd/src/Generic.Abp.DDD.Domain.Shared",
    "modules/ddd/src/Generic.Abp.DDD.Domain",
    
    # modules/application
    "modules/application/src/Generic.Abp.Application",
    
    # modules/identity
    "modules/identity/src/Generic.Abp.Identity.Domain.Shared",
    "modules/identity/src/Generic.Abp.Identity.Domain",
    "modules/identity/src/Generic.Abp.Identity.EntityFrameworkCore",
    "modules/identity/src/Generic.Abp.Identity.Application.Contracts",
    "modules/identity/src/Generic.Abp.Identity.Application",
    "modules/identity/src/Generic.Abp.Identity.HttpApi",
    
    # modules/identityServer
    "modules/IdentityServer/src/Generic.Abp.IdentityServer.Domain.Shared",
    "modules/IdentityServer/src/Generic.Abp.IdentityServer.Application.Contracts",
    "modules/IdentityServer/src/Generic.Abp.IdentityServer.Application",
    "modules/IdentityServer/src/Generic.Abp.IdentityServer.HttpApi",
    "modules/IdentityServer/src/Generic.Abp.IdentityServer.Web",
    
    # modules/FileManagement
    "modules/FileManagement/src/Generic.Abp.FileManagement.Domain.Shared",
    "modules/FileManagement/src/Generic.Abp.FileManagement.Domain",
    "modules/FileManagement/src/Generic.Abp.FileManagement.EntityFrameworkCore",
    "modules/FileManagement/src/Generic.Abp.FileManagement.Application.Contracts",
    "modules/FileManagement/src/Generic.Abp.FileManagement.Application"

    # modules/themes
    # "modules/themes/src/Generic.Abp.Themes.Shared",
    # "modules/themes/src/Generic.Abp.Themes",

    # modules/account
    # "modules/account/src/Generic.Abp.Account.Application.Contracts",
    # "modules/account/src/Generic.Abp.Account.Application",
    # "modules/account/src/Generic.Abp.Account.HttpApi",
    # "modules/account/src/Generic.Abp.Account.Identity.Web",
    # "modules/account/src/Generic.Abp.Account.Web",
    # "modules/account/src/Generic.Abp.Account.IdentityServer.Web"
        
)
