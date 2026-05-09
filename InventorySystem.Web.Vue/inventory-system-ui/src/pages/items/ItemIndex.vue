<script setup lang="ts">
import ItemListDisplay from './components/ItemListDisplay.vue';
</script>
<template>
    <div>
        <ItemListDisplay/>
    </div>
</template>
<style lang="css" scoped>
.items {
    padding: var(--space-2);
}
.type {
    font-size: var(--small-font);
}
</style>
