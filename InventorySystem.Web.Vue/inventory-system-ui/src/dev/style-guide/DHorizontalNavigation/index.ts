import type { RouteRecordRaw } from 'vue-router';

export const DHorizontalNavigationRouteNames = {
    dHorizontal :{
        index : 'style-guide-navigation'
    }
}

export const DHorizontalNavigationRoutes :RouteRecordRaw[] = [
    {
        path: 'navigation',
        component: () => import('./DHorizontalNavigationPage.vue'),
        meta: {
            display: 'DNavigation'
        },
        children: [
            {
                path: ':example?',
                name : DHorizontalNavigationRouteNames.dHorizontal.index,
                component : () => import('./NormalNavigation.vue'),
                meta: {
                    display: 'Basics'
                }
            }
        ]
    },
];
