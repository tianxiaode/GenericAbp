/* global Metro, Cake */
(function(Metro, $) {
    'use strict';
    var Utils = Metro.utils;
    var CrudToolbarDefaultConfig = {
        noRefresh: false,
        noNew: false,
        noEdit: true,
        noDelete: false,
        targets: "button",
        noText: false,
        refreshPermission: null,
        newPermission: 'Create',
        editPermission: 'Update',
        deletePermission: 'Delete',
        permissionPrefix: null,
        customButtons: null,
        onButtonClick: 'onCrudToolbarButtonClick',
        onCrudToolbarCreate: Metro.noop
    };

    Metro.crudToolbarSetup = function (options) {
        CrudToolbarDefaultConfig = $.extend({}, CrudToolbarDefaultConfig, options);
    };

    if (typeof window["metroCrudToolbarSetup"] !== undefined) {
        Metro.crudToolbarSetup(window["metroCrudToolbarSetup"]);
    }

    Metro.Component('crud-toolbar', {
        init: function( options, elem ) {
            this._super(elem, options, CrudToolbarDefaultConfig,{
                active: null,
                id: Utils.elementId("crud-toolbar")
            });
            return this;
        },

        _create: function(){
            var element = this.element;
            element.addClass('crud-toolbar');

            this._createButtons();
            this._createEvents();

            this._fireEvent("crud-toolbar-create", {
                element: element
            });
        },

        _createButtons(){
            var me = this, element = this.element, o = this.options;
            var customButtons = o.customButtons;

            if(!o.noRefresh){
                me._createButton(element ,'Refresh', 'fa fa-undo', 'primary', 10000, o.noText);
            }

            if(!o.noNew){
                me._createButton(element ,'New', 'fa fa-file', 'primary', 10100, o.noText);
            }

            if(!o.noEdit){
                me._createButton(element ,'Edit', 'fa fa-edit', 'primary', 10200, o.noText);
            }

            if(!o.noDelete){
                me._createButton(element ,'Delete', 'fa fa-trash', 'alert', 10300, o.noText);
            }
            if(customButtons){
                if(!_.isArray(customButtons)) {
                   console.error(`Custom buttons ${JSON.stringify(customButtons)} are not valid arrays`);
                   return;
                }
                customButtons.forEach(b => {
                    me._createButton(element, ...b);
                });
            }
        },

        _createButton(el, text, icon, color, order, noText, permission){
            if(!this._isGranted(permission)) return;
            var l = abp.localization.getResource('AbpUi');
            var displayText = noText ? '' : l(text);
            var button = $('<button>');
            button.addClass(`button mr-1 ${color}`);
            button.attr('data-action', text);
            button.css('order', order);
            var icon = `<span class="${icon} ${ !noText && 'mr-1'}"></span>`;
            button.html(`${icon} ${displayText}`);
            button.appendTo(el);
        },

        _isGranted(permission){
            if(_.isEmpty(permission)) return true;
            var o = this.options;
            var auth = abp.auth;
            var permissionPrefix = o.permissionPrefix;
            if(_.isEmpty(permissionPrefix)) return auth.isGranted(permission);            
            return auth.isGranted(`${permissionPrefix}.${permission}`)
                || auth.isGranted(permission);
        },

        _createEvents: function(){
            var that = this, element = this.element, o = this.options;   
            element.on(Metro.events.click, o.targets, function(){
                var el = $(this);
                var aciton = el.attr('data-action');

                that._fireEvent("button-click", {
                    button: this,
                    action: aciton
                })

            });
        },

        changeAttribute: function(){
        },

        destroy: function(){
            var element = this.element, o = this.options;
            element.off(Metro.events.click, o.targets);
            return element;
        }
    });
}(Metro, m4q));