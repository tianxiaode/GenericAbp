import { onMounted, ref } from "vue";
import { account, ExternalProviderType } from "~/libs";

export function useExternalProviders() {
    const providers = ref<ExternalProviderType[]>([]);

    const loginWithProvider = (provider: ExternalProviderType) => {
        account.loginWithExternalProvider(provider.provider).then((result) => {
            console.log(result);
        });
    };

    onMounted(() => {
        account.getExternalProviders().then((result) => {
            providers.value = result;
            console.log(result);
            providers.value = [
                { provider: "google", displayName: "Google" },
                { provider: "GitHub", displayName: "GitHub" },
            ];
        });
    });

    return {
        providers,
        loginWithProvider,
    };
}
