<template>
    <el-form label-width="100px" class="w-full">
        <div class="w-full" v-if="isGranted('SettingManagement', 'PasswordPolicy')">
            <h4 class="py-2 m-0 w-full " style="border-bottom: 1px solid #e6e6e6;">
                {{ t('AbpIdentity.Permission:PasswordPolicy') }} 
            </h4>

        </div>
        <div class="w-full" v-if="isGranted('SettingManagement', 'LookupPolicy')">
            <h4 class="py-2 m-0 mt-2 w-full " style="border-bottom: 1px solid #e6e6e6;">
                {{ t('AbpIdentity.Permission:LookupPolicy') }} 

            </h4>

        </div>
    </el-form>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
import { isGranted } from '~/libs';

const { t } = useI18n();
</script>