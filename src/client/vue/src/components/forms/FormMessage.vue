<template>
    <div class="w-full" v-if="formMessage">
        <el-alert :type="formMessageType" show-icon closable @close="clearFormMessage">
            <template #title>
                <span v-html="t(formMessage, messageParams)"> </span>
            </template>
        </el-alert>
    </div>

</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
const { t } = useI18n();

const formMessage = defineModel();
const formMessageType = defineModel('formMessageType');
defineProps({
    messageParams:{
        type: Object,
        default: () => ({})
    }
})
const clearFormMessage = () => {
    formMessage.value = '';
    formMessageType.value = '';    
};

</script>