import type { RouteRecordRaw } from 'vue-router';

export const CssVariableEditorRouteNames = {
    Index : 'css-editor'
} as const;

export const CssVariableEditorRoutes: RouteRecordRaw[] = [
    {
        path: '/css-variable-editor',
        name: CssVariableEditorRouteNames.Index,
        component: () => import('./CssVariableEditor.vue'),
    },
];
