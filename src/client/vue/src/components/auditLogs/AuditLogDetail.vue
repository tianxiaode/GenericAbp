<template>
    <el-drawer v-bind="$attrs" :title="drawerTitle" direction="rtl" @open="onOpen" size="50%">
        <el-descriptions :column="1" size="large" border class="audit-log-detail">
            <el-descriptions-item label="ID">{{ data?.id }}</el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.ExecutionTime')" > {{ formatDate(data?.executionTime, 'yyyy-MM-dd HH:mm:ss') || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.ExecutionDuration')" > {{ data?.executionDuration || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.ApplicationName')" > {{ data?.applicationName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.Url')" > {{ data?.url || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.HttpMethod')" > {{ data?.httpMethod || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.HttpStatusCode')" > {{ data?.httpStatusCode || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:UserId')" > {{ data?.userId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.UserName')" > {{ data?.userName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:TenantId')" > {{ data?.tenantId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:TenantName')" > {{ data?.tenantName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.CorrelationId')" > {{ data?.correlationId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:ImpersonatorUserId')" > {{ data?.impersonatorUserId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:ImpersonatorUserName')" > {{ data?.impersonatorUserName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:ImpersonatorTenantId')" > {{ data?.impersonatorTenantId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:ImpersonatorTenantName')" > {{ data?.impersonatorTenantName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.AuditLog:ClientId')" > {{ data?.clientId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.ClientName')" > {{ data?.clientName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.BrowserInfo')" > {{ data?.browserInfo || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.Exceptions')" > 
                <span v-if="isEmpty(data?.exceptions)">-</span>
                <JsonPretty v-if="!isEmpty(data?.exceptions)" :data="JSON.parse(data?.exceptions)"></JsonPretty> 
            </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.Comments')" > {{ data?.comments || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('AbpAuditLogging.Actions')" > <JsonPretty :data="data?.actions"></JsonPretty> </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import { formatDate, isEmpty, textToHtml } from '~/libs';
import { useI18n, useRepository } from '~/composables';
import { AuditLogType } from '~/repositories';
import JsonPretty from 'vue-json-pretty';
import 'vue-json-pretty/lib/styles.css';
const props = defineProps({
    entityId: {
        type: String,
        required: true
    }
})

const drawerTitle = ref('');
const data = ref<AuditLogType>(null as any);
const api = useRepository('auditLog');
const {t} = useI18n();

const onOpen = () => {
    if(isEmpty(props.entityId)) return;
    api.getEntity(props.entityId).then((res:any) => {
        drawerTitle.value = t.value('Components.Details') + ' - ' + res.url;
        data.value = res;
    });
};


</script>

<style lang="scss">
    .audit-log-detail{
        .el-descriptions__label{
            width: 250px;
        }
    }
</style>