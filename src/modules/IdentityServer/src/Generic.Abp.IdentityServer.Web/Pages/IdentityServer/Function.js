function inheritPrototype(subType, superType){
    let prototype = Object(superType.prototype);
    prototype.constructor = subType;
    subType.prototype = prototype;
}

window.isFunction =
        // Safari 3.x and 4.x returns 'function' for typeof <NodeList>, hence we need to fall back
        // to using Object.prototype.toString (slower)
        (typeof document !== 'undefined' &&
         typeof document.getElementsByTagName('body') === 'function')
            ? function(value) {
                return !!value && toString.call(value) === '[object Function]';
            }
            : function(value) {
                return !!value && typeof value === 'function';
            };