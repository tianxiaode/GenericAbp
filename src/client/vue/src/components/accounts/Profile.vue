<template>
    <div class="container mx-auto flex justify-between items-start h-full gap-4 py-2 box-border"
        style="min-height: calc(100vh - 60px);"
    >
        <el-menu default-active="1" @select="handlerMenuSelect">
            <el-menu-item index="1">
                {{ t('Pages.profile.PersonalInfo') }}
            </el-menu-item>
            <el-menu-item index="2">
                {{ t('Pages.profile.ChangePassword') }}
            </el-menu-item>
        </el-menu>
        <div class="flex-1">
            <div class="w-full h-full" v-if="currentIndex == 1">
                <PersonalInfo></PersonalInfo>
            </div>
            <div class="w-full h-full" v-if="currentIndex == 2">
                <ChangePassword></ChangePassword>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import {  ref } from 'vue';
import { useI18n } from '~/composables';
import ChangePassword from './ChangePassword.vue';
import PersonalInfo from './PersonalInfo.vue';

const {t} = useI18n();
const currentIndex= ref(1);

const handlerMenuSelect = (index:number)=>{
    currentIndex.value = index;
}


</script>