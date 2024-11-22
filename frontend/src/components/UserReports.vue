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

            <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
                <div class="col-12 col-md-6 d-flex justify-content-center">
                    <div class="w-100" style="max-width: 40%;"> <!-- Limitar el ancho al 30% -->
                        <h5 for="companySelect" style="display: block; margin-top: 8px;">Reporte a desplegar</h5>
                    </div>
                </div>
                <select v-model="selectedStatus" @change="changeComponent" class="dropdown-select">
                    <option value="completados">Reporte de Órdenes Completadas</option>
                    <option value="cancelados">Reporte de Órdenes Cancelados</option>
                    <option value="pendientes">Reporte de Órdenes Pendientes</option>
                    <option value="ganancias">Reporte de Reporte de Ganancias</option>
                    <option value="completedClient">Reporte de Ordenes completadas (Cliente)</option>
                    <option value="clientInProgress">Reporte de Ordenes en progreso (Cliente)</option>
                    <option value="clientCancelled">Reporte de Ordenes canceladas (Cliente)</option>
                </select>

                <component :is="currentComponent" />
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
    import CompletedOrdersReport from './CompletedOrdersReport.vue';
    import CompletedClientReport from './CompletedClientReport.vue';
    import InProgressClientReport from './InProgressClientReport.vue';
    import CancelledClientReport from './CancelledClientReport.vue';
    import { mapGetters, mapState } from 'vuex';

    export default {
        mixins: [commonMethods],
        components: {
            CompletedOrdersReport,
            CompletedClientReport,
            InProgressClientReport,
            CancelledClientReport
        },
        computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
        },
        data() {
            return {
                selectedStatus: 'completed',
                currentComponent: ''
            };
        },
        methods: {
            changeComponent() {
            switch (this.selectedStatus) {
                case 'completados':
                this.currentComponent = 'CompletedOrdersReport';
                break;
                case 'cancelados':
                this.currentComponent = 'CompletedOrdersReport';
                break;
                case 'pendientes':
                this.currentComponent = 'CompletedOrdersReport';
                break;
                case 'ganancias':
                this.currentComponent = 'CompletedOrdersReport';
                break;
                case 'completedClient':
                this.currentComponent = 'CompletedClientReport'
                break;
                case 'clientInProgress':
                this.currentComponent = 'InProgressClientReport'
                break;
                case 'clientCancelled':
                this.currentComponent = 'CancelledClientReport'
                break;
                default:
                this.currentComponent = '';
            }
            }
        },
        mounted() {
        },
    };
</script>

<style>
.dropdown-select {
  font-size: 1.5rem; /* Ajusta el tamaño de la fuente para que sea igual al de h2 */
  padding: 0.5rem 1rem; /* Agrega relleno para que el tamaño sea similar */
  border-radius: 0.375rem; /* Redondea los bordes */
  border: 1px solid #ccc; /* Borde del dropdown */
  background-color: #fff; /* Color de fondo */
  width: 70%; /* Ancho del dropdown */
  box-sizing: border-box; /* Para que el padding no afecte el ancho total */
  display: block; /* Hace que el select sea un bloque para centrarlo */
  margin: 20px auto 0; /* Centrado con margen superior de 20px */
}

/* Asegúrate de que el tamaño del select sea adecuado */
.dropdown-select:focus {
  outline: none; /* Quitar el contorno del enfoque */
  border-color: #007bff; /* Color de borde cuando está enfocado */
}
.col-10 {
    height: calc(50vh - 100px);
}
</style>
