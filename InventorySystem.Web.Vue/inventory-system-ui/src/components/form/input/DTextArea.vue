<script setup lang="ts">
const model = defineModel<string | number | readonly string[] | null | undefined>();
</script>
<template>
    <textarea class="input single-line-input" v-bind="$attrs" v-model="model" rows="5" />
</template>
<style lang="css" scoped>
@import '../form.css';
</style>
