<template>
    <div class="flex flex-col flex-1">
        <el-form-item :label="t(ApplicationSettings.AccessToken.displayName)" prop="tokenLifetimesSettings.accessToken"
            label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.accessToken" :min="0"> </el-input-number>
        </el-form-item>
        <el-form-item :label="t(ApplicationSettings.AuthorizationCode.displayName)"
            prop="tokenLifetimesSettings.authorizationCode" label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.authorizationCode" :min="0"> </el-input-number>
        </el-form-item>
        <el-form-item :label="t(ApplicationSettings.DeviceCode.displayName)" prop="tokenLifetimesSettings.deviceCode"
            label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.deviceCode" :min="0"> </el-input-number>
        </el-form-item>
        <el-form-item :label="t(ApplicationSettings.IdentityToken.displayName)"
            prop="tokenLifetimesSettings.identityToken" label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.identityToken" :min="0"> </el-input-number>
        </el-form-item>
        <el-form-item :label="t(ApplicationSettings.RefreshToken.displayName)"
            prop="tokenLifetimesSettings.refreshToken" label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.refreshToken" :min="0"> </el-input-number>
        </el-form-item>
        <el-form-item :label="t(ApplicationSettings.UserCode.displayName)" prop="tokenLifetimesSettings.userCode"
            label-width="210">
            <el-input-number v-model="tokenLifetimesSettings.userCode" :min="0"> </el-input-number>
        </el-form-item>
        <el-alert :title="t('OpenIddict.Settings:TokenLifetime.Description')" type="info" :closable="false" />
    </div>
</template>

<script setup lang="ts">
import { reactive,  watch } from 'vue';
import { useI18n } from '~/composables';
import { capitalize, convertSecondsToTimeSpan, parseTimeSpanToSeconds } from '~/libs';
import { ApplicationSettings } from '~/repositories';
const model = defineModel<any>({});

const { t } = useI18n();
const emit = defineEmits(['update:modelValue']);
const tokenLifetimesSettings = reactive({
    accessToken: 0,
    authorizationCode: 0,
    deviceCode: 0,
    identityToken: 0,
    refreshToken: 0,
    userCode: 0,
} as any);

watch(() => model.value, (newValue: any) => {
    tokenLifetimesSettings.accessToken = parseTimeSpanToSeconds(newValue[ApplicationSettings.AccessToken.value] || '00:00:00');
    tokenLifetimesSettings.authorizationCode = parseTimeSpanToSeconds(newValue[ApplicationSettings.AuthorizationCode.value] || '00:00:00');
    tokenLifetimesSettings.deviceCode = parseTimeSpanToSeconds(newValue[ApplicationSettings.DeviceCode.value] || '00:00:00');
    tokenLifetimesSettings.identityToken = parseTimeSpanToSeconds(newValue[ApplicationSettings.IdentityToken.value] || '00:00:00');
    tokenLifetimesSettings.refreshToken = parseTimeSpanToSeconds(newValue[ApplicationSettings.RefreshToken.value] || '00:00:00');
    tokenLifetimesSettings.userCode = parseTimeSpanToSeconds(newValue[ApplicationSettings.UserCode.value] || '00:00:00');
}, { deep: true });

watch(tokenLifetimesSettings, (newValue: any) => {
    const newSettings = { } as any;
    //通过循环遍历newValue，如果值为0，则删除对应的key，否则更新对应的key
    for (const key in newValue) {
        const settingKey = ApplicationSettings[capitalize(key)].value
        if (newValue[key] === 0) {
            delete newSettings[settingKey];
        } else {
            newSettings[settingKey] = convertSecondsToTimeSpan(newValue[key].toString());
        }
    }
    model.value = { ...newSettings };
}, { deep: true });


</script>
