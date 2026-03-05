import type { RouteRecordRaw } from 'vue-router';

export const StyleGuideRoutes: RouteRecordRaw[] = [
    {
        path: '/style-guide',
        name: 'style-guide',
        component: () => import('./StyleGuidePage.vue'),
        children: [
            {
                path: 'cards',
                name: 'style-guide-card',
                component: () => import('./CardPage.vue'),
            },
        ],
    },
];
