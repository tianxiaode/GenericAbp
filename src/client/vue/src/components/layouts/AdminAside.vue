<template>
    <el-aside :class="asideStore.isExpanded ? '' : 'collapsed'" class="aside">
        <el-menu router :collapse="asideStore.isExpanded" :default-active="$route.path" size="large"
            style="--el-menu-text-color:var(--el-color-info);"
        >
            <el-menu-item index="/" >
                <el-icon><i class="fa fa-home"></i></el-icon>
                <span>首页</span>
            </el-menu-item>
            <el-sub-menu index="4">
                <template #title>
                    <el-icon><i class="fa fa-cogs"></i></el-icon>
                    <span>系统管理</span>                    
                </template>
                <el-menu-item index="/users" v-if="isGranted('AbpIdentity', 'Users')">
                    <el-icon><i class="fa fa-users"></i></el-icon>
                    <span>用户管理</span>                    
                </el-menu-item>
                <el-menu-item index="/roles" v-if="isGranted('AbpIdentity', 'Roles')">
                    <el-icon><i class="fa fa-lock"></i></el-icon>
                    <span>角色管理</span>                    
                </el-menu-item>
            </el-sub-menu>
        </el-menu>
    </el-aside>

</template>

<script setup lang="ts">
import { isGranted } from '~/libs';
import { useAsideStore } from '~/store';
const asideStore = useAsideStore();
</script>

<style lang="scss" scoped>
.aside {
    width: 220px;
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.aside .collapsed {
    width: 60px;
}

.aside .el-menu {
    min-height: calc(100vh - 60px);
}
</style>