import { URL, fileURLToPath } from 'node:url';

import { defineConfig, PluginOption } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueDevTools from 'vite-plugin-vue-devtools';

export const DevPlugin = () : PluginOption =>({
    name: 'dev-tools',
    apply: 'serve',

    transformIndexHtml(html) {
        return {
            html,
            tags : [{
                tag: 'script',
                injectTo: 'head',
                attrs: {
                    type: 'module',
                    src: '/src/dev/devToolPlugin.ts',
                },
            }]
        }
    },
});

// https://vite.dev/config/
export default defineConfig({
    plugins: [vue(), vueDevTools(), DevPlugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url)),
            '@pages': fileURLToPath(new URL('./src/pages', import.meta.url)),
            '@components': fileURLToPath(new URL('./src/components', import.meta.url)),
        },
    },
});
