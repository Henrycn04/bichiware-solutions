<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
            <div class="header__search">
                <input type="text"
                       v-model="searchQuery"
                       placeholder="Buscar"
                       @keydown.enter="performSearch" />
                <button @click="performSearch" class="header__search__button">
                    <img src="../assets/SearchIcon.png" alt="Buscar" />
                </button>
            </div>
            <div class="header__actions">
                <a @click="isLoggedIn ? null : goToLogin()" class="header__login">
                {{ isLoggedIn ? '' : 'Accesar' }}
                </a>
                <div v-if="isLoggedIn" class="header__profile-container" ref="profileContainer">
                    <button @click="toggleProfileMenu" class="header__profile">
                        <img src="../assets/ProfileIcon.png" alt="Perfil" />
                    </button>
                    <div v-if="isProfileMenuVisible" class="header__profile-menu">
                        <router-link to="/userProfile" class="header__profile-menu-item" style="color: #463a2e">Cuenta</router-link>
                        <div v-if="isAdminOrEntrepreneur">
                        <router-link to="/companyRegistration" class="header__profile-menu-item" style="color: #463a2e">Registro empresa</router-link>
                        </div>
                        <a v-if="isAdminOrEntrepreneur" @click="toggleCompaniesDropdown" class="header__profile-menu-item" style="color: #463a2e; cursor: pointer">
                            Ver empresas
                        </a>
                        <ul v-if="isAdminOrEntrepreneur && isCompaniesDropdownVisible">
                            <li v-for="company in userCompanies" :key="company.companyID" @click="selectCompany(company.companyID)">
                                {{ company.companyName }}
                            </li>
                        </ul>
                        <a @click=goTologout href="/" class="header__profile-menu-item" style="color: #463a2e">Salir</a>
                    </div>  
                    <button @click="goToCart" class="header__cart">
                        <img src="../assets/CartIcon.png" alt="Carrito" />
                    </button>
                </div> 
            </div>
        </header>
        <main class="main-content">
            <div class="subheader">
                <a href="/all-products" class="element_button">
                    <img src="../assets/AllIcon.png" style="width: 24px; height: 24px; cursor: pointer;" alt="Todos" />
                    <div>&nbsp; Todos los productos</div>
                </a>
                <a href="/non-perishable-products">No perecederos</a>
                <a href="/perishable-products">Perecederos</a>
                <a v-if="this.isAdminOrEntrepreneur" href="/users-list">Lista de usuarios</a>
                <a v-if="this.isAdminOrEntrepreneur" href="/companies-list">Lista de empresas</a>
                <div>
                    <a v-if="this.isAdmin" href="/pendingOrders">Pedidos pendientes</a>
                </div>
            </div>

            <div v-if="this.isLoggedInVar && this.userTypeNumber === 1" class="logged-in-section">
                <div class="container-fluid bg-light">
                    <div class="row">

                        <div class="col-lg-5 col-md-5 p-3 d-flex flex-column">
                            <h5>Productos</h5>
                            <div class="bg-white p-3 rounded border shadow-sm">
                                <ul class="list-unstyled">
                                    <!-- Muestra de productos -->
                                </ul>
                            </div>
                        </div>


                        <div class="col-lg-7 col-md-7 d-flex flex-column">
                            <div class="row">
                                <div class="col-10 mb-5">
                                    <h5>Órdenes en progreso</h5>
                                    <div class="bg-white p-4 rounded border shadow-sm h-100">
                                        <OrdersList :orders="ordersListed" />      
                                    </div>
                                </div>


                                <div class="col-10 mb-5">
                                    <h5>Comprar de nuevo</h5>
                                    <div class="bg-white p-4 rounded border shadow-sm h-100">
                                        <ProductList :products="productsListed" /> 
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>




            </div>

        </main>
        <footer class="footer">
            <div class="footer_columns">
                <div class="footer__column">
                    <strong>Contacto</strong>
                    <router-link to="/email">Correo</router-link>
                </div>
                <div class="footer__column">
                    <strong>Inscripción</strong>
                    <a href="/login">Inicio de sesión</a>
                    <a href="/register">Registro</a>
                </div>
                <div class="footer__column">
                    <strong>Créditos</strong>
                    <a href="/creators">Creadores</a>
                </div>
            </div>
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import commonMethods from '@/mixins/commonMethods';
    import axios from "axios";
    import OrdersList from './OrdersList.vue';
    import ProductList from './ProductList.vue';
    import { mapGetters, mapState } from 'vuex';

    export default {
        mixins: [commonMethods],
        components: {
            OrdersList, 
            ProductList
        },
        computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
        },
        data() {
            return {
                ordersListed: [],
                productsListed: []
            }
        },
        methods: {
            getOrdersInProgress() {
                axios.get(`${this.$backendAddress}api/ClientDashboard/getOrdersInProgress/${this.userCredentials.userId}`)
                .then((response) => {
                    if (typeof response.data === "string") {
                        console.warn(response.data, this.userCredentials.userId);
                        this.ordersListed = [];
                    } else {
                        console.warn(response.data);
                        this.ordersListed = response.data;
                        console.log(this.ordersListed, this.userCredentials.userId);
                    }                })
                .catch((error) => {
                    console.error("Error obtaining cart products:", error);
                });
            },
        getLastProductsOrdered() {
            axios.get(`${this.$backendAddress}api/ClientDashboard/getLastProductsOrdered/${this.userCredentials.userId}`)
                .then((response) => {

                    if (typeof response.data === "string") {
                        console.warn(response.data, this.userCredentials.userId);
                        this.productsListed = []; 
                    } else {
                        this.productsListed = response.data.map(product => ({
                    ...product, 
                    productionLimit: product.limit, 
                    productName: product.name 
                }));
                    }
                })
                .catch((error) => {
                    console.error("Error obtaining products:", error);
                    this.productsListed = [];  
                });
        },
    },
        mounted() {
            if (this.isLoggedInVar && this.userTypeNumber === 3) {
                this.getOrdersInProgress();
                this.getLastProductsOrdered();
            } else if (this.isLoggedInVar && this.userTypeNumber === 2) {
                this.getOrdersInProgress();
                this.getLastProductsOrdered();
            } else if (this.isLoggedInVar && this.userTypeNumber === 1) {
                this.getOrdersInProgress();
                this.getLastProductsOrdered();
            }
        },
    };
</script>

<style>

.col-10 {
    height: calc(50vh - 100px);    
}
</style>
