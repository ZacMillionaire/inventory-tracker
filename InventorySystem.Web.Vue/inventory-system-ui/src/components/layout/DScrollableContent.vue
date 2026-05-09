<script setup lang="ts"></script>
<template>
    <div class="container">
        <div class="header">
            <slot name="header" />
        </div>
        <div class="content">
            <slot />
        </div>
    </div>
</template>
<style lang="css" scoped>
.container {
    height: 100%;
    display: flex;
    flex-direction: column;
}
.header {
    /* height: 3rem; */
}
.content {
    overflow: auto;
}
</style>
