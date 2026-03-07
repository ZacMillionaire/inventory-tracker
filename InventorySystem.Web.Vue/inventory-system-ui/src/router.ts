import { createRouter, createWebHistory } from 'vue-router';
import { HomeRoutes } from '@/pages/Home';
import { StyleGuideRoutes } from '@/pages/style-guide';

const routes = [...HomeRoutes, ...StyleGuideRoutes];
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routes,
});

export default router;
