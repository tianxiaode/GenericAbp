<template>
    <div class="logo">
        <a href="/" ><img :src="logo" alt="logo" class="h-full"></a>
    </div>
</template>

<script setup lang="ts">
import logo from "~/assets/logo.png";
</script>

<style lang="scss" scoped>
.logo {
    height: 50px;    
}
</style>