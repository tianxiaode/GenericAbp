$apiKey = $args[0]

$packages = (
    # modules/helper
    "Generic.Abp.Helper.File",

    # modules/extmenu
    "Generic.Abp.ExtMenu.Application.Contracts",
    "Generic.Abp.ExtMenu.Application",
    "Generic.Abp.ExtMenu.HttpApi",

    # modules/extresource
    "Generic.Abp.ExtResource.Application.Contracts",

    # modules/texttemplate
    "Generic.Abp.TextTemplate",

    # modules/enumeration
    "Generic.Abp.Enumeration.Domain.Shared",
    "Generic.Abp.Enumeration.Application.Contracts",
    "Generic.Abp.Enumeration.Application",
    "Generic.Abp.Enumeration.HttpApi",

    # modules/themes
    "Generic.Abp.Themes.Shared",
    "Generic.Abp.Themes",

    # modules/account
    "Generic.Abp.Account.Application.Contracts",
    "Generic.Abp.Account.Application",
    "Generic.Abp.Account.HttpApi",
    "Generic.Abp.Account.Identity.Web",
    "Generic.Abp.Account.Web",
    "Generic.Abp.Account.IdentityServer.Web"
	
	)
	
foreach($package in $packages) {
	Write-Host("Delete package:" + $package)
    dotnet nuget delete $package 0.0.6 --non-interactive -s http://localhost:1234/v3/index.json --api-key "$apiKey"
}
