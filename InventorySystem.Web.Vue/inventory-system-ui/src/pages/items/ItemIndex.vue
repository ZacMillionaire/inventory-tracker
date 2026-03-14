<script setup lang="ts">
import { DCard, DCardContainer } from '@/components/card';
</script>
<template>
    <div class="items">
        <DCardContainer columns="6">
            <DCard v-for="t in 200" padding="3" :key="t"> item {{ t }} </DCard>
        </DCardContainer>
    </div>
</template>
<style lang="css" scoped>
.items {
    padding: var(--space-2);
}
</style>
