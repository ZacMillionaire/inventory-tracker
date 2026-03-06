import type { RouteRecordRaw } from 'vue-router';

export const StyleGuideRouteNames = {
    styleGuide : {
        index: 'style-guide',
        cards : 'style-guide-card',
        horizontalMenu : 'style-guide-horizontal-menu'
    },
}

const Test = {
    whatever : {
        index : 'asdf'
    }
}

const t :typeof StyleGuideRouteNames & typeof Test = {...StyleGuideRouteNames, ...Test};

export const StyleGuideRoutes: RouteRecordRaw[] = [
    {
        path: '/style-guide',
        name: StyleGuideRouteNames.styleGuide.index,
        component: () => import('./StyleGuidePage.vue'),
        redirect: {
            name: StyleGuideRouteNames.styleGuide.cards
        },
        children: [
            {
                path: 'cards',
                name: StyleGuideRouteNames.styleGuide.cards,
                component: () => import('./DCardPage.vue'),
                meta: {
                    display: 'DCards'
                }
            },
            {
                path: 'horizontal-menu/:example?',
                name: StyleGuideRouteNames.styleGuide.horizontalMenu,
                component: () => import('./DHorizontalNavigationPage.vue'),
                meta : {
                    display: 'DHorizontalMenu'
                }
            },
        ],
    },
];
