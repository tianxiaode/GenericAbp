import { createRouter, createWebHistory } from "vue-router";

const routes = [
    { path: "/", component: ()=>import("../views/Home.vue") },
    { path: "/admin", component: ()=>import("../views/Admin.vue"),
        children: [
            { path: "/roles", component: ()=>import("../components/roles/RoleView.vue") },
            { path: "/users", component: ()=>import("../components/users/UserView.vue") },

        ]        
    },
    { path: "/:pathMatch(.*)*", redirect: "/" }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// 路由守卫
router.beforeEach(async () => {});

export default router;
