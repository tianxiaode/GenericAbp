<template>
    <el-container class="min-height-100vh">
        <el-header class="bg-primary px-0 py-2 box-border h-15">
            <AdminHeader />
        </el-header>
        <el-container>
            <AdminAside />
            <el-main>
                <router-view />
            </el-main>
        </el-container>
    </el-container>
</template>


<script setup lang="ts">
import AdminAside from './AdminAside.vue';
import AdminHeader from './AdminHeader.vue';
</script>