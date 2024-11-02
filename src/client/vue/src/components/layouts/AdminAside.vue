<template>
    <el-aside :class="asideStore.isExpanded ? 'collapsed' : ''" class="aside">
        <el-menu router :collapse="asideStore.isExpanded" :default-active="$route.path" size="large"
            style="--el-menu-text-color:var(--el-color-info);"
            class="aside-menu"
        >
            <MenuItem v-for="item in menus" :menu="item" :key="item.name" />
            <!-- <el-sub-menu index="4">
                <template #title>
                    <el-icon><i class="fa fa-cogs"></i></el-icon>
                    <span>系统管理</span>                    
                </template>
                <el-menu-item index="/users" v-if="isGranted('AbpIdentity.Users')">
                    <el-icon><i class="fa fa-users"></i></el-icon>
                    <span>用户管理</span>                    
                </el-menu-item>
                <el-menu-item index="/roles" v-if="isGranted('AbpIdentity.Roles')">
                    <el-icon><i class="fa fa-lock"></i></el-icon>
                    <span>角色管理</span>                    
                </el-menu-item>
                <el-menu-item index="/tenants" v-if="isGranted('AbpTenantManagement.Tenants')">
                    <el-icon><i class="fa fa-building-user"></i></el-icon>
                    <span>租户管理</span>
                </el-menu-item>
                <el-sub-menu index="4-1">
                    <template #title>
                        <el-icon><i class="fa fa-user-shield"></i></el-icon>
                        <span>OpenIddict</span>                    
                    </template>
                    <el-menu-item index="/applications">Applications</el-menu-item>
                    <el-menu-item index="/scopes">Scopes</el-menu-item>
                </el-sub-menu>
                <el-menu-item index="/settings" v-if="isGranted('AbpIdentity.Roles')">
                    <el-icon><i class="fa fa-cog"></i></el-icon>
                    <span>设置</span>                    
                </el-menu-item>
                <el-menu-item index="/security-logs" v-if="isGranted('AbpIdentity.SecurityLogs')">
                    <el-icon><i class="fa fa-cog"></i></el-icon>
                    <span>安全日志</span>                    
                </el-menu-item>
                <el-menu-item index="/audit-logs" v-if="isGranted('AuditLogging.AuditLogs')">
                    <el-icon><i class="fa fa-cog"></i></el-icon>
                    <span>审计日志</span>                    
                </el-menu-item>
            </el-sub-menu> -->
        </el-menu>
    </el-aside>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import { onMounted } from 'vue';
import {  useRepository } from '~/composables';
import { MenuType } from '~/repositories';
import { useAsideStore } from '~/store';
import MenuItem from '../menus/MenuItem.vue';
const asideStore = useAsideStore();
const api = useRepository('menu');
const menus = ref<MenuType[]>([]);


onMounted(()=>{
    api.getByGroup('default').then((res:any) => {
        menus.value = res.items;
    })
})
</script>

<style lang="scss" scoped>
.aside {
    width: 250px;
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.aside.collapsed {
    width: 60px;
}

.aside .aside-menu {
    min-height: calc(100vh - 60px);
}
</style>