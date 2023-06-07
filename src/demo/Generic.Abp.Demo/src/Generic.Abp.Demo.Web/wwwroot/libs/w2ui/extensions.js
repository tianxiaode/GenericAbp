(function ($) {

    if (abp.localization.currentCulture.cultureName === 'zh-CN' || abp.localization.currentCulture.cultureName === 'zh-Hans') {
        w2utils.locale('/libs/w2ui/locale/zh-cn.json');
    }

    w2grid.prototype.rejectChanges = function () {
        var changes = this.getChanges();
        for (var c = 0; c < changes.length; c++) {
            var record = this.get(changes[c][this.recid || 'recid']);
            if (record.w2ui) delete record.w2ui.changes;
        }
        this.refresh();

    }

    w2grid.prototype.onRequest = function(){
        console.log(argument)
    }

})(Metro);