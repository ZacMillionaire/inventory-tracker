<script setup lang="ts">
import { RouterView } from 'vue-router';
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
</script>

<template>
    <div id="main">
        <DHorizontalNavigationBar class="navigation">
            <template #logo>
                <img alt="Vue logo" class="logo" src="@/assets/logo.svg" />
            </template>
            <DHorizontalNavigationLink :to="{ name: 'home' }"> Home </DHorizontalNavigationLink>
            <DHorizontalNavigationLink :to="{ name: 'item-index' }"> Items </DHorizontalNavigationLink>
        </DHorizontalNavigationBar>

        <div id="content" class="full-height-no-scroll">
            <RouterView />
        </div>
    </div>
</template>

<style scoped>
#main {
    height: 100%;
    display: flex;
    flex-direction: column;
}

.navigation {
    height: 3rem;
}
.logo {
    height: 100%;
}
#content {
}
</style>
