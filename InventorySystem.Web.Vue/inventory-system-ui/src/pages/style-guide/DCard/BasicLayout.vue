<script setup lang="ts">
import DCard from '@/components/card/DCard.vue';
import DCardContainer from '@/components/card/DCardContainer.vue';
import { DScrollableContent } from '@/components/layout';
import { computed, ref } from 'vue';
const controlWidth = ref<number>(100);
const widthPercent = computed(() => `${controlWidth.value}%`);
</script>
<template>
    <DScrollableContent class="c">
        <template #header>
            <div class="header">
                <h2>Controlled width</h2>
                <input type="range" min="0" max="100" v-model="controlWidth" /> {{ widthPercent }}
            </div>
        </template>
        <div class="content">
            <DCard padding="3" :style="{ width: widthPercent }">
                <DCardContainer columns="1">
                    <DCard padding="3" v-for="cols in 6" :key="`col-${cols}`">
                        <template #header>
                            <h1>
                                <code>column="{{ cols }}"</code>
                            </h1>
                        </template>
                        <DCardContainer :columns="`${cols}`">
                            <DCard v-for="i in 6" :key="i"> Card content </DCard>
                        </DCardContainer>
                    </DCard>
                </DCardContainer>
            </DCard>
        </div>
    </DScrollableContent>
</template>
<style lang="css" scoped>
.c {
    margin: var(--space-3);
    border: 1px solid var(--component-border-gray);
    border-radius: 3px;
    height: 500px;
}

.header {
    padding: var(--space-2);
    border-bottom: 1px solid var(--component-border-gray);
}

.content {
    display: flex;
    justify-content: center;
    padding: var(--space-2);
}

.full-width {
    width: 100%;
}

.half-width {
    width: 50%;
}
</style>
