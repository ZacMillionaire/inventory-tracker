<script setup lang="ts">
import { useField } from 'vee-validate';

const props = defineProps<{
    name: string;
}>();

const { value, errors } = useField<boolean>(() => props.name);
</script>
<template>
    <input ref="input" type="checkbox" class="input" :name="name" v-bind="$attrs" v-model="value" />
    <span v-for="error in errors" :key="Symbol(error)"> error: {{ error }} </span>
</template>
<style lang="css" scoped>
@import '../form.css';
</style>
