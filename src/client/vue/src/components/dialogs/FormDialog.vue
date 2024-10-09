<template>
    <el-dialog v-bind="$attrs" :before-close="handleBeforeClose" destroy-on-close v-model="internalVisible">
        <slot name="form-items"></slot>
        <template #footer>
            <slot name="dialog-footer">
                <MessageButton v-if="submitMessage" :is-error="isSubmitError" :message="submitMessage"
                    style="order: 100;" circle class="none-border">
                </MessageButton>
                <IconButton icon="undo" @click="resetForm" style="order: 200;" circle class="none-border"></IconButton>
                <span class="flex-grow" style="order: 300;"></span>
                <el-button style="order: 400;" @click="handleBeforeClose">取消</el-button>
                <el-button type="primary" @click="submitForm" style="order: 500;">提交</el-button>
            </slot>
        </template>
    </el-dialog>
</template>

<script setup lang="ts">
import { ref,  watch } from 'vue';
import MessageButton from "../buttons/MessageButton.vue";
import IconButton from "../buttons/IconButton.vue";
import { ElMessageBox } from 'element-plus';
import { isFunction } from '../../libs';

// Define props
const props = defineProps({
    submit: { type: Function, default: () => { } },
    isVisible: { type: Boolean, default: false },
    checkChange: { type: Function, default: false },
    resetForm: { type: Function, default: false },
    validateForm: { type: Function, default: false },
});

const emit = defineEmits(['update:isVisible', 'success', 'failure']);

const isSubmitError = ref(false);
const submitMessage = ref('');
const internalVisible = ref(props.isVisible);

// 用于监听属性变化
watch(() => props.isVisible, (newValue) => {
    internalVisible.value = newValue;
});

// Form submission logic
const submitForm = async () => {
    const isValid = await props.validateForm();
    if (!isValid) return;
    try {
        const response = await props.submit();
        isSubmitError.value = false;
        submitMessage.value = '提交成功，窗口将在 3 秒后关闭';
        emit('success', response);

        setTimeout(() => {
            closeDialog();
        }, 3000);
    } catch (error: any) {
        console.log('Error submitting form:', error);
        isSubmitError.value = true;
        submitMessage.value = error.message;
        emit('failure', error);
    }
};

// Reset the form
const resetForm = () => {
    props.resetForm(); // 触发表单重置
};

// Handle dialog close logic
const handleBeforeClose = (done: any) => {
    const isChange = props.checkChange();
    const isDone = isFunction(done) && done();
    if (!isChange) {
        isDone && done();
        !isDone && closeDialog();
        return;
    };
    ElMessageBox.confirm('表单已改变，确认不保存吗？').then(() => {
        isDone && done(false);
        !isDone && closeDialog();
    }).catch(() => {
        isDone && done();
    });
};

// Close the dialog
const closeDialog = () => {
    internalVisible.value = false; // 本地隐藏
    emit('update:isVisible', false); // 通知父组件更新状态};
    isSubmitError.value = false;
    submitMessage.value = '';
}


</script>
