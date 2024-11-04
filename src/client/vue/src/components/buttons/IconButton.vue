<template>
    <el-button v-bind="{circle: true, ...$attrs}" :title="t(title)">
        <template #icon>
            <i :class="icon"></i>
        </template>
    </el-button>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';


defineProps({
    title:{
        type: String,
        default: ''
    },
    icon: {
        type: String,
        required: true
    }
});

const {t} = useI18n();

</script>
