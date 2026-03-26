<script setup lang="ts">
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { DCardRouteNames } from '.';
import { DScrollableContent } from '@/components/layout';
import devToolRouter from '@/dev/devToolRouter';

const navigationRoutes = [
    {
        name: DCardRouteNames.dCard.layout,
        route: devToolRouter.resolve({ name: DCardRouteNames.dCard.layout }),
    },
    {
        name: DCardRouteNames.dCard.spacing,
        route: devToolRouter.resolve({ name: DCardRouteNames.dCard.spacing }),
    },
    {
        name: DCardRouteNames.dCard.variants,
        route: devToolRouter.resolve({ name: DCardRouteNames.dCard.variants }),
    },
];
</script>
<template>
    <DScrollableContent>
        <template #header>
            <DHorizontalNavigationBar class="navigation" align-end>
                <DHorizontalNavigationLink v-for="styleRoute of navigationRoutes" :key="styleRoute.name" :to="styleRoute.route">
                    {{ styleRoute.route.meta.display }}
                </DHorizontalNavigationLink>
            </DHorizontalNavigationBar>
        </template>
        <RouterView />
    </DScrollableContent>
</template>
<style lang="css" scoped></style>
