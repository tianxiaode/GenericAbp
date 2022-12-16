function Claims(el, name, services){
    let me = this;
    me.el = $(el);
    me.name = name;
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.services = services;

    me.init();
}

Claims.prototype = {

    init(){
        let me = this,
            l = me.localization;
        me.grid = me.el.w2grid({
            name: me.name,
            columns: [
                { field: 'name', text: l("Claims"), size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
                { field: 'value', text: l("OwnedClaims"), size: '100%', 
                    editable: { type: 'checkbox', style: 'text-align: center' } 
                },
            ],
            onChange: me.onChange.bind(me)
        })
    },

    onChange(event){
        let me = this,            
            grid = me.grid,
            currentRecord = me.currentRecord,
            claim = grid.get(event.recid);
        if(claim.value){
            me.services
                .removeClaim(currentRecord.id, { type: claim.name } )
                .then(function () {
                    grid.mergeChanges();
                });
        }else{
            me.services
            .addClaim(currentRecord.id, { type: claim.name })
            .then(function () {
                grid.mergeChanges();
            });
        }        

    },

    onRefresh(){
        let me = this,
            recs = [],
            data = me.data,
            index = 0,
            claims = me.claims;

        if(!claims){
            me.getClaims();
            return;        
        }
        if(!data) return;
        claims.forEach(c=>{
            let rec = { recid: index, name: c.name, value: data.find(d=>d.type === c.name) };
            index++;
            recs.push(rec);
        });
        me.grid.add(recs);

    },

    getClaims(){
        window.generic.abp.identityServer.claimTypes.claimType.getList()
        .then((d)=>{
            this.claims = d.items;
            this.onRefresh();
        })

    },

    refresh(record){
        let me= this,
            grid = me.grid;
        me.currentRecord = record;
        grid.clear();
        if(record && record.id){
            me.services.getClaims(record.id).then((ret)=>{
                me.data = ret.items;
                me.onRefresh();
            })
        }else{
            me.data = null;
            me.onRefresh();
        }

    }
}