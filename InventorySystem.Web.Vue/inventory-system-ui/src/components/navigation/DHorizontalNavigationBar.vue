<script setup lang="ts">
withDefaults(defineProps<{ alignEnd?: boolean }>(), { alignEnd: false });
</script>
<template>
    <header :class="[{ 'align-end': alignEnd }]">
        <slot name="logo" />

        <nav class="navigation-content">
            <slot />
        </nav>
    </header>
</template>
<style lang="css" scoped>
header {
    height: 2rem;
    min-height: 2rem;
    display: flex;
    align-items: stretch;
    border-bottom: 1px solid var(--gray-6);
    flex: 0 0 auto;
}

.navigation-content {
    display: flex;
    flex: 0 0 auto;
}
.align-end {
    justify-content: flex-end;
}
</style>
