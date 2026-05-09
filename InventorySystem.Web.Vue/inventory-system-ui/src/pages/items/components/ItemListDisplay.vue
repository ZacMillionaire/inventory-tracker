<script setup lang="ts">
import { DCard, DCardContainer } from '@/components/card';
import { ItemRepository } from '@/api/items/ItemRepository';
import { onMounted, ref } from 'vue';
import type { ItemDto } from '@/api/items';

const items = ref<ItemDto[]>();

onMounted(async () => {
    items.value = await ItemRepository().GetItems();
    console.log(items.value);
});
</script>
<template>
    <div class="items">
        <DCardContainer columns="3">
            <DCard v-for="item in items" padding="3" :key="item.id">
                <template #header> {{ item.name }}</template>
                <div>
                    <table>
                        <tbody>
                            <tr>
                                <td>Created</td>
                                <td>{{ item.createdUtc }}</td>
                            </tr>
                            <tr>
                                <td>Updated</td>
                                <td>{{ item.updatedUtc }}</td>
                            </tr>
                            <tr>
                                <td>Description</td>
                                <td>{{ item.description }}</td>
                            </tr>
                            <tr>
                                <td>Distinct</td>
                                <td>{{ item.distinct }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </DCard>
        </DCardContainer>
    </div>
</template>
<style lang="css" scoped></style>
