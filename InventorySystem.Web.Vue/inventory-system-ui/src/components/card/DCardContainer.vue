<script setup lang="ts">
const props = withDefaults(
    defineProps<{
        columns?: '1' | '2' | '3' | '4' | '5' | '6';
    }>(),
    {
        columns: '3',
    },
);
</script>
<template>
    <div class="card-grid-container" :class="`cols-${props.columns}`">
        <slot />
    </div>
</template>
<style lang="css" scoped>
.card-grid-container {
    display: grid;
    gap: var(--space-1);
}
.cols-1 {
    grid-template-columns: repeat(1, 1fr);
}
.cols-2 {
    grid-template-columns: repeat(2, 1fr);
}
.cols-3 {
    grid-template-columns: repeat(3, 1fr);
}
.cols-4 {
    grid-template-columns: repeat(4, 1fr);
}
.cols-5 {
    grid-template-columns: repeat(5, 1fr);
}
.cols-6 {
    grid-template-columns: repeat(6, 1fr);
}
</style>
