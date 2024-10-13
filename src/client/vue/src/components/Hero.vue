<template>
    <div class="hero" v-bind="$attrs">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.hero {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
}   
</style>