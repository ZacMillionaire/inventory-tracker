<script setup lang="ts">
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { navigationRoutes } from '.';
import { DScrollableContent } from '@/components/layout';
</script>
<template>
    <DScrollableContent>
        <template #header>
            <DHorizontalNavigationBar class="navigation" align-end>
                <DHorizontalNavigationLink v-for="styleRoute of navigationRoutes" :key="styleRoute.name" :to="{ name: styleRoute.name }">
                    {{ styleRoute.displayName }}
                </DHorizontalNavigationLink>
            </DHorizontalNavigationBar>
        </template>
        <RouterView />
    </DScrollableContent>
</template>
<style lang="css" scoped></style>
