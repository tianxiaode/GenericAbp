import { appConfig } from "~/libs";
import { useConfigStore } from "~/store";
import { storeToRefs } from "pinia";
import { onMounted } from "vue";
import router from "~/router";

export function useAuthentication(isAuthenticatedPage?: boolean) {
    const configStore = useConfigStore();
    const { isAuthenticated } = storeToRefs(configStore);

    const currentUser = appConfig.currentUser;

    onMounted(() => {
        if (isAuthenticatedPage && !isAuthenticated.value) {
            router.push("/login");
        }
    });

    return {
        isAuthenticated,
        currentUser,
    };
}
