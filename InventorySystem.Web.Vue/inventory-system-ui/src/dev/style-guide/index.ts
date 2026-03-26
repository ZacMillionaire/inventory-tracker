import type { RouteRecordRaw } from 'vue-router';
import { DCardRouteNames, DCardRoutes } from './DCard';
import { DHorizontalNavigationRouteNames, DHorizontalNavigationRoutes } from './DHorizontalNavigation';

export const StyleGuideRouteNames = {
    styleGuide: {
        index: 'style-guide',
        horizontalMenu: 'style-guide-horizontal-menu',
        ...DCardRouteNames,
        ...DHorizontalNavigationRouteNames
    },
};

const Test = {
    whatever: {
        index: 'asdf',
    },
};

const t: typeof StyleGuideRouteNames & typeof Test = { ...StyleGuideRouteNames, ...Test };

export const StyleGuideRoutes: RouteRecordRaw[] = [
    {
        path: '/style-guide',
        name: StyleGuideRouteNames.styleGuide.index,
        component: () => import('./StyleGuidePage.vue'),
        children: [
            ...DCardRoutes,
            ...DHorizontalNavigationRoutes,
        ],
    },
];

export const navigationRoutes = [
    {
        name: StyleGuideRouteNames.styleGuide.dCard.index,
        displayName: 'DCards',
    },
    {
        name: StyleGuideRouteNames.styleGuide.dHorizontal.index,
        displayName: 'DNavigation',
    },
];
