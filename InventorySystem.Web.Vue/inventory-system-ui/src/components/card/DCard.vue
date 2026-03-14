<script setup lang="ts">
import { computed } from 'vue';

const props = withDefaults(defineProps<{ padding?: '1' | '2' | '3' | '4' | '5' | '6'; variant?: 'accent' | 'gray' }>(), {
    variant: 'accent',
});
const padding = computed(() => (props.padding ? `p${props.padding}` : undefined));
</script>
<template>
    <div class="card-container" :class="variant">
        <slot name="header" />
        <div class="card-content" :class="padding">
            <slot />
        </div>
    </div>
</template>
<style lang="css" scoped>
.card-container {
    border-radius: 3px;
    align-items: baseline;
    overflow: hidden;
}

.accent {
    background-color: var(--component-background-acccent);
    border: 1px solid var(--component-border-acccent);
    color: var(--component-font-contrast);
}

.gray {
    background-color: var(--component-background-gray);
    border: 1px solid var(--component-border-gray);
    color: var(--component-font-contrast);
}

.card-content {
}

.p1 {
    padding: var(--space-1);
}
.p2 {
    padding: var(--space-2);
}
.p3 {
    padding: var(--space-3);
}
.p4 {
    padding: var(--space-4);
}
.p5 {
    padding: var(--space-5);
}
.p6 {
    padding: var(--space-6);
}
</style>
