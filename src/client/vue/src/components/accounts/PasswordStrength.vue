<template>
    <div class="grid grid-cols-5 gap-2 w-full mt-2" v-if="value">
        <el-progress :show-text="false" :percentage="indicators[0]?.value" :status="indicators[0]?.status" />
        <el-progress :show-text="false" :percentage="indicators[1]?.value" :status="indicators[1]?.status" />
        <el-progress :show-text="false" :percentage="indicators[2]?.value" :status="indicators[2]?.status" />
        <el-progress :show-text="false" :percentage="indicators[3]?.value" :status="indicators[3]?.status" />
        <el-progress :show-text="false" :percentage="indicators[4]?.value" :status="indicators[4]?.status" />
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import zxcvbn from 'zxcvbn-lite';
import { isEmpty } from '~/libs';

type IndicatorType = {
    value: Number;
    status: String;
};


const props = defineProps({
    value: {
        type: String
    }
});
const indicators = ref<IndicatorType[]>([]);

watch(() => props.value, () => {
    refreshIndicator();
});

const refreshIndicator = () => {
    const password = props.value;
    if (isEmpty(password)) {
        for (let i = 0; i <= 4; i++) {
            setIndicatorValue(i, 0, '');
        }
        return;
    }
    let result = zxcvbn(password || ''),
        score = result.guesses_log10,
        present = Math.floor((score / 12) * 100),
        mainColors = present <= 50 ? 'exception' : present <= 75 ? 'warning' : 'success';
    present = present > 100 ? 100 : present;
    for (let i = 0; i <= 4; i++) {
        if (present < 0) {
            setIndicatorValue(i, present, '');
            continue;
        }
        let progress = present % 20;
        if (progress === present) {
            progress = progress * 5;
        } else {
            progress = 100;
        }
        present -= 20;
        setIndicatorValue(i, progress, mainColors);
    }
};

const setIndicatorValue = (i: number, value: number, status: string) => {
    indicators.value[i] = { value: value < 0 ? 0 : value, status };
};


onMounted(() => {
    refreshIndicator();
});


</script>