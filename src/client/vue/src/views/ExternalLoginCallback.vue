<template>  

</template>

<script setup lang="ts">
import { globalConfig,BaseHttp, http } from '../libs';
import { onMounted } from 'vue';

onMounted(async () => {
    var user = await globalConfig.userManager?.signinRedirectCallback() as any;
    console.log('signinRedirectCallback', user);
    BaseHttp.setToken(user.access_token);

    var needRegistered = await http.get('/need-register');
    if(needRegistered){

    }else{
        
    }
    console.log('needRegistered', needRegistered);

});
</script>