<template>
    <el-form-item :label="label" :prop="formItemProp">
        <el-input v-bind="{
            placeholder: label,
            clearable: true,
            autoComplete: 'off',
            showPassword: $attrs.type === 'password',
            ...$attrs
        }" v-model="prop">
        </el-input>
        <PasswordStrength v-if="showPasswordStrength" />
    </el-form-item>

</template>

<script setup lang="ts">
import PasswordStrength from '../accounts/PasswordStrength.vue';

const prop = defineModel();
defineProps({
    label: {
        type: String,
        default: ''
    },
    showPasswordStrength: {
        type: Boolean,
        default: false
    },
    formItemProp:{
        type: String,
        default: ''    }
});
</script>