<template>
    <FormDialog ref="formRef" width="900px" :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title="dialogTitle" v-bind="$attrs" :reset="resetForm" :before-close="checkChange" :label-width="160">
        <template #form-items>
            <div class="form-container">
                <el-form-item :label="t('OpenIddict.Application:ClientId')" prop="clientId">
                    <el-input v-model="formData.clientId"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ApplicationType')" prop="applicationType">
                    <el-select v-model="formData.applicationType" >
                        <el-option v-for="type in ApplicationTypes" :value="type.value" >
                            {{ t(type.display) }}
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:DisplayName')" prop="displayName">
                    <el-input v-model="formData.displayName"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ClientType')" prop="clientType">
                    <el-select v-model="formData.clientType" @change="onClientTypeChange">
                        <el-option v-for="type in ApplicationClientTypes" :value="type.value">
                            {{ t(type.display) }}
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ClientSecret')" prop="clientSecret">
                    <el-input v-model="formData.clientSecret" 
                        :disabled="formData.clientType === ApplicationClientTypes[1].value" >
                    </el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ConsentType')" prop="consentType">
                    <el-select v-model="formData.consentType">
                        <el-option v-for="type in ApplicationConsentTypes" :value="type.value">
                            {{ t(type.display) }}
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ClientUri')" prop="ClientUri">
                    <el-input v-model="formData.ClientUri"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:LogoUri')" prop="logoUri">
                    <el-input v-model="formData.logoUri"></el-input>
                </el-form-item>
            </div>
            <div class="w-full h-80">
                <el-tabs v-model="activeTab" class="h-full w-full">
                    <el-tab-pane :label="t('OpenIddict.Application:Permissions')" name="permissions">
                        <div class="grid grid-cols-2 gap-1 overflow-auto h-full">
                            <el-checkbox-group v-model="formData.permissions">
                                <div class="flex flex-col">
                                    <el-checkbox
                                        v-for="permission in ApplicationPermissions.filter(p => p.type === 'endpoint')"
                                        :value="permission.value">
                                        {{ t(permission.display) }}
                                    </el-checkbox>
                                </div>
                            </el-checkbox-group>
                            <el-checkbox-group v-model="formData.permissions">
                                <div class="flex flex-col">
                                    <el-checkbox
                                        v-for="permission in ApplicationPermissions.filter(p => p.type === 'grant_type')"
                                        :value="permission.value">
                                        {{ t(permission.display) }}
                                    </el-checkbox>
                                </div>
                            </el-checkbox-group>
                        </div>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Scopes')" name="scopes">
                        <el-checkbox-group v-model="formData.permissions">
                            <div class="grid grid-cols-2 gap-1 overflow-auto">
                                <el-checkbox v-for="scope in ApplicationDefaultScopes" :value="scope.value">
                                    {{ t(scope.display) }}
                                </el-checkbox>
                                <el-checkbox v-for="scope in allScopes" :value="`scp:${scope}`">
                                    {{ capitalize(scope) }}
                                </el-checkbox>

                            </div>
                        </el-checkbox-group>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Application:RedirectUris')" name="redirectUris" class="h-full">
                        <ValueListInput  :rules="{ url: true }" v-model="formData.redirectUris"
                            class="flex-1">
                        </ValueListInput>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Application:PostLogoutRedirectUris')" class="h-full"
                        name="postLogoutRedirectUris">
                        <ValueListInput :rules="{ url: true }" v-model="formData.postLogoutRedirectUris"
                            class="flex-1"></ValueListInput>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Application:Settings')" name="settings" class="h-full">
                        <ApplicationSettingsInput v-model="formData.settings" class="h-full"></ApplicationSettingsInput>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Application:Properties')" name="properties" class="h-full">
                        <PropertyInput v-model="formData.properties" class="h-full"></PropertyInput>
                    </el-tab-pane>
                    <el-tab-pane :label="t('OpenIddict.Application:Requirements')" name="requirements" class="h-full">
                        <ValueListInput v-model="formData.requirements" class="h-full"></ValueListInput>
                    </el-tab-pane>
                </el-tabs>
            </div>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { onMounted, ref } from 'vue';
import { ApplicationClientTypes, ApplicationConsentTypes, ApplicationDefaultScopes, ApplicationPermissions, ApplicationTypes } from '~/repositories';
import { capitalize } from '~/libs';
import ValueListInput from '../forms/ValueListInput.vue';
import PropertyInput from '../forms/PropertyInput.vue';
import ApplicationSettingsInput from './ApplicationSettingsInput.vue'

const activeTab = ref('permissions');
const allScopes = ref([] as string[]);
const { t } = useI18n();
const api = useRepository('application');
const scopeApi = useRepository('scope');
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
};


const { formRef, formData, dialogTitle, initValues, resetForm, submitForm, checkChange } = useEntityForm(api, props);
const { rules, updateRules } = useFormRules(formRules, formRef);

const onClientTypeChange = (value: string) => {
    if (value === ApplicationClientTypes[1].value) {
        console.log('client secret disabled', value);
        formData.value.clientSecret = '';
    }
    updateRules('clientSecret', { required: value === ApplicationClientTypes[0].value })
}

onMounted(() => {
    scopeApi.getAll().then((res: any) => {
        allScopes.value = res.items.map((item: any) => item.name);
    })
    if (!props.entityId) {
        const defaultValue = {
            applicationType: ApplicationTypes[1].value,
            clientType: ApplicationClientTypes[1].value,
            consentType: ApplicationConsentTypes[2].value,
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

<style lang="css" scoped>
.form-container {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
}

.form-container .el-form-item {
    flex: 1 1 45%;
    /* 每个表单项占45%的宽度 */
    /* 右侧间距 */
}

/* 如果需要将最后一列的右侧空白去掉 */
.form-container .el-form-item:nth-child(odd) {
    margin-right: 0;
}
</style>