<template>
    <el-drawer v-bind="$attrs" :title="drawerTitle" direction="rtl" @open="onOpen">
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item label="ID">{{ data?.id }}</el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Scope:Name')" > {{ data?.name || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Scope:DisplayName')" > {{ data?.displayName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Scope:Description')" > {{ data?.description || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Scope:Resources')" > 
                <List :data="data?.resources"></List>
            </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import { isEmpty } from '~/libs';
import { useI18n, useRepository } from '~/composables';
import { ScopeType } from '~/repositories';
import List from '../lists/List.vue';

const props = defineProps({
    entityId: {
        type: String,
        required: true
    }
})

const drawerTitle = ref('');
const data = ref<ScopeType>(null as any);
const api = useRepository('scope');
const {t} = useI18n();

const onOpen = () => {
    if(isEmpty(props.entityId)) return;
    api.getEntity(props.entityId).then((res:any) => {
        drawerTitle.value = t.value('Components.Details') + ' - ' + res.name;
        data.value = res;
    });
};


</script>

