<template>
    <div class="hero">
        {{ t('Home') }}

        <PropertyInput :list="[
            { label: 'en', field: 'en' },
            { label: 'zh-Hans', field: 'zh-Hans' },
            { label: 'zh-Hant',  field: 'zh-Hant' }
        ]" v-model="model1">
        </PropertyInput>
        <PropertyInput :list="[
            { label: 'name',  type:'input' ,  group: 'en', field: 'name' },
            { label: 'description',  type:'input' ,  group: 'en', field: 'description', props: { type: 'textarea', rows:3 } },
            { label: 'name',  type:'input' ,  group: 'zh-Hans', field: 'name' },
            { label: 'description',  type:'input' ,  group: 'zh-Hans', field: 'description', props: { type: 'textarea', rows:3 } },
            { label: 'name',  type:'input' ,  group: 'zh-Hant', field: 'name' },
            { label: 'description',  type:'input' ,  group: 'zh-Hant', field: 'description', props: { type: 'textarea', rows:3 } },
        ]" v-model="model2">
        </PropertyInput>
    </div>
</template>

<script setup lang="ts">
import ValueListInput from '~/components/forms/ValueListInput.vue';
import { useI18n } from '../composables'
import { onMounted, ref, watch,reactive } from 'vue';
import PropertyInput from '~/components/forms/PropertyInput.vue';
import AccountForm from '~/components/forms/AccountForm.vue';
import { hexToHsl } from '~/libs';
import Input from '~/components/forms/Input.vue';
import { ElInput } from 'element-plus';
const { t } = useI18n();


const valueList = ref({})
const model1 = ref({
    en: 'Hello',
    "zh-Hans": '你好',
    "zh-Hant": '你好'
})

const model2 = ref({
    en: {
        name: 'Hello',
        description: 'This is a test'
    },
    "zh-Hans": {
        name: '你好',
        description: '這是一个测试'
    },
    "zh-Hant": {
        name: '你好',
        description: '這是一個測試'
    }
})

watch(model1, (newValue) => {
    console.log(newValue)
}, { deep: true })

watch(model2, (newValue) => {
    console.log(newValue)
}, { deep: true })


</script>