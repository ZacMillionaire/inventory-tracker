<script setup lang="ts">
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { StyleGuideRoutes } from '.';
import { routeCrusher } from '@/router';

const r = routeCrusher(StyleGuideRoutes);
</script>
<template>
    <div>
        <DHorizontalNavigationBar class="navigation">
            <DHorizontalNavigationLink v-for="styleRoute of r" :key="styleRoute.name" :to="styleRoute.route">
                {{ styleRoute.name }}
            </DHorizontalNavigationLink>
        </DHorizontalNavigationBar>
        <h1>Style Guide</h1>
        <RouterView />
    </div>
</template>
<style lang="css" scoped>
.navigation {
}
</style>
