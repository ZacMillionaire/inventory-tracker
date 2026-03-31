<script setup lang="ts">
import { DScrollableContent } from '@/components/layout';

</script>
<template>
    <DScrollableContent>
        <template #header>
            <h1>Navigation</h1>
        </template>
        <RouterView />
    </DScrollableContent>
</template>
<style lang="css" scoped></style>
