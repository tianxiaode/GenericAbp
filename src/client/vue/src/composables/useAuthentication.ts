// useAuthentication.ts
import { appConfig, LocalStorage, logger } from "~/libs";
import { useConfigStore } from "~/store";
import { storeToRefs } from "pinia";
import { ref, watch, onMounted } from "vue";
import router from "~/router";

export function useAuthentication(isAuthenticatedPage?: boolean) {
    const configStore = useConfigStore();
    const { isConfigReady } = storeToRefs(configStore);

    const currentUser = ref<any>(null);

    // 计算属性，根据 currentUser 判断是否已认证
    const isAuthenticated = ref(false);

    const updateAuthentication = () => {
        currentUser.value = appConfig.currentUser;
        isAuthenticated.value = currentUser.value?.isAuthenticated || false;
        logger.debug('[useAuthentication][updateAuthentication]', 'currentUser.value', currentUser.value, 'isAuthenticatedPage', isAuthenticatedPage, 'isConfigReady.value', isConfigReady.value);

        if (!isAuthenticated.value && isAuthenticatedPage) {
            if(isConfigReady.value){
                redirectToLogin();
            }
        }
    };

    const redirectToLogin = () => {
        LocalStorage.setRedirectPath(router.currentRoute.value.path);
        router.push("/login");
    };

    onMounted(() => {
        updateAuthentication();

        // 监听 isConfigReady 的变化
        watch(isConfigReady, (newValue) => {
            if (newValue) {
                updateAuthentication();
            }
        });
    });

    return {
        currentUser,
        isAuthenticated
    };
}