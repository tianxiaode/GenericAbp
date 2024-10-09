import { ref, watch,onMounted } from 'vue';
import { useConfigStore } from '../store';
import { storeToRefs } from 'pinia';

export function useConfigStoreWatcher(refreshCallback?: Function, authCallback?: Function) {
  const configStore = useConfigStore();
  const { isReady, isAuthenticated } = storeToRefs(configStore);

  const isReadyRef = ref(isReady.value);
  const isAuthenticatedRef = ref(isAuthenticated.value);

  watch(isReady, (newVal) => {
    isReadyRef.value = newVal;
    refreshCallback && refreshCallback(newVal);
  });

  watch(isAuthenticated, (newVal) => {
    isAuthenticatedRef.value = newVal;
      authCallback && authCallback(newVal);
  });

  onMounted(() => {
    refreshCallback && refreshCallback(isReady.value);
    authCallback && authCallback(isAuthenticated.value);
});


  return {
    isReady: isReadyRef,
    isAuthenticated: isAuthenticatedRef,
    setReadyState: configStore.setReadyState,
    setAuthentication: configStore.setAuthentication,
    refreshReadyState: configStore.refreshReadyState,
  };
}