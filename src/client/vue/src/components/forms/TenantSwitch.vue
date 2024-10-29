<template>
    <div v-if="visible">
        <el-tooltip :content="t('AbpTenantManagement.SwitchToHostMessage')" placement="bottom" trigger="click">
            <el-input v-model="tenant" size="large" :placeholder="t('AbpTenantManagement.DisplayName:TenantName')"
                @keyup.enter="switchTenant">
                <template #append>
                    <span class="cursor-pointer" @click="switchTenant">{{ t('AbpTenantManagement.Switch') }}</span>
                </template>
            </el-input>
        </el-tooltip>
    </div>
    <el-divider />
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n, useRepository } from '~/composables';
import { appConfig, isEmpty, LocalStorage } from '~/libs';
import router from '~/router';

const props = defineProps({
    error: {
        type: Function,
        default: () => ({})
    },
    success: {
        type: Function,
        default: () => ({})
    }
})

const tenant = ref(LocalStorage.getTenant());
const { t } = useI18n();
const api = useRepository('tenant');
const path = router.currentRoute.value.path;
const visible = ref(appConfig.enableMultiTenancy && path !== '/reset-password');
const switchTenant = async () => {
    const value = tenant.value;
    if (isEmpty(value)) {
        props.success('AbpTenantManagement.TenantSwitchSuccess', { name: t.value('AbpTenantManagement.Host') });
        LocalStorage.setTenant('');
        return;
    }
    try {
        const res = await api.getByName(value);
        if (!res.success) {
            props.error('AbpTenantManagement.TenantNotFoundMessage', { name: tenant.value });
            return;
        }
        if (!res.isActive) {
            props.error('AbpTenantManagement.TenantNotActiveMessage', { name: tenant.value });
            return;
        }
        console.log(res.name, res)
        LocalStorage.setTenant(res.name);
        props.success('AbpTenantManagement.TenantSwitchSuccess', { name: res.name });
    } catch (e: any) {
        props.error(e.message);
    }
}


</script>