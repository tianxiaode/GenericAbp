<template>
<el-switch v-bind="$attrs" :active-text="t(activeText)" :inactive-text="t(inactiveText)"></el-switch>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';

defineProps({
    activeText: {
        type: String,
        default: 'Components.Yes'
    },
    inactiveText: {
        type: String,
        default: 'Components.No'
    },
})
const { t } = useI18n();

</script>