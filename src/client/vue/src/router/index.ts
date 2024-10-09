import { createRouter, createWebHistory } from "vue-router";
import Admin from "../views/Admin.vue"; // 主页组件
import Login from "../views/Login.vue"; // 登录组件

const routes = [
    { path: "/", component: import("../views/Home.vue") },
    { path: "/admin", component: Admin ,
        children: [
            { path: "/roles", component: ()=>import("../components/roles/RoleView.vue") },
            { path: "/users", component: ()=>import("../components/users/UserView.vue") },

        ]        
    },
    { path: "/login", component: Login },
    { path: "/:pathMatch(.*)*", redirect: "/" }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// 路由守卫
router.beforeEach(async () => {});

export default router;
