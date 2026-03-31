<script setup lang="ts">
import { DCard, DCardContainer } from '@/components/card';
import { DHorizontalNavigationBar, DHorizontalNavigationLink } from '@/components/navigation';
const sample = `<DHorizontalNavigationBar>
    <DHorizontalNavigationLink :to="{ params: { example: 'link-1' } }"> Link 1 </DHorizontalNavigationLink>
    <DHorizontalNavigationLink :to="{ params: { example: 'link-2' } }"> Link 2 </DHorizontalNavigationLink>
    <DHorizontalNavigationLink :to="{ params: { example: 'link-3' } }"> Link 3 </DHorizontalNavigationLink>
    <DHorizontalNavigationLink :to="{ params: { example: 'link-4' } }"> Link 4 </DHorizontalNavigationLink>
</DHorizontalNavigationBar>
`;
</script>
<template>
    <div class="content">
        <DCardContainer>
            <DCard class="card" padding="3">
                <template #header> </template>
                <div class="description">
                    <div>Can be used in cards or nested in any element, but cannot be used as tabs:</div>
                    <DHorizontalNavigationBar class="nav">
                        <DHorizontalNavigationLink :to="{ params: { example: 'link-1' } }"> Link 1 </DHorizontalNavigationLink>
                        <DHorizontalNavigationLink :to="{ params: { example: 'link-2' } }"> Link 2 </DHorizontalNavigationLink>
                        <DHorizontalNavigationLink :to="{ params: { example: 'link-3' } }"> Link 3 </DHorizontalNavigationLink>
                        <DHorizontalNavigationLink :to="{ params: { example: 'link-4' } }"> Link 4 </DHorizontalNavigationLink>
                    </DHorizontalNavigationBar>
                    <DCard variant="gray" padding="2">
                        <pre>{{ sample }}</pre>
                    </DCard>
                </div>
            </DCard>
        </DCardContainer>
    </div>
</template>
<style lang="css" scoped>
.content {
    margin: var(--space-2);
}
.description {
    display: flex;
    flex-direction: column;
    gap: var(--space-2);
}
.card {
    margin: var(--space-2);
}
.nav {
    border: 1px solid var(--component-border-acccent);
}
</style>
