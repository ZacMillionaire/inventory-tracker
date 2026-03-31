<script setup lang="ts">
const env = import.meta.env;
</script>
<template>
    <div class="environment-variables">
        <span>Environment</span>
        <pre>{{ env }}</pre>
    </div>
</template>
<style lang="css" scoped>
.environment-variables {
    padding: 5px;
}
</style>
