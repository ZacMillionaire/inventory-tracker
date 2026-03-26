<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import type { CssVariable } from './CssVariableEditor.vue';

const props = defineProps<{ modelValue: CssVariable }>();

// Create a local reference of the props so we don't mutate anything that is passed in but
// we can still react to via v-model.
// Even though the parent often updates the value passed in, preference is that the owner of the
// object passed to the props is responsible for mutating it
const model = ref<CssVariable>({ ...props.modelValue });

const range = ref({
    min: 0,
    max: 50,
    step: 1,
});

onMounted(() => {
    if (props.modelValue.unit === 'rem') {
        range.value.min = 0.1;
        range.value.max = 3;
        range.value.step = 0.01;
    }
});

const emits = defineEmits<{
    valueChange: [
        {
            value: number;
            unit: string;
            computed: string;
        },
    ];
}>();

const outputValue = computed(() => `${model.value.value}${model.value.unit}`);

const valueUpdated = function () {
    emits('valueChange', {
        value: model.value.value,
        unit: model.value.unit,
        computed: outputValue.value,
    });
};

const reset = function () {
    model.value.value = props.modelValue.value;
    model.value.unit = props.modelValue.unit;
    emits('valueChange', {
        value: model.value.value,
        unit: model.value.unit,
        computed: outputValue.value,
    });
};
</script>
<template>
    <div class="container">
        <div class="header">
            <span>{{ model.name }}</span>
            <span>{{ props.modelValue.value }}{{ props.modelValue.unit }} </span>
        </div>
        <div class="input-container">
            <input class="range-slider" type="range" v-model.number="model.value" :min="range.min" :max="range.max" :step="range.step" @input="valueUpdated" />
            <div class="range-accessible">
                <input class="input input-value" type="numeric" v-model.number="model.value" @change="valueUpdated" />
                <input class="input input-unit" type="text" v-model="model.unit" @change="valueUpdated" />
                <button @click="reset">Reset</button>
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
.container {
    display: flex;
    flex-direction: column;
}
.header {
    display: flex;
    flex-direction: row;
    gap: 5px;
}
.input-container {
    flex: 1 1 auto;
    display: grid;
    grid-template-columns: 50% 50%;
}
.range-slider {
    grid-column: 1;
}
.range-accessible {
    grid-column: 2;
    display: flex;
    flex-direction: row;
}
.input {
    width: 100%;
}
</style>
