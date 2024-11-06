<template>  

</template>

<script setup lang="ts">
import router from '~/router';
import { account, LocalStorage, logger } from '../libs';
import { onMounted } from 'vue';

onMounted(async () => {
    //第三方登录，先清理rememberMe状态，并将useStore设置为sessionStorage
    LocalStorage.removeRememberMe();
    account.resetUserStore(false);

    var user = await account.signinRedirectCallback() as any;
    logger.debug('[SigninOidc.vue][onMounted]', 'signinRedirectCallback', user)
    if(!user){
        router.push('/login');
        return;
    }
    const redirectPath = LocalStorage.getRedirectPath() || '/';
    window.location.href = redirectPath;

});
</script>