<template>
    <div class="flex justify-center items-center gap-2" v-if="providers.length > 0">
        <a v-for="provider in providers" href="#" @click="loginWithProvider(provider)" :title="provider.displayName">
            <i class="fab text-2xl" :class="`fa-${provider.provider.toLocaleLowerCase()}`"></i>
        </a>
    </div>

</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { account, ExternalProviderType } from "~/libs";
const providers = ref<ExternalProviderType[]>([]);

const loginWithProvider = (provider: ExternalProviderType) => {
    account.loginWithExternalProvider(provider.provider).then((result) => {
        console.log(result);
    });
};

onMounted(() => {
    account.getExternalProviders().then((result) => {
        providers.value = result.items;
        providers.value = [
            { provider: "google", displayName: "Google1" },
            { provider: "GitHub", displayName: "GitHub" },
        ];
    });
});

</script>