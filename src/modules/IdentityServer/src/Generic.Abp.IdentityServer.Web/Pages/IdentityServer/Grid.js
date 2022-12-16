function Grid(config){
    let me = this;
    me.initConfig = config;
    for (let i in config) {
        me[i] = config[i];
    }
    me.el = $(me.el);
    me.localization = window.abp.localization.getResource('AbpIdentityServer');

    me.initGrid();
    me.initCreateAndEditModal();
}

Grid.prototype = {
    constructor: Grid,
    render:{
        boolean(value){
            let cls = value ? 'fa-check-square' : 'fa-square';
            return `<i class="far ${cls}"></i>`;    
        }
    },

    initGrid(){
        let me = this,
            l = me.localization,
            columns = [],
            hasAction = false;
        me.columns.forEach(c=>{
            if(c.isMessage) me.messageField = c.field;
            if(c.isAction) hasAction = true;
            columns.push({
                field: c.field,
                text: l(c.text),
                size: c.size,
                tooltip: l(c.text),
                style: c.style,
                editable: c.editable,
                render: c.render
            })
        })
        me.grid = me.el.w2grid({
            name: me.name,
            dataType:'HTTP',
            url: me.url,
            limit: 25,
            header: true,
            toolbar: true,
            method: 'GET', 
            multiSelect: true,
            show: {
                selectColumn: true,
                toolbar: true,
                footer: true,
                toolbarSearch: false,
                toolbarColumns: false,
                toolbarInput: false,
                toolbarAdd: { text: null},   
                toolbarEdit: true,   
                toolbarDelete: true,   
                skipRecords: false,
                lineNumbers: true,
             },
            columns: columns,
            onRequest(event) {
                let postData = event.postData;
                postData.skipCount = postData.offset;
                postData.MaxResultCount = postData.limit;
            },
            parser(data) {
                data.total = data.totalCount;
                data.records = data.items;
                return data;
            },
            onSelect: me.onSelect.bind(me),
            onUnselect: me.onDeselect.bind(me),    
            onAdd: me.onAdd.bind(me),
            onEdit: me.onEdit.bind(me),
            delete: me.onDelete.bind(me),
            onChange: me.onChange.bind(me),
            onColumnClick: me.onColumnClick.bind(me)
        });

        if(hasAction){
            me.el.delegate('span.action', 'click',me.onActionClick.bind(me));
        }
    },

    initCreateAndEditModal(){
        let me = this;
        me.createModal = new window.abp.ModalManager(
            window.abp.appPath + me.modal.create
        );
    
        me.createModal.onResult(me.updateSuccess.bind(me));
    
        me.editModal = new window.abp.ModalManager(
            window.abp.appPath + me.modal.edit
        );
    
        me.editModal.onResult(me.updateSuccess.bind(me));

    },

    onAdd(event){
        this.createModal.open();
    },

    onEdit(event){
        let me = this,
            record = me.grid.get(event.recid);
        if(!record) return;
        me.editModal.open({
            id: record.id
        });

    },

    onDelete(event){
        let me = this,
            l = me.localization,
            grid = me.grid,
            data = grid.getSelection(),
            title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l(me.name)),
            message = [],
            ids = [];
        data.forEach(m => {
            let rec = grid.get(m);
            if (!rec) return;
            message.push(`${rec[me.messageField]}`);
            ids.push(rec.id);
        })
        window.abp.message.confirm(message.join(','), title, function (confirm) {
            if (!confirm) retrun ;

            me.api.delete(ids)
                .then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));
        });

    },

    updateCheckChange(apiName, id){
        let me = this,
            fn = this.api[apiName];
        if(!isFunction(fn)) return;
        fn.call(null,id).then(me.mergeChanges.bind(me),me.rejectChanges.bind(me));

    },

    updateSuccess(){
        this.grid.reload();
    },

    mergeChanges(){
        this.grid.mergeChanges();
    },

    rejectChanges(){
        this.grid.rejectChanges();
    },

    ajaxFailure(error){
        if(error.code){
            abp.message.error(this.localization(error.code));
        }
    },

    onSelect(){},

    onDeselect(){},

    onChange(){},

    onColumnClick(){},

    onActionClick(){}
}