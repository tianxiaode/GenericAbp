<<template>
    <el-drawer v-bind="$attrs" :title="drawerTitle" direction="rtl" @open="onOpen">
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item label="ID">{{ data?.id }}</el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:ClientId')" > {{ data?.clientId || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:DisplayName')" > {{ data?.displayName || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:ApplicationType')" > {{ data?.applicationType || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:ClientType')" > {{ data?.clientType || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:ConsentType')" > {{ data?.consentType || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:ClientSecret')" > {{ data?.clientSecret || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:LogoUri')" > {{ data?.logoUri || '-' }} </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:Permissions')" >
                <List :data="data?.permissions"></List>
            </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:RedirectUris')" > 
                <List :data="data?.redirectUris"></List>
            </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:PostLogoutRedirectUris')" > 
                <List :data="data?.postLogoutRedirectUris"></List>
            </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:Settings')" > 
                <PropertyList :data="data?.settings" class="list-none"></PropertyList>
            </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:Properties')" > 
                <PropertyList :data="data?.properties" class="list-none"></PropertyList>
            </el-descriptions-item>
            <el-descriptions-item :label="t('OpenIddict.Application:Requirements')" > 
                <List :data="data?.requirements"></List>
            </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import { isEmpty, textToHtml } from '~/libs';
import { useI18n, useRepository } from '~/composables';
import { ApplicationType } from '~/repositories';
import List from '../lists/List.vue';
import PropertyList from '../lists/PropertyList.vue';

const props = defineProps({
    entityId: {
        type: String,
        required: true
    }
})

const drawerTitle = ref('');
const data = ref<ApplicationType>(null as any);
const api = useRepository('application');
const {t} = useI18n();

const onOpen = () => {
    if(isEmpty(props.entityId)) return;
    api.getEntity(props.entityId).then((res:any) => {
        drawerTitle.value = t.value('Components.Details') + ' - ' + res.displayName;
        data.value = res;
    });
};


</script>

