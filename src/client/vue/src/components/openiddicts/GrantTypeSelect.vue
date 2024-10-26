<template>
    <fieldset>
        <legend>{{ t('OpenIddict.GrantTypes') }}</legend>
        <div class="grid grid-cols-1 gap-1">
            <el-checkbox v-model="model.authorizationCode">
                {{ t(ApplicationPermissions.GrantTypes.AuthorizationCode.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="model.implicit">
                {{ t(ApplicationPermissions.GrantTypes.Implicit.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="model.password">
                {{ t(ApplicationPermissions.GrantTypes.Password.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="model.refreshToken" :disabled="isRefreshTokenDisabled">
                {{ t(ApplicationPermissions.GrantTypes.RefreshToken.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="model.clientCredentials"
                :disabled="isPublic">
                {{ t(ApplicationPermissions.GrantTypes.ClientCredentials.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="model.deviceCode"
                :disabled="isPublic">
                {{ t(ApplicationPermissions.GrantTypes.DeviceCode.displayName) }}
            </el-checkbox>
        </div>
    </fieldset>
</template>

<script setup lang="ts">
import { ref,watch } from 'vue';
import { useI18n } from '~/composables';
import { ApplicationPermissions } from '~/repositories';

const emit = defineEmits(['update:modelValue']);
const { t} = useI18n();
const model = defineModel<any>();
defineProps({
    isPublic:{
        type: Boolean,
        default: false
    },    
});

watch(model, (value: any) => {
    console.log('model',value)
    if(value.authorizationCode || value.password){
        isRefreshTokenDisabled.value = false;
    }else{
        isRefreshTokenDisabled.value = true;
        value.refreshToken = false;        
    }
}, {deep: true});


const isRefreshTokenDisabled = ref(true);



</script>