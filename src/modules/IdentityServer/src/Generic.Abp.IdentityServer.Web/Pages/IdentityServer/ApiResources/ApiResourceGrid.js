function ApiResourceGrid(config){
    Grid.call(this, config);
}

inheritPrototype(ApiResourceGrid, Grid);

ApiResourceGrid.prototype.onChange = function(event){
    let me = this,
        grid = me.grid,
        column = grid.columns[event.column],
        record = grid.get(event.recid),
        id = record.id,
        fnName;
    if(column.field === 'enabled'){
        fnName = record[column.field] ? "disable": "enable";
        //api.disable(id).then(me.mergeChanges.bind(me),me.rejectChanges.bind(me));
    }

    if(column.field === 'showInDiscoveryDocument'){
        fnName = record[column.field] ? "hide": "show";
    }

    if(fnName) me.updateCheckChange(fnName, id);

}


ApiResourceGrid.prototype.onActionClick = function(event){
    let me = this,
        target = event.target,
        id = target.getAttribute('data-id'),
        record = me.grid.get(id);
    w2ui.layout.currentRecord = record;
    w2ui.layout.toggle('main');
    w2ui.layout.toggle('right');
}

