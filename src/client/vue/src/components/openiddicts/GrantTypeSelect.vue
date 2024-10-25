<template>
    <fieldset>
        <legend>{{ t('OpenIddict.GrantTypes') }}</legend>
        <div class="grid grid-cols-1 gap-1">
            <el-checkbox v-model="isHybridEnabled">
                {{ t('OpenIddict.Permissions:GrantTypes.Hybrid') }}
            </el-checkbox>
            <el-checkbox v-model="isAuthorizationCodeEnabled">
                {{ t(ApplicationPermissions.GrantTypes.AuthorizationCode.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="isImplicitEnabled">
                {{ t(ApplicationPermissions.GrantTypes.Implicit.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="isPasswordEnabled">
                {{ t(ApplicationPermissions.GrantTypes.Password.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="isRefreshTokenEnabled">
                {{ t(ApplicationPermissions.GrantTypes.RefreshToken.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="isClientCredentialsEnabled"
                :disabled="isPublic">
                {{ t(ApplicationPermissions.GrantTypes.ClientCredentials.displayName) }}
            </el-checkbox>
            <el-checkbox v-model="isDeviceCodeEnabled"
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

const props = defineProps({
    modelValue: {
        type: Object,
        default: () => ({
            authorizationCode: false,
            clientCredentials: false,
            deviceCode: false,
            implicit: false,    
            password: false,
            refreshToken: false
        })
    },
    isPublic:{
        type: Boolean,
        default: false
    },    
});

const isAuthorizationCodeEnabled = ref(false);
const isClientCredentialsEnabled = ref(false);
const isDeviceCodeEnabled = ref(false);
const isImplicitEnabled = ref(false);
const isPasswordEnabled = ref(false);
const isRefreshTokenEnabled = ref(false);
const isHybridEnabled = ref(false);

const isAuthorizationCodeUpdate = ref(true);
const isImplicitUpdate = ref(true);

watch(props.modelValue, (value: any) => {
    isAuthorizationCodeEnabled.value = value.authorizationCode;
    isClientCredentialsEnabled.value = value.clientCredentials;
    isDeviceCodeEnabled.value = value.deviceCode;
    isImplicitEnabled.value = value.implicit;
    isPasswordEnabled.value = value.password;
    isRefreshTokenEnabled.value = value.refreshToken;
    // if(value.authorizationCode && value.implicit){
    //     isHybridEnabled.value = true;
    // }
});

watch(isHybridEnabled, (value: boolean) => {
    if(value){
        isAuthorizationCodeUpdate.value = false;
        isImplicitUpdate.value = false;
        isAuthorizationCodeEnabled.value = true;
        isImplicitEnabled.value = true;
        emit('update:modelValue', {
            ...props.modelValue,
             authorizationCode: true,
             implicit: true,
        });
    }
});

watch(isAuthorizationCodeEnabled, (value: boolean) => {
    if(!value){
        isHybridEnabled.value = false;
    }
    if(!isAuthorizationCodeUpdate.value){
        isAuthorizationCodeUpdate.value = true;
        return;
    }
    console.log('update:modelValue.authorizationCode', value);
    emit('update:modelValue', {
        ...props.modelValue,
         authorizationCode: value,
    });
});

watch(isImplicitEnabled, (value: boolean) => {
    console.log('isImplicitEnabled', value);
    if(!value){
        isHybridEnabled.value = false;
    }
    if(!isImplicitUpdate.value){
        isImplicitUpdate.value = true;
        return;
    }
    console.log('update:modelValue.implicit', value);
    emit('update:modelValue', {
        ...props.modelValue,
         implicit: value,
    });
});

watch(isClientCredentialsEnabled, (value: boolean) => {
    emit('update:modelValue', {
        ...props.modelValue,
         clientCredentials: value,
    });
})

watch(isDeviceCodeEnabled, (value: boolean) => {
    emit('update:modelValue', {
        ...props.modelValue,
         deviceCode: value,
    });
})

watch(isPasswordEnabled, (value: boolean) => {
    emit('update:modelValue', {
        ...props.modelValue,
         password: value,
    });
})

watch(isRefreshTokenEnabled, (value: boolean) => {
    emit('update:modelValue', {
        ...props.modelValue,
         refreshToken: value,
    });
})




</script>