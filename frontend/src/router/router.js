import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
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
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router