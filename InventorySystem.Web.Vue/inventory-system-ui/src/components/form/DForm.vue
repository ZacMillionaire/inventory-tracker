<script lang="ts">
export type FormSubmit = SubmitEvent & { target: HTMLFormElement };
</script>
<script setup lang="ts">
const emits = defineEmits<{
    submit: [payload: FormSubmit];
}>();
</script>
<template>
    <div class="form-container">
        <form @submit.prevent="(e) => emits('submit', e as FormSubmit)">
            <slot />
            <div class="form-actions">
                <slot name="actions" />
            </div>
        </form>
    </div>
</template>
<style lang="css" scoped>
@import './form.css';
.form-container {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: var(--space-2);
}
.form-actions {
    grid-column: 2/-1;
    display: flex;
    align-items: flex-end;
    flex-direction: column;
}
</style>
