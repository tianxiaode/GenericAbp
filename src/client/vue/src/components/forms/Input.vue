<template>
    <el-form-item :label="label" :prop="prop">
        <el-input v-bind="{
            placeholder: label,
            clearable: true,
            autoComplete: 'off',
            ...$attrs
        }" v-model="prop">
        </el-input>
    </el-form-item>

</template>

<script setup lang="ts">


const prop = defineModel();

defineProps({
    label: {
        type: String,
        default: ''
    }
});
</script>