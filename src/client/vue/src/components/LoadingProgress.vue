<template>
    <div id="loading-progress" class="loading-progress"></div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';

onMounted(() => {
    const progressBar = document.getElementById('loading-progress');
    let width = 1;
    let id = setInterval(frame, 10);

    function frame() {
        if (width >= 100) {
            clearInterval(id);
            setTimeout(() => {
                progressBar!.style.display = 'none'; // 加载完成后隐藏进度条
            }, 500); // 延迟隐藏，可视情况调整
        } else {
            width++;
            progressBar!.style.width = width + '%';
        }
    }
});
</script>

<style lang="scss">
.loading-progress {
    height: 3px;
    background-color: var(--el-color-success);
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    z-index: 9999;
}
</style>
