<template>  
<div v-html="highlightText(text, filter)"></div>
</template>

<script setup lang="ts">
import { highlightText } from '~/libs';

defineProps({
    text: {
        type: String,
        default: ''
    },
    filter: {
        type: String,
        default: ''
    }
});
</script>