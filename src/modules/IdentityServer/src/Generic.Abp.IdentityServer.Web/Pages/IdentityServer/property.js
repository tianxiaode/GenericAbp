function Property(el, name, services, createModalName){
    let me = this;
    me.el = $(el);
    me.name = name;
    me.createModalName = createModalName;
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.services = services;

    me.initGrid();
    me.initCreateAndEditModal();
}

Property.prototype = {

    initGrid(){
        let me = this,
            l = me.localization;
        me.grid = me.el.w2grid({
            name: me.name,
            show: {
                selectColumn: true,
                toolbar: true,
                toolbarSearch: false,
                toolbarColumns: false,
                toolbarInput: false,
                toolbarAdd: { text: null},   // indicates if toolbar add new button is visible
                toolbarEdit: false,   // indicates if toolbar edit button is visible
                toolbarDelete: true,   // indicates if toolbar delete button is visible
                skipRecords: false,    // indicates if skip records should be visible,
                lineNumbers: true,
             },
            columns: [
                { field: 'key', text: l("Properties:Key"), size: '40%', tooltip: l("Properties:Key")},
                { field: 'value', text: l("Properties:Value"), size: '60%', tooltip: l("Properties:Value")},
            ],
            onAdd: me.onAdd.bind(me),
            delete: me.onDelete.bind(me),
            onReload: me.onReoloadClick.bind(me)
        })
    },

    initCreateAndEditModal(){
        let me = this;
        me.createModal = new window.abp.ModalManager(
            window.abp.appPath + `IdentityServer/ApiScopes/${me.createModalName}`
        );
    
        me.createModal.onResult(function () {
            me.grid.clear();
            me.onRefresh();
        });
    

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
                .delete(ids)
                .then(function () {
                    grid.clear();
                    me.onRefresh();
                },
                me.updateFailure.bind(me))
        });

    },


    onReoloadClick(){
        let me = this;
        if(me.grid) me.grid.clear();
        me.onRefresh();
    },


    onRefresh(){
        let me = this,
            data = me.data;

        if(!data) {
            me.getData();
            return;
        };
        me.grid.add(data);

    },

    getData(){
        let me = this,
            record = me.currentRecord;
        if(!record) return;
        me.services.getProperties(record.id).then((ret)=>{
            me.data = ret.items;
            me.onRefresh();
        })
    },


    refresh(record){
        let me= this,
            grid = me.grid;
        me.currentRecord = record;
        grid.clear();
        me.data = null;
        me.onRefresh();

    }
}