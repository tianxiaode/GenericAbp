(function ($) {

    var l = window.abp.localization.getResource('AbpIdentityServer');

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("ApiResources") },
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
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#claimsTab" type="button" role="tab" aria-controls="claimsTab" aria-selected="false">${l("Claims")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#scopesTab" type="button" role="tab" aria-controls="scopesTab" aria-selected="false">${l("Scopes")}</button>
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
                <div class="tab-pane fade h-100" id="claimsTab" role="tabpanel" aria-labelledby="profile-tab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="scopesTab" role="tabpanel" aria-labelledby="contact-tab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="secretsTab" role="tabpanel" aria-labelledby="disabled-tab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="propertyTab" role="tabpanel" aria-labelledby="disabled-tab" tabindex="0"></div>
              </div>
              `,
              onShow(){
                  let record = w2ui.layout.currentRecord;
                  $('#detailTitle').html(`${l('Details')} - ${record.name}`);
                //   $('#tabList button.active').removeClass("active");                  
                //   $('#tabList button[data-bs-target="#detailTab"]').addClass('active');
                  switchTab();
              }
            },
        ]
    });

    $('#backButton').click(()=>{
        w2ui.layout.toggle('main');
        w2ui.layout.toggle('right');    
    })

    var apiResourceGrid = new ApiResourceGrid();

    $('#tabList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
        switchTab();
    });

    function switchTab() {
        let record = w2ui.layout.currentRecord,
            data = $('#tabList button.active').data(),
            active = data.bsTarget;
        if(active.includes('detail')){
            let grid = window.detailGrid;
            if(!grid){
                grid = window.detailGrid = new PropertyGrid({
                    name: 'detailGrid',
                    el: '#detailTab',
                    fields: [
                        { text: 'ApiResource:Name', field: 'name' },
                        { text: 'ApiResource:DisplayName', field: 'displayName' },
                        { text: 'ApiResource:Description', field: 'description' },
                        { text: 'ApiResource:AllowedAccessTokenSigningAlgorithms', field: 'allowedAccessTokenSigningAlgorithms' },
                        { text: 'ApiResource:Enabled', field: 'enabled', render: 'boolean' },
                        { text: 'ApiResource:ShowInDiscoveryDocument', field: 'showInDiscoveryDocument', render: 'boolean' },
                    ]
                })
            }
            grid.refresh(record);
            return;
        }
        if(active.includes('claims')){
            let grid = window.apiResourceClaimsGrid;
            if(!grid){
                grid = window.apiResourceClaimsGrid = new ClaimGrid({
                    el: '#claimsTab',
                    api: window.generic.abp.identityServer.apiResources.apiResource,
                    name: 'apiResourceClaimsGrid'
                });
            }
            grid.refresh(record);
            return;
        }
        
        if(active.includes('scopes')){
            return;
        }
        
        if(active.includes('secrets')){
            let grid = window.apiResourceSecretGrid;
            if(!grid){
                grid = window.apiResourceSecretGrid = new SecretGrid({
                    el: '#secretsTab',
                    api: window.generic.abp.identityServer.apiResources.apiResource,
                    name: 'apiResourceSecretGrid',
                    modal:{
                        create: "IdentityServer/ApiResources/CreateApiResourceSecretModal"
                    },
                });
            }
            grid.refresh(record);
            
            return;
        }

        if(active.includes('property')){
            let grid = window.apiResourcePropertyGrid;
            if(!grid){
                grid = window.apiResourcePropertyGrid = new ResourcePropertyGrid({
                    el: '#propertyTab',
                    api: window.generic.abp.identityServer.apiResources.apiResource,
                    name: 'apiResourcePropertyGrid',
                    modal:{
                        create: "IdentityServer/ApiResources/CreateApiResourcePropertyModal"
                    },
                });
            }
            grid.refresh(record);
            
            return;
        }

    };




})(jQuery);
