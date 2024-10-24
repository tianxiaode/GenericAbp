<template>  

</template>

<script setup lang="ts">
import { account,http, LocalStorage } from '../libs';
import { onMounted } from 'vue';

onMounted(async () => {
    var user = await account.userManager?.signinRedirectCallback() as any;
    console.log('signinRedirectCallback', user);
    LocalStorage.setToken(user.access_token);

    var needRegistered = await http.get('/need-register');
    if(needRegistered){

    }else{
        
    }
    console.log('needRegistered', needRegistered);

});
</script>