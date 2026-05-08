<script setup lang="ts">
defineProps<{
    inputId: string;
}>();
</script>
<template>
    <div class="field-row">
        <div class="field-label">
            <label :for="inputId" class="label"><slot name="label" /></label>
        </div>
        <div class="field-input">
            <slot />
            <div class="field-hint">
                <slot name="input-hint" />
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
@import './form.css';

.field-row {
    grid-column: 1/-1;
    display: grid;
    grid-template-columns: subgrid;
}

.field-label {
    grid-column: 1/2;
    padding: var(--space-1);
    min-height: var(--input-height);
    display: flex;
    flex-direction: column;
    align-items: flex-end;
}

.label {
    display: block;
    color: var(--input-colour);
}

.field-input {
    grid-column: 2/-1;
    display: flex;
    align-items: stretch;
    flex-direction: column;
    justify-content: center;
}

.field-hint {
    color: var(--input-hint);
    font-size: 0.8rem;
}
</style>
