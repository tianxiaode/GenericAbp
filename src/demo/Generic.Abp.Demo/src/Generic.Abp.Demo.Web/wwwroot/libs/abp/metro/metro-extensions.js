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

    Metro.Component('input',{
        _createStructure(){
            console.log(arguments);
        }
    })

    Metro.validatorSetup({
        onError: function(){
            console.log('onError', arguments);
        },
        onSubmit(){
            console.log('onSubmit', arguments);
        }
    });
})

