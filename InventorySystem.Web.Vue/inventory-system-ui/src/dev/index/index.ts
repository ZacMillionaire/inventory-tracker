import type { RouteRecordRaw } from 'vue-router';

export const DevToolHomeRouteNames = {
    Index : 'dev-tools-home'
} as const;

export const DevToolHomeRoutes: RouteRecordRaw[] = [
    {
        path: '/',
        name: DevToolHomeRouteNames.Index,
        component: () => import('./DevToolIndex.vue'),
    },
];
