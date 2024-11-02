<template>
    <el-tabs v-if="settingManagement.canManageEmailingTest" v-model="activeName" type="border-card">
        <el-tab-pane :label="t('AbpSettingManagement.BasicSettings')" name="settings">
            <EmailingSettingForm />
        </el-tab-pane>
        <el-tab-pane :label="t('AbpSettingManagement.MailingTest')" name="test">
            <EmailingTestForm />
        </el-tab-pane>
    </el-tabs>
    <EmailingSettingForm v-if="!settingManagement.canManageEmailingTest" />
</template>


<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import EmailingSettingForm from './EmailingSettingForm.vue';
import EmailingTestForm  from './EmailingTestForm.vue'
import { settingManagement } from '~/libs';
const { t } = useI18n();

const activeName = ref<any>('settings');



</script>