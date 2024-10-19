import { createRouter, createWebHistory } from 'vue-router'
import LandingPage from '@/components/LandingPage.vue'
import CorreoEmail from '@/components/CorreoEmail.vue'

import AllProducts from '@/components/AllProducts.vue'
import AllPerishableProducts from '@/components/AllPerishableProducts.vue'
import AllNonPerishableProducts from '@/components/AllNonPerishableProducts.vue'
import ModifyCompanyData from '@/components/ModifyCompanyData.vue'
import CompanyRegistration from '@/components/CompanyRegistration.vue'
import MapFromAddress from '@/components/MapForAddress.vue'
import CompanyProfile from '@/components/CompanyProfile.vue'
import UserProfile from '@/components/UserProfile.vue'
import LogIn from '@/components/LogIn.vue'
import ConfirmationPage from '@/components/ConfirmationPage.vue'
import ChangeAccountType from '@/components/ChangeAccountType.vue'
import ChangePassword from '@/components/ChangePassword.vue'
import AddProductPage from '@/components/AddProductPage.vue'
import CompanyInventory from '@/components/CompanyInventory.vue'
import registerAccount from '@/components/registerAccount.vue'
import CreditsPage from '@/components/CreditsPage.vue'
import ListUsers from '@/components/ListUsers.vue'
import ListCompanies from '@/components/ListCompanies.vue'
import ListAddresses from '@/components/ListAddresses.vue'
import AddDeliveryPage from '@/components/AddDeliveryPage.vue'
import AddAddress from '@/components/AddAddress.vue'



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
        name: 'CompanyProfile',
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
        path: '/changeAccountType',
        name: 'ChangeAccountType',
        component: ChangeAccountType

    },
    {
        path: '/changePassword',
        name: 'ChangePassword',
        component: ChangePassword
    },
    {
        path: '/add-product',
        name: 'AddProductPage',
        component: AddProductPage
    },
    {
        path: '/register',
        name: 'registerAccount',
        component: registerAccount
    },
    {
        path: '/creators',
        name: 'Credits',
        component: CreditsPage
    },
    {
        path: '/users-list',
        name: 'List of Users',
        component: ListUsers
    },
    {
        path: '/companies-list',
        name: 'List of Companies',
        component: ListCompanies
    },
    {
        path: '/addresses-list',
        name: 'List of addresses',
        component: ListAddresses
    },
    {
        path: '/add-delivery',
        name: 'AddDeliveryPage',
        component: AddDeliveryPage
    },
    {
        path: '/addAddress',
        name: 'AddAddress',
        component: AddAddress
    },
    {
        path: '/companyInventory',
        name: 'CompanyInventory',
        component: CompanyInventory

    },
    {
        path: '/modifyCompanyData',
        name: 'ModifyCompanyData',
        component: ModifyCompanyData

    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
