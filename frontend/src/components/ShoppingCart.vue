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
                </div> 
                <button @click="goToCart" class="header__cart">
                    <img src="../assets/CartIcon.png" alt="Carrito" />
                    {{ cartItems.length }}
                </button>
                
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
                <a v-if="isAdminOrEntrepreneur" href="/users-list">Lista de usuarios</a>
                <a v-if="isAdminOrEntrepreneur" href="/companies-list">Lista de empresas</a>
            </div>

            <div class="results-container" v-if="cartItems.length">
                <div class="row justify-content-center">
                <div class="col-12 col-md-10">
                    <div style="background-color: #f1d897;">
                        <ShoppingCartProductsList :products="cartItems" />                
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

    import axios from "axios";
    import { mapGetters, mapState, mapActions } from 'vuex';
    import ShoppingCartProductsList from './ShoppingCartProductsList.vue';
    import commonMethods from '@/mixins/commonMethods';

    export default {
        mixins: [commonMethods],
        components: {
            ShoppingCartProductsList, 
        },
        computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
        },
        data() {
            return {
                isCompaniesDropdownVisible: false,
                userCompanies: [],
                searchQuery: '',
                isProfileMenuVisible: false,
                isAdminOrEntrepreneur: false, 
                cantidad: 0,
                cartItems: []
            }
        },
        methods: {
            ...mapActions(['openCompany']),
            ...mapActions(['closeCompany']),
            getAllCartProducts() {
                axios.get(`https://localhost:7263/api/ShoppingCart/getAllCartProducts/${this.userCredentials.userId}`)
                .then((response) => {
                    if (typeof response.data === "string") {
                        console.warn(response.data); // Mensaje de no hay productos
                        this.cartItems = [];
                    } else {
                        console.warn(response.data);
                        this.cartItems = response.data; // Asigna los productos
                    }                })
                .catch((error) => {
                    console.error("Error obtaining cart products:", error);
                });
            },

            getUserCompanies() {
                axios.get("https://localhost:7263/api/CompanyProfileData/UserCompanies", {
                    params: {
                        userID: this.userCredentials.userId
                    }
                })
                    .then((response) => {
                        this.userCompanies = response.data;
                       
                    })
                    .catch((error) => {
                        console.error("Error obtaining user companies:", error);
                    });
            },
            toggleCompaniesDropdown() {
                this.isCompaniesDropdownVisible = !this.isCompaniesDropdownVisible;
            },
            selectCompany(companyID) {
                this.openCompany(companyID);
                this.$router.push(`/companyProfile`);
            },
            ...mapGetters(["getUserType"]),
            performSearch() {
                // para el boton de buscar
                console.log('Buscando:', this.searchQuery);
            },
            goToProfile() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/profile');
            },
            goToCart() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/shoppingCart');
            },
            goToHome() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/');
            },
            toggleProfileMenu() { // activa y desactiva el cuadro que se despliega en el icono del perfil
                this.isProfileMenuVisible = !this.isProfileMenuVisible;
            },
            goToAccount() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/account');
                this.isProfileMenuVisible = false;
            },
            goToRegisterCompany() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/register-company');
                this.isProfileMenuVisible = false;
            },
            goTologout() {
                console.log('Logout');
                this.$store.dispatch('logOut');
                this.isProfileMenuVisible = false;
                this.closeCompany();
            },
            goToLogin() {
                this.$router.push('/login'); 
            },
            handleClickOutside(event) {
                // verifica si el clic fue fuera del contenedor del perfil
                const profileContainer = this.$refs.profileContainer;
                if (profileContainer && !profileContainer.contains(event.target)) {
                    this.isProfileMenuVisible = false;
                }
            }
        },
        mounted() {
            //annade el listener al montar el componente
            document.addEventListener('click', this.handleClickOutside);
           
            var userType = Number(this.getUserType()); 
            console.log(userType);
            this.isAdminOrEntrepreneur = userType === 2 || userType === 3;
            if (this.isAdminOrEntrepreneur) {
                this.getUserCompanies();
            }
            if (this.isLoggedIn) {
                this.getAllCartProducts();
            }
        },
        beforeUnmount() {
            // remueve el listener antes de destruir el componente
            document.removeEventListener('click', this.handleClickOutside);
        }
    }
</script>




<style scoped>
    html, body {
        margin: 0;
        padding: 0;
        height: 100%;
        background-color: #ffeec2;
    }

    .page-container {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 10px 20px;
        background-color: #f07800;
        color: white;
        font-family: 'League Spartan', sans-serif;
        max-width: 100vw;
        box-sizing: border-box;
    }

    .header__home-link {
        text-decoration: none;
        color: #332f2b;
    }

    .header__brand h1 {
        margin: 0;
        font-size: 24px;
    }

    .header__search {
        display: flex;
        align-items: center;
        margin: 0 20px;
        flex-grow: 1;
        border-radius: 50%;

    }

    .header__search input {
        flex-grow: 1;
        padding: 8px;
        border: none;
        border-radius: 15px 0 0 15px;
        font-family: 'League Spartan', sans-serif;
        box-sizing: border-box;
    }

    .header__search input:focus {
        outline: none;
        box-shadow: 0 0 3px rgba(255, 165, 0, 0.8);
        background-color: #fff8e1;
    }

    .header__search__button {
        padding: 8px;
        border: none;
        background-color: transparent;
        cursor: pointer;
        display: flex;
        align-items: center;
        margin-left: -20px;
    }

    .header__search__button img {
        width: 30px;
        height: 30px;
    }

    .header__actions {
        display: flex;
        align-items: center;
    }

    .header__actions a,
    .header__actions button {
        margin-left: 15px;
        color: white;
        text-decoration: none;
        background: none;
        border: none;
        cursor: pointer;
    }

    .header__actions img {
        width: 24px;
        height: 24px;
    }

    .header__actions a:hover,
    .header__actions button:hover {
        text-decoration: none;
    }

    .header__profile-container {
        position: relative;
    }

    .header__profile-menu {
        position: absolute;
        top: 100%;
        right: 0;
        background: white;
        color: #332f2b;
        border-radius: 5px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        display: flex;
        flex-direction: column;
        width: 150px;
        padding: 0;
        margin: 0;
        box-sizing: border-box;
    }

    .header__profile-menu-item {
        padding: 10px;
        text-decoration: none;
        color: #463a2e;
        display: block;
        margin: 0 0 0 -10px;
        border-radius: 5px;
    }

    .header__profile-menu-item:hover {
        background-color: #f5f5f5;
    }

    .main-content {
        background-color: #ffeec2;
        flex: 1;
    }
    .subheader {
        list-style-type: none;
        display: flex;
        margin: 0;
        padding: 0;
        align-items: center;
        padding: 10px 20px;
        background-color: #d57623;
        font-family: 'League Spartan', sans-serif;
        max-width: 100vw;
        text-decoration: none;
    }
    .element_button {
        list-style-type: none;
        display: flex;
        margin: 0;
        padding: 0;
        align-items: center;

        background-color: #d57623;
        font-family: 'League Spartan', sans-serif;

    }
    .subheader a{
        color: #463a2e;
        text-decoration: none;
        margin-right: 15px;
        font-weight: bold;
    }
    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }
    .footer_columns {
        display: flex;
        justify-content: space-between;
        padding: 1rem;
        text-align: center;
        box-sizing: border-box;
        color: #f2f2f2;
        margin: 0 0 -30px 0;
    }

    .footer__column {
        font-family: 'League Spartan', sans-serif;
        flex: 1;
        margin: 0 10px;
        color: #f2f2f2;
    }

    .footer__column strong {
        display: block;
        margin-bottom: 0.5rem;
        font-weight:bolder;
        font-size: large;
    }

    .footer__column a {
        font-family: 'Poppins', sans-serif;
        display: block;
        margin: 0.5rem 0;
        text-decoration: none;
        color: #f2f2f2;
    }

    .footer__column a:hover {
        text-decoration: underline;
    }

    .footer__column p {
        font-family: 'Poppins', sans-serif;
        margin: 0.5rem 0;
        text-decoration: none;
    }
    .footer__column p:hover {
        text-decoration: underline;
    }

    li {
        list-style: none; 
        cursor: pointer; 
        user-select: none; 
    }
        li:hover {
            background-color: #e0e0e0; 
        }

        .cart-section {
    margin-top: 20px;
    padding: 15px;
    background-color: #f9f9f9;
    border-radius: 8px;
}

.cart-item {
    border: 1px solid #ccc;
    padding: 10px;
    margin-bottom: 10px;
    border-radius: 5px;
    background-color: #fff;
}
</style>
