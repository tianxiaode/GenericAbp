Metro.validatorSetup({
    onErrorForm: function(logs, data){
        logs.forEach(log=>{
            var input = $(log.input);
            var div = input.closest('div').parent();
            var span = div.find('.invalid_feedback');
            var texts =[];
            log.errors.forEach(error=>{
                let t = input.attr(`data-val-${error}`);
                if(error === 'min' || error === 'max'){
                    t = input.attr(`data-val-range`);
                }
                if(t)texts.push(t);
            })
            span.html(texts.join('<br/>'));
        })
    },
    onSubmit(){
        console.log('onSubmit', arguments);
    }
});

Metro.inputSetup({
    onInputCreate(input){
        var div = input.closest('div').parent();
        var span = $('<span>').addClass('invalid_feedback pos-absolute');
        var invalidInput = div.find(`input[name="__Invariant"]`);
        var lable = div.find('label');
        if(invalidInput){
            invalidInput.remove();
        }
        span.css('left', lable.width());
        span.css('bottom', 2);
        span.appendTo(div);

    }
})

Metro.textareaSetup({
    onTextareaCreate(input){
        var div = input.closest('div').parent();
        var span = $('<span>').addClass('invalid_feedback pos-absolute');
        var invalidInput = div.find(`input[name="__Invariant"]`);
        var lable = div.find('label');
        if(invalidInput){
            console.log(invalidInput);
            invalidInput.remove();
        }
        span.css('left', lable.width());
        span.css('bottom', 2);
        span.appendTo(div);

    }
})

Metro.checkboxSetup({
    onCheckboxCreate(input){
        var div = $(input.closest('div'));
        var label = div.find('.checkbox-label');
        var html = label.html();
        var span = div.find('span.caption');
        console.log(html, span);
        span.text(html);
    }
})

$(function () {

    $.extend(Metro.locales['zh-CN'],{
        "table": {
            "rowsCount": "显示实体:",
            "search": "搜索:",
            "info": "显示 $1 到 $2 的 $3 条目",
            "prev": "上一页",
            "next": "下一页",
            "all": "全部",
            "inspector": "查看",
            "skip": "转到页面",
            "empty": "没有数据"
        },
        "colorSelector": {
            addUserColorButton: "添加到颜色板",
            userColorsTitle: "用户颜色"
        },
        "switch": {
            on: "开",
            off: "关"
        }
    });

})

