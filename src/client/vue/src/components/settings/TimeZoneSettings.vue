<template>
    <el-form ref="formRef" :model="formData" :label-width="300" label-suffix=":" size="large"
        :validate-on-rule-change="false" :scroll-to-error="true" require-asterisk-position="right">
        <el-form-item prop="timeZone" :label="t('AbpTiming.DisplayName:Abp.Timing.Timezone')">
            <el-select v-model="formData.timeZone"  filterable>
                <el-option v-for="timeZone in timeZones" :label="timeZone.name" :value="timeZone.value">
                    {{ timeZone.name }}
                </el-option>

            </el-select>
        </el-form-item>
        <div class="flex items-center justify-between gap-4 mt-4">
            <el-button type="primary" @click="handleSubmit">{{ t('Components.Save') }}</el-button>
            <FormMessage ref="formMessage" class="flex-1" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n,  useForm } from '~/composables';
import FormMessage from '../forms/FormMessage.vue';
import { settingManagement } from '~/libs';
const { t } = useI18n();

const formMessage = ref<any>(null);


const onSubmit = async () => {
    try {
        await settingManagement.updateTimeZone(formData.value.timeZone);
        formMessage.value.success('Message.SaveSuccess');
    } catch (error: any) {
        formMessage.value.error(error.message);
    }

};

const { formRef, formData, handleSubmit } = useForm(onSubmit);

const timeZones = ref<any>([]);

onMounted(() => {
    if (settingManagement.canManageTimeZone) {
        settingManagement.getTimeZone().then(value => {
            formData.value.timeZone = value;
        });
        settingManagement.getTimeZones().then(res => {
            timeZones.value = res;
        });
    }
});
</script>