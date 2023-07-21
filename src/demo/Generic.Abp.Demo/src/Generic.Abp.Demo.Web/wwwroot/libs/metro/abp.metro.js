var abp = abp || {};
(function($) {

    if (!$) {
        throw "abp/jquery library requires the jquery library included to the page!";
    }

    // ABP CORE OVERRIDES /////////////////////////////////////////////////////

    abp.message._showMessage = function (message, title) {
        alert((title || '') + ' ' + message);

        return $.setTimeout(function ($dfd) {
            $dfd.resolve();
        },50);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);

        return $.setTimeout(function ($dfd) {
            $dfd.resolve(result);
        }, 50);
    };

    abp.utils.isFunction = function (obj) {
        return $.isFunction(obj);
    };

    // JQUERY EXTENSIONS //////////////////////////////////////////////////////

    $.fn.findWithSelf = function (selector) {
        return this.filter(selector).add(this.find(selector));
    };

    // DOM ////////////////////////////////////////////////////////////////////

    abp.dom = abp.dom || {};

    abp.dom.onNodeAdded = function (callback) {
        abp.event.on('abp.dom.nodeAdded', callback);
    };

    abp.dom.onNodeRemoved = function (callback) {
        abp.event.on('abp.dom.nodeRemoved', callback);
    };

    var mutationObserverCallback = function (mutationsList) {
        for (var i = 0; i < mutationsList.length; i++) {
            var mutation = mutationsList[i];
            if (mutation.type === 'childList') {
                if (mutation.addedNodes && mutation.removedNodes.length) {
                    for (var k = 0; k < mutation.removedNodes.length; k++) {
                        abp.event.trigger(
                            'abp.dom.nodeRemoved',
                            {
                                $el: $(mutation.removedNodes[k])
                            }
                        );
                    }
                }

                if (mutation.addedNodes && mutation.addedNodes.length) {
                    for (var j = 0; j < mutation.addedNodes.length; j++) {
                        abp.event.trigger(
                            'abp.dom.nodeAdded',
                            {
                                $el: $(mutation.addedNodes[j])
                            }
                        );
                    }
                }
            }
        }
    };

    $(function(){
        new MutationObserver(mutationObserverCallback).observe(
            $('body')[0],
            {
                subtree: true,
                childList: true
            }
        );
    });    

    // AJAX ///////////////////////////////////////////////////////////////////

    abp.ajax = async function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend({}, abp.ajax.defaultOpts, userOptions);

        options.success = undefined;
        options.error = undefined;

        if(options.data && options.data.requestVerificationToken){
            options.headers.RequestVerificationToken = options.data.requestVerificationToken;
        }

        if (options.data) {
            options.body = typeof options.data === 'string' ? options.data : JSON.stringify(options.data);
        }
        options.method = options.type;
        //options.credentials = 'include';
        options.headers.RequestVerificationToken = abp.security.antiForgery.getToken();
        let defer = $.defer();
        fetch(options.url, options)
            .then(response => {
                let result = response.json();
                result.then(data => {
                    response.responseJson = data;
                    if (response.ok) {
                        defer.resolve(response);
                    } else {
                        if (response.headers.get('_AbpErrorFormat') === 'true') {
                            abp.ajax.handleAbpErrorResponse(response, userOptions);
                        } else {
                            abp.ajax.handleNonAbpErrorResponse(response,  userOptions);
                        }
                        defer.reject(response);

                    }
                })
                .catch(data => {
                    if (response.ok) {
                        defer.resolve(response);
                        return ;
                    }
                    response.error = data;
                    abp.ajax.handleNonAbpErrorResponse(response,  userOptions);
                    defer.reject(response);
                });
            });


        return defer.promise();

    };

    $.extend(abp.ajax, {
        defaultOpts: {
            dataType: 'json',
            method: 'POST',
            contentType: false,
            parseJson: true,
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            }
        },

        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.'
        },

        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.'
        },

        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.'
        },

        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.'
        },

        logError: function (error) {
            abp.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return abp.message.error(error.details, error.message);
            } else {
                return abp.message.error(error.message || abp.ajax.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = abp.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleErrorStatusCode: function (status) {
            switch (status) {
                case 401:
                    abp.ajax.handleUnAuthorizedRequest(
                        abp.ajax.showError(abp.ajax.defaultError401),
                        abp.appPath
                    );
                    break;
                case 403:
                    abp.ajax.showError(abp.ajax.defaultError403);
                    break;
                case 404:
                    abp.ajax.showError(abp.ajax.defaultError404);
                    break;
                default:
                    abp.ajax.showError(abp.ajax.defaultError);
                    break;
            }
        },

        handleNonAbpErrorResponse: function (response, userOptions) {
            if (userOptions.abpHandleError !== false) {
                abp.ajax.handleErrorStatusCode(response.status);
            }

            userOptions.error && userOptions.error.apply(this, arguments);
        },

        handleAbpErrorResponse: function (response, userOptions) {
            var messagePromise = null;

            var responseJson = response.responseJson;

            if (userOptions.abpHandleError !== false) {
                messagePromise = abp.ajax.showError(responseJson.error);
            }

            abp.ajax.logError(responseJson.error);

            userOptions.error && userOptions.error(responseJson.error, response);

            if (response.status === 401 && userOptions.abpHandleError !== false) {
                abp.ajax.handleUnAuthorizedRequest(messagePromise);
            }
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    abp.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                abp.ajax.handleTargetUrl(targetUrl);
            }
        },

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    abp.ui.setBusy();
                } else { //block an element
                    abp.ui.setBusy(options.blockUI);
                }
            }
        },

        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    abp.ui.clearBusy();
                } else { //unblock an element
                    abp.ui.clearBusy(options.blockUI);
                }
            }
        },

        ajaxSendHandler: function (event, request, settings) {
            var token = abp.security.antiForgery.getToken();
            if (!token) {
                return;
            }

            if (!settings.headers || settings.headers[abp.security.antiForgery.tokenHeaderName] === undefined) {
                request.setRequestHeader(abp.security.antiForgery.tokenHeaderName, token);
            }
        }
    });


    abp.event.on('abp.configurationInitialized', function () {
        var l = abp.localization.getResource('AbpExceptionHandling');

        abp.ajax.defaultError.message = l('DefaultErrorMessage');
        abp.ajax.defaultError.details = l('DefaultErrorMessageDetail');
        abp.ajax.defaultError401.message = l('DefaultErrorMessage401');
        abp.ajax.defaultError401.details = l('DefaultErrorMessage401Detail');
        abp.ajax.defaultError403.message = l('DefaultErrorMessage403');
        abp.ajax.defaultError403.details = l('DefaultErrorMessage403Detail');
        abp.ajax.defaultError404.message = l('DefaultErrorMessage404');
        abp.ajax.defaultError404.details = l('DefaultErrorMessage404Detail');
    });

    // RESOURCE LOADER ////////////////////////////////////////////////////////

    /* UrlStates enum */
    var UrlStates = {
        LOADING: 'LOADING',
        LOADED: 'LOADED',
        FAILED: 'FAILED'
    };

    /* UrlInfo class */
    function UrlInfo(url) {
        this.url = url;
        this.state = UrlStates.LOADING;
        this.loadCallbacks = [];
        this.failCallbacks = [];
    }

    UrlInfo.prototype.succeed = function () {
        this.state = UrlStates.LOADED;
        for (var i = 0; i < this.loadCallbacks.length; i++) {
            this.loadCallbacks[i]();
        }
    };

    UrlInfo.prototype.failed = function () {
        this.state = UrlStates.FAILED;
        for (var i = 0; i < this.failCallbacks.length; i++) {
            this.failCallbacks[i]();
        }
    };

    UrlInfo.prototype.handleCallbacks = function (loadCallback, failCallback) {
        switch (this.state) {
            case UrlStates.LOADED:
                loadCallback && loadCallback();
                break;
            case UrlStates.FAILED:
                failCallback && failCallback();
                break;
            case UrlStates.LOADING:
                this.addCallbacks(loadCallback, failCallback);
                break;
        }
    };

    UrlInfo.prototype.addCallbacks = function (loadCallback, failCallback) {
        loadCallback && this.loadCallbacks.push(loadCallback);
        failCallback && this.failCallbacks.push(failCallback);
    };

    /* ResourceLoader API */

    abp.ResourceLoader = (function () {

        var _urlInfos = {};

        function getCacheKey(url) {
            return url;
        }

        function appendTimeToUrl(url) {

            if (url.indexOf('?') < 0) {
                url += '?';
            } else {
                url += '&';
            }

            url += '_=' + new Date().getTime();

            return url;
        }

        var _loadFromUrl = function (url, loadCallback, failCallback, serverLoader) {

            var cacheKey = getCacheKey(url);

            var urlInfo = _urlInfos[cacheKey];

            if (urlInfo) {
                urlInfo.handleCallbacks(loadCallback, failCallback);
                return;
            }

            _urlInfos[cacheKey] = urlInfo = new UrlInfo(url);
            urlInfo.addCallbacks(loadCallback, failCallback);

            serverLoader(urlInfo);
        };

        var _loadScript = function (url, loadCallback, failCallback) {
            _loadFromUrl(url, loadCallback, failCallback, function (urlInfo) {
                $.get({
                    url: url,
                    dataType: 'text'
                })
                .done(function (script) {
                    $.globalEval(script);
                    urlInfo.succeed();
                })
                .fail(function () {
                    urlInfo.failed();
                });
            });
        };

        var _loadStyle = function (url) {
            _loadFromUrl(url, undefined, undefined, function (urlInfo) {

                $('<link/>', {
                    rel: 'stylesheet',
                    type: 'text/css',
                    href: appendTimeToUrl(url)
                }).appendTo('head');
            });
        };

        return {
            loadScript: _loadScript,
            loadStyle: _loadStyle
        }
    })();

})(m4q);