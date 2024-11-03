import { appConfig, LocalStorage, logger } from "~/libs";
import { useConfigStore } from "~/store";
import { storeToRefs } from "pinia";
import { onMounted, ref, watch } from "vue";
import router from "~/router";

export function useAuthentication(isAuthenticatedPage?: boolean) {
    const configStore = useConfigStore();
    const { isAuthenticated, isConfigReady } = storeToRefs(configStore);

    const currentUser = ref<any>(null);

    watch(isConfigReady, (newValue) => {
        logger.debug('[useAuthentication][watch]', 'isAuthenticated.value', isAuthenticated.value, 'isAuthenticatedPage', isAuthenticatedPage, 'isReady.value', isConfigReady.value)        
        currentUser.value = appConfig.currentUser;
        if(newValue && !isAuthenticated && isAuthenticatedPage)
        {
            redirectToLogin();
        }
    })

    const redirectToLogin = () => {
        LocalStorage.setRedirectPath(router.currentRoute.value.path)
        router.push("/login");
    };

    onMounted(() => {
        currentUser.value = appConfig.currentUser;
        if(isConfigReady.value && !isAuthenticated.value && isAuthenticatedPage){
            logger.debug('[useAuthentication][onMounted]', 'isAuthenticated.value', isAuthenticated.value, 'isAuthenticatedPage', isAuthenticatedPage, 'isReady.value', isConfigReady.value)
            redirectToLogin();
        }        
    });

    return {
        isAuthenticated,
        currentUser,
    };
}
