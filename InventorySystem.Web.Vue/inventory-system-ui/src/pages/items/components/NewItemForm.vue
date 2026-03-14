<script setup lang="ts">
import { DCard } from '@/components/card';
import { DForm, DFormRow, DTextArea, DTextInput, type FormSubmit, type ValidationEvents } from '@/components/form';
import { computed, ref } from 'vue';

type formInput = {
    itemName?: string;
    description?: string;
};

const formModel = ref<formInput>({});

const formSubmit = (e: FormSubmit) => {
    e.target.checkValidity();
};

// Bit of an ugly way to do form validation but whenever the validationState has any key added/changed,
// this computed recalculates
const formState = computed(() => {
    console.log('recomputing validation state');
    for (const k in validationState.value) {
        if (!validationState.value[k]) {
            return false;
        }
    }
    return true;
});

const validationState = ref<{ [field: string]: boolean }>({
    itemName: false,
});

const validators = {
    itemName: {
        keyup: (input?: string): { valid: boolean; message?: string } => {
            const valid = input !== undefined && input.length > 10;
            const validationMessage = !valid ? 'At least 10 characters' : undefined;

            validationState.value['itemName'] = valid;

            return {
                valid: valid,
                message: validationMessage,
            };
        },
        // TODO: decide if I want to have on submit validators later
        submit: (input?: string): { valid: boolean; message?: string } => {
            if (input && input.length > 10) {
                return {
                    valid: false,
                    message: 'nah just not really aye?',
                };
            }

            return {
                valid: true,
            };
        },
    } as ValidationEvents,
};
</script>
<template>
    <div class="new-item-form">
        <DForm @submit="(e) => formSubmit(e)">
            <DFormRow input-id="name">
                <template #label> Name: </template>
                <DTextInput id="name" placeholder="Name" v-model="formModel.itemName" :validation="validators.itemName" />
                <template #input-hint>10 characters minimum</template>
            </DFormRow>
            <DFormRow input-id="description">
                <template #label> Description: </template>
                <DTextArea id="description" placeholder="Name" v-model="formModel.description" />
                <template #input-hint>Description (optional)</template>
            </DFormRow>
            <template #actions>
                <button :disabled="!formState">Create</button>
            </template>
        </DForm>
    </div>
</template>
<style lang="css" scoped>
.new-item-form {
    padding: var(--space-2);
}
</style>
