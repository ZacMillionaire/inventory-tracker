<script setup lang="ts">
import { DCardContainer } from '@/components/card';
import DCard from '@/components/card/DCard.vue';
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { RouterView } from 'vue-router';
import NewItemForm from './components/NewItemForm.vue';
</script>
<template>
    <div class="grid-content full-height-no-scroll">
        <div class="left-nav">
            <ul>
                <li>Items</li>
                <li>Create</li>
                <li>Filter</li>
            </ul>
        </div>
        <div class="items-content-container full-height-scroll">
            TODO make full height menu with side bar and content that scrolls maybe idk what I want this UI to be should probably maybe have a form to quickly create a new item first then a list of
            existing items as cards I guess but that can live in ItemIndex.vue or something
            <NewItemForm />
            <RouterView />
        </div>
    </div>
    <!-- <div class="c">
        <DHorizontalNavigationBar class="header">
            <DHorizontalNavigationLink :to="{ name: 'item-index' }">Index</DHorizontalNavigationLink>
        </DHorizontalNavigationBar>
        <div class="content">
            <RouterView />
        </div>
    </div> -->
</template>
<style lang="css" scoped>
.grid-content {
    display: grid;
    grid-template-columns: repeat(24, 1fr);
}
.left-nav {
    grid-column: 1/3;
}
.items-content-container {
    background-color: rgba(0 255 255 /0.2);
    grid-column: 3/-1;
}
/* .c {
    margin: var(--space-3);
}

.header {
    position: sticky;
    top: 0;
    border: 1px solid var(--component-border-gray);
    border-radius: 3px 3px 0px 0px;
    background-color: var(--main-background-colour);
}

.content {
    border: 1px solid var(--component-border-gray);
    border-top: none;
    overflow: hidden;
} */
</style>
