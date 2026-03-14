<script setup lang="ts">
import { DCard } from '@/components/card';
</script>
<template>
    <DCard>
        <form @submit.prevent>
            <div>
                <label for="name">Name:</label>
                <input type="text" id="name" />
            </div>
            <button>Create</button>
        </form>
    </DCard>
</template>
<style lang="css" scoped></style>
