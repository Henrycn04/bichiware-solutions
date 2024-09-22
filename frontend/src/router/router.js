import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import LogIn from '@/components/LogIn.vue'
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
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router