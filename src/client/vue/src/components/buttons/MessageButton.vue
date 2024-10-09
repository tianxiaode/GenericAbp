<template>
    <el-popover v-if="message" placement="top" :visible="visible" width="240">
        <template #default>
            <span :class="buttonType" v-html="formattedMessage" ></span>
        </template>
        <template #reference>
            <IconButton :icon="icon" :type="buttonType" circle slot="reference" @mouseenter="visible = true"
                @mouseleave="visible = false" />
        </template>
    </el-popover>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import IconButton from './IconButton.vue';

const visible = ref(false)
const props = defineProps({
    isError: {
        type: Boolean,
        default: false
    },
    message: {
        type: String,
        required: true
    }
});

// 计算属性
const icon = computed(() => {
    return props.isError ? 'x' : 'check';
});

const buttonType = computed(() => {
    return props.isError ? 'danger' : 'success';
});

const formattedMessage = computed(() => {
    return props.message;
});


onMounted(() => {
    if (props.message !== '') {
        visible.value = true;
        setTimeout(() => {
            visible.value = false;
        }, 1000);
    }
});

</script>
