<template>
    <div class="bg-primary pr-5 flex justify-between items-center h-full w-full box-border text-white gap-2">
        <div :class="['logo-wrap', { collapse }]" class="flex pl-4 ">
            <img src='/logo.png' alt="Logo" class="logo-icon cursor-pointer" @click="router.push('/')" />
            <span v-if="!collapse" class="logo-text cursor-pointer overflow-hidden" @click="router.push('/')">
                {{ t("AppShortName") }}
            </span>
        </div>
        <el-icon class="trigger" @click="toggleCollapse" :rotate="collapse ? 'el-icon-s-fold' : 'el-icon-s-unfold'">
            <template v-if="collapse">
                <Expand />
            </template>
            <template v-else>
                <Fold />
            </template>
        </el-icon>
        <div class="spacer"></div>
        <LanguageButton />
        <ThemeSwitch />
        <UserButton />
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { Fold, Expand } from '@element-plus/icons-vue';
import UserButton from '../buttons/UserButton.vue';
import { useI18n } from '~/composables';
import { useAsideStore } from '~/store';
import router from '~/router';
import LanguageButton from '../buttons/LanguageButton.vue';
import ThemeSwitch from '../buttons/ThemeSwitch.vue';
const collapse = ref(false);

const { t } = useI18n();
const asideStore = useAsideStore();
const toggleCollapse = () => {
    collapse.value = !collapse.value;
    asideStore.toggleAside();
};

</script>

<style scoped>
.logo-wrap {
    /* 垂直居中对齐 */
    font-size: 24px;
    font-weight: bold;
    width: 250px;
    color: #fff;
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.logo-icon {
    width: 30px;
    /* 设置图标的宽度 */
    height: 30px;
    /* 设置图标的高度 */
    margin-right: 8px;
    background: transparent;
    /* 图标与文字之间的间距 */
}

.logo-wrap.collapse {
    width: 60px;
}

/* 设置文本的过渡效果 */
.logo-text {
    opacity: 1;
    height: auto;
    overflow: hidden;
    /* 隐藏溢出文本 */
    white-space: nowrap;
    /* 防止换行 */
    transition: opacity 0.3s ease, height 0.3s ease;
    /* 添加不透明度和高度的过渡效果 */
}

.logo-wrap.collapse .logo-text {
    opacity: 0;
    /* 折叠时隐藏文本 */
    height: 0;
    /* 设置高度为0 */
}
</style>