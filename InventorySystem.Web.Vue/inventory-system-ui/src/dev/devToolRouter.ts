import { StyleGuideRoutes } from '@/dev/style-guide';
import { createMemoryHistory, createRouter } from 'vue-router';
import { DevToolHomeRoutes } from '@/dev/index';
import { CssVariableEditorRoutes } from './css-variable-editor';

const devToolRoutes = [ ...DevToolHomeRoutes, ...StyleGuideRoutes, ...CssVariableEditorRoutes];
const devToolRouter = createRouter({
    // We use memory routing for the plugin as we don't want it to have URL routes
    history: createMemoryHistory('/dev-tools/'),
    routes: devToolRoutes,
});

export default devToolRouter;
