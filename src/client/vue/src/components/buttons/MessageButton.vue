<template>
    <el-popover v-if="message" placement="top" :visible="visible" width="240">
        <template #default>
            <span :class="type === 'error'? 'danger' :'success'" v-html="t(message)" ></span>
        </template>
        <template #reference>
            <i v-if="type !== 'error'" class="fa fa-check success" @mouseenter="visible = true"></i>
            <i v-if ="type === 'error'" class="fa fa-x danger" @mouseenter="visible = true"></i>
        </template>
    </el-popover>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n } from '~/composables';

const visible = ref(false)
const props = defineProps({
    type: String,
    message: String
});

const {t} = useI18n();

onMounted(() => {
    if (props.message !== '') {
        visible.value = true;
        setTimeout(() => {
            visible.value = false;
        }, 1000);
    }
});

</script>
