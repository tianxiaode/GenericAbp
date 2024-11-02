<template>  

</template>

<script setup lang="ts">
import router from '~/router';
import { account, LocalStorage } from '../libs';
import { onMounted } from 'vue';

onMounted(async () => {
    var user = await account.userManager?.signinRedirectCallback() as any;
    console.log('signinRedirectCallback', user);
    if(!user){
        router.push('/login');
        return;
    }
    LocalStorage.setToken(user?.access_token);
    const redirectPath = LocalStorage.getRedirectPath() || '/';
    window.location.href = redirectPath;

});
</script>