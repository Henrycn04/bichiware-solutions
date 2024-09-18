import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import InicioSesion from '@/components/InicioSesion.vue'
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
        component: InicioSesion
    },
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router