function ApiResourceScope(){
    let me = this;
    me.service = window.generic.abp.identityServer.apiScopes.apiScope;
    me.apiResourceService = window.generic.abp.identityServer.apiResources.apiResource;
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.initLayout();
    me.initGrid();
    me.initCreateAndEditModal();
    me.initClaimGrid();
} 

ApiResourceScope.prototype = {

   initLayout(){
       let me = this,
            l = me.localization;
        $('#scopesTab').w2layout({
            name: 'scopesLayout',
            panels: [
                { type: 'main', },
                { type: 'bottom', title: l("Claims"), size: '45%', resizable: true}
            ]
        });
    },

    initGrid(){
        let me = this;
        me.scopeGrid = new ApiResourceScopeGrid();
        me.claimGrid = new ClaimGrid({
            el: '#layout_scopesLayout_panel_bottom div.w2ui-panel-content',
            api: window.generic.abp.identityServer.apiScopes.apiScope,
            name: 'apiResourceScopeClaimsGrid'
        })
    },


}