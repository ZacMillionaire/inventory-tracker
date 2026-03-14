<script setup lang="ts">
import { DCardContainer } from '@/components/card';
import DCard from '@/components/card/DCard.vue';
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { RouterView } from 'vue-router';
import NewItemForm from './components/NewItemForm.vue';
</script>
<template>
    <div class="grid-content full-height-no-scroll">
        <div class="left-nav full-height-scroll">
            <ul>
                <li>Items</li>
                <li>Create</li>
                <li>Filter</li>
            </ul>
        </div>
        <div class="items-content-container full-height-no-scroll">
            <div class="new-item-form-container">
                <NewItemForm />
            </div>
            <div class="items-content">
                <RouterView />
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
.grid-content {
    display: grid;
    grid-template-columns: repeat(24, 1fr);
}
.left-nav {
    grid-column: 1/3;
    border-right: 1px solid var(--component-border-gray);
}
.items-content-container {
    grid-column: 3/-1;
    display: grid;
    grid-template-rows: auto 1fr;
}
.new-item-form-container {
    border-bottom: 1px solid var(--component-border-gray);
}
.items-content {
    overflow: auto;
}
</style>
