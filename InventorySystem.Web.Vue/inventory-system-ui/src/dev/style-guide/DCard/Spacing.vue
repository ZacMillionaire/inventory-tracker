<script setup lang="ts">
import DCard from '@/components/card/DCard.vue';
import DCardContainer from '@/components/card/DCardContainer.vue';
const paddings = ['1', '2', '3', '4', '5', '6'];
</script>
<template>
    <div class="content">
        <div>
            Card content (not headers), can have a padding applied using <code>padding="{{ paddings.join('|') }}"</code>
        </div>

        <DCard>
            <DCardContainer columns="1">
                <template v-for="padding in paddings" :key="padding">
                    <DCard :padding="padding">
                        <template #header> Padding {{ padding }} </template>
                        Content
                    </DCard>
                </template>
            </DCardContainer>
        </DCard>
    </div>
</template>
<style lang="css" scoped>
.content {
    margin: var(--space-2);
}
</style>
