<template>
    <i v-bind="$attrs" class="text-lg"  :class="value ? 'fa fa-check-square text-primary' : 'fa fa-square text-primary'"></i>
</template>

<script setup lang="ts">
defineProps({
    value: {
        type: Boolean,
        required: true,
        default: false
    }
})
</script>