// src/libs/locales/Locale.ts
import {
    capitalize,
    deepMerge,
    isEmpty,
    logger,
    normalizedLanguage,
} from "../utils";
import { http } from "../http"; // 确保引入 http 工具
import { LocalStorage } from "../LocalStorage";

export interface LocaleConfig extends Record<string, any> {
    languagePacks: {
        [key: string]: Array<Promise<any>>;
    };
    defaultLanguage?: string;
    remoteTextUrl?: string; // 新增字段用于远程加载文本
    remoteLanguageParam?: string; // 新增字段指定语言参数
}

export interface LocalFormat {
    Date: string;
    Time: string; 
    DateTime: string;
    SortDate: string;
    ShortTime:string,
    ShortDateTime:string,
    Year:string,
    Month:string,
    Day:string,
    Hour:string,
    Minute:string,
    Second:string
}


export class Locale {
    $className: string = "Locale";
    translation: Record<string, any> = {};
    private languagePacks: { [key: string]: Array<Promise<any>> } = {};
    language: string = "en";
    defaultLanguage: string = "en";
    remoteTextUrl: string = "";
    remoteLanguageParam: string = "CultureName";
    isLoaded: boolean = false;

    init(config: LocaleConfig) {
        this.languagePacks = config.languagePacks;
        this.defaultLanguage = config.defaultLanguage || "en";
        this.language = this.defaultLanguage;
        if (config.remoteTextUrl) {
            this.remoteTextUrl = config.remoteTextUrl;
            this.remoteLanguageParam =
                config.remoteLanguageParam || "CultureName";
        }
    }

    async loadLanguage(): Promise<void> {
        // 加载语言包和远程文本
        Promise.all([
            this.loadLanguagePacks(), 
            this.loadRemoteText()
        ])
            .then(() => {
                Promise.resolve();
                this.isLoaded = true;
                logger.debug(this, '[loadLanguage]', "Language loaded", this.translation);
            })
            .catch((error: any) => {
                logger.error(this, "Failed to load language", error);
                throw new Error("Failed to load language");
            });
    }

    get(...params: any[]): string {
        let remainingParams: string[] = [];
        let replacements: any = {};

        if(params.length === 0 || isEmpty(params[0])) return "";

        // 如果参数多于2个，且最后一个参数是对象，则将其视为替换值
        if (
            params.length >= 2 &&
            typeof params[params.length - 1] === "object"
        ) {
            replacements = params.pop(); // 自定义替换值
        }

        // 处理剩余参数
        for (const param of params) {
            const paramStr = param.toString();
            //如果paramStr包含冒号，则冒号对冒号之前的.进行拆分，冒号之后的作为remainingParams的一部分，
            //譬如AbpIdentity.DisplayName:Abp.Identity.Password.RequiredLength
            //则拆分为["AbpIdentity","DisplayName:Abp.Identity.Password.RequiredLength"]这样才能正确按路径取值
            if (paramStr.includes(":")) {
                let [path, value] = paramStr.split(":");
                // path要对.进行拆分，且最后一个点后面部分与value一起作为remainingParams的一部分
                const paths = path.split(".");
                value = paths.pop() + ":" + value;
                remainingParams.push(...paths, value);
                continue;
            } 
            paramStr.includes(".")
                ? remainingParams.push(...paramStr.split("."))
                : remainingParams.push(paramStr);
        }

        let value = this.getTranslationByPath(remainingParams);

        return value
            ? this.replaceParams(value, replacements)
            : remainingParams.join(".");
    }

    merge(translation: Record<string, any>): void {
        this.translation = deepMerge(this.translation, translation || {});
    }

    get format(): LocalFormat{
        return this.translation.Format;
    }

    get defaultFormat(): LocalFormat{
        return {
            Date: "yyyy-MM-dd",
            Time: "HH:mm:ss",
            DateTime: "yyyy-MM-dd HH:mm:ss",
            SortDate: "yyyy-MM-dd HH:mm:ss",
            ShortTime: "HH:mm",
            ShortDateTime: "yyyy-MM-dd HH:mm",
            Year: "yyyy",
            Month: "MM",
            Day: "dd",
            Hour: "HH",
            Minute: "mm",
            Second: "ss"
        };
    }

    private async loadLanguagePacks(): Promise<void> {
        const languagePacks = this.languagePacks;
        if(isEmpty(languagePacks)) return;
        const language = normalizedLanguage(LocalStorage.getLanguage());
        const currentLanguagePacks =
            languagePacks[language] ||
            languagePacks[this.defaultLanguage];
        if (!currentLanguagePacks) {
            logger.error(
                this,
                `No language packs found for ${this.language} or default language.`
            );
            return;
        }
        try {
            const packs = await Promise.all(currentLanguagePacks);
            for (const pack of packs) {
                const json = pack.default || pack;
                this.merge(json);
            }
        } catch (error) {
            logger.error(this, "Failed to load language packs", error);
        }
    }


    private async loadRemoteText(): Promise<void> {
        if (isEmpty(this.remoteTextUrl)) return; // 确保有远程网址
        try {
            const response = await http.get(this.remoteTextUrl, {
                [this.remoteLanguageParam]: LocalStorage.getLanguage(), // 使用语言参数
            });
            const resources = response?.resources || {};
            for (const key in resources) {
                const value = resources[key]?.texts;
                if(!value) continue;
                const source = this.translation[key];
                source ? Object.assign(source, value) : this.translation[key] = value;
            }
        } catch (error) {
            logger.error(this, "Failed to load remote text", error);
        }
    }

    getTranslationByPath(keys: string[]): string {
        let current: any = this.translation;
        for (let k of keys) {
            k = capitalize(k);
            if (k in current) {
                current = current[k];
            } else {
                return "";
            }
        }
        return current as string;
    }

    replaceParams(translation: string, params: any): string {
        if (isEmpty(translation)) return "";
        if (isEmpty(params)) return translation;
        for (let key in params) {
            translation = translation.replace(`{${key}}`, params[key]);
        }
        return translation;
    }
}

export const i18n = new Locale();
