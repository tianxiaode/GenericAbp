﻿using Generic.Abp.Metro.UI.Packages.Core;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Packages.SignalR;

[DependsOn(typeof(CoreScriptContributor))]
public class SignalRBrowserScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //context.Files.AddIfNotContains("/libs/signalr/browser/signalr.js");
    }
}
