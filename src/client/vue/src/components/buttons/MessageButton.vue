<template>
    <el-popover v-if="message" placement="top" :visible="visible" width="240">
        <template #default>
            <span :class="type === 'error'? 'text-danger' :'text-success'" v-html="t(message)" ></span>
        </template>
        <template #reference>
            <i v-if="type !== 'error'" class="fa fa-check text-success" @mouseenter="autoClose"></i>
            <i v-if ="type === 'error'" class="fa fa-x text-danger" @mouseenter="autoClose"></i>
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
        autoClose();
    },
    error:(msg: string) => {
        type.value = 'error';
        message.value = msg;
        autoClose();
    },
    clear: () => {
        if(message){
            message.value = '';
        }
        visible.value = false;
    }
});

const autoClose = () => {
    visible.value = true;
    setTimeout(() => {
        visible.value = false;
    },2000);
}

onMounted(() => {
    if (message.value !== '') {
        visible.value = true;
        setTimeout(() => {
            visible.value = false;
        }, 1000);
    }
});

</script>
