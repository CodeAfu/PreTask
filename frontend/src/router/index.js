import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import ErrorView from '@/views/NotFoundView.vue'
import FileView from '@/views/FileView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/files/:id',
      name: 'file',
      component: FileView,
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'notFound',
      component: ErrorView,
      props: (route) => ({
        statusCode: 404,
        message: "Page not found",
      }),
    },
  ],
});

export default router;

