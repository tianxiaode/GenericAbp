import { createRouter, createWebHistory } from "vue-router";

const routes = [
    { path: "/", component: ()=>import("~/components/layouts/PageLayout.vue") ,
        children: [
            { path: "", component: ()=>import("~/views/Home.vue") },
            { path: "about", component: ()=>import("~/views/About.vue") },
            { path: "login", component: ()=>import("~/components/accounts/Login.vue") },
            { path: "register", component: ()=>import("~/components/accounts/Register.vue") },
            { path: "forgot-password", component: ()=>import("~/components/accounts/ForgotPassword.vue") },
            { path: "reset-password", component: ()=>import("~/components/accounts/ResetPassword.vue") },
            { path: "profile", meta: { requiresAuth: true }, component: ()=>import("~/components/accounts/Profile.vue") },
            { path: "page404", component: ()=>import("~/views/Page404.vue") },
        ]
    },
    { path: "/", meta: { requiresAuth: true }, component: ()=>import("~/components/layouts/AdminLayout.vue"),
        children: [
            { path: "dashboard", component: ()=>import("~/views/Dashboard.vue") },
            { path: "roles", component: ()=>import("~/components/admins/roles/RoleView.vue") },
            { path: "users", component: ()=>import("~/components/admins/users/UserView.vue") },
            { path: 'tenants', component: ()=>import("~/components/admins/tenants/TenantView.vue") },
            { path: 'menus', component: ()=>import("~/components/admins/menus/MenuView.vue") },
            { path: "applications", component: ()=>import("~/components/admins/openiddicts/ApplicationView.vue") },
            { path: "scopes", component: ()=>import("~/components/admins/openiddicts/ScopeView.vue") },
            { path: "security-logs", component: ()=>import("~/components/admins/securitylogs/SecurityLogView.vue") },            
            { path: "audit-logs", component: ()=>import("~/components/admins/auditLogs/AuditLogView.vue") },            
            { path: "settings", component: ()=>import("~/components/admins/settings/SettingManagement.vue") }

        ]        
    },
    { path:'/signin-oidc', component: ()=>import('../views/SigninOidc.vue')},
    { path: "/:pathMatch(.*)*", redirect: "/page404"  }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// 路由守卫
router.beforeEach(async () => {


});


export default router;
