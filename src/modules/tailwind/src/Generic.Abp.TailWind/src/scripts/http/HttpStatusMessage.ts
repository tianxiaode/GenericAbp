import { type HttpStatusTextType } from "./HttpStatusText";

export class HttpStatusMessage {
    private httpStatusText: HttpStatusTextType;
    private currentLocale: string;

    constructor(httpStatusText: HttpStatusTextType) {        
        this.httpStatusText = httpStatusText;
        this.currentLocale = "en-US";
    }

    set locale(locale: string) {
        this.currentLocale = locale;
    }

    public get(statusCode: string | number): string {
        const message = this.httpStatusText[this.currentLocale] || this.httpStatusText["en-US"];
        return message[statusCode] || "";
    }
}



