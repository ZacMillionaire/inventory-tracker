<script setup lang="ts">
import { computed, ref } from 'vue';

const appCssVariables = ref<string[]>([]);
const cssVariables = getComputedStyle(document.documentElement);
// TODO: return a list of all filtered variables starting with a -- for faster lookup
for (const k of cssVariables) {
    if (k.startsWith('--')) {
        appCssVariables.value.push(k);
    }
}

const cssVariable = ref('');

const filteredList = computed(() => {
    if (cssVariable.value === '') {
        return;
    }

    const limitedResults = appCssVariables.value.filter((x) => x.startsWith(cssVariable.value));

    return limitedResults.splice(0, 20);

    // const value = cssVariables.getPropertyValue(cssVariable.value);
    // return value === '' ? 'not found' : value;
});

const currentVariables = ref<{ [key: string]: { value: string; name: string; inputType: 'text' | 'range' | 'color' } }>({});

const addVariable = () => {
    if (!cssVariableSearchResult.value.found) {
        return;
    }

    currentVariables.value[cssVariable.value] = {
        value: cssVariableSearchResult.value.value!,
        name: cssVariableSearchResult.value.name!,
        inputType: 'text',
    };
    cssVariable.value = '';
    cssVariableSearchResult.value = { found: false };
};

type cssSearchResult = {
    found: boolean;
    name?: string;
    value?: string;
};

// TODO: this should only search the filtered variables starting with a --
const findVariable = (varName: string) => {
    const res: cssSearchResult = {
        found: false,
    };

    if (varName !== '') {
        const cssPropertyValue = cssVariables.getPropertyValue(varName);

        if (cssPropertyValue !== '') {
            res.found = true;
            res.name = varName;
            res.value = cssPropertyValue;
        }
    }

    cssVariableSearchResult.value = res;
};

const cssVariableSearchResult = ref<cssSearchResult>({
    found: false,
});

const updateVariable = (cssVariable: string, value: string) => {
    document.documentElement.style.setProperty(cssVariable, value);
};
</script>
<template>
    <div class="css-editor">
        <div class="search">
            <input type="text" v-model="cssVariable" />
            <ul>
                <li v-for="filter in filteredList" :key="filter" @click="() => (cssVariable = filter)">{{ filter }}</li>
            </ul>
            <div class="buttons">
                <button @click="findVariable(cssVariable)">find</button>
                <button @click="addVariable">add</button>
            </div>
            <div v-if="cssVariableSearchResult.found">
                <div>{{ cssVariableSearchResult.name }}</div>
                <div>{{ cssVariableSearchResult.value }}</div>
            </div>
        </div>
        <div>
            <div v-for="addedVariable in currentVariables" :key="addedVariable.name">
                {{ addedVariable.name }}
                <select v-model="addedVariable.inputType">
                    <option value="text">Text</option>
                    <option value="range">Slider</option>
                    <option value="color">Colour</option>
                </select>
                <input :type="addedVariable.inputType" v-model="addedVariable.value" /><button @click="updateVariable(addedVariable.name, addedVariable.value)">update</button>
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
.css-editor {
    border: none;
    border-left: 1px solid #fff;
    border-bottom: 1px solid #fff;
    border-bottom-left-radius: 5px;
    background-clip: border-box;
    padding: 8px;
}
</style>
<style>
#__css-dev-tool__ {
    position: absolute;
    top: 0;
    right: 0;
    background-color: #000;
}
</style>
