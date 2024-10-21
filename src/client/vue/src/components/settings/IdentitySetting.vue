<template>
    <div class="w-full">
        <div class="w-full" v-if="settingManagement.canManagePasswordPolicy">
            <h4 class="py-2 m-0 w-full " style="border-bottom: 1px solid #e6e6e6;">
                {{ t('AbpIdentity.Permission:PasswordPolicy') }} 
            </h4>
            <el-form :model="passwordFormData" :label-width="300" class="flex flex-col gap-2 mt-2" label-suffix=":" 
                :rules="passwordRules">
                <el-form-item :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequiredLength')">
                    <el-input-number v-model="passwordFormData.requiredLength" :min="1" :max="100" />
                </el-form-item>
                <el-form-item :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireNonAlphanumeric')">
                    <Switch v-model="passwordFormData.requireNonAlphanumeric" />
                </el-form-item>
                <el-form-item :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireLowercase')">
                    <Switch v-model="passwordFormData.requireLowercase" />
                </el-form-item>
                <el-form-item :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireUppercase')">
                    <Switch v-model="passwordFormData.requireUppercase" />
                </el-form-item>
                <el-form-item :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireDigit')">
                    <Switch v-model="passwordFormData.requireDigit" />
                </el-form-item>
                <el-button type="primary" @click="savePasswordPolicy">{{ t('Components.Save') }}</el-button>
            </el-form>
        </div>
        <div class="w-full" v-if="settingManagement.canManagePasswordPolicy">
            <h4 class="py-2 m-0 mt-2 w-full " style="border-bottom: 1px solid #e6e6e6;">
                {{ t('AbpIdentity.Permission:LookupPolicy') }} 
                <el-form :model="lookupPolicyFormData" label-width="100px">
            </el-form>

            </h4>

        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from '~/composables';
import { settingManagement } from '~/libs';
import Switch from '../forms/Switch.vue';

const passwordFormData = ref<any>({});
const lookupPolicyFormData = ref<any>({});
const { t } = useI18n();

const passwordRules = {
    requiredLength: { required: true}    
}


const savePasswordPolicy = () => {
    settingManagement.updatePasswordPolicy(passwordFormData.value).then(() => {

    });
};

onMounted(() => {
    if(settingManagement.canManagePasswordPolicy){
        settingManagement.getPasswordPolicy().then(result => {
            passwordFormData.value = result;
        });
    }
    if(settingManagement.canManageLookupPolicy){
        settingManagement.getLookupPolicy().then(result => {
            lookupPolicyFormData.value = result;
        });
    }
});
</script>