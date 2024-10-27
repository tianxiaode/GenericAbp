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
const type = ref('success');
const message = ref<string>('');

const {t} = useI18n();

defineExpose({
    success:(msg: string) => {
        type.value ='success';
        message.value = msg;
        visible.value = true;
    },
    error:(msg: string) => {
        type.value = 'error';
        message.value = msg;
        visible.value = true;
    },
    clear: () => {
        message.value = '';
        visible.value = false;
    }
});

onMounted(() => {
    if (message.value !== '') {
        visible.value = true;
        setTimeout(() => {
            visible.value = false;
        }, 1000);
    }
});

</script>
