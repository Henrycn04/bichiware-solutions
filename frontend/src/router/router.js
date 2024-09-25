import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import AllProducts from '@/components/AllProducts.vue'
import AllPerishableProducts from '@/components/AllPerishableProducts.vue'
import AllNonPerishableProducts from '@/components/AllNonPerishableProducts.vue'

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
        path: '/all-products',
        name: 'AllProducts',
        component: AllProducts
    },
    {
        path: '/perishable-products',
        name: 'AllPerishableProducts',
        component: AllPerishableProducts
    },
    {
        path: '/non-perishable-products',
        name: 'AllNonPerishableProducts',
        component: AllNonPerishableProducts
    },
    
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router