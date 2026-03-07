<script setup lang="ts">
import { DCard, DCardContainer } from '@/components/card';
</script>
<template>
    <DCardContainer columns="2">
        <DCard variant="accent">
            <template #header>
                <div class="header">
                    <h1><code>variant="accent"</code></h1>
                </div>
            </template>
            <div class="content">This is the default variant style for a <kbd>DCard</kbd>, but if desired <kbd>variant="accent"</kbd> can be used to be explicit</div>
        </DCard>
        <DCard variant="gray">
            <template #header>
                <div class="header">
                    <h1><code>variant="gray"</code></h1>
                </div>
            </template>
            <div class="content">This is the variant style for a <kbd>DCard</kbd> with <kbd>variant="gray"</kbd></div>
        </DCard>
    </DCardContainer>
    <h1>Stacking</h1>
    <DCardContainer columns="2">
        <DCard variant="accent">
            <template #header>
                <div class="header">
                    <h1><code>variant="accent"</code></h1>
                </div>
            </template>
            <div class="content">
                <DCard variant="accent">
                    <template #header>
                        <div class="header">
                            <h1><code>variant="accent"</code></h1>
                        </div>
                    </template>
                    <div class="content">
                        <DCard variant="accent">
                            <template #header>
                                <div class="header">
                                    <h1><code>variant="accent"</code></h1>
                                </div>
                            </template>
                            <div class="content">
                                <DCard variant="accent">
                                    <template #header>
                                        <div class="header">
                                            <h1><code>variant="accent"</code></h1>
                                        </div>
                                    </template>
                                    <div class="content">Cards can be stacked and the colour for each layer is additive to the one below it</div>
                                </DCard>
                            </div>
                        </DCard>
                    </div>
                </DCard>
            </div>
        </DCard>
        <DCard variant="gray">
            <template #header>
                <div class="header">
                    <h1><code>variant="gray"</code></h1>
                </div>
            </template>
            <div class="content">
                <DCard variant="gray">
                    <template #header>
                        <div class="header">
                            <h1><code>variant="gray"</code></h1>
                        </div>
                    </template>
                    <div class="content">
                        <DCard variant="gray">
                            <template #header>
                                <div class="header">
                                    <h1><code>variant="gray"</code></h1>
                                </div>
                            </template>
                            <div class="content">
                                <DCard variant="gray">
                                    <template #header>
                                        <div class="header">
                                            <h1><code>variant="gray"</code></h1>
                                        </div>
                                    </template>
                                    <div class="content">Cards can be stacked and the colour for each layer is additive to the one below it</div>
                                </DCard>
                            </div>
                        </DCard>
                    </div>
                </DCard>
            </div>
        </DCard>
    </DCardContainer>
</template>
<style lang="css" scoped>
.header {
    padding: var(--space-2);
}
.content {
    padding: var(--space-2);
}
kbd {
    border: 1px solid var(--gray-7);
    background-color: var(--gray-4);
    border-radius: 3px;
    display: inline-block;
    padding: 0 2px;
}
</style>
