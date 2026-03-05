<script setup lang="ts"></script>
<template>
    <div class="card-container">
        <slot name="header" />
        <slot />
    </div>
</template>
<style lang="css" scoped>
.card-container {
    background-color: var(--component-background-acccent-level-1);
    border-radius: 3px;
    padding: var(--space-1);
    align-items: baseline;
    border: 1px solid var(--component-border-acccent-level-1);
    color: var(--component-font-contrast);
}
</style>
