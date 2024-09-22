import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import LogIn from '@/components/LogIn.vue'
import ConfirmationPage from '@/components/ConfirmationPage.vue'

const routes = [
    {
        path: '/',
        name: 'LandingPage',
        component: LandingPage
    },
    {
        path: '/email',
        name: 'CorreoEmail',
        component: CorreoEmail
    },
    {
        path: '/login',
        name: 'login',
        component: LogIn
    },
    {
        path: '/confirmation',
        name: 'ConfirmationPage',
        component: ConfirmationPage
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router