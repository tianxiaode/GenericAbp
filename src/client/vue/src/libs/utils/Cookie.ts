/*\
|*|
|*|  :: cookies.js ::
|*|
|*|  A complete cookies reader/writer framework with full unicode support.
|*|
|*|  https://developer.mozilla.org/zh-CN/docs/DOM/document.cookie
|*|
|*|  This framework is released under the GNU Public License, version 3 or later.
|*|  http://www.gnu.org/licenses/gpl-3.0-standalone.html
|*|
|*|  Syntaxes:
|*|
|*|  * docCookies.setItem(name, value[, end[, path[, domain[, secure]]]])
|*|  * docCookies.getItem(name)
|*|  * docCookies.removeItem(name[, path], domain)
|*|  * docCookies.hasItem(name)
|*|  * docCookies.keys()
|*|
\*/


export class Cookie {
    static getItem(name: string) {
        return (
            decodeURIComponent(
                document.cookie.replace(
                    new RegExp(
                        "(?:(?:^|.*;)\\s*" +
                            encodeURIComponent(name).replace(
                                /[-.+*]/g,
                                "\\$&"
                            ) +
                            "\\s*\\=\\s*([^;]*).*$)|^.*$"
                    ),
                    "$1"
                )
            ) || null
        );
    }

    static setItem(
        name: string,
        value: any,
        expires?: number | string | Date,
        path?: string,
        domain?: string,
        secure?: boolean
    ) {
        if (!name || /^(?:expires|max\-age|path|domain|secure)$/i.test(name)) {
            return false;
        }
        var sExpires = "";
        if (expires) {
            switch (expires.constructor) {
                case Number:
                    sExpires =
                        expires === Infinity
                            ? "; expires=Fri, 31 Dec 9999 23:59:59 GMT"
                            : "; max-age=" + expires;
                    break;
                case String:
                    sExpires = "; expires=" + expires;
                    break;
                case Date:
                    sExpires = "; expires=" + (expires as Date).toUTCString();
                    break;
            }
        }
        document.cookie =
            encodeURIComponent(name) +
            "=" +
            encodeURIComponent(value) +
            sExpires +
            (domain ? "; domain=" + domain : "") +
            (path ? "; path=" + path : "") +
            (secure ? "; secure" : "");
        return true;
    }

    static  removeItem(name: string, path?: string, domain?: string) {
        if (!name || !this.hasItem(name)) {
            return false;
        }
        document.cookie =
            encodeURIComponent(name) +
            "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" +
            (domain ? "; domain=" + domain : "") +
            (path ? "; path=" + path : "");
        return true;
    }
    static hasItem(name: string) {
        return new RegExp(
            "(?:^|;\\s*)" +
                encodeURIComponent(name).replace(/[-.+*]/g, "\\$&") +
                "\\s*\\="
        ).test(document.cookie);
    }

    /* optional method: you can safely remove it! */
    static keys() {
        var aKeys = document.cookie
            .replace(
                /((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g,
                ""
            )
            .split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    }
    static clear(){
        var cookies = document.cookie.split("; ");
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }

    static getAll(){
        var cookies = {} as { [key: string]: string };
        var pairs = document.cookie.split("; ");
        for (var i = 0; i < pairs.length; i++) {
            var pair = pairs[i].split("=");
            var key = decodeURIComponent(pair[0]) as string;
            cookies[key] = decodeURIComponent(pair[1]);
        }
        return cookies;
    }

    static key(){
        return this.keys()[0];
    }
};
