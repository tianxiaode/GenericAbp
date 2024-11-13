<template>
    <BaseDialog v-bind="formDialogProps" ref="formDialogRef">
        <el-form v-bind="formProps" :rules="rules" :model="formData" ref="formRef">
            <el-tabs v-model="activeTab" class="w-full openiddict-application-form-tabs">
                <el-tab-pane :label="t('OpenIddict.Tabs:Basic')" name="basic">
                    <el-form-item :label="t(getLabel('ApplicationType'))" prop="applicationType">
                        <el-radio-group v-model="formData.applicationType">
                            <el-radio :value="ApplicationTypes.Web.value">
                                {{ t(ApplicationTypes.Web.displayName) }}
                            </el-radio>
                            <el-radio :value="ApplicationTypes.Native.value">
                                {{ t(ApplicationTypes.Native.displayName) }}
                            </el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <Input :label="t(getLabel('ClientId'))" v-model="formData.clientId" form-item-prop="clientId" />
                    <Input :label="t(getLabel('DisplayName'))" v-model="formData.displayName" form-item-prop="displayName" />
                    <Input :label="t(getLabel('ClientUri'))" v-model="formData.clientUri" form-item-prop="ClientUri" />
                    <Input :label="t(getLabel('LogoUri'))" v-model="formData.logoUri" form-item-prop="logoUri" />
                    <el-form-item :label="t('OpenIddict.Application:ClientType')" prop="clientType">
                        <el-radio-group v-model="formData.clientType" @change="onClientTypeChange">
                            <el-radio :value="ApplicationClientTypes.Confidential.value">
                                {{ t(ApplicationClientTypes.Confidential.displayName) }}
                            </el-radio>
                            <el-radio :value="ApplicationClientTypes.Public.value">
                                {{ t(ApplicationClientTypes.Public.displayName) }}
                            </el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <el-form-item :label="t('OpenIddict.Application:ClientSecret')" prop="clientSecret">
                        <el-input v-model="formData.clientSecret"
                            :disabled="formData.clientType === ApplicationClientTypes.Public.value">
                        </el-input>
                    </el-form-item>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:Permissions')" name="permissions"
                    class="flex flex-col h-full"
                >
                    <div class="grid grid-cols-2 gap-2 flex-1">
                        <GrantTypeSelect 
                            v-model="formData" 
                            :is-public="isPublic" 
                            v-model:disabled-redirect-uris="disabledRedirectUris"
                            v-model:consent-options="consentOptions"
                        >

                        </GrantTypeSelect>
                        <fieldset>
                            <legend>{{ t('OpenIddict.CustomGrantTypes') }}</legend>
                            <ValueListInput v-model="formData.customPermissions" 
                                :convert-model="customPermissionConvert"
                                :convert-value="customPermissionValueConvert"
                            ></ValueListInput>
                        </fieldset>                        
                    </div>
                    <el-form-item :label="t('OpenIddict.Application:ConsentType')" prop="consentType" class="mt-2">
                        <el-radio-group v-model="formData.consentType" class="w-full">
                            <div class="grid grid-cols-4 gap-1 w-full">
                                <el-radio 
                                    :value="ApplicationConsentTypes.Explicit.value"
                                    :disabled="!consentOptions.explicit"
                                >
                                    {{ t(ApplicationConsentTypes.Explicit.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :value="ApplicationConsentTypes.External.value"
                                    :disabled="!consentOptions.external"
                                >
                                    {{ t(ApplicationConsentTypes.External.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :value="ApplicationConsentTypes.Implicit.value"
                                    :disabled="!consentOptions.implicit"
                                >
                                    {{ t(ApplicationConsentTypes.Implicit.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :value="ApplicationConsentTypes.Systematic.value"
                                    :disabled="!consentOptions.systematic"
                                >
                                    {{ t(ApplicationConsentTypes.Systematic.displayName) }}
                                </el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:Scopes')" name="scopes">
                    <el-checkbox-group v-model="formData.scopes">
                            <div class="grid grid-cols-2 gap-2">
                                <el-checkbox v-for="scope in Object.values(ApplicationPermissions.Scopes)" 
                                    :value="scope.value">
                                    {{ t(scope.displayName) }}
                            </el-checkbox>
                            <el-checkbox v-for="scope in allScopes" 
                                :value="`scp:${scope}`">
                                {{ scope }}
                            </el-checkbox>
                        </div>
                    </el-checkbox-group>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:RedirectUris')" name="redirectUris"
                    :disabled="disabledRedirectUris">
                        <div class="grid grid-cols-2 gap-2">
                            <fieldset>
                                <legend>{{ t('OpenIddict.Application:RedirectUris') }}</legend>
                                <ValueListInput  :rules="{ url: true }" v-model="formData.redirectUris"
                                    class="w-full h-full">
                                </ValueListInput>
                            </fieldset>
                            <fieldset>
                                <legend>{{ t('OpenIddict.Application:PostLogoutRedirectUris') }}</legend>
                                <ValueListInput  :rules="{ url: true }" v-model="formData.postLogoutRedirectUris"
                                    class="w-full h-full">
                                </ValueListInput>
                            </fieldset>
                        </div>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:Advanced')" name="advanced" class="h-full flex flex-col gap-2">
                    <ApplicationSettingForm v-model="formData.settings" ></ApplicationSettingForm>
                </el-tab-pane>
            </el-tabs>
        </el-form>
    </BaseDialog>
</template>

<script setup lang="ts">
import BaseDialog from '~/components/dialogs/BaseDialog.vue';

import { useFormDialog, useRepository, useFormRules, useI18n } from '~/composables'
import { onMounted,   ref, watch } from 'vue';
import {  AllApplicationPermissionsGrantTypesValue, ApplicationClientTypes, ApplicationConsentTypes, ApplicationPermissions,  ApplicationTypes, customPermissionConvert, customPermissionValueConvert } from '~/repositories';
import ValueListInput from '../../forms/ValueListInput.vue';
import GrantTypeSelect from './GrantTypeSelect.vue';
import ApplicationSettingForm from './ApplicationSettingForm.vue';
import Input from '../../forms/Input.vue';

const activeTab = ref('basic');
const allScopes = ref([] as string[]);
const { t } = useI18n();
const entityId = defineModel('entityId');
const visible = defineModel<boolean>({ default: false });
const api = useRepository('application');
const scopeApi = useRepository('scope');
const disabledRedirectUris = ref(true);
const isPublic = ref(false);
const consentOptions = ref({
    explicit: false,
    implicit: false,
    external: false,
    systematic: false,
});



const formRules = {
    clientId: { required: true },
    displayName: { required: true },
    ClientUri: { url: true },
};


const { formRef,formData, formDialogRef, formDialogProps,formProps, formSetInitValues, getLabel } = useFormDialog(api, entityId,{
    visible,
    dialogProps:{ width: '800px'},
    afterGetData(data: any){
        const permissions = data.permissions || [];
        isPublic.value = data.clientType === ApplicationClientTypes.Public.value;
        disabledRedirectUris.value = !permissions.includes(ApplicationPermissions.GrantTypes.Implicit.value) && !permissions.includes(ApplicationPermissions.GrantTypes.AuthorizationCode.value);
        formSetInitValues({
            customPermissions: permissions.filter((permission: any) => permission.startsWith('gt:') && !AllApplicationPermissionsGrantTypesValue.includes(permission)),
            scopes: permissions.filter((permission: any) => permission.startsWith('scp:')),
            authorizationCode: permissions.includes(ApplicationPermissions.GrantTypes.AuthorizationCode.value),
            password: permissions.includes(ApplicationPermissions.GrantTypes.Password.value),
            clientCredentials: permissions.includes(ApplicationPermissions.GrantTypes.ClientCredentials.value),
            refreshToken: permissions.includes(ApplicationPermissions.GrantTypes.RefreshToken.value),
            deviceCode: permissions.includes(ApplicationPermissions.GrantTypes.DeviceCode.value),
            implicit: permissions.includes(ApplicationPermissions.GrantTypes.Implicit.value),        
        });
    },
    beforeSubmit: async(data: any): Promise<boolean>=>{
        const permissions = [ ...data.customPermissions, ...data.scopes];
        data.authorizationCode && permissions.push(ApplicationPermissions.GrantTypes.AuthorizationCode.value);
        data.password && permissions.push(ApplicationPermissions.GrantTypes.Password.value);
        data.clientCredentials && permissions.push(ApplicationPermissions.GrantTypes.ClientCredentials.value);
        data.refreshToken && permissions.push(ApplicationPermissions.GrantTypes.RefreshToken.value);
        data.deviceCode && permissions.push(ApplicationPermissions.GrantTypes.DeviceCode.value);
        data.implicit && permissions.push(ApplicationPermissions.GrantTypes.Implicit.value);
        data.permissions = permissions;
        return Promise.resolve(true);
    }
});
const { rules, updateRules } = useFormRules(formRules, formRef);

// Authorization Code：支持 Explicit 和 Implicit。
// Implicit：支持 Implicit 和 Systematic。
// Client Credentials：支持 Systematic 和 External。
// Resource Owner Password：支持 Explicit 和 Systematic。
// Device Code：支持 Explicit 和 Systematic。
// Refresh Token：支持 Systematic

watch(()=>({
    authorizationCode:formData.value.authorizationCode,
    implicit:formData.value.implicit,
    password:formData.value.password,
    refreshToken:formData.value.refreshToken,
    deviceCode:formData.value.deviceCode,
    clientCredentials:formData.value.clientCredentials,
}), (value) => {
    consentOptions.value.explicit = value.authorizationCode || value.password || value.deviceCode;
    consentOptions.value.implicit = value.implicit || value.authorizationCode;
    consentOptions.value.external = value.clientCredentials;
    consentOptions.value.systematic = value.implicit || value.clientCredentials || value.password || value.deviceCode || value.refreshToken;
    formData.value.consentType = '';
    disabledRedirectUris.value = !value.implicit && !value.authorizationCode;
},{ deep: true } )

const onClientTypeChange = (value: string) => {
    console.log('onClientTypeChange', value);
    //如果值为Public，则clientSecret不可编辑，且将ClientCredentials和DeviceCode的value移出permissions
    isPublic.value = value === ApplicationClientTypes.Public.value;
    if (isPublic.value) {
        formData.value.deviceCode = false;
        formData.value.clientCredentials = false;
        formData.value.clientSecret = '';
    }
    updateRules('clientSecret', { required: !isPublic.value });
}

onMounted(() => {
    scopeApi.getAll().then((res: any) => {
        allScopes.value = res.items.map((item: any) => item.name);
    })
    if (!entityId.value) {
        formSetInitValues({
            applicationType: ApplicationTypes.Web.value,
            clientType: ApplicationClientTypes.Confidential.value,
            redirectUris: [],
            postLogoutRedirectUris: [],
            settings: {},
            properties: {},
            requirements: [],
            permissions: [],
            customPermissions:[],
            authorizationCode:false,
            implicit:false,
            password:false,
            clientCredentials:false,
            refreshToken:false,
            deviceCode:false,
            scopes: []
        })
        updateRules('clientSecret', { required: true });
    }
});

</script>

<style lang="css">
.openiddict-application-form-tabs {
    .el-tabs__content{
        min-height: 430px;
    }
}
</style>