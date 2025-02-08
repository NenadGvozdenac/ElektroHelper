import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import DashboardView from '@/views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import ForumsFrontPageView from '@/views/ForumsFrontPageView.vue'
import ForumView from '@/views/ForumView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: DashboardView,
    },
    {
      path: '/forums',
      name: 'forums',
      component: ForumsFrontPageView,
    },
    {
      path: '/forums/:forumId',
      name: 'forum',
      component: ForumView,
    }
  ],
})

export default router
