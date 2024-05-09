function PropertyGrid(config) {
    Grid.call(this, config);
}

window.inherits(PropertyGrid, Grid);


PropertyGrid.prototype.initGrid = function () {
    let me = this;
    me.grid = new w2grid({
        box: me.el,
        name: me.entityName,
        show: { header: false, columnHeaders: false },
        columns: [
            { field: 'name', text: 'Name', size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
            { field: 'value', text: 'Value', size: '100%' }
        ],
    });
    me.el = $(me.el);
}


PropertyGrid.prototype.refresh = function (record) {
    let me = this,
        l = me.localization,
        grid = me.grid,
        records = [];
    me.clear();
    record = record || {};
    me.fields.forEach((f, i) => {
        let field = f && f.name || f,
            value = record[field],
            n = { recid: `recid-${i}`, name: me.getColumnTitle(field), value: value };
        if (f.render && me.render[f.render]) {
            n.value = me.render[f.render].call(null, value);
        }
        records.push(n);
    })
    grid.records = records;
    grid.refresh()

}

