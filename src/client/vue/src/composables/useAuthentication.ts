import { appConfig } from "~/libs";
import { useConfigStore } from "~/store";
import { storeToRefs } from "pinia";

export function useAuthentication() {
    const configStore = useConfigStore();
    const { isAuthenticated } = storeToRefs(configStore);

    const currentUser = appConfig.currentUser;

    return {
        isAuthenticated,
        currentUser
    };
}
