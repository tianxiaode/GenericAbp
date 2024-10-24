<template>
    <el-descriptions v-bind="$attrs" :column="1" border class="h-full overflow-auto" size="small">
        <template #title>
            <div class="w-full">
                <span class="text-xs text-gray-500">{{ t('OpenIddict.Settings:TokenLifetime.Description') }} </span>
            </div>
        </template>
        <el-descriptions-item v-for="setting in ApplicationSettings" size="default" :label="t(setting.display)"
            class-name="w-1/2">
            <el-input-number v-model="settingValues[setting.field!]" size="small">

            </el-input-number>
        </el-descriptions-item>
    </el-descriptions>
</template>

<script setup lang="ts">
import { ref, PropType, watch } from 'vue';
import { useI18n } from '~/composables';
import { isEmpty } from '~/libs';
import { ApplicationSettings, SelectionOption } from '~/repositories';

const props = defineProps({
    modelValue: {
        type: Object as PropType<any>,
        default: () => { },
    }
})

const emit = defineEmits(['update:modelValue']);
const { t } = useI18n();
const settingValues = ref<any>({
    accessToken: null,
    authorizationCode: null,
    deviceCode: null,
    identityToken: null,
    refreshToken: null,
    userCode: null,
})

watch(settingValues.value, (newValue) => {
    const updateValue = {} as any;

    ApplicationSettings.forEach((setting: SelectionOption) => {
        let value = newValue[setting.field!];
        if(!isEmpty(value) && value >=0){
            updateValue[setting.value!] = newValue[setting.field!];
        }
    });

    console.log('settingValues.value',updateValue);
    

    emit('update:modelValue', updateValue);
});


watch(() => props.modelValue, (newValue) => {
    if (newValue) {
        const updateValue = {} as any;

        ApplicationSettings.forEach((setting: SelectionOption) => {
            updateValue[setting.field!] = newValue[setting.value!];
        });
        settingValues.value = { ...updateValue }
    } else {
        settingValues.value = {
            accessToken: null,
            authorizationCode: null,
            deviceCode: null,
            identityToken: null,
            refreshToken: null,
            userCode: null,
        }
    }
})
</script>