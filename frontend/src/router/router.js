import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'
import CompanyRegistration from '@/components/CompanyRegistration.vue'
import MapFromAddress from '@/components/MapForAddress.vue'
import CompanyProfile from '@/components/CompanyProfile.vue'
import UserProfile from '@/components/UserProfile.vue'
import LogIn from '@/components/LogIn.vue'
import ConfirmationPage from '@/components/ConfirmationPage.vue'
import UserAddresses from '@/components/UserAddresses.vue'
import ChangeAccountType from '@/components/ChangeAccountType.vue'


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
        path: '/userProfile',
        name: 'UserProfile',
        component: UserProfile

    },
    {
        path: '/userAddresses/:userID',
        name: 'UserAddresses',
        component: UserAddresses

    },
    {
        path: '/changeAccountType',
        name: 'ChangeAccountType',
        component: ChangeAccountType

    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router