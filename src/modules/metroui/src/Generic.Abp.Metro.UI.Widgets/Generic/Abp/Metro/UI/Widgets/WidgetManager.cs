﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Users;

namespace Generic.Abp.Metro.UI.Widgets;

public class WidgetManager : IWidgetManager
{
    protected MetroWidgetOptions Options { get; }
    protected IAuthorizationService AuthorizationService { get; }
    protected ICurrentUser CurrentUser { get; }

    public WidgetManager(
        IOptions<MetroWidgetOptions> widgetOptions,
        IAuthorizationService authorizationService,
        ICurrentUser currentUser)
    {
        AuthorizationService = authorizationService;
        CurrentUser = currentUser;
        Options = widgetOptions.Value;
    }

    public async Task<bool> IsGrantedAsync(Type widgetComponentType)
    {
        var widget = Options.Widgets.Find(widgetComponentType);
        if (widgetComponentType.FullName == null) return false;
        return await IsGrantedAsyncInternal(widget, widgetComponentType.FullName);
    }

    public async Task<bool> IsGrantedAsync(string name)
    {
        var widget = Options.Widgets.Find(name);
        return await IsGrantedAsyncInternal(widget, name);
    }

    private async Task<bool> IsGrantedAsyncInternal(WidgetDefinition widget, string wantedWidgetName)
    {
        if (widget == null)
        {
            throw new ArgumentNullException(wantedWidgetName);
        }

        if (widget.RequiredPolicies.Any())
        {
            foreach (var requiredPolicy in widget.RequiredPolicies)
            {
                if (!(await AuthorizationService.AuthorizeAsync(requiredPolicy)).Succeeded)
                {
                    return false;
                }
            }
        }
        else if (widget.RequiresAuthentication && !CurrentUser.IsAuthenticated)
        {
            return false;
        }

        return true;
    }
}
