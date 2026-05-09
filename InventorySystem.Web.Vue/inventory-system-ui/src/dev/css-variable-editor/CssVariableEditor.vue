<script setup lang="ts">
import { computed, ref } from 'vue';
import CssValueRange from './CssValueRange.vue';
import { DScrollableContent } from '@/components/layout';

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

type UnitType = 'px' | 'rem' | 'em' | '%';
export interface CssVariable {
    name: string;
    value: number;
    unit: UnitType;
    type: 'size';
}

const currentVariables = ref<{ [key: string]: CssVariable }>({
    '--space-1': {
        name: '--space-1',
        type: 'size',
        unit: 'px',
        value: 4,
    },
    '--small-font': {
        name: '--small-font',
        type: 'size',
        unit: 'rem',
        value: 0.75,
    },
});

const addVariable = () => {
    if (!cssVariableSearchResult.value.found || !cssVariableSearchResult.value.value || !cssVariableSearchResult.value.name) {
        return;
    }

    const type = determineCssType(cssVariableSearchResult.value.name, cssVariableSearchResult.value.value);

    currentVariables.value[cssVariable.value] = type;

    // clear the search form and previously found css variable
    // TODO: cleanup/remove this statefulness
    cssVariable.value = '';
    cssVariableSearchResult.value = { found: false };
};

const determineCssType = function (name: string, cssValue: string): CssVariable {
    const pixelRegex = /^(\d+\.?\d+?)(px|em|rem)$/;
    if (pixelRegex.test(cssValue)) {
        const matches = cssValue.match(pixelRegex)!;

        // we bang the indexes on matches as we've already asserted that both groups matched via
        // the test above so we can trust they'll have a value
        return {
            name: name,
            value: parseInt(matches[1]!, 10),
            // technically not great to do this, but the regex should have ensured that the unit
            // we matched on is part of this union type
            unit: matches[2]! as UnitType,
            type: 'size',
        };
    }
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

const updateVariable = (
    cssVariable: string,
    value: {
        value: number;
        unit: string;
        computed: string;
    },
) => {
    if (!currentVariables.value[cssVariable]) {
        return;
    }
    currentVariables.value[cssVariable].value = value.value;
    currentVariables.value[cssVariable].unit = value.unit as UnitType;

    document.documentElement.style.setProperty(cssVariable, value.computed);
};
</script>
<template>
    <DScrollableContent>
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
            <div class="inputs">
                <template v-for="addedVariable in currentVariables" :key="addedVariable.name">
                    <CssValueRange v-if="addedVariable.type === 'size'" :model-value="addedVariable" @value-change="(e) => updateVariable(addedVariable.name, e)" />
                </template>
            </div>
        </div>
    </DScrollableContent>
</template>
<style lang="css" scoped>
.css-editor {
    /* border: none;
    border-left: 1px solid #fff;
    border-bottom: 1px solid #fff;
    border-bottom-left-radius: 5px;
    background-clip: border-box;
    width: 20%; */
    padding: 8px;
    display: flex;
    flex-direction: column;
    max-width: 300px;
}
.inputs {
    display: flex;
    flex-direction: column;
    flex: 1 1 auto;
}
</style>
