// src/store/config.ts
import { defineStore } from 'pinia';

export const useConfigStore = defineStore('config', {
    state: () => ({
        isReady: false, // 配置是否已就绪
        isAuthenticated: false, // 用户的认证状态
    }),
    actions: {
        setReadyState(status: boolean) {
            this.isReady = status; // 更新配置就绪状态
        },
        setAuthentication(status: boolean) {
            this.isAuthenticated = status; // 更新用户的认证状态
        },
        refreshState(isReady: boolean, isAuthenticated: boolean) {
            // 配置已就绪，只需检查 response 是否存在
            this.setReadyState(isReady);
            
            // 根据 response 来更新认证状态
            this.setAuthentication(isAuthenticated);
        }
    },
});
