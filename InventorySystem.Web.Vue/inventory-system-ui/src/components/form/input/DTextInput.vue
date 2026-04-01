<script setup lang="ts">
import { useField } from 'vee-validate';

const props = defineProps<{
    name: string;
}>();

const { value, errorMessage } = useField(() => props.name);
</script>
<template>
    <input ref="input" type="text" class="input single-line-input" :name="name" v-bind="$attrs" v-model="value" />
    <span>
        {{ errorMessage }}
    </span>
</template>
<style lang="css" scoped>
@import '../form.css';
</style>
