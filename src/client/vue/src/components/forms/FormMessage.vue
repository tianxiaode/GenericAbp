<template>
    <div class="w-full" v-if="formMessage">
        <el-alert :type="formMessageType" show-icon closable @close="clearFormMessage">
            <template #title>
                <span v-html="t(formMessage)"> </span>
            </template>
        </el-alert>
    </div>

</template>

<script setup lang="ts">
import { useFormMessage, useI18n } from '~/composables';
const { t } = useI18n();
const { formMessage, formMessageType, clearFormMessage, formMessageExposed } = useFormMessage();


defineExpose({
    ...formMessageExposed()
});

</script>