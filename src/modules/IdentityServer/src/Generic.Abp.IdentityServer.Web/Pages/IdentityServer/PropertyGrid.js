function PropertyGrid(config){
    Grid.call(this, config);
}

inherits(PropertyGrid, Grid);


PropertyGrid.prototype.initGrid = function(){
    let me = this;
    me.grid = me.el.w2grid({
        name: me.name,
        show: { header: false, columnHeaders: false },
        columns: [
            { field: 'name', text: 'Name', size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
            { field: 'value', text: 'Value', size: '100%' }
        ],
    });

}


PropertyGrid.prototype.refresh= function(record){
    let me = this,
        l = me.localization,
        grid = me.grid,
        records = [];
    grid.clear();
    me.fields.forEach((f, i)=>{
        let n = { recid: `recid-${i}`, name: l(f.text), value: record[f.field]};
        if(f.render && me.render[f.render]){
            n.value = me.render[f.render].call(null, record[f.field]);
        }
        records.push(n);
    })
    grid.add(records);

}

