import { createRouter, createWebHistory } from 'vue-router';
import { HomeRoutes } from '@/pages/Home';
import { ItemPageRoutes } from './pages/items';

const routes = [...HomeRoutes, ...ItemPageRoutes];
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routes,
});

export default router;
