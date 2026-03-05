<script setup lang="ts">
import DCard from '@/components/card/DCard.vue';
import DCardContainer from '@/components/card/DCardContainer.vue';
</script>
<template>
    <h1>DCard</h1>
    <h2>Full width</h2>
    <div class="full-width">
        <DCardContainer v-for="cols in 6" :key="`col-${cols}`" :columns="`${cols}`">
            <DCard v-for="i in 6" :key="i">
                <template #header>
                    <h1>Column {{ cols }}</h1>
                </template>
                Card content
            </DCard>
        </DCardContainer>
    </div>
    <h2>Controlled width</h2>
    <div class="half-width">
        <DCardContainer v-for="cols in 6" :key="`col-${cols}`" :columns="`${cols}`">
            <DCard v-for="i in 6" :key="i">
                <template #header>
                    <h1>Column {{ cols }}</h1>
                </template>
                Card content
            </DCard>
        </DCardContainer>
    </div>
</template>
<style lang="css" scoped>
.full-width {
    width: 100%;
}

.half-width {
    width: 50%;
}
</style>
