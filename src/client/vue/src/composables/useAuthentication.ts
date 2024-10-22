import { appConfig, LocalStorage, logger } from "~/libs";
import { useConfigStore } from "~/store";
import { storeToRefs } from "pinia";
import { onMounted, watch } from "vue";
import router from "~/router";

export function useAuthentication(isAuthenticatedPage?: boolean) {
    const configStore = useConfigStore();
    const { isAuthenticated, isReady } = storeToRefs(configStore);

    const currentUser = appConfig.currentUser;

    watch(isReady, (newValue) => {
        logger.debug('[useAuthentication][watch]', 'isAuthenticated.value', isAuthenticated.value, 'isAuthenticatedPage', isAuthenticatedPage, 'isReady.value', isReady.value)
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
        if(isReady.value && !isAuthenticated.value && isAuthenticatedPage){
            logger.debug('[useAuthentication][onMounted]', 'isAuthenticated.value', isAuthenticated.value, 'isAuthenticatedPage', isAuthenticatedPage, 'isReady.value', isReady.value)
            redirectToLogin();
        }        
    });

    return {
        isAuthenticated,
        currentUser,
    };
}
