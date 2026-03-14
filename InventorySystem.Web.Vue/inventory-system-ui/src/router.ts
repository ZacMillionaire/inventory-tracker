import { createRouter, createWebHistory } from 'vue-router';
import { HomeRoutes } from '@/pages/Home';
import { StyleGuideRoutes } from '@/pages/style-guide';
import { ItemPageRoutes } from './pages/items';

const routes = [...HomeRoutes, ...StyleGuideRoutes, ...ItemPageRoutes];
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routes,
});

export default router;
