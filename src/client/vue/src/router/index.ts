import { createRouter, createWebHistory } from "vue-router";

const routes = [
    { path: "/", component: ()=>import("../views/Page.vue") ,
        children: [
            { path: "", component: ()=>import("../views/Home.vue") },
            { path: "about", component: ()=>import("../views/About.vue") },
            { path: "login", component: ()=>import("../views/Login.vue") },
        ]
    },
    { path: "/admin", component: ()=>import("../views/Admin.vue"),
        children: [
            { path: "/roles", component: ()=>import("../components/roles/RoleView.vue") },
            { path: "/users", component: ()=>import("../components/users/UserView.vue") },

        ]        
    },
    { path:'/callback', component: ()=>import('../views/ExternalLoginCallback.vue')},
    { path: "/:pathMatch(.*)*", redirect: "/" }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// 路由守卫
router.beforeEach(async () => {});

export default router;
