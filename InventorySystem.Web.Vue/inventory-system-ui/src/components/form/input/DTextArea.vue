<script setup lang="ts">
import { useField } from 'vee-validate';

const props = defineProps<{
    name: string;
}>();
const { value, errorMessage } = useField<string | number | readonly string[] | null | undefined>(() => props.name);
</script>
<template>
    <textarea class="input single-line-input" :name="name" v-bind="$attrs" v-model="value" rows="5" />
    <span>{{ errorMessage }}</span>
</template>
<style lang="css" scoped>
@import '../form.css';
</style>
