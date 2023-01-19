function ModalManager(config) {
    let me = this;
    if (_.isString(config)) {
        config = { viewUrl: config };
    }
    for (let i in config) {
        me[i] = config[i];
    }
    me.initConfig = config;
    me.callBacks = [];
}

ModalManager.prototype.open = function (data) {
    data = data || {};
    let me = this;

    $.get(me.viewUrl, data)
        .then(me.getDialogSuccess.bind(me), me.getDialogFailure.bind(me));

}

ModalManager.prototype.getDialogSuccess = function (data) {
    let me = this;
    if (me.initConfig.scriptUrl) {
        //加载资源
        // abp.ResourceLoader.loadScript(options.scriptUrl, function () {
        //     _initAndShowDialog();
        // });
    } else {
        let id = 'Metro_Dialog_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
        me.dialogId = id;
        data = data.replace('{id}', id);
        let body = $('body');
        body.append(data);
        me.initAndShowDialog();
    }

}

ModalManager.prototype.getDialogFailure = function () {
    console.log(arguments);
}

ModalManager.prototype.initAndShowDialog = function () {
    let me = this,
        id = `#${me.dialogId}`,
        dialog, form, dialogElement;
    dialog = me.dialog = Metro.getPlugin(id, 'dialog');
    if (!dialog) {
        _.delay(me.initAndShowDialog.bind(me), 50);
        return;
    }
    dialog.options.removeOnClose = true;
    dialogElement = dialog.element;
    form = me.form = Metro.getPlugin(dialogElement.find('form'), 'validator');
    formElement = form.element;
    formElement.on('validate-form', function(){console.log(arguments)});
    formElement.on('error-form', function(){console.log(arguments)});
    formElement.on('submit', me.onFormSubmit.bind(me));
    if (formElement.attr('data-check-form-on-close') !== 'false') {
        //form.needConfirmationOnUnsavedClose(dialog);
    }

    //dialogElement.on('colse', me.onClose.bind(me));

    if (me.init) me.init.call(me);

    dialog.open();

}

ModalManager.prototype.onSubmitSuccess = function () {
    this.setResult(arguments);
    this.dialog.close();
}

ModalManager.prototype.onClose = function () {
    let me = this;
    delete me.dialog;
    delete me.form;
    delete me.callBacks;
}


ModalManager.prototype.getModalId = function () {
    return this.dialogId;
}

ModalManager.prototype.getDialog = function () {
    return this.dialog;
}

ModalManager.prototype.getForm = function () {
    return this.form;
}

ModalManager.prototype.getData = function () {
    return this.initConfig.data;
}

ModalManager.prototype.getConfig = function () {
    return this.initConfig;
}

ModalManager.prototype.setResult = function () {
    this.callBacks.forEach(f => {
        f.apply(null, arguments);
    })
    _onResultCallbacks.triggerAll(_publicApi, arguments);
}

ModalManager.prototype.onResult = function (callback) {
    this.callBacks.push(callback);
}

ModalManager.prototype.onFormSubmit = function(e) {
    e.preventDefault();

    console.log(arguments);
}