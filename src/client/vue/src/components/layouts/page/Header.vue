<template>
    <div class="w-full bg-primary text-white h-14 px-4 py-1 flex justify-between items-center box-border">
        <img :src="logo" alt="logo" class="h-full">
        <div class="flex-1"></div>
        menu
        <div class="flex-1"></div>
        <LanguageButton />
        <UserButton />
    </div>
</template>


<script setup lang="ts">
import LanguageButton from "../../buttons/LanguageButton.vue";
import logo from "../../../assets/logo.png";
import UserButton from "../../../components/buttons/UserButton.vue";


</script>

<style lang="scss" scoped>
</style>