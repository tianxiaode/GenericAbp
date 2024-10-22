<template>
    <el-form ref="formRef" :model="formData" :label-width="300" 
        label-suffix=":" 
        :rules="rules" size="large"
        require-asterisk-position="right"
        :validate-on-rule-change="false" :scroll-to-error="true">
        <el-collapse v-model="activeNames">
            <el-collapse-item name="1">
                <template #title>
                    <div class="title">{{ t('AbpIdentity.PasswordSettings') }} </div>
                </template>
                <div class="flex flex-col gap-2">
                    <el-form-item prop="requiredLength" :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequiredLength')">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequiredLength')"
                            placement="right">
                            <el-input-number v-model="formData.requiredLength" :min="6" :max="100" />
                        </el-tooltip>
                    </el-form-item>
                    <el-form-item prop="requiredUniqueChars" :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.RequiredUniqueChars')">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequiredUniqueChars')"
                            placement="right">
                            <el-input-number v-model="formData.requiredUniqueChars" :min="1" :max="100" />
                        </el-tooltip>
                    </el-form-item>
                    <el-checkbox v-model="formData.requireNonAlphanumeric">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequireNonAlphanumeric')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireNonAlphanumeric') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.requireLowercase">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequireLowercase')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireLowercase') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.requireUppercase">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequireUppercase')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireUppercase') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.requireDigit">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Password.RequireDigit')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Password.RequireDigit') }}
                        </el-tooltip>
                    </el-checkbox>

                </div>
            </el-collapse-item>
            <el-collapse-item name="2">
                <template #title>
                    <div class="title">{{ t('AbpIdentity.PasswordRenewingSettings') }} </div>
                </template>
                <div class="w-full flex flex-col gap-2">
                    <el-checkbox v-model="formData.forceUsersToPeriodicallyChangePassword">
                        <el-tooltip
                            :content="t('AbpIdentity.Description:Abp.Identity.Password.ForceUsersToPeriodicallyChangePassword')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Password.ForceUsersToPeriodicallyChangePassword')
                            }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-form-item prop="passwordChangePeriodDays" :label="t('AbpIdentity.DisplayName:Abp.Identity.Password.PasswordChangePeriodDays')">
                        <el-tooltip
                            :content="t('AbpIdentity.Description:Abp.Identity.Password.PasswordChangePeriodDays')"
                            placement="right">
                            <el-input-number v-model="formData.passwordChangePeriodDays" :min="0" />
                        </el-tooltip>
                    </el-form-item>

                </div>
            </el-collapse-item>
            <el-collapse-item name="3">
                <template #title>
                    <div class="title">{{ t('AbpIdentity.LockoutSettings') }} </div>
                </template>
                <div class="w-full flex flex-col gap-2">
                    <el-checkbox v-model="formData.allowedForNewUsers">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Lockout.AllowedForNewUsers')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.Lockout.AllowedForNewUsers') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-form-item prop="lockoutDuration" :label="t('AbpIdentity.DisplayName:Abp.Identity.Lockout.LockoutDuration')">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Lockout.LockoutDuration')"
                            placement="right">
                            <el-input-number v-model="formData.lockoutDuration" :min="1" />
                        </el-tooltip>
                    </el-form-item>
                    <el-form-item prop="maxFailedAccessAttempts" :label="t('AbpIdentity.DisplayName:Abp.Identity.Lockout.MaxFailedAccessAttempts')">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.Lockout.MaxFailedAccessAttempts')"
                            placement="right">
                            <el-input-number v-model="formData.maxFailedAccessAttempts" :min="1" :max="100" />
                        </el-tooltip>
                    </el-form-item>
                </div>
            </el-collapse-item>
            <el-collapse-item name="4">
                <template #title>
                    <div class="title">{{ t('AbpIdentity.SigninSettings') }} </div>
                </template>
                <div class="w-full flex flex-col gap-2">
                    <el-checkbox v-model="formData.requireConfirmedEmail">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.SignIn.RequireConfirmedEmail')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.SignIn.RequireConfirmedEmail') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.enablePhoneNumberConfirmation">
                        <el-tooltip
                            :content="t('AbpIdentity.Description:Abp.Identity.SignIn.EnablePhoneNumberConfirmation')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.SignIn.EnablePhoneNumberConfirmation') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.requireConfirmedPhoneNumber">
                        <el-tooltip
                            :content="t('AbpIdentity.Description:Abp.Identity.SignIn.RequireConfirmedPhoneNumber')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.SignIn.RequireConfirmedPhoneNumber') }}
                        </el-tooltip>
                    </el-checkbox>
                </div>
            </el-collapse-item>
            <el-collapse-item name="5">
                <template #title>
                    <div class="title">{{ t('AbpIdentity.UserSettings') }} </div>
                </template>
                <div class="w-full flex flex-col gap-2">
                    <el-checkbox v-model="formData.isEmailUpdateEnabled">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.User.IsEmailUpdateEnabled')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.User.IsEmailUpdateEnabled') }}
                        </el-tooltip>
                    </el-checkbox>
                    <el-checkbox v-model="formData.isUserNameUpdateEnabled">
                        <el-tooltip :content="t('AbpIdentity.Description:Abp.Identity.User.IsUserNameUpdateEnabled')"
                            placement="right">
                            {{ t('AbpIdentity.DisplayName:Abp.Identity.User.IsUserNameUpdateEnabled') }}
                        </el-tooltip>
                    </el-checkbox>
                </div>
            </el-collapse-item>
        </el-collapse>
        <div class="flex items-center justify-between gap-4 mt-4">
            <el-button type="primary" @click="handleSubmit">{{ t('Components.Save') }}</el-button>
            <FormMessage ref="formMessage" class="flex-1" />
        </div>

    </el-form>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n, useFormRules, useForm } from '~/composables';
import FormMessage from '../forms/FormMessage.vue';
import { appConfig, LocalStorage, settingManagement } from '~/libs';
const activeNames = ref(['1', '2', '3', '4', '5']);
const { t } = useI18n();

const formMessage = ref<any>(null);


const onSubmit = async () => {
    try {
        await settingManagement.updateIdentitySettings(formData.value);
        formMessage.value.success('Message.SaveSuccess');
        LocalStorage.setRedirectPath('/settings');
        setTimeout(() => {
            appConfig.loadConfig();            
        }, 3000);

    } catch (error: any) {
        formMessage.value.error(error.message);
    }

};

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules({
    requiredLength: { required: true, min: 6, max: 128 },
    requiredUniqueChars: { required: true, min: 1, max: 100 },
    passwordChangePeriodDays: { required: true, min: 0 },
    lockoutDuration: { required: true, min: 0 },
    maxFailedAccessAttempts: { required: true, min: 1, max: 100 },
}, formRef);

onMounted(() => {
    if (settingManagement.canManageIdentitySettings) {
        settingManagement.getIdentitySettings().then((res: any) => {
            formData.value = { ...res };
        });
    }
});

</script>

<style scoped>
.title {
    font-size: 18px;
    font-weight: bold;
    margin-right: 10px;
    color: #333;
}
</style>