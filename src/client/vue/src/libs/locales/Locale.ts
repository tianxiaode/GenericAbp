// src/libs/locales/Locale.ts
import { capitalize, deepMerge, isEmpty, logger, normalizedLanguage } from "../utils";
import { http } from "../http"; // 确保引入 http 工具

export interface LocaleConfig extends Record<string, any> {
    languagePacks: {
        [key: string]: Array<Promise<any>>;
    };
    defaultLanguage?: string;
    remoteTextUrl?: string; // 新增字段用于远程加载文本
    remoteLanguageParam?: string; // 新增字段指定语言参数
}

export class Locale {
    $className: string = 'Locale';
    translation: Record<string, any> = {};
    private languagePacks: { [key: string]: Array<Promise<any>>; } = {};
    language: string = 'en';  
    defaultLanguage: string = 'en';
    remoteTextUrl: string = '';
    remoteLanguageParam: string = 'CultureName';
    isLoaded: boolean = false;

    init(config: LocaleConfig){
        this.languagePacks = config.languagePacks;
        this.defaultLanguage = config.defaultLanguage || 'en';
        this.language = this.defaultLanguage;
        if (config.remoteTextUrl) {
            this.remoteTextUrl = config.remoteTextUrl;
            this.remoteLanguageParam = config.remoteLanguageParam || 'CultureName';
        }
        this.loadLanguage();
    }

    async setLanguage(language: string): Promise<void> {
        if (language === this.language && this.isLoaded) return;
        language = normalizedLanguage(language);
        if (!this.languagePacks[language]) {
            throw new Error(`Language pack for ${language} not found.`);
        }
        this.language = language;
        this.loadLanguage();
    }

    async loadLanguage(): Promise<void> {
        // 加载语言包和远程文本
        Promise.all([
            this.loadLanguagePacks(),
            this.loadRemoteText(),
        ]).then(() => {
            Promise.resolve();
            this.isLoaded = true;
        }).catch((error: any) => {
            logger.error(this, 'Failed to load language', error);
            throw new Error("Failed to load language");
            
        });
    }

    get(key: string, resourceName?: string, entity?: string, params: any = undefined): string {

        const translation = this.translation;
        let capitalizeKey = capitalize(key);

        if (capitalizeKey in translation) {
            return this.replaceParams(translation[capitalizeKey], params);
        }

        if (capitalizeKey.includes('.')) {
            const value = this.getTranslationByPath(capitalizeKey);
            if (value) return this.replaceParams(value, params);
        }

        if (!isEmpty(resourceName)) {
            resourceName = capitalize(resourceName!);
            let resourceKey = `${resourceName}.${capitalizeKey}`;
            let value = this.getTranslationByPath(resourceKey);
            if (value && value !== resourceKey) return this.replaceParams(value, params);

            const displayName = "DisplayName:";
            const displayKey = `${resourceName}.${displayName}${capitalizeKey}`;
            value = this.getTranslationByPath(displayKey);
            if (value && value !== displayKey) return this.replaceParams(value, params);

            if (!isEmpty(entity)) {
                entity = capitalize(entity!);
                const entityKey = `${resourceName}.${entity}:${capitalizeKey}`;
                value = this.getTranslationByPath(entityKey);
                if (value && value !== entityKey) return this.replaceParams(value, params);
            }
        }

        return key;
    }

    merge(translation: Record<string, any>): void {
        this.translation = deepMerge(this.translation, translation || {});
    }

    private async loadLanguagePacks(): Promise<void> {
        const languagePacks = this.languagePacks[this.language] || this.languagePacks[this.defaultLanguage];
        if (!languagePacks) {
            logger.error(this, `No language packs found for ${this.language} or default language.`);
            return;
        }
        console.log('languagePacks', languagePacks)
        try {
            const packs = await Promise.all(languagePacks);
            for (const pack of packs) {
                const json = pack.default || pack;
                this.merge(json);
            }
            this.isLoaded = true;
        } catch (error) {
            logger.error(this, 'Failed to load language packs', error);
        }
    }

    private async loadRemoteText(): Promise<void> {
        if (isEmpty(this.remoteTextUrl)) return; // 确保有远程网址
        try {
            const response = await http.get(this.remoteTextUrl, {
                [this.remoteLanguageParam]: this.language, // 使用语言参数
            });
            this.merge(response?.resources || {});
        } catch (error) {
            logger.error(this, 'Failed to load remote text', error);
        }
    }

    getTranslationByPath(key: string): string {
        const keys = key.split('.');
        let current: any = this.translation;
        console.log(current);
        for (let k of keys) {
            k = capitalize(k);
            if (k in current) {
                current = current[k];
            } else {
                return '';
            }
        }
        return current as string;
    }


    replaceParams(translation: string, params: any): string {
        if (isEmpty(translation)) return '';
        if (isEmpty(params)) return translation;
        for (let key in params) {
            const regex = new RegExp(`{${key}}`, 'g');
            translation = translation.replace(regex, params[key]);
        }
        return translation;
    }
}

export const i18n = new Locale();