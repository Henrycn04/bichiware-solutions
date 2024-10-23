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
                </button>
            </div>
        </header>
        <main class="main-content">
            <div class="subheader">
                <a href="/all-products" class="element_button">
                    <img src="../assets/AllIcon.png" style="width: 24px; height: 24px; cursor: pointer;" alt="Todos" />
                    <div style=""> &nbsp; Todos los productos</div>
                </a>
                <a href="/non-perishable-products" >No perecederos</a>
                <a href="/perishable-products" >Perecederos</a>
                <a v-if="this.isAdminOrEntrepreneur"
                    href="/users-list">Lista de usuarios</a>
                <a v-if="this.isAdminOrEntrepreneur"
                    href="/companies-list">Lista de empresas</a>
            </div>
            <div class="main-content2">
                <div class="filters" style="font-family: 'League Spartan', sans-serif;">
                <h2>Filtros</h2>
                <label for="category">Categoría: &nbsp;</label>
                <select v-model="selectedCategory" @change="sendCategory">
                    <option v-for="category in categories" :key="category" :value="category">{{ category }}</option>
                </select>
                <div style="margin-top: 10px;">
                    <label for="price">Rango: </label>
                    <span id="price-range">{{ priceRangeDisplay }}</span>
                    <div ref="priceSlider" class="layered_slider_container" style="margin-top: 40px;"></div>
                </div>     
                <div style="margin-top: 20px;">
                    <label for="companies">Empresas:</label>
                    <div v-for="company in uniqueCompanies" :key="company.companyID" style="margin-top: 5px;">
                        <input type="checkbox" :id="company.companyID" :value="company.companyID" v-model="selectedCompanies" />
                        <label :for="company.companyID">   &nbsp; {{ company.companyName }}</label>
                    </div>
                </div>                                        
                </div>
                <div class="inventory container" style="font-family: 'League Spartan', sans-serif; margin-left: 25px;">
                    <h2 style="background-color: #f07800; color: #332f2b; margin: 0; padding: 10px; width: 100%;">Todos los productos</h2>
                    <div v-if="loading">Cargando...</div>
                    <div v-else class="row">
                        <div v-for="item in items" :key="item.id" class="col-12 col-md-2">
                            <div class="item-card">
                                <img :src="item.imageURL" alt="Imagen del producto" />
                                <h3>{{ item.productName }}</h3>
                                <p>{{ item.productDescription }}</p>
                                <p>Precio: {{ item.price }}</p>
                                <div class="add-to-cart-row">
                                    <button @click="addToCart(item)" class="add-to-cart-btn">Agregar al carrito</button>
                                    <div class="quantity-selector">
                                        <button @click="decreaseQuantity(item)" :disabled="item.quantity <= 1">-</button>
                                        <input type="number" v-model.number="item.quantity" min="1" @input="validateQuantity(item)" />
                                        <button @click="increaseQuantity(item)">+</button>
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
import axios from 'axios';
import 'nouislider/dist/nouislider.css';
import noUiSlider from 'nouislider';
import _ from 'lodash';
import { mapGetters, mapState, mapActions } from 'vuex';


    export default {
        computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
            ...mapGetters(['getCart']),
            cartItems() {
                return this.getCart;
            },
        },
        data() {
            return {
                searchQuery: '',
                isProfileMenuVisible: false,
                categories: ['Todas', 'Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza'], // To store the filter categories
                selectedCategory: '', // For the selected filter
                items: [], // For the inventory items
                loading: false, // To show a loading indicator
                priceRange: [], // Initial price values
                priceRangeDisplay: '', // Text displaying the current price range
                uniqueCompanies: [],
                selectedCompanies: [],

                userCompanies: [],
                isCompaniesDropdownVisible: false,
                isAdminOrEntrepreneur: false,
            }
        },
        created() {
            this.fetchItems();
            this.selectedCategory = 'Todas';
        },
        methods: {
            ...mapActions(['openCompany']),
            ...mapActions(['closeCompany']),
            increaseQuantity(item) {
                item.quantity = (item.quantity || 1) + 1;
            },
            decreaseQuantity(item) {
            if (item.quantity > 1) {
                item.quantity--;
            }
            },
            validateQuantity(item) {
                if (item.quantity < 1 || isNaN(item.quantity)) {
                    item.quantity = 1;
                }
            },
            addToCart(item) {
                const productToAdd = {
                    id: item.id,
                    productName: item.productName,
                    price: item.price,
                    quantity: item.quantity || 1,
                    imageURL: item.imageURL,
                };
                this.$store.dispatch('addToCart', productToAdd);
                item.quantity = 1;
            },
            getUserCompanies() {
                axios.get(this.$backendAddress + "api/CompanyProfileData/UserCompanies", {
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
                this.$router.push({
                path: '/SearchPage',
                query: { search: this.searchQuery }
            });
            },
            goToProfile() {
                this.$router.push('/profile');
            },
            goToCart() {
                this.$router.push('/shoppingCart');
            },
            goToHome() {
                this.$router.push('/');
            },
            toggleProfileMenu() { // Toggles the dropdown box in the profile icon
                this.isProfileMenuVisible = !this.isProfileMenuVisible;
            },
            goToAccount() {
                this.$router.push('/account');
                this.isProfileMenuVisible = false;
            },
            goToRegisterCompany() {
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
                // Checks if the click was outside the profile container
                const profileContainer = this.$refs.profileContainer;
                if (profileContainer && !profileContainer.contains(event.target)) {
                    this.isProfileMenuVisible = false;
                }
            },
            async fetchItems() {
                this.loading = true;
                try {
                    // Makes both requests in parallel, including the price range and selected companies
                    const params = {
                        categoria: this.selectedCategory,
                        precioMin: this.priceRange[0],
                        precioMax: this.priceRange[1],
                        empresas: this.selectedCompanies 
                    };

                    const responseNoPerecederos = await axios.get(this.$backendAddress + 'api/products/non-perishable', {
                        params,
                        paramsSerializer: (params) => {
                            return Object.keys(params)
                                .map(key => 
                                    Array.isArray(params[key]) 
                                        ? params[key].map(val => `${encodeURIComponent(key)}=${encodeURIComponent(val)}`).join('&') 
                                        : `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`
                                )
                                .join('&');
                        }
                    });
                    console.log('Params:', params);
                    const productosNoPerecederosFiltrados = responseNoPerecederos.data;

                    console.log('Productos no perecederos filtrados:', productosNoPerecederosFiltrados);
                    this.items = [...productosNoPerecederosFiltrados];
                    this.items.forEach(item => {
                        item.quantity = 1;
                    });
                } catch (error) {
                    console.error('Error al filtrar productos:', error);
                } finally {
                    this.loading = false;
                }
            },
            updatePriceRange: _.debounce(function(values) {
                this.priceRange = values.map(value => Math.round(value)); // Rounds the values
                this.priceRangeDisplay = `₡${this.priceRange[0].toLocaleString()} - ₡${this.priceRange[1].toLocaleString()}`;
                this.fetchItems(); // Updates the products based on the new range
            }, 100), // Waits 100 ms before calling fetchItems
        },
        watch: {
            selectedCategory() {
                this.fetchItems();
            },
            selectedCompanies() {
                

                this.fetchItems();
            },
        },
        mounted() {
            
            var userType = Number(this.getUserType());
            this.isAdminOrEntrepreneur = userType === 2 || userType === 3;
            this.isAdmin = userType === 3;
            if (this.isAdminOrEntrepreneur) {
                this.getUserCompanies();
            }
            document.addEventListener('click', this.handleClickOutside);
            
            // Get the price range dynamically from the backend
            axios.get(this.$backendAddress + 'api/products/price-range/non-perishable')
                .then((response) => {
                    const { minPrice, maxPrice } = response.data;
                    
                     // Initializes noUiSlider with the obtained values
                    noUiSlider.create(this.$refs.priceSlider, {
                        start: [minPrice, maxPrice],
                        connect: true,
                        range: {
                            'min': minPrice,
                            'max': maxPrice
                        },
                        step: 1,
                        tooltips: true,
                        format: {
                            to: (value) => Math.round(value),
                            from: (value) => Number(value)
                        }
                    });
                    const sliderBase = this.$refs.priceSlider.querySelector('.noUi-base');
                    const sliderConnect = this.$refs.priceSlider.querySelector('.noUi-connect');

                    if (sliderBase) {
                        sliderBase.style.background = '#e0e0e0';
                    }
                    if (sliderConnect) {
                        sliderConnect.style.background = '#f07800';
                    }

                    // Listen to changes in the slider
                    this.$refs.priceSlider.noUiSlider.on('update', (values) => {
                        this.updatePriceRange(values);
                    });
                })
                .catch((error) => {
                    console.error('Error fetching price range:', error);
                });
            // Get unique company IDs
            axios.get(this.$backendAddress + 'api/products/companies/non-perishable')
                .then((response) => {
                    this.uniqueCompanies = response.data; // Ahora es un array de objetos con { IDEmpresa, NombreEmpresa }
                    console.log('Empresas únicas:', this.uniqueCompanies);
                })
                .catch((error) => {
                    console.error('Error fetching unique companies:', error);
                });

            
        },
        beforeUnmount() {
            // Removes the listener before destroying the component
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
    .main-content2 {
    display: flex;
    margin: 20px;
    }
    .filters {
    width: 15%;
    padding: 10px;
    border-right: 1px solid #ddd;
    float: left;
}

.inventory {
    margin: 10px 0;
}

.item-card {
    margin: 10px 0;
    border: 1px solid #ddd;
    padding: 10px;
    box-sizing: border-box;
}

.item-card img {
    width: 100%;
    height: auto;
}



    .layered_slider_container {
    margin-top: 10px;
    width: 65%;
    }

.noUi-handle {
  background-color: #463a2e;
}

.noUi-connect {
  background: #00b894;
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
        .add-to-cart-row {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: 10px;
}

.add-to-cart-btn {
    background-color: #f07800;
    color: #fff;
    border: none;
    padding: 8px 12px;
    cursor: pointer;
    font-size: 14px;
    border-radius: 5px;
    margin-right: 10px;
}

.quantity-selector {
    display: flex;
    align-items: center;
}

.quantity-selector button {
    background-color: #ccc;
    border: none;
    padding: 5px;
    cursor: pointer;
    font-size: 14px;
}

.quantity-selector input {
    width: 40px;
    text-align: center;
    border: 1px solid #ccc;
    margin: 0 5px;
}
</style>
