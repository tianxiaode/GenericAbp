<template>
    <component :is="formItemProp ? 'el-form-item' : 'div'"
        v-bind="formItemProp ? { label: label, prop: formItemProp } : null">
        <el-input v-bind="{
            placeholder: label,
            clearable: true,
            autoComplete: 'off',
            showPassword: $attrs.type === 'password',
            ...$attrs
        }" v-model="prop">
        </el-input>
        <PasswordStrength v-if="showPasswordStrength" :value="prop" />
    </component>

</template>

<script setup lang="ts">
import PasswordStrength from '../accounts/PasswordStrength.vue';

const prop = defineModel<any>();
defineProps({
    label: {
        type: String,
        default: ''
    },
    showPasswordStrength: {
        type: Boolean,
        default: false
    },
    formItemProp: {
        type: String,
        default: ''
    }
});
</script>