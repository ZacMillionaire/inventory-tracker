<script setup lang="ts">
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
import { StyleGuideRouteNames } from './style-guide';
import { DevToolHomeRouteNames } from './index';
import { CssVariableEditorRouteNames } from './css-variable-editor';
import { ref } from 'vue';
const toolVisible = ref<boolean>(false);
const toggleToolVisibility = function () {
    toolVisible.value = !toolVisible.value;
};
</script>
<template>
    <div class="dev-tool-container full-height-no-scroll">
        <div class="full-height-hack-container full-height-no-scroll">
            <div class="container-handle" @click="toggleToolVisibility">
                {{ toolVisible ? 'hide' : 'show' }}
            </div>
            <div class="tool-content full-height-no-scroll" v-if="toolVisible">
                <DHorizontalNavigationBar class="navigation" align-end>
                    <DHorizontalNavigationLink :to="{ name: DevToolHomeRouteNames.Index }"> Index </DHorizontalNavigationLink>
                    <DHorizontalNavigationLink :to="{ name: CssVariableEditorRouteNames.Index }"> CSS Variable </DHorizontalNavigationLink>
                    <DHorizontalNavigationLink :to="{ name: StyleGuideRouteNames.styleGuide.index }"> Style Guide </DHorizontalNavigationLink>
                </DHorizontalNavigationBar>
                <div class="content full-height-no-scroll">
                    <RouterView />
                </div>
            </div>
        </div>
    </div>
</template>
<style lang="css" scoped>
.dev-tool-container {
    position: absolute;
    top: 0;
    right: 0;
    display: flex;
    flex-direction: row-reverse;
    align-items: start;
}
/*
Wrapper to make the contents take up the whole width, but also keep the tool toggle visible.

Not really a hack, I just can't think of a better name currently lol
*/
.full-height-hack-container {
    display: flex;
    flex-direction: row;
    align-items: start;
}
.container-handle {
    background-color: rgba(200 200 200 /0.1);
    padding: 5px;
    border-bottom-left-radius: 5px;
    border-width: 0 0 1px 1px;
    border-style: solid;
    border-color: rgba(200 200 200 /0.1);
}

.tool-content {
    border-left: 1px solid rgba(200 200 200 /0.1);
    background-color: #1a1a1a;
    display: flex;
    flex-direction: column;
}
.navigation {
}
.content {
}
</style>
<style>
#__css-dev-tool__ {
}
</style>
