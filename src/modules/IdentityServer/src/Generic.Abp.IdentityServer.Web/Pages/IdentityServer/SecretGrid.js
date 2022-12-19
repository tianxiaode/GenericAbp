function SecretGrid(config){
    Object.assign(config, {
        apiGetName: 'getSecrets',
        columns:[
            { 
                field: 'type', text: "Secrets:Type", size: '20%', 
                render(record, row, col, value){ 
                    if(value === "SharedSecret") return "Shared Secret";
                    if(value === "X509Thumbprint") return "X509 Thumbprint";
                }
            },
            { field: 'value', text: "Secrets:Value", size: '30%' },
            { field: 'description', text: "Secrets:Description", size: '25%' },
            { 
                field: 'expiration', text: "Secrets:Expiration", size: '160px',
                render(record, row, col, value){ 
                    if(!value) return "";
                    return window.luxon.DateTime
                    .fromISO(value, { locale: window.abp.localization.currentCulture.name })
                    .toLocaleString(window.luxon.DateTime.DATETIME_SHORT);

                }
            },    
        ]
    });
    RelationalGrid.call(this, config);
}

inherits(SecretGrid, RelationalGrid);

SecretGrid.prototype.onDelete = function(event){
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
        message.push(`${rec.type} - ${rec.value}`);
    })
    window.abp.message.confirm(message.join(','), title, function (confirm) {
        if (!confirm) retrun ;
        records.forEach(m=>{
            me.api.removeSecret(id, { type: m.type, value: m.value })
                .then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));

        })
    });

}

