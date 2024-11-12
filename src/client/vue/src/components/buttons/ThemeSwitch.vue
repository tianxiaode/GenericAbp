<template>
    <i v-if="isDark" class="fa fa-sun font-size-6 text-warning cursor-pointer" @click="isDark = false"></i>
    <i v-if="!isDark" class="fa fa-moon font-size-6 text-white cursor-pointer" @click="isDark = true"></i>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
const isDark = ref(document.documentElement.classList.contains('dark'));

//这里要根据value修改html的class，所以需要监听value的变化
watch(isDark, (newValue) => {
    document.documentElement.classList.toggle('dark', newValue);
});
</script>