import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'

import AllProducts from '@/components/AllProducts.vue'
import AllPerishableProducts from '@/components/AllPerishableProducts.vue'
import AllNonPerishableProducts from '@/components/AllNonPerishableProducts.vue'

import CompanyRegistration from '@/components/CompanyRegistration.vue'
import MapFromAddress from '@/components/MapForAddress.vue'
import CompanyProfile from '@/components/CompanyProfile.vue'
import LogIn from '@/components/LogIn.vue'
import ConfirmationPage from '@/components/ConfirmationPage.vue'
import AddProductPage from '@/components/AddProductPage.vue'
import AddDeliveryPage from '@/components/AddDeliveryPage.vue'


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
    
    {
        path: '/companyRegistration',
        name: 'CompanyRegistration',
        component: CompanyRegistration
    },
    {
        path: '/mapForAddress',
        name: 'mapForAddress',
        component: MapFromAddress
    },
    {
        path: '/companyProfile',
        name: 'companyProfile',
        component: CompanyProfile
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
    {
        path: '/add-product',
        name: 'AddProductPage',
        component: AddProductPage

    },
    {
        path: '/add-delivery',
        name: 'AddDeliveryPage',
        component: AddDeliveryPage

    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router