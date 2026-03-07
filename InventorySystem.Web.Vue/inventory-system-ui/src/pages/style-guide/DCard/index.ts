import type { RouteRecordRaw } from 'vue-router';

export const DCardRouteNames = {
    dCard :{
        index : 'style-guide-card',
        layout: 'style-guide-layout',
        spacing: 'style-guide-spacing'
    }
}

export const DCardRoutes :RouteRecordRaw[] = [
    {
        path: 'cards',
        name : DCardRouteNames.dCard.index,
        component: () => import('./DCardPage.vue'),
        meta: {
            display: 'DCards'
        },
        redirect: {
            name: DCardRouteNames.dCard.layout
        },
        children: [
            {
                path: 'layout',
                name : DCardRouteNames.dCard.layout,
                component : () => import('./BasicLayout.vue'),
                meta: {
                    display: 'Basic Layout'
                },
            },
            {
                path: 'spacing',
                name: DCardRouteNames.dCard.spacing,
                component : () => import('./Spacing.vue'),
                meta : {
                    display: 'Spacing'
                }
            }
        ]
    },
];
