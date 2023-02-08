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
        .then(me.getModalSuccess.bind(me), me.getModalFailure.bind(me));

}

ModalManager.prototype.getModalSuccess = function (data) {
    let me = this;
    if (me.initConfig.scriptUrl) {
        //加载资源
        // abp.ResourceLoader.loadScript(options.scriptUrl, function () {
        //     _initAndShowDialog();
        // });
    } else {
        let id = 'Metro_Modal_' + $.uniqueId(),
            container = $('<div>');
        me.container = container;
        container.attr('id', id);        
        me.containerId = id;
        let body = $('body');
        container.html(data);
        container.appendTo(body);
        me.initAndShowModal();
    }

}

ModalManager.prototype.getModalFailure = function () {
    console.log(arguments);
}

ModalManager.prototype.initAndShowModal = function () {
    let me = this,
        container = me.container,
        modal = container.find('.dialog'),
        form = container.find('form');
    modal = Metro.getPlugin(modal, 'modal');
    if (!modal) {
        _.delay(me.initAndShowModal.bind(me), 50);
        return;
    }
    me.modal = modal;
    form = me.form = Metro.makePlugin(form, 'validator',{
        onBeforeSubmit: me.onBeforeFormSubmit.bind(me),
        onSubmit: me.onFormSubmit.bind(me),
        onError: me.onValidateError.bind(me),
        onValidate: me.onValidate.bind(me),
    });

    if (me.init) me.init.call(me);

    modal.open();

}

ModalManager.prototype.onSubmitSuccess = function () {
    this.setResult(arguments);
    this.modal.close();
}

ModalManager.prototype.onClose = function () {
    let me = this;
    me.container.remove();
    delete me.modal;
    delete me.form;
    delete me.container;
    delete me.callBacks;
}


ModalManager.prototype.getModalId = function () {
    return this.containerId;
}

ModalManager.prototype.getModal = function () {
    return this.modal;
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

ModalManager.prototype.onBeforeFormSubmit = function(e) {
    e.preventDefault();

    console.log(arguments);
}

ModalManager.prototype.onFormSubmit = function(e) {
    e.preventDefault();

    console.log(arguments);
}

ModalManager.prototype.onValidateError = function(logs, data) {
    logs.forEach(log=>{
        let input = $(log.input),
        div = input.parent().parent(),
        span = div.find('.invalid_feedback'),
        texts =[];
        log.errors.forEach(error=>{
            let t = input.attr(`data-val-${error}`);
            if(error === 'min' || error === 'max'){
                t = input.attr(`data-val-range`);
            }
            if(t)texts.push(t);
        })
        span.html(texts.join('<br/>'));
    })

    console.log(arguments);
}

ModalManager.prototype.onValidate = function(e) {
    e.preventDefault();

    console.log(arguments);
}