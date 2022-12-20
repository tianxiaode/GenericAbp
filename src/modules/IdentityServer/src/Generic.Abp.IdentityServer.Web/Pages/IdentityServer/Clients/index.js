(function ($) {

    var l = window.abp.localization.getResource('AbpIdentityServer');

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("Clients") },
            {
                type: 'right', 
                size: '100%',
                hidden: true,
                title: `
                    <div class="d-flex flex-row">
                        <div id="backButton" class="px-2"><span id="backButton" style="cursor:pointer;" class="fas fa-arrow-left" st></span></div>
                        <div id="detailTitle"></div>
                        <div class="flex-fill"></div>
                    </div>
                `,
                content:`
                <ul class="nav nav-tabs" id="tabList" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#detailTab" type="button" role="tab" aria-controls="detailTab" aria-selected="true">${l("Details")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#scopesTab" type="button" role="tab" aria-controls="scopesTab" aria-selected="false">${l("Scopes")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#redirectUrisTab" type="button" role="tab" aria-controls="redirectUrisTab" aria-selected="false">${l("RedirectUris")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#grantTypesTab" type="button" role="tab" aria-controls="grantTypesTab" aria-selected="false">${l("GrantTypes")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#postLogoutRedirectUrisTab" type="button" role="tab" aria-controls="postLogoutRedirectUrisTab" aria-selected="false">${l("PostLogoutRedirectUris")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#identityProviderRestrictionTab" type="button" role="tab" aria-controls="identityProviderRestrictionTab" aria-selected="false">${l("IdentityProviderRestrictions")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#corsOriginsTab" type="button" role="tab" aria-controls="corsOriginsTab" aria-selected="false">${l("CorsOrigins")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#claimsTab" type="button" role="tab" aria-controls="claimsTab" aria-selected="false">${l("Claims")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#secretsTab" type="button" role="tab" aria-controls="secretsTab" aria-selected="false" >${l("Secrets")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#propertyTab" type="button" role="tab" aria-controls="propertyTab" aria-selected="false" >${l("Properties")}</button>
                    </li>
              </ul>
              <div class="tab-content" style="height: calc(100% - 41px);">
                <div class="tab-pane fade h-100 show active" id="detailTab" role="tabpanel" aria-labelledby="detailTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="scopesTab" role="tabpanel" aria-labelledby="scopesTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="redirectUrisTab" role="tabpanel" aria-labelledby="redirectUrisTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="grantTypesTab" role="tabpanel" aria-labelledby="grantTypesTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="postLogoutRedirectUrisTab" role="tabpanel" aria-labelledby="postLogoutRedirectUrisTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="identityProviderRestrictionTab" role="tabpanel" aria-labelledby="identityProviderRestrictionTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="corsOriginsTab" role="tabpanel" aria-labelledby="corsOriginsTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="claimsTab" role="tabpanel" aria-labelledby="claimsTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="secretsTab" role="tabpanel" aria-labelledby="secretsTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="propertyTab" role="tabpanel" aria-labelledby="propertyTab" tabindex="0"></div>
              </div>
              `,
              onShow(){
                  let record = w2ui.layout.currentRecord;
                  $('#detailTitle').html(`${l('Details')} - ${record.clientId}`);
                  switchTab();
              }
            },
        ]
    });

    $('#backButton').click(()=>{
        w2ui.layout.toggle('main');
        w2ui.layout.toggle('right');    
    })

    new ClientGrid();

    $('#tabList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
        switchTab();
    });

    function switchTab() {
        let record = w2ui.layout.currentRecord,
            data = $('#tabList button.active').data(),
            api = window.generic.abp.identityServer.clients.client,
            active = data.bsTarget;
        if(active.includes('detail')){
            let grid = window.detailGrid;
            if(!grid){
                grid = window.detailGrid = new PropertyGrid({
                    name: 'detailGrid',
                    el: '#detailTab',
                    fields: [
                        { text: 'Client:ClientId', field: 'clientId' },
                        { text: 'Client:ClientName', field: 'clientName' },
                        { text: 'Client:Enabled', field: 'enabled', render: 'boolean' },
                        { text: 'Client:Description', field: 'description' },
                        { 
                            text: 'Client:ProtocolType', field: 'protocolType',
                            render(record, row, col, value){ 
                                if(value === "odic") return "OpenID Connect";
                            }
                                    },
                        { text: 'Client:RequireClientSecret', field: 'requireClientSecret', render: 'boolean' },
                        { text: 'Client:RequireRequestObject', field: 'requireRequestObject', render: 'boolean' },
                        { text: 'Client:RequirePkce', field: 'requirePkce', render: 'boolean' },
                        { text: 'Client:AllowPlainTextPkce', field: 'allowPlainTextPkce', render: 'boolean' },
                        { text: 'Client:AllowOfflineAccess', field: 'allowOfflineAccess', render: 'boolean' },
                        { text: 'Client:AllowAccessTokensViaBrowser', field: 'allowAccessTokensViaBrowser', render: 'boolean' },
                        { text: 'Client:FrontChannelLogoutUri', field: 'frontChannelLogoutUri' },
                        { text: 'Client:FrontChannelLogoutSessionRequired', field: 'frontChannelLogoutSessionRequired', render: 'boolean' },
                        { text: 'Client:BackChannelLogoutUri', field: 'backChannelLogoutUri' },
                        { text: 'Client:BackChannelLogoutSessionRequired', field: 'backChannelLogoutSessionRequired', render: 'boolean' },
                        { text: 'Client:EnableLocalLogin', field: 'enableLocalLogin', render: 'boolean' },
                        { text: 'Client:UserSsoLifetime', field: 'userSsoLifetime' },
                        { text: 'Client:IdentityTokenLifetime', field: 'identityTokenLifetime' },
                        { text: 'Client:AllowedIdentityTokenSigningAlgorithms', field: 'allowedIdentityTokenSigningAlgorithms' },
                        { text: 'Client:AccessTokenLifetime', field: 'accessTokenLifetime' },
                        { text: 'Client:AccessTokenType', field: 'accessTokenType' },
                        { text: 'Client:AuthorizationCodeLifetime', field: 'authorizationCodeLifetime' },
                        { text: 'Client:AbsoluteRefreshTokenLifetime', field: 'absoluteRefreshTokenLifetime' },
                        { text: 'Client:SlidingRefreshTokenLifetime', field: 'slidingRefreshTokenLifetime' },
                        { text: 'Client:RefreshTokenUsage', field: 'refreshTokenUsage' },
                        { text: 'Client:RefreshTokenExpiration', field: 'refreshTokenExpiration' },
                        { text: 'Client:UpdateAccessTokenClaimsOnRefresh', field: 'updateAccessTokenClaimsOnRefresh', render: 'boolean' },
                        { text: 'Client:IncludeJwtId', field: 'includeJwtId', render: 'boolean' },
                        { text: 'Client:AlwaysSendClientClaims', field: 'alwaysSendClientClaims', render: 'boolean' },
                        { text: 'Client:AlwaysIncludeUserClaimsInIdToken', field: 'alwaysIncludeUserClaimsInIdToken', render: 'boolean' },
                        { text: 'Client:ClientClaimsPrefix', field: 'clientClaimsPrefix' },
                        { text: 'Client:PairWiseSubjectSalt', field: 'pairWiseSubjectSalt' },
                        { text: 'Client:RequireConsent', field: 'requireConsent', render: 'boolean' },
                        { text: 'Client:ConsentLifetime', field: 'consentLifetime' },
                        { text: 'Client:AllowRememberConsent', field: 'allowRememberConsent', render: 'boolean' },
                        { text: 'Client:ClientUri', field: 'clientUri' },
                        { text: 'Client:LogoUri', field: 'logoUri' },
                        { text: 'Client:UserCodeType', field: 'userCodeType' },
                        { text: 'Client:DeviceCodeLifetime', field: 'deviceCodeLifetime' },
                    ]
                })
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('scopes')){
            let grid = window.clientScopesGrid;
            if(!grid){
                grid = window.clientScopesGrid = new RelationalGrid({
                    el: '#scopesTab',
                    api: api,
                    name: 'clientScopesGrid',
                    apiGetName: 'getScopes',
                    apiDeleteName: 'removeScope',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientScopeModal'
                    },
                    columns:[
                        { text: 'ClientScope:Scope', field: 'scope', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('redirectUris')){
            let grid = window.clientRedirectUrisGrid;
            if(!grid){
                grid = window.clientRedirectUrisGrid = new RelationalGrid({
                    el: '#redirectUrisTab',
                    api: api,
                    name: 'clientRedirectUrisGrid',
                    apiGetName: 'getRedirectUris',
                    apiDeleteName: 'removeRedirectUri',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientRedirectUriModal'
                    },
                    columns:[
                        { text: 'RedirectUris', field: 'redirectUri', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('grantTypes')){
            let grid = window.clientGrantTypesGrid;
            if(!grid){
                grid = window.clientGrantTypesGrid = new RelationalGrid({
                    el: '#grantTypesTab',
                    api: api,
                    name: 'clientGrantTypesGrid',
                    apiGetName: 'getGrantTypes',
                    apiDeleteName: 'removeGrantType',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientGrantTypeModal'
                    },
                    columns:[
                        { text: 'ClientGrantType:GrantType', field: 'grantType', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('postLogoutRedirectUris')){
            let grid = window.clientPostLogoutRedirectUrisGrid;
            if(!grid){
                grid = window.clientPostLogoutRedirectUrisGrid = new RelationalGrid({
                    el: '#postLogoutRedirectUrisTab',
                    api: api,
                    name: 'clientPostLogoutRedirectUrisGrid',
                    apiGetName: 'getPostLogoutRedirectUris',
                    apiDeleteName: 'removePostLogoutRedirectUri',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientPostLogoutRedirectUriModal'
                    },
                    columns:[
                        { text: 'ClientPostLogoutRedirectUri', field: 'postLogoutRedirectUri', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('identityProviderRestriction')){
            let grid = window.clientIdentityProviderRestrictionGrid;
            if(!grid){
                grid = window.clientIdentityProviderRestrictionGrid = new RelationalGrid({
                    el: '#identityProviderRestrictionTab',
                    api: api,
                    name: 'clientIdentityProviderRestrictionGrid',
                    apiGetName: 'getIdentityProviderRestrictions',
                    apiDeleteName: 'removeIdentityProviderRestriction',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientIdentityProviderRestrictionModal'
                    },
                    columns:[
                        { text: 'IdentityProviderRestriction:Provider', field: 'provider', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('corsOrigins')){
            let grid = window.clientCorsOriginsGrid;
            if(!grid){
                grid = window.clientCorsOriginsGrid = new RelationalGrid({
                    el: '#corsOriginsTab',
                    api: api,
                    name: 'clientCorsOriginsGrid',
                    apiGetName: 'getCorsOrigins',
                    apiDeleteName: 'removeCorsOrigin',
                    modal:{
                        create: 'IdentityServer/Clients/CreateClientCorsOriginModal'
                    },
                    columns:[
                        { text: 'CorsOrigin:Origin', field: 'origin', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if(active.includes('claims')){
            let grid = window.clientClaimsGrid;
            if(!grid){
                grid = window.clientClaimsGrid = new ClientClaimGrid();
            }
            grid.refresh(record);
            return;
        }
        
        
        if(active.includes('secrets')){
            let grid = window.cleintSecretGrid;
            if(!grid){
                grid = window.cleintSecretGrid = new SecretGrid({
                    el: '#secretsTab',
                    api: api,
                    name: 'cleintSecretGrid',
                    modal:{
                        create: "IdentityServer/Clients/CreateClientSecretModal"
                    },
                });
            }
            grid.refresh(record);
            
            return;
        }

        if(active.includes('property')){
            let grid = window.clientPropertyGrid;
            if(!grid){
                grid = window.clientPropertyGrid = new ResourcePropertyGrid({
                    el: '#propertyTab',
                    api: api,
                    name: 'clientPropertyGrid',
                    modal:{
                        create: "IdentityServer/Clients/CreateClientPropertyModal"
                    },
                });
            }
            grid.refresh(record);
            
            return;
        }

    };




})(jQuery);
