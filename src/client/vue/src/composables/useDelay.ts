import { ref } from "vue";

//延时500ms，在这个时间内有新的动作，则继续延时，否则取消延时，执行后续
export function useDelay() {
    const timer = ref<any>(null);
    const delay = (callback: Function, delayTime?: number) => {
        if (timer.value) {
            clearTimeout(timer.value);
        }
        timer.value = setTimeout(() => {
            callback();
            timer.value = null;
        }, delayTime || 500);
    };

    return {
        delay,
    };
}
