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
                <a href="/login" class="header__login">Accesar</a>
                <div class="header__profile-container" ref="profileContainer">
                    <button @click="toggleProfileMenu" class="header__profile">
                        <img src="../assets/ProfileIcon.png" alt="Perfil" />
                    </button>
                    <div v-if="isProfileMenuVisible" class="header__profile-menu">
                        <a href="/profile" class="header__profile-menu-item" style="color: #463a2e">Cuenta</a>
                        <a href="/company-register" class="header__profile-menu-item" style="color: #463a2e">Registrar empresa</a>
                        <a href="/exit-account" class="header__profile-menu-item" style="color: #463a2e">Salir</a>
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
                    <div v-for="company in uniqueCompanies" :key="company.idEmpresa" style="margin-top: 5px;">
                        <input type="checkbox" :id="company.idEmpresa" :value="company.idEmpresa" v-model="selectedCompanies" />
                        <label :for="company.idEmpresa">   &nbsp; {{ company.nombreEmpresa }}</label>
                    </div>
                </div>                                         
                </div>
                <div class="inventory container" style="font-family: 'League Spartan', sans-serif; margin-left: 25px;">
                    <h2 style="background-color: #f07800; color: #332f2b; margin: 0; padding: 10px; width: 100%;">Productos perecederos</h2>
                    <div v-if="loading">Cargando...</div>
                    <div v-else class="row">
                        <div v-for="item in items" :key="item.id" class="col-12 col-md-2">
                            <div class="item-card">
                                <img :src="item.imagenURL" alt="Imagen del producto" />
                                <h3>{{ item.nombreProducto }}</h3>
                                <p>{{ item.descripcion }}</p>
                                <p>Precio: {{ item.precio }}</p>
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

    export default {
        data() {
            return {
                searchQuery: '',
                isProfileMenuVisible: false,
                categories: ['Todas', 'Alimentos', 'Electronicos', 'DecoracionCasa', 'Automoviles', 'DecoracionExteriores', 'Ropa', 'Joyeria', 'Limpieza'], // Para guardar las categorías de filtros
                selectedCategory: '', // Para el filtro seleccionado
                items: [], // Para los ítems del inventario
                loading: false, // Para mostrar un indicador de carga
                priceRange: [], // Valores iniciales de precio
                priceRangeDisplay: '', // Texto que muestra el rango actual
                uniqueCompanies: [],
                selectedCompanies: [],
            }
        },
        created() {
            this.fetchCategories();
            this.fetchItems();
            this.selectedCategory = 'Todas';
        },
        methods: {
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
                this.$router.push('/cart');
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
            logout() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                console.log('Logout');
                this.$router.push('/login');
                this.isProfileMenuVisible = false;
            },
            handleClickOutside(event) {
                // verifica si el clic fue fuera del contenedor del perfil
                const profileContainer = this.$refs.profileContainer;
                if (profileContainer && !profileContainer.contains(event.target)) {
                    this.isProfileMenuVisible = false;
                }
            },
            async fetchCategories() {
                try {
                    const response = await axios.get('https://localhost:7263/api/products/categories');
                    this.categories = response.data;
                } catch (error) {
                    console.error('Error fetching categories:', error);
                }
            },
            async fetchItems() {
                this.loading = true;
                try {
                    // Realiza ambas peticiones en paralelo, incluyendo el rango de precios y las empresas seleccionadas
                    const params = {
                        categoria: this.selectedCategory,
                        precioMin: this.priceRange[0],
                        precioMax: this.priceRange[1],
                        empresas: this.selectedCompanies // Asigna el array directamente
                    };


                    const responsePerecederos = await axios.get('https://localhost:7263/api/products/perishable', {
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

                    // Guarda los resultados de ambas respuestas
                    const productosPerecederosFiltrados = responsePerecederos.data;

                    console.log('Productos perecederos filtrados:', productosPerecederosFiltrados);

                    // Combina los resultados de ambos tipos de productos
                    this.items = [...productosPerecederosFiltrados];

                } catch (error) {
                    console.error('Error al filtrar productos:', error);
                } finally {
                    this.loading = false;
                }
            },
            updatePriceRange: _.debounce(function(values) {
                this.priceRange = values.map(value => Math.round(value)); // Redondea los valores
                this.priceRangeDisplay = `₡${this.priceRange[0].toLocaleString()} - ₡${this.priceRange[1].toLocaleString()}`;
                this.fetchItems(); // Actualiza los productos según el nuevo rango
            }, 100), // Espera 100 ms antes de llamar a fetchItems
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
            // Añade el listener al montar el componente
            document.addEventListener('click', this.handleClickOutside);
            
            // Cargar categorías
            this.fetchCategories();
            
            // Obtener el rango de precios dinámicamente desde el backend
            axios.get('https://localhost:7263/api/products/price-range/perishable')
                .then((response) => {
                    const { minPrice, maxPrice } = response.data;
                    
                    // Inicializa noUiSlider con los valores obtenidos
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
                    // Agregar estilos CSS dinámicamente (opcional)
                    const sliderBase = this.$refs.priceSlider.querySelector('.noUi-base');
                    const sliderConnect = this.$refs.priceSlider.querySelector('.noUi-connect');

                    // Cambiar colores
                    if (sliderBase) {
                        sliderBase.style.background = '#e0e0e0'; // Color de la barra base
                    }
                    if (sliderConnect) {
                        sliderConnect.style.background = '#f07800'; // Color de la barra conectada
                    }

                    // Escucha cambios en el slider
                    this.$refs.priceSlider.noUiSlider.on('update', (values) => {
                        this.updatePriceRange(values);
                    });
                })
                .catch((error) => {
                    console.error('Error fetching price range:', error);
                });
            // Obtener los IDs de empresas únicas
            axios.get('https://localhost:7263/api/products/companies/perishable')
                .then((response) => {
                    this.uniqueCompanies = response.data; // Ahora es un array de objetos con { IDEmpresa, NombreEmpresa }
                    console.log('Empresas únicas:', this.uniqueCompanies);
                })
                .catch((error) => {
                    console.error('Error fetching unique companies:', error);
                });

            
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
    float: left; /* Esto mantiene los filtros a la izquierda */
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
</style>
