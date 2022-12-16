function SimpleDetailGrid(options){
    let me = this;
    me.options = options;
    me.el = $(opt.el);
    me.name = options.name;
    me.createModalName = options.createModalName;
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.service = options.service;

    me.initGrid();
    me.initCreateModal();
}

SimpleDetailGrid.prototype = {

    initGrid(){
        let me = this,
            l = me.localization,
            columns = [];
        me.options.columns.forEach(c=>{
            columns.push({
                field: c.field,
                text: l(c.text),
                size: c.size,
                tooltip: l(c.text)
            })
        })
        me.grid = me.el.w2grid({
            name: me.name,
            show: {
                selectColumn: true,
                toolbar: true,
                toolbarSearch: false,
                toolbarColumns: false,
                toolbarInput: false,
                toolbarAdd: { text: null},
                toolbarEdit: false, 
                toolbarDelete: true,
                skipRecords: false, 
                lineNumbers: true,
             },
            columns: columns,
            onAdd: me.onAdd.bind(me),
            delete: me.onDelete.bind(me),
            onReload: me.loadData.bind(me)
        })
    },

    initCreateModal(){
        let me = this;
        me.createModal = new window.abp.ModalManager(
            window.abp.appPath + me.createModalName
        );
    
        me.createModal.onResult(me.updateSuccess.bind(me));
    

    },

    onAdd(event){
        let me = this,
            recrod = me.currentRecord;
        this.createModal.open({foreignKeyId: recrod.id});
    },

    onDelete(event){
        let me = this,
            l = me.localization,
            grid = me.grid,
            data = grid.getSelection(),
            title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('Properties')),
            message = [],
            ids = [];
        data.forEach(m => {
            let rec = grid.get(m);
            if (!rec) return;
            message.push(`${rec.key}`);
            ids.push(rec.id);
        })
        window.abp.message.confirm(message.join(','), title, function (confirm) {
            if (!confirm) retrun ;

            me.service
                .delete.apply(null, [ids])
                .then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));
        });

    },

    updateSuccess(){
        let me = this;
        me.clear();
        me.loadData();
    },

    ajaxFailure(error){
        if(error.code){
            abp.message.error(this.localization(error.code));
        }
    },

    clear(){
        let me = this;
        if(me.grid) me.grid.clear();
        me.data = null;
        me.onRefresh();
    },


    refreshGrid(){
        this.grid.add(this.data);

    },

    loadData(){
        let me = this,
            record = me.currentRecord;
        if(!record) return;
        me.services.read.apply(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
    },

    loadDataSuccess(data){
        let me = this;
        me.data = data.items;
        me.refreshGrid();
    },


    refresh(record){
        let me= this;
        me.currentRecord = record;
        me.clear();
        me.loadData();
    }
}