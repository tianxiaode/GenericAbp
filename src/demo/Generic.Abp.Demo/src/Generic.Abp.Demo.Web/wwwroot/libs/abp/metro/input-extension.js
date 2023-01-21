/* global Metro, Cake */
(function(Metro, $) {
    'use strict';
    var Utils = Metro.utils;
    var InputDefaultConfig = {
        inputDeferred: 0,

        label: "",

        autocomplete: null,
        autocompleteUrl: null,
        autocompleteUrlMethod: "GET",
        autocompleteUrlKey: null,
        autocompleteDivider: ",",
        autocompleteListHeight: 200,

        history: false,
        historyPreset: "",
        historyDivider: "|",
        preventSubmit: false,
        defaultValue: "",
        size: "default",
        prepend: "",
        append: "",
        copyInlineStyles: false,
        searchButton: false,
        clearButton: true,
        revealButton: true,
        clearButtonIcon: "<span class='default-icon-cross'></span>",
        revealButtonIcon: "<span class='default-icon-eye'></span>",
        searchButtonIcon: "<span class='default-icon-search'></span>",
        customButtons: [],
        searchButtonClick: 'submit',

        clsComponent: "",
        clsInput: "",
        clsPrepend: "",
        clsAppend: "",
        clsClearButton: "",
        clsRevealButton: "",
        clsCustomButton: "",
        clsSearchButton: "",
        clsLabel: "",

        onAutocompleteSelect: Metro.noop,
        onHistoryChange: Metro.noop,
        onHistoryUp: Metro.noop,
        onHistoryDown: Metro.noop,
        onClearClick: Metro.noop,
        onRevealClick: Metro.noop,
        onSearchButtonClick: Metro.noop,
        onEnterClick: Metro.noop,
        onInputCreate: Metro.noop
    };

    Metro.inputSetup = function (options) {
        InputDefaultConfig = $.extend({}, InputDefaultConfig, options);
    };

    if (typeof window["metroInputSetup"] !== undefined) {
        Metro.inputSetup(window["metroInputSetup"]);
    }

    Metro.Component('inputextension', {
        init: function( options, elem ) {
            this._super(elem, options, InputDefaultConfig, {
                history: [],
                historyIndex: -1,
                autocomplete: []
            });

            return this;
        },

        _create: function(){
            var element = this.element;

            this._createStructure();
            this._createEvents();

            this._fireEvent("input-create", {
                element: element
            });
        },

        _createStructure: function(){
            var me = this, element = me.element, o = me.options;
            var input = element.find('input');
            var buttons = $("<div>").addClass("button-group");
            var clearButton, revealButton, searchButton;

            buttons.appendTo(element);

            if (o.clearButton === true && !input[0].readOnly) {
                clearButton = $("<button>").addClass("button input-clear-button").addClass(o.clsClearButton).attr("tabindex", -1).attr("type", "button").html(o.clearButtonIcon);
                clearButton.appendTo(buttons);
            }
            if (element.attr('type') === 'password' && o.revealButton === true) {
                revealButton = $("<button>").addClass("button input-reveal-button").addClass(o.clsRevealButton).attr("tabindex", -1).attr("type", "button").html(o.revealButtonIcon);
                revealButton.appendTo(buttons);
            }

            if (input.attr('dir') === 'rtl' ) {
                container.addClass("rtl").attr("dir", "rtl");
            }
        },

        _createEvents: function(){
            var me = this, element = me.element, o = me.options;
            var input = element.find("input");

            element.on(Metro.events.click, ".input-clear-button", function(){
                var curr = input.val();
                input.val(Utils.isValue(o.defaultValue) ? o.defaultValue : "").fire('clear').fire('change').fire('keyup').focus();

                me._fireEvent("clear-click", {
                    prev: curr,
                    val: element.val()
                });

            });

            element.on(Metro.events.click, ".input-reveal-button", function(){
                if (input.attr('type') === 'password') {
                    input.attr('type', 'text');
                } else {
                    input.attr('type', 'password');
                }

                that._fireEvent("reveal-click", {
                    val: input.val()
                });

            });

            element.on(Metro.events.stop, ".input-reveal-button", function(){
                element.attr('type', 'password').focus();
            });


            input.on(Metro.events.blur, function(){
                input.removeClass("focused");
            });

            input.on(Metro.events.focus, function(){
                input.addClass("focused");
            });

        },


        clear: function(){
            this.element.val('');
        },

        toDefault: function(){
            this.element.val(Utils.isValue(this.options.defaultValue) ? this.options.defaultValue : "");
        },

        disable: function(){
            this.element.data("disabled", true);
            this.element.parent().addClass("disabled");
        },

        enable: function(){
            this.element.data("disabled", false);
            this.element.parent().removeClass("disabled");
        },

        toggleState: function(){
            if (this.elem.disabled) {
                this.disable();
            } else {
                this.enable();
            }
        },


        changeAttribute: function(attributeName){
            switch (attributeName) {
                case 'disabled': this.toggleState(); break;
            }
        },

        destroy: function(){
            var element = this.element;
            var input = element.find('input');
            var clearBtn = element.find(".input-clear-button");
            var revealBtn = element.find(".input-reveal-button");
            var customBtn = element.find(".input-custom-button");

            if (clearBtn.length > 0) {
                clearBtn.off(Metro.events.click);
            }
            if (revealBtn.length > 0) {
                revealBtn.off(Metro.events.start);
                revealBtn.off(Metro.events.stop);
            }
            if (customBtn.length > 0) {
                clearBtn.off(Metro.events.click);
            }

            input.off(Metro.events.blur);
            input.off(Metro.events.focus);

            return element;
        }
    });

}(Metro, m4q));