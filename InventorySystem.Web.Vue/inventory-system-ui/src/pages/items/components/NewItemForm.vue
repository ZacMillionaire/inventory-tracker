<script setup lang="ts">
import { ItemRepository } from '@/api/items';
import { CreateItemRequestValidators } from '@/api/items/dtos/CreateItemRequestDto';
import { DForm, DFormRow, DTextArea, DTextInput } from '@/components/form';
import { useForm } from 'vee-validate';
import * as z from 'zod';

const { handleSubmit, resetForm, meta } = useForm<{
    name: string;
    description?: string;
}>({
    validationSchema: {
        name: (name: string) => {
            if (name === undefined) {
                return;
            }
            try {
                // validate against the name rule
                const validated = CreateItemRequestValidators.name.parse(name);
            } catch (error) {
                // if the exception was an error
                if (error instanceof z.ZodError) {
                    console.log(error.issues);
                    // return the array of errors for the form row to loop over and display
                    return error.issues.map((x) => x.message);
                }
            }
            return true;
            const valid = name !== undefined && name.length > 10;
            if (!valid) {
                return 'At least 10 characters';
            }

            return true;
        },
    },
});

const formSubmit = handleSubmit(async (e) => {
    await ItemRepository().CreateItem({
        name: e.name,
        description: e.description,
    });
    resetForm();
    emits('itemCreated');
});

const emits = defineEmits<{
    itemCreated: [];
}>();
</script>
<template>
    <div class="new-item-form">
        <DForm @submit="(e) => formSubmit(e)">
            <DFormRow input-id="name">
                <template #label> Name* </template>
                <DTextInput name="name" placeholder="Name" />
                <template #input-hint>10 characters minimum</template>
            </DFormRow>
            <DFormRow input-id="description">
                <template #label> Description </template>
                <DTextArea id="description" name="description" placeholder="Description (optional)" />
            </DFormRow>
            <template #actions>
                <button :disabled="!meta.valid">Create</button>
            </template>
        </DForm>
    </div>
</template>
<style lang="css" scoped>
.new-item-form {
    padding: var(--space-2);
    max-width: 1024px;
    flex: 1 1 auto;
}
</style>
