<template>
    <div v-bind="$attrs" class="flex flex-row items-center gap-1 w-full box-border" >
        <div :style="`padding-left: ${((data?.code || '').split('.').length - 1) * 16}px ;`"></div>
        <div v-if="data?.leaf !== true" class="cursor-pointer">
            <i v-if="data?.expanded === true" class="fa fa-caret-down w-4 h-4" @click="expand(data, false)"></i>
            <i v-else class="fa fa-caret-right w-4 h-4" @click="expand(data, true)"></i>
        </div>
        <div v-else class="w-4 h-4"></div>
        <Loading v-if="data?.loading === true" />
        <HighlightText class="flex-1" :text="text" :filter="filterText"></HighlightText>
    </div>

</template>

<script setup lang="ts">
import { PropType } from 'vue';
import HighlightText from '../HighlightText.vue';
import Loading from '../Loading.vue';

defineProps({
    data:{
        type: Object as PropType<any>,
        default: null
    },
    text:{
        type: String,
        default: ''
    },
    filterText: {
        type: String,
        default: ''
    },
    expand: {
        type: Function,
        default: () => { }
    }
});

</script>