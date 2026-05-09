import { createApp, h } from 'vue';
import devToolRouter from './devToolRouter';
import DevToolOverlay from '@/dev/DevToolOverlay.vue';

const CONTAINER_ID = '__css-dev-tool__';

const el = document.createElement('div');
el.setAttribute('id', CONTAINER_ID);
el.setAttribute('data-v-inspector-ignore', 'true');
document.getElementsByTagName('body')[0]!.appendChild(el);

const devApp = createApp({
    name: 'dev-tools',
    render: () => h(DevToolOverlay),
    devtools: {
        // hide: true,
    },
})
devApp.use(devToolRouter)
    .mount(el);
