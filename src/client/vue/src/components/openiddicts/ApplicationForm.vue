<template>
    <FormDialog ref="formRef" width="800px" :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title="dialogTitle" v-bind="$attrs" :reset="resetForm" :before-close="checkChange" :label-width="160">
        <template #form-items>
            <div class="form-container">
                <el-form-item :label="t('OpenIddict.Application:ClientId')" prop="clientId">
                    <el-input v-model="formData.clientId"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ClientSecret')" prop="clientSecret">
                    <el-input v-model="formData.clientSecret"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:DisplayName')" prop="displayName">
                    <el-input v-model="formData.displayName"></el-input>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ApplicationType')" prop="applicationType">
                    <el-select v-model="formData.applicationType">
                        <el-option v-for="type in ApplicationTypes" :value="type.value">
                            {{ t(type.display) }}
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item :label="t('OpenIddict.Application:ClientType')" prop="clientType">
                    <el-select v-model="formData.clientType">
                        <el-option v-for="type in ApplicationClientTypes" :value="type.value">
                            {{ t(type.display) }}
                        </el-option>
                    </el-select>
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
            <el-tabs v-model="activeTab">
                <el-tab-pane :label="t('OpenIddict.Application:RedirectUris')" name="redirectUris">

                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Application:PostLogoutRedirectUris')" name="postLogoutRedirectUris">

                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Application:Permissions')" name="permissions">
                        <div class="grid grid-cols-2 gap-2">
                            <el-checkbox-group v-model="formData.permissions">
                                <div class="flex flex-col">
                                    <el-checkbox 
                                    v-for="permission in ApplicationPermissions.filter(p => p.type === 'endpoint')" 
                                    :value="permission.value">{{ t(permission.display) }}</el-checkbox>
                                </div>
                            </el-checkbox-group>
                            <el-checkbox-group v-model="formData.permissions">
                                <div class="flex flex-col">
                                    <el-checkbox 
                                    v-for="permission in ApplicationPermissions.filter(p => p.type === 'grant_type')" 
                                    :value="permission.value">{{ t(permission.display) }}</el-checkbox>
                                </div>                                
                            </el-checkbox-group>
                        </div>
                </el-tab-pane>
                <el-tab-pane :label="t('OpenIddict.Scopes')" name="scopes">

</el-tab-pane>
</el-tabs>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import PasswordStrength from '../accounts/PasswordStrength.vue';

import { useConfig, useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { appConfig } from '~/libs';
import { onMounted, ref } from 'vue';
import Switch from '../forms/Switch.vue';
import { ApplicationClientTypes, ApplicationConsentTypes, ApplicationPermissions, ApplicationTypes } from '~/repositories';
import { el } from 'element-plus/es/locales.mjs';

const activeTab = ref('basic');
const allRoles = ref([] as string[]);
const { t } = useI18n();
const api = useRepository('application');
const applicationTypes = ref([] as string[]);
const consentTypes = ref([] as string[]);
const allowedManageRoles = api.canManageRoles;
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
    clientId: {required: true}
};


const { formRef, formData, dialogTitle, initValues, resetForm, submitForm, checkChange } = useEntityForm(api, props);
const { rules } = useFormRules(formRules, formRef);



onMounted(() => {
    if (props.entityId) {
        api.getEntity(props.entityId).then((res: any) => {
            formRef.value.se
        })
    } else {
        const defaultValue = {
            applicationType: ApplicationTypes[1].value,
            clientType: ApplicationClientTypes[1].value,
            consentType: ApplicationConsentTypes[2].value
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