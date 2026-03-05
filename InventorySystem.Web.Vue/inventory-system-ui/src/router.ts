import { createRouter, createWebHistory } from 'vue-router';
import { HomeRoutes } from '@/pages/Home';
import { StyleGuideRoutes } from '@/pages/style-guide';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [...HomeRoutes, ...StyleGuideRoutes],
});

export default router;

