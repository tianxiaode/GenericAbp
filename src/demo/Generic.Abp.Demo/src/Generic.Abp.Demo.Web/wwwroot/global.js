(function ($) {

    console.log(abp.localization)
    abp.localization.localize = function (key, sourceName) {
        if (sourceName === '_') {
            //A convention to suppress the localization
            return key;
        }
        sourceName = sourceName || abp.localization.defaultResourceName;
        if (!sourceName) {
            abp.log.warn('Localization source name is not specified and the defaultResourceName was not defined!');
            return key;
        }
        var source = abp.localization.resources[sourceName];
        if (!source) {
            abp.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }
        var value = source.texts[key];
        if (value == undefined) {
            return key;
        }
        var copiedArguments = Array.prototype.slice.call(arguments, 0);
        copiedArguments.splice(1, 1);
        copiedArguments[0] = value;
        return abp.utils.formatString.apply(this, copiedArguments);
    }


})(jQuery);