import { createApp } from "vue";
import { createPinia } from 'pinia';
import ElementPlus, { ElMessageBox } from "element-plus";
import '@fortawesome/fontawesome-free/css/all.css'; 
import "element-plus/dist/index.css";
import "~/styles/index.scss";
import "uno.css";

import App from "./App.vue";
import { logger, Repository, envConfig,  toast ,LocalStorage, i18n, BaseHttp, normalizedLanguage, account } from "./libs";
import router from "./router"; // 引入 router.ts
import { useLocalizationStore } from "./store";

logger.setLevel(process.env.NODE_ENV === "development" ? "debug" : "info");



const app = createApp(App);
const pinia = createPinia();
app.use(pinia);

// 初始化 http
BaseHttp.init({
    ...envConfig.defaultHttpConfig,
    errorMessageHandler(error: any) {
        logger.debug("http error", error.status, error.message);
        const t = i18n.get.bind(i18n);
        switch (error.status) {
            case 200:
                break;
            case 401: 
                // 未登录
                break;        
            default:
                toast.error(t(error.message, "Http"));
                break;
        }
    }
});

// 初始化 i18n
const defaultLanguage = normalizedLanguage(LocalStorage.getLanguage());

i18n.init({
    languagePacks: {
        en: [import('./libs/locales/en.json'), import('~/locales/en.json')],
        'zh-CN': [import('./libs/locales/zh-CN.json'), import('~/locales/zh-CN.json')],
    },
    defaultLanguage: defaultLanguage,
    remoteTextUrl: '/api/abp/application-localization',
    remoteLanguageParam: 'CultureName',
});

// 初始化本地化
const localizationStore = useLocalizationStore();
localizationStore.setLocale(defaultLanguage);

/// 初始化账户
account.init({
    authority: envConfig.oidcAuthority,
    client_id: envConfig.oidcClientId,
    redirect_uri:envConfig.baseUrl + "/oidc-callback",
    response_type: envConfig.oidcResponseType,
    scope: envConfig.oidcScope,
    automaticSilentRenew: true,
    loadUserInfo: false
});

// 初始化仓库
Repository.initGlobalConfig({
    apiPrefix: "/api",
    pageSizes: envConfig.defaultPageSizes,
    pageSizeParamName: "MaxResultCount",
    deleteConfirmHandler: async (message: string): Promise<boolean> => {
        try {
            const t = i18n.get.bind(i18n);
            await ElMessageBox.confirm(message, t("Message.DeleteConfirm"), {
                confirmButtonText: t("Confirm"),
                cancelButtonText: t("Cancel"),
                type: "warning",
            });
            return true; // 用户点击"确定"
        } catch {
            return false; // 用户点击"取消"
        }
    },
    messageHandler: (message: string, type: string) => {
        const t = i18n.get.bind(i18n);
        message = t(message);
        if(type === "success") toast.success(message);
        else if(type === "error") toast.error(message);
        else if(type === "warning") toast.warning(message);
        else if(type === "info") toast.info(message);
    },
});

// 使用路由
app.use(router);
app.use(ElementPlus);
app.mount("#app");
