<script setup lang="ts">
import { DCard, DCardContainer } from '@/components/card';
</script>
<template>
    <div class="items">
        <DCardContainer columns="6">
            <DCard v-for="t in 200" padding="3" :key="t">
                <template #header> item {{ t }} </template>
                <div class="type">example type</div>
            </DCard>
        </DCardContainer>
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
