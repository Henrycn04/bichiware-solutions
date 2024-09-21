import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import CompanyRegistration from '@/components/CompanyRegistration.vue'
import MapFromAddress from '@/components/MapForAddress.vue'
import CompanyProfile from '@/components/CompanyProfile.vue'
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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router