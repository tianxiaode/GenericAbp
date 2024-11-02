<template v-slot:footer>
    <div class="flex justify-center items-center gap-2" v-if="providers.length > 0" slot="footer">
        <a v-for="provider in providers" href="#" @click="loginWithProvider(provider)" :title="provider.displayName">
            <i class="fab text-2xl" :class="`fa-${provider.provider.toLocaleLowerCase()}`"></i>
        </a>
    </div>
</template>

<script setup lang="ts">
import { account, ExternalProviderType } from "~/libs";
const providers = defineModel<ExternalProviderType[]>({default: []});
const loginWithProvider = (provider: ExternalProviderType) => {
    account.loginWithExternalProvider(provider.provider);
};


</script>