<script setup lang="ts">
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { DCardRouteNames } from '.';
import router from '@/router';
import { DScrollableContent } from '@/components/layout';

const navigationRoutes = [
    {
        name: DCardRouteNames.dCard.layout,
        route: router.resolve({ name: DCardRouteNames.dCard.layout }),
    },
    {
        name: DCardRouteNames.dCard.spacing,
        route: router.resolve({ name: DCardRouteNames.dCard.spacing }),
    },
    {
        name: DCardRouteNames.dCard.variants,
        route: router.resolve({ name: DCardRouteNames.dCard.variants }),
    },
];
</script>
<template>
    <DScrollableContent>
        <template #header>
            <DHorizontalNavigationBar class="navigation">
                <DHorizontalNavigationLink v-for="styleRoute of navigationRoutes" :key="styleRoute.name" :to="styleRoute.route">
                    {{ styleRoute.route.meta.display }}
                </DHorizontalNavigationLink>
            </DHorizontalNavigationBar>
        </template>
        <RouterView />
    </DScrollableContent>
</template>
<style lang="css" scoped></style>
