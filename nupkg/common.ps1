# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../src/"

# List of solutions
$solutions = (
    "modules/w2ui",
    "modules/jspreadsheet",
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
    "modules/FileManagement",
    "modules/openiddict",
    "modules/phonelogin",
    "modules/metroui"
    # "modules/themes",
    # "modules/account"
)

# List of projects
$projects = (

    # modules/w2ui
    "modules/w2ui/src/Generic.Abp.W2Ui",

    # modules/jspreadsheet
    "modules/jspreadsheet/src/Generic.Abp.Jspreadsheet",

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
    "modules/FileManagement/src/Generic.Abp.FileManagement.Application",

    # modules/openiddict
    "modules/openiddict/src/Generic.Abp.OpenIddict.Domain.Shared",
    "modules/openiddict/src/Generic.Abp.OpenIddict.Application.Contracts",
    "modules/openiddict/src/Generic.Abp.OpenIddict.Application",
    "modules/openiddict/src/Generic.Abp.OpenIddict.HttpApi",
    "modules/openiddict/src/Generic.Abp.OpenIddict.Web",

    # modules/phonelogin
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.Domain.Shared",
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.Domain",
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.Application",
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.IdentityServer.Domain",
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.Account.Web",
    "modules/phonelogin/src/Generic.Abp.PhoneLogin.OpenIddict.AspNetCore",

    # modules/metroui
    "modules/metroui/src/Generic.Abp.Metro.UI",
    "modules/metroui/src/Generic.Abp.Metro.UI.Widgets",    
    "modules/metroui/src/Generic.Abp.Metro.UI.Packages",
    "modules/metroui/src/Generic.Abp.Metro.UI.Bundling",
    "modules/metroui/src/Generic.Abp.Metro.UI.Theme.Shared",
    "modules/metroui/src/Generic.Abp.Metro.UI.Theme.Basic",
    "modules/metroui/src/Generic.Abp.Metro.UI.Theme.Basic.Demo",
    "modules/metroui/src/Generic.Abp.Metro.UI.MultiTenancy",
    "modules/metroui/src/Generic.Abp.Metro.UI.Account.Web",
    "modules/metroui/src/Generic.Abp.Metro.UI.Identity.Web",
    "modules/metroui/src/Generic.Abp.Metro.UI.OpenIddict.AspNetCore",
    "modules/metroui/src/Generic.Abp.Metro.UI.OpenIddict.Web",
    "modules/metroui/src/Generic.Abp.Metro.UI.Account.Web.OpenIddict"

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
