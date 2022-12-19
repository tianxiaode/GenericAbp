function inherits(ctor, superCtor) {
    if (superCtor) {
        ctor.super_ = superCtor;
        ctor.prototype = Object.create(superCtor.prototype, {
            constructor: {
                value: ctor,
                enumerable: false,
                writable: true,
                configurable: true,
            },
        });
    }
}

window.isFunction =
    // Safari 3.x and 4.x returns 'function' for typeof <NodeList>, hence we need to fall back
    // to using Object.prototype.toString (slower)
    typeof document !== "undefined" &&
    typeof document.getElementsByTagName("body") === "function"
        ? function (value) {
              return !!value && toString.call(value) === "[object Function]";
          }
        : function (value) {
              return !!value && typeof value === "function";
          };

window.capitalize = function (string) {
    if (string) {
        string = string.charAt(0).toUpperCase() + string.substr(1);
    }

    return string || "";
};

window.uncapitalize = function (string) {
    if (string) {
        string = string.charAt(0).toLowerCase() + string.substr(1);
    }

    return string || "";
};
