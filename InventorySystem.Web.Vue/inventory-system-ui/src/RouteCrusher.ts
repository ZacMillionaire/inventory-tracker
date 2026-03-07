import type { RouteRecordRaw } from 'vue-router';

export type DisplayRoute = {
    name: string;
    route: RouteRecordRaw;
};

// TODO: work out a way where it'll crush all top level routes from routes above
// TODO: somehow workout how I'll make any fly out menus work from route meta as children can't be used that way
//  not that that matters for me this is just trying to work out an approach for work
export const routeCrusher = (routes: RouteRecordRaw[], topLevelOnly: boolean = true) => {
    const topLevel: DisplayRoute[] = [];
    for (const route of routes) {
        if (route.meta?.display) {
            topLevel.push({
                name: route.meta.display as string,
                route: route,
            });
        }
        if (!topLevelOnly && route.children) {
            topLevel.push(...routeCrusher(route.children, topLevelOnly));
        }
    }
    return topLevel;
};
