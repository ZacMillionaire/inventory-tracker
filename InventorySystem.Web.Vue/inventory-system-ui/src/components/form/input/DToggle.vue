<script setup lang="ts">
import { useField } from 'vee-validate';

const props = defineProps<{
    name: string;
}>();

const { value, errors } = useField<boolean>(() => props.name);
</script>
<template>
    <div class="toggle-wrapper">
        <input ref="input" type="checkbox" class="toggle" role="switch" :name="name" v-bind="$attrs" v-model="value" />
        <label :for="name">
            <slot />
        </label>
    </div>
    <span v-for="error in errors" :key="Symbol(error)"> error: {{ error }} </span>
</template>
<style lang="css" scoped>
@import '../form.css';

.toggle-wrapper {
    display: flex;
    position: relative;
    align-items: center;
    gap: var(--space-2);
}

.toggle {
    height: var(--input-height);
    width: var(--input-height);
}
</style>
