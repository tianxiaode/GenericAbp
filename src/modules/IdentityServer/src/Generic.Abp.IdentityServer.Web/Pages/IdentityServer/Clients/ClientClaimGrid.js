function ClientClaimGrid(config){
    config = config || {
        el: '#claimsTab',
        api: window.generic.abp.identityServer.clients.client,
        name: 'clientClaimsGrid',
        apiGetName: 'getClaims',
        apiDeleteName: 'removeClaim',
        modal:{
            create: 'IdentityServer/Clients/CreateClientClaimModal'
        },
        columns:[
            { text: 'ClientClaim:Type', field: 'type', size: '50%', isMessage: true },
            { text: 'ClientClaim:Value', field: 'value', size: '50%'},
        ]
    };
    RelationalGrid.call(this, config);
}

inherits(ClientClaimGrid, RelationalGrid);

ClientClaimGrid.prototype.onDelete = function(event){
    let me = this,
        l = me.localization,
        grid = me.grid,
        data = grid.getSelection(),
        title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l("Records")),
        message = [],
        id = me.currentRecord.id,
        records = [];
    data.forEach(m => {
        let rec = grid.get(m);
        if (!rec) return;
        records.push(rec);
        message.push(`${rec.type}(${rec.value})`);
    })
    window.abp.message.confirm(message.join(','), title, function (confirm) {
        if (!confirm) retrun ;
        forEachAsync(records, (m)=>{
            return me.api.removeClaim(id, {type: m.type, value: m.value});
        }).then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));
    });

}

