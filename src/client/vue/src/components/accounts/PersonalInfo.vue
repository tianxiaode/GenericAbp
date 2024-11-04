<template>
    <el-form ref="formRef" :rules="rules" :model="formData" size="large" :inline-message="true"
        :validate-on-rule-change="false" :scroll-to-error="true" :label-width="120" require-asterisk-position="right">
        <el-input type="hidden" v-model="formData.concurrencyStamp" />
        <el-form-item prop="userName" :label="t('Pages.Profile.Username')">
            <el-input v-model="formData.userName" clearable autocomplete="off"
                :disabled="!appConfig.isUserNameUpdateEnabled" :placeholder="t('Pages.Profile.Username')">
            </el-input>
        </el-form-item>
        <el-form-item prop="name" :label="t('Pages.Profile.name')">
            <el-input v-model="formData.name" clearable autocomplete="off" :placeholder="t('Pages.Profile.name')">
            </el-input>
        </el-form-item>
        <el-form-item prop="surname" :label="t('Pages.Profile.surname')">
            <el-input v-model="formData.surname" clearable autocomplete="off" :placeholder="t('Pages.Profile.surname')">
            </el-input>
        </el-form-item>
        <el-form-item prop="email" :label="t('Pages.Profile.email')">
            <el-input v-model="formData.email" clearable autocomplete="off" :disable="!appConfig.isEmailUpdateEnabled"
                :placeholder="t('Pages.Profile.email')">
            </el-input>
        </el-form-item>
        <el-form-item prop="phoneNumber" :label="t('Pages.Profile.phoneNumber')">
            <el-input v-model="formData.phoneNumber" clearable autocomplete="off"
                :placeholder="t('Pages.Profile.phoneNumber')">
            </el-input>
        </el-form-item>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t('Components.Save') }}
        </el-button>
        <FormMessage ref="formMessageRef" />
    </el-form>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useConfig, useForm, useFormRules, useI18n } from '~/composables'
import { account, appConfig } from '~/libs';
import FormMessage from '../forms/FormMessage.vue';

const { t } = useI18n();
const formData = ref<any>({});
const formMessageRef = ref<any>();
const formRules = {
    userName: { required: true },
    email: { required: true, email: true }
};

const refreshRules = () => {
    updateRules({
        userName: { required: appConfig.isUserNameUpdateEnabled },
        email: { required: appConfig.isEmailUpdateEnabled },
    });
};

const { } = useConfig(refreshRules);


const onSubmit = async () => {
    try {
        await account.updateProfile(formData.value);
        formMessageRef.value.success('Pages.Profile.UpdateProfileSuccess');
    } catch (error: any) {
        formMessageRef.value.error(error.message);
    }
}

const { formRef, handleSubmit } = useForm(onSubmit);
const { rules, updateRules } = useFormRules(formRules, formRef);

onMounted(() => {
    account.getProfile().then(data => {
        formData.value = data;
    }).catch(error => {
        formMessageRef.value.error(error.message);
    });
});


</script>