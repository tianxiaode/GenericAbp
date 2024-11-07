<template>
    <span :class="`ml-2 font-bold ${value < 200 || value >= 300? 'text-danger' : 'text-success'}`">{{ value }}</span>
</template>

<script setup lang="ts">


const { value } = defineProps({
    value: {
        type: Number,
        required: true,
        default: 200
    }
})

</script>
