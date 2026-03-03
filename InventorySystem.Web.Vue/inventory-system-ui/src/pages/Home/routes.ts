import type { RouteRecordRaw } from "vue-router";

export const HomeRoutes: RouteRecordRaw[] = [
    {
        path: "/",
        name: "home",
        // route level code-splitting
        // this generates a separate chunk (About.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import("./HomePage.vue"),
    },
];
