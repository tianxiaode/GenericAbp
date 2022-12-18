function ApiResourceScopeDetail(){
    let me = this;
    me.titleEl = $('#scopesDetailModalLabel');
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.dialog = $('#scopesDetailModal');
    me.dialogBackdrop = $('#scopesDetailModalBackdrop');
    $('#scopesDetailModalCloseButton').click(()=>{ 
        me.dialog.hide(); 
        me.dialogBackdrop.hide();
    });
    me.initTabs();
}

ApiResourceScopeDetail.prototype = {
    initTabs(){
        let l = this.localization;
        $(`#scopesDetailList button[data-bs-target="#scopeDetailTab"]`).html(l("Details"));
        $(`#scopesDetailList button[data-bs-target="#scopeClaimsTab"]`).html(l("Claims"));
        $(`#scopesDetailList button[data-bs-target="#scopePropertyTab"]`).html(l("Properties"));
    
        $('#scopesDetailList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
            this.switchTab();
        });

        
    
    },

    switchTab(){
        let me = this,
            record = me.currentRecord,
            data = $('#scopesDetailList button.active').data(),
            active = data.bsTarget;
        if(active.includes('Detail')){
            let grid = me.detailGrid;
            if(!grid){
                grid = me.detailGrid = new PropertyGrid({
                    name: 'scopeDetailGrid',
                    el: '#scopeDetailTab',
                    fields: [
                        { text: 'ApiScope:Name', field: 'name' },
                        { text: 'ApiScope:DisplayName', field: 'displayName' },
                        { text: 'ApiScope:Description', field: 'description' },
                        { text: 'ApiScope:Enabled', field: 'enabled', render: 'boolean' },
                        { text: 'ApiScope:Required', field: 'required', render: 'boolean' },
                        { text: 'ApiScope:Emphasize', field: 'emphasize', render: 'boolean' },
                        { text: 'ApiScope:ShowInDiscoveryDocument', field: 'showInDiscoveryDocument', render: 'boolean' },
                    ],
                })
            }
            grid.refresh(record);
            return;
        }
        if(active.includes('Claims')){
            let grid = me.apiResourceClaimsGrid;
            if(!grid){
                grid = me.apiResourceClaimsGrid = new ClaimGrid({
                    el: '#scopeClaimsTab',
                    api: window.generic.abp.identityServer.apiScopes.apiScope,
                    name: 'apiScopeClaimsGrid'
                });
            }
            grid.refresh(record);
            return;
        }
        
        if(active.includes('Property')){
            let grid = me.apiResourcePropertyGrid;
            if(!grid){
                grid = me.apiResourcePropertyGrid = new ResourcePropertyGrid({
                    el: '#scopePropertyTab',
                    api: window.generic.abp.identityServer.apiScopes.apiScope,
                    name: 'apiScopePropertyGrid',
                    modal:{
                        create: "IdentityServer/ApiScopes/CreateApiScopePropertyModal"
                    },
                });
            }
            grid.refresh(record);
            
            return;
        }

    },

    refresh(record){
        let me = this,
            l = me.localization;
        me.currentRecord = record;
        me.titleEl.html(`${l("ApiScopes")}${l("Details")} - ${record.name}`);
        me.dialogBackdrop.show();
        me.dialog.show();
        me.switchTab();
    },


}