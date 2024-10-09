import { createApp } from "vue";
import { createPinia } from 'pinia';
import ElementPlus, { ElMessageBox } from "element-plus";
import '@fortawesome/fontawesome-free/css/all.css'; 
import "element-plus/dist/index.css";
import "~/styles/index.scss";
import "uno.css";

import App from "./App.vue";
import { logger, Repository,  globalConfig, i18n } from "./libs";
import router from "./router"; // 引入 router.ts
import { toast } from "./libs/Toast";
import zhCn from "element-plus/es/locale/lang/zh-cn";

logger.setLevel(process.env.NODE_ENV === "development" ? "debug" : "info");



const app = createApp(App);
const pinia = createPinia();
app.use(pinia);



globalConfig.init({
    httpOptions: {
        errorMessageHandler(error: any) {
            logger.debug("http error", error.status, error.message);
            const t = i18n.get.bind(i18n);
            if (error.status === 200) {
            } else {
                toast.error(t(error.message, "Http"));
            }
        },
    },
});

i18n.init({
    languagePacks: {
        en: [import('./libs/locales/en.json')],
        zh: [import('./libs/locales/zh-CN.json')],
    },
    defaultLanguage: 'en',
    remoteTextUrl: '/api/abp/application-localization',
    remoteLanguageParam: 'CultureName',
});

Repository.initGlobalConfig({
    apiPrefix: "/api",
    pageSizes: globalConfig.defaultPageSizes.split(",").map(Number),
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
