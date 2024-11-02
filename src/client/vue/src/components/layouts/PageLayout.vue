<template>
    <PageHeader />
    <div class="content w-full h-full">
        <RouterView />
    </div>
    <PageFooter />
</template>


<script setup lang="ts">
import PageFooter from './PageFooter.vue';
import PageHeader from './PageHeader.vue';

</script>

<style lang="scss" scoped>
</style>