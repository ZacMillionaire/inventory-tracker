import type { RouteRecordRaw } from 'vue-router';

export const ItemPageRoutes :RouteRecordRaw[] = [
    {
        path: '/items',
        component : () => import('./ItemsLayoutPage.vue'),
        children: [
            {
                path: '',
                name : 'item-index',
                component: () => import('./ItemIndex.vue')
            }
        ]
}
];
