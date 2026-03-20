import { createApp, h } from 'vue';
import ViteCssPlugin from './ViteCssPlugin.vue';

export const installViteCssPlugin = () =>{

    // TODO: continue this CSS variable editor in place later
    return;
    const CONTAINER_ID = '__css-dev-tool__';
    const el = document.createElement('div');
    el.setAttribute('id', CONTAINER_ID);
    el.setAttribute('data-v-inspector-ignore', 'true');
    document.getElementsByTagName('body')[0]!.appendChild(el);
    const app = createApp({
        render: () => h(ViteCssPlugin),
        devtools: {
        hide: true,
        },
    })
    app.mount(el);
}
