import { ref, computed } from "vue";
import { onMounted, onUnmounted } from "vue";

export function useBrowseEnv() {
    const isMobile = ref(window.innerWidth < 768);
    
    const handleResize = () => {
        isMobile.value = window.innerWidth < 768;
    };

    // 检测设备是否支持 hover
    const hasHover = computed(() => {
        return window.matchMedia('(hover: hover)').matches;
    });

    onMounted(() => {
        window.addEventListener("resize", handleResize);
    });

    onUnmounted(() => {
        window.removeEventListener("resize", handleResize);
    });

    return {
        isMobile,
        hasHover
    };
};
