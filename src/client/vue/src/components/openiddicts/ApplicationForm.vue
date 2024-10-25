<template>
    <FormDialog ref="formRef" width="900px" :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title="dialogTitle" v-bind="$attrs" :reset="resetForm" :before-close="checkChange" :label-width="160">
        <template #form-items>
            <el-tabs v-model="activeTab" class="w-full application-form-tabs">
                <el-tab-pane :label="t('OpenIddict.Tabs:Basic')" name="basic">
                    <el-form-item :label="t('OpenIddict.Application:ApplicationType')" prop="applicationType">
                        <el-radio-group v-model="formData.applicationType">
                            <el-radio :value="ApplicationTypes.Web.value">
                                {{ t(ApplicationTypes.Web.displayName) }}
                            </el-radio>
                            <el-radio :value="ApplicationTypes.Native.value">
                                {{ t(ApplicationTypes.Native.displayName) }}
                            </el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <el-form-item :label="t('OpenIddict.Application:ClientId')" prop="clientId">
                        <el-input v-model="formData.clientId"></el-input>
                    </el-form-item>
                    <el-form-item :label="t('OpenIddict.Application:DisplayName')" prop="displayName">
                        <el-input v-model="formData.displayName"></el-input>
                    </el-form-item>
                    <el-form-item :label="t('OpenIddict.Application:ClientUri')" prop="ClientUri">
                        <el-input v-model="formData.ClientUri"></el-input>
                    </el-form-item>
                    <el-form-item :label="t('OpenIddict.Application:LogoUri')" prop="logoUri">
                        <el-input v-model="formData.logoUri"></el-input>
                    </el-form-item>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:Permissions')" name="permissions">
                    <div class="grid grid-cols-2 gap-2">
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
                    </div>
                    <div class="grid grid-cols-2 gap-2" style="height: 350px;">
                        <GrantTypeSelect v-model="grantTypes" :is-public="formData.clientType === ApplicationClientTypes.Public.value"></GrantTypeSelect>
                        <fieldset>
                            <legend>{{ t('OpenIddict.CustomGrantTypes') }}</legend>
                            <ValueListInput v-model="formData.permission" 
                                :convert-model="customPermissionConvert"
                                :convert-value="customPermissionValueConvert"
                            ></ValueListInput>
                        </fieldset>                        
                    </div>
                    <el-form-item :label="t('OpenIddict.Application:ConsentType')" prop="consentType" class="mt-2">
                        <!--
                            Authorization Code：支持 Explicit 和 Implicit。
                            Implicit：支持 Implicit 和 Systematic。
                            Client Credentials：支持 Systematic 和 External。
                            Resource Owner Password：支持 Explicit 和 Systematic。
                            Device Code：支持 Explicit 和 Systematic。
                            Refresh Token：支持 Systematic
                        -->
                        <el-radio-group v-model="formData.consentType" class="w-full">
                            <div class="grid grid-cols-4 gap-1 w-full">
                                <el-radio 
                                    :value="ApplicationConsentTypes.Explicit.value"
                                    :disabled="!grantTypes.authorizationCode && !grantTypes.password && !grantTypes.deviceCode"
                                >
                                    {{ t(ApplicationConsentTypes.Explicit.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :value="ApplicationConsentTypes.External.value"
                                    :disabled="!grantTypes.clientCredentials"
                                >
                                    {{ t(ApplicationConsentTypes.External.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :disabled="!grantTypes.authorizationCode && !grantTypes.implicit"
                                    :value="ApplicationConsentTypes.Implicit.value"
                                >
                                    {{ t(ApplicationConsentTypes.Implicit.displayName) }}
                                </el-radio>
                                <el-radio 
                                    :value="ApplicationConsentTypes.Systematic.value"
                                    :disabled="!grantTypes.implicit && !grantTypes.clientCredentials && !grantTypes.password && !grantTypes.deviceCode && !grantTypes.refreshToken"
                                >
                                    {{ t(ApplicationConsentTypes.Systematic.displayName) }}
                                </el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Tabs:Scopes')" name="scopes">
                    <el-checkbox-group v-model="formData.permission">
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
                    :disabled="!grantTypes.authorizationCode && !grantTypes.implicit">
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
                    <ApplicationSettingFieldset v-model="formData.settings" ></ApplicationSettingFieldset>
                    <div class="grid grid-cols-2 gap-2 flex-1">
                        <fieldset>
                            <legend>{{ t('OpenIddict.Application:Properties') }}</legend>
                            <PropertyInput v-model="formData.properties" class="w-full h-full"></PropertyInput>
                        </fieldset>
                        <fieldset>  
                            <legend>{{ t('OpenIddict.Application:Requirements') }}</legend>
                            <ValueListInput v-model="formData.requirements" class="w-full h-full"></ValueListInput>
                        </fieldset>
                    </div>
                </el-tab-pane>
            </el-tabs>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { onMounted,  ref, watch } from 'vue';
import { AllApplicationPermissionsGrantTypesValue, ApplicationClientTypes, ApplicationConsentTypes, ApplicationPermissions, ApplicationSettings, ApplicationTypes, customPermissionConvert, customPermissionValueConvert } from '~/repositories';
import { capitalize } from '~/libs';
import ValueListInput from '../forms/ValueListInput.vue';
import PropertyInput from '../forms/PropertyInput.vue';
import GrantTypeSelect from './GrantTypeSelect.vue';
import ApplicationSettingFieldset from './ApplicationSettingFieldSet.vue';
const activeTab = ref('basic');
const allScopes = ref([] as string[]);
const { t } = useI18n();
const api = useRepository('application');
const scopeApi = useRepository('scope');


const grantTypes = ref({
    authorizationCode: false,
    implicit: false,
    password: false,
    refreshToken: false,
    clientCredentials: false,
    deviceCode: false,
});


const props = defineProps({
    visible: {
        type: Boolean,
        default: false
    },
    entityId: {
        type: String,
        default: ''
    }
});

const formRules = {
    clientId: { required: true },
    displayName: { required: true },
    clientSecret: { required: true },
    consentType: { required: true },
    logoUri: { url: true },
    ClientUri: { url: true },
};


const { formRef, formData, dialogTitle, initValues, resetForm, submitForm, checkChange } = useEntityForm(api, props);
const { rules, updateRules } = useFormRules(formRules, formRef);


watch(grantTypes, (newValue: any) => {
    //通过循环根据grantTypes中各个值，如果为true，则为formData.permissions添加相应的权限值，否则移除
    const oldPermissions = formData.value.permissions.filter((m:string)=> AllApplicationPermissionsGrantTypesValue.includes(m));
    const permissions = [];
    for (const key in newValue) {
        oldPermissions.pop(key)
        if (newValue[key]) {
            permissions.push(ApplicationPermissions.GrantTypes[capitalize(key)].value);
        }
    }
    formData.value.permissions = [...oldPermissions,...permissions];
    formData.value.consentType = null;
    console.log('grantTypes', formData.value.permissions);
});


const onClientTypeChange = (value: string) => {
    //如果值为Public，则clientSecret不可编辑，且将ClientCredentials和DeviceCode的value移出permissions
    if (value === ApplicationClientTypes.Public.value) {
        grantTypes.value.clientCredentials = false;
        grantTypes.value.deviceCode = false;
        formData.value.clientSecret = '';
    }
    updateRules('clientSecret', { required: value === ApplicationClientTypes.Confidential.value });
}

onMounted(() => {
    scopeApi.getAll().then((res: any) => {
        allScopes.value = res.items.map((item: any) => item.name);
    })
    if (!props.entityId) {
        const defaultValue = {
            applicationType: ApplicationTypes.Web.value,
            clientType: ApplicationClientTypes.Confidential.value,
            redirectUris: [],
            postLogoutRedirectUris: [],
            settings: {},
            properties: {},
            requirements: [],
            permissions: []
        }
        formData.value = { ...defaultValue };
        initValues.value = { ...defaultValue };
    }
});

</script>

<style lang="css">
.application-form-tabs {
    .el-tabs__content{
        height: 480px;
    }
}
</style>