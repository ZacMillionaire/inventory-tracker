<script setup lang="ts">
import type { RouteLocationRaw } from 'vue-router';

defineProps<{ to: RouteLocationRaw }>();
</script>
<template>
    <RouterLink custom :to="to" v-slot="{ isActive, href, navigate }">
        <div class="nav-item" :class="isActive ? 'active' : ''">
            <a v-bind="$attrs" :href="href" @click="navigate">
                <slot />
            </a>
        </div>
    </RouterLink>
</template>
<style lang="css" scoped>
.nav-item {
    display: flex;
    align-items: center;
    & a {
        color: var(--gray-11);
        text-decoration: none;
        font-weight: bold;
        align-items: center;
        display: flex;
        height: 100%;
        padding: var(--space-1) var(--space-2);
        &:hover {
            color: var(--gray-12);
        }
    }
}
.nav-item.active {
    background-color: var(--accent-5);
    & a {
        color: var(--gray-12);
    }
}
.nav-item:hover {
    background-color: var(--gray-10);
}
</style>
