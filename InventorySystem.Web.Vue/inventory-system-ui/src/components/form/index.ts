export { default as DForm } from './DForm.vue';
export { default as DFormRow }from './DFormRow.vue';
export { default as DTextArea } from './input/DTextArea.vue';
export { default as DTextInput } from './input/DTextInput.vue';
// types
export type InputType = string | undefined;
export type ValidationEvents = {
    keyup: (string?: InputType) => { valid: boolean; message?: string };
};
export {type FormSubmit} from './DForm.vue';
