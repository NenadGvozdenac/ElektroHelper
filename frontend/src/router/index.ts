import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import DashboardView from '@/views/DashboardView.vue'
import LoginView from '@/views/LoginView.vue'
import ForumsFrontPageView from '@/views/ForumsFrontPageView.vue'
import ForumView from '@/views/ForumView.vue'
import PostView from '@/views/PostView.vue'
import UserProfileView from '@/views/UserProfileView.vue'
import RssFeedView from '@/views/RssFeedView.vue'
import PaymentView from '@/views/PaymentView.vue'
import LearnMoreView from '@/views/LearnMoreView.vue'

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
    },
    {
      path: '/posts/:postId',
      name: 'post',
      component: PostView
    },
    {
      path: '/profile/:userId',
      name: 'profile',
      component: UserProfileView
    },
    {
      path: '/rss',
      name: 'rss',
      component: RssFeedView
    }, 
    {
      path: '/payments',
      name: 'payments',
      component: PaymentView
    },
    {
      path: '/learn-more',
      name: 'learn-more',
      component: LearnMoreView
    }
  ],
})

export default router
