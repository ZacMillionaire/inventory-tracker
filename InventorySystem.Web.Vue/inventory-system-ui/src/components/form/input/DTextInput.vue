<script setup lang="ts">
import { useTemplateRef } from 'vue';
import type { InputType, ValidationEvents } from '..';

const model = defineModel<InputType>();
const props = defineProps<{
    validation?: ValidationEvents;
}>();

const validate = (validationFunc: (string: InputType) => { valid: boolean; message?: string }, input: InputType) => {
    if (!props.validation || !inputRef.value) {
        return;
    }
    const validationResult = validationFunc(input);

    inputRef.value.setCustomValidity(validationResult.message ?? '');
    inputRef.value.reportValidity();
};

const inputRef = useTemplateRef('input');
</script>
<template>
    <input ref="input" type="text" class="input single-line-input" v-bind="$attrs" v-model="model" v-on:keyup="validation?.keyup ? validate(validation?.keyup, model) : undefined" />
</template>
<style lang="css" scoped>
@import '../form.css';
</style>
