function ResourcePropertyGrid(config){
    Object.assign(config, {
        apiGetName: 'getProperties',
        columns:[
            { field: 'key', text: "Properties:Key", size: '50%'},
            { field: 'value', text: "Properties:Value", size: '50%'},
        ]
    });
    RelationalGrid.call(this, config);
}

inherits(ResourcePropertyGrid, RelationalGrid);

ResourcePropertyGrid.prototype.onDelete = function(event){
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
        message.push(`${rec.key}`);
    })
    window.abp.message.confirm(message.join(','), title, function (confirm) {
        if (!confirm) retrun ;
        forEachAsync(records, (m)=>{
            return me.api.removeProperty(id, {key: m.key});
        }).then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));
    });

}

