<template>
<div class="w-full h-full flex justify-center items-center"> 
    <h4>{{ t('AbpIdentity.') }} </h4>

</div>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';

const {t } = useI18n();
</script>