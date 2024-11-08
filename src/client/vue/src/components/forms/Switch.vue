<template>
    <component :is="formItemProp ? 'el-form-item' : 'div'"
        v-bind="formItemProp ? { label: label, prop: formItemProp } : null">
        <el-switch v-bind="$attrs" :active-text="t(activeText)" :inactive-text="t(inactiveText)"></el-switch>
    </component>
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
    label: {
        type: String,
        default: ''
    },
    formItemProp: {
        type: String,
        default: ''
    }
})
const { t } = useI18n();

</script>