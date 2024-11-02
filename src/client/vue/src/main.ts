import { createApp } from "vue";
import { createPinia } from 'pinia';
//import './styles/element/index.scss'
import ElementPlus, { ElMessage } from "element-plus";
import '@fortawesome/fontawesome-free/css/all.css'; 
import 'element-plus/theme-chalk/dark/css-vars.css'
import "element-plus/dist/index.css";
import "~/styles/index.scss";
import "uno.css";

import App from "./App.vue";
import { logger, envConfig,  LocalStorage, i18n, BaseHttp, normalizedLanguage, account, RepositoryGlobalConfig, RepositoryFactory,textToHtml } from "./libs";
import router from "./router"; // 引入 router.ts
import { useLocalizationStore } from "./store";
import { Logger, WebStorageStateStore } from "oidc-client-ts";
import {repositoryRegisters} from "./repositories"
import { useConfirm } from "./composables";

logger.setLevel(process.env.NODE_ENV === "development" ? "debug" : "info");

const { confirm } = useConfirm();

const app = createApp(App);
const pinia = createPinia();
app.use(pinia);

// 初始化 http
BaseHttp.init({
    ...envConfig.defaultHttpConfig,
    errorMessageHandler(error: any) {
        const t = i18n.get.bind(i18n);
        switch (error.status) {
            case 200:
                break;
            case 401: 
                // 未登录
                break;        
            default:
                ElMessage.error(t(error.message));
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


Logger.debug('[main.ts]',"app started");
// 初始化仓库
RepositoryGlobalConfig.init({
    api:{
        prefix: "/api",
    },
    paging:{
        sizes: envConfig.defaultPageSizes || [10, 20, 50, 100],
        pageSizeParamName: "MaxResultCount",
        skipCountParamName: "SkipCount",
    },
    sorting:{
        sortParamName: "Sorting",
    },
    filterParamName: "filter",
    deleteConfirmHandler: async (message: (string | number)[]): Promise<boolean> => {
        try {
            const t = i18n.get.bind(i18n);
            const messageStr = textToHtml(message as string[], 'dot-danger');
            await confirm(t('Message.ConfirmDeleteMessage') + messageStr, t('Message.ConfirmDeleteTitle'));
            return true; // 用户点击"确定"
        } catch {
            return false; // 用户点击"取消"
        }
    },
    messageHandler: (message: string, type: string) => {
        const t = i18n.get.bind(i18n);
        message = t(message);
        switch (type) {
            case 'success':
                ElMessage.success(message);                
                break;
            case 'error':
                ElMessage.error(message);
                break;
            case 'warning':
                ElMessage.warning(message);
                break;
            default:
                ElMessage.info(message);
                break;
        }
    },
});

RepositoryFactory.register(repositoryRegisters);

/// 初始化账户
account.init({
    authority: envConfig.oidcAuthority,
    client_id: envConfig.oidcClientId,
    redirect_uri:envConfig.baseUrl + "signin-oidc",
    response_type: envConfig.oidcResponseType,
    scope: envConfig.oidcScope,
    automaticSilentRenew: true,
    revokeTokensOnSignout: true,
    post_logout_redirect_uri: envConfig.baseUrl + "signout-callback-oidc",
    loadUserInfo: false,
    userStore: new WebStorageStateStore({ store: localStorage }),

}).then(()=>{
    // 初始化本地化
    const localizationStore = useLocalizationStore();
    localizationStore.setLocale(defaultLanguage);
});


// 使用路由
app.use(router);
app.use(ElementPlus);
app.mount("#app");
