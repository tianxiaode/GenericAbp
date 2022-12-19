function ClientGrid(config){
    config = config || {
        el: '#layout_layout_panel_main div.w2ui-panel-content',
        modal: {
            create: 'IdentityServer/Clients/CreateModal',
            edit: 'IdentityServer/Clients/EditModal'
        },
        url: '/api/clients',
        api: window.generic.abp.identityServer.clients.client,
        name: 'Clients',
        columns:[
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record){ 
                    let text = window.abp.localization.getResource('AbpIdentityServer')('Details');
                    return  `<span style="cursor:pointer;" data-id="${record.recid}" class="action">${text}</span>`;
                },

            },
            { field: 'clientId', text: "Client:ClientId", size: '100px', isMessage: true },
            { field: 'clientName', text: "Client:ClientName", size: '100px', isMessage: true },
            { field: 'description', text: "Client:Description", size: '100px'  },
            { field: 'clientClaimsPrefix', text: "Client:ClientClaimsPrefix", size: '100px'  },
            { field: 'protocolType', text: "Client:ProtocolType", size: '100px'  },
            { field: 'identityTokenLifetime', text: "Client:IdentityTokenLifetime", size: '100px'  },
            { field: 'accessTokenLifetime', text: "Client:AccessTokenLifetime", size: '100px'  },
            { field: 'authorizationCodeLifetime', text: "Client:AccessTokenLifetime", size: '100px'  },
            { field: 'absoluteRefreshTokenLifetime', text: "Client:AbsoluteRefreshTokenLifetime", size: '100px'  },
            { field: 'slidingRefreshTokenLifetime', text: "Client:SlidingRefreshTokenLifetime", size: '100px'  },
            { field: 'deviceCodeLifetime', text: "Client:DeviceCodeLifetime", size: '100px'  },
            { field: 'RefreshTokenExpiration', text: "Client:DeviceCodeLifetime", size: '100px'  },
            { field: 'enabled', text: "Client:Enabled", size: '80px', style: 'text-align: center',
                action:{ check: 'disable', uncheck: 'enable' },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { field: 'requireClientSecret', text: "Client:RequireClientSecret", size: '80px', style: 'text-align: center',
                action:{ check: 'optionalClientSecret', uncheck: 'requireClientSecret' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'requireRequestObject', text: "Client:RequireRequestObject", size: '80px', style: 'text-align: center',
                action:{ check: 'OptionalRequestObject', uncheck: 'requireRequestObject' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'requirePkce', text: "Client:RequirePkce", size: '80px', style: 'text-align: center',
                action:{ check: 'optionalPkce', uncheck: 'requirePkce' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'allowPlainTextPkce', text: "Client:AllowPlainTextPkce", size: '80px', style: 'text-align: center',
                action:{ check: 'ForbidPlainTextPkce', uncheck: 'AllowPlainTextPkce' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'allowOfflineAccess', text: "Client:AllowOfflineAccess", size: '80px', style: 'text-align: center',
                action:{ check: 'ForbidOfflineAccess', uncheck: 'AllowOfflineAccess' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'allowAccessTokensViaBrowser', text: "Client:AllowAccessTokensViaBrowser", size: '80px', style: 'text-align: center',
                action:{ check: 'ForbidAccessTokensViaBrowser', uncheck: 'AllowAccessTokensViaBrowser' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'frontChannelLogoutUri', text: "Client:FrontChannelLogoutUri", size: '100px'  },
            { field: 'frontChannelLogoutSessionRequired', text: "Client:FrontChannelLogoutSessionRequired", size: '80px', style: 'text-align: center',
                action:{ check: 'FrontChannelLogoutSessionOptional', uncheck: 'FrontChannelLogoutSessionRequired' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'backChannelLogoutUri', text: "Client:BackChannelLogoutUri", size: '100px'  },
            { field: 'backChannelLogoutSessionRequired', text: "Client:BackChannelLogoutSessionRequired", size: '80px', style: 'text-align: center',
                action:{ check: 'BackChannelLogoutSessionOptional', uncheck: 'BackChannelLogoutSessionRequired' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'enableLocalLogin', text: "Client:EnableLocalLogin", size: '80px', style: 'text-align: center',
                action:{ check: 'DisableLocalLogin', uncheck: 'EnableLocalLogin' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'updateAccessTokenClaimsOnRefresh', text: "Client:UpdateAccessTokenClaimsOnRefresh", size: '80px', style: 'text-align: center',
                action:{ check: 'KeepAccessTokenClaimsOnRefresh', uncheck: 'UpdateAccessTokenClaimsOnRefresh' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'includeJwtId', text: "Client:IncludeJwtId", size: '80px', style: 'text-align: center',
                action:{ check: 'ExcludeJwtId', uncheck: 'IncludeJwtId' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'alwaysSendClientClaims', text: "Client:AlwaysSendClientClaims", size: '80px', style: 'text-align: center',
                action:{ check: 'NeverSendClientClaims', uncheck: 'AlwaysSendClientClaims' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'alwaysIncludeUserClaimsInIdToken', text: "Client:AlwaysIncludeUserClaimsInIdToken", size: '80px', style: 'text-align: center',
                action:{ check: 'NeverIncludeUserClaimsInIdToken', uncheck: 'AlwaysIncludeUserClaimsInIdToken' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'requireConsent', text: "Client:RequireConsent", size: '80px', style: 'text-align: center',
                action:{ check: 'OptionalConsent', uncheck: 'RequireConsent' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            { field: 'allowRememberConsent', text: "Client:AllowRememberConsent", size: '80px', style: 'text-align: center',
                action:{ check: 'ForbidRememberConsent', uncheck: 'AllowRememberConsent' },
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
        ]
    };
    Grid.call(this, config);
}

inherits(ClientGrid, Grid);


ClientGrid.prototype.onActionClick = function(event){
    let me = this,
        target = event.target,
        id = target.getAttribute('data-id'),
        record = me.grid.get(id);
    w2ui.layout.currentRecord = record;
    w2ui.layout.toggle('main');
    w2ui.layout.toggle('right');
}

