import { createRouter, createWebHistory } from "vue-router";
import { LocalStorage } from "~/libs/LocalStoreage";

const routes = [
    { path: "/", component: ()=>import("~/components/layouts/PageLayout.vue") ,
        children: [
            { path: "", component: ()=>import("~/views/Home.vue") },
            { path: "about", component: ()=>import("~/views/About.vue") },
            { path: "login", component: ()=>import("~/components/accounts/Login.vue") },
            { path: "register", component: ()=>import("~/views/Register.vue") },
            { path: "forgot-password", component: ()=>import("~/views/ForgotPassword.vue") },
            { path: "reset-password", component: ()=>import("~/views/ResetPassword.vue") },
            { path: "profile", component: ()=>import("~/views/Profile.vue") },
            { path: "page404", component: ()=>import("~/views/Page404.vue") },
        ]
    },
    { path: "/admin", component: ()=>import("../views/Admin.vue"),
        children: [
            { path: "/roles", component: ()=>import("../components/roles/RoleView.vue") },
            { path: "/users", component: ()=>import("../components/users/UserView.vue") },

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
router.beforeEach(async (to, from) => {
    if(to.path === '/signin-oidc' || to.path === '/login'){
        LocalStorage.setRedirectPath(from.path);
    }
    
});

export default router;
