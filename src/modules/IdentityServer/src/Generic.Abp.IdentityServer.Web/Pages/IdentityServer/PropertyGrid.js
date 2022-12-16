function PropertyGrid(config){
    let me = this;
    me.initConfig = config;
    for (let i in config) {
        me[i] = config[i];
    }
    me.el = $(me.el);
    me.localization = window.abp.localization.getResource('AbpIdentityServer');

    me.initGrid();
}

PropertyGrid.prototype = {
    constructor: PropertyGrid,
    render:{
        boolean(value){
            let cls = value ? 'fa-check-square' : 'fa-square';
            return `<i class="far ${cls}"></i>`;    
        }
    },

    initGrid(){
        let me = this,
            l = me.localization;
        me.grid = me.el.w2grid({
            name: me.name,
            show: { header: false, columnHeaders: false },
            columns: [
                { field: 'name', text: 'Name', size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
                { field: 'value', text: 'Value', size: '100%' }
            ],
        });

    },

    refresh(record){
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

}