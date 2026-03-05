<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router';
</script>

<template>
    <div id="main">
        <header>
            <img alt="Vue logo" class="logo" src="@/assets/logo.svg" />

            <nav class="navigation">
                <RouterLink custom to="/" v-slot="{ isActive, href, navigate }">
                    <div class="nav-item" :class="isActive ? 'active' : ''">
                        <a v-bind="$attrs" :href="href" @click="navigate"> Home </a>
                    </div>
                </RouterLink>
                <RouterLink custom to="/about" v-slot="{ isActive, href, navigate }">
                    <div class="nav-item" :class="isActive ? 'active' : ''">
                        <a v-bind="$attrs" :href="href" @click="navigate"> About </a>
                    </div>
                </RouterLink>
                <RouterLink custom to="/style-guide" v-slot="{ isActive, href, navigate }">
                    <div class="nav-item" :class="isActive ? 'active' : ''">
                        <a v-bind="$attrs" :href="href" @click="navigate"> Style Guide </a>
                    </div>
                </RouterLink>
            </nav>
        </header>

        <div id="content">
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
header {
    min-height: 2rem;
    max-height: 4vh;
    display: flex;
    align-items: stretch;
    border-bottom: 1px solid var(--gray-6);
}
.logo {
    height: 100%;
}
.navigation {
    display: flex;
}
.nav-item {
    display: flex;
    align-items: center;
    padding: var(--space-2);
    & a {
        color: var(--gray-11);
        text-decoration: none;
        font-weight: bold;
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

#content {
    overflow: auto;
}
</style>
