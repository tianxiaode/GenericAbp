import { defineStore } from 'pinia';

export const useAsideStore = defineStore('aside', {
    state: () => ({
        isExpanded: false,
    }),
    actions: {
        toggleAside() {
            this.isExpanded = !this.isExpanded;
        },
    },
});
