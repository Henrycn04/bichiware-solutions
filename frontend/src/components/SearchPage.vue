<template>
  <div class="page-container">
    <header class="header">
      <div class="header__brand">
        <a href="/" class="header__home-link" style="font-size: x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
      </div>
      <div class="header__search">
        <input 
          type="text"
          v-model="searchQuery"
          placeholder="Buscar"
          @keydown.enter="fetchSearchResults" 
        />
        <button @click="fetchSearchResults" class="header__search__button">
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
            <a v-if="isAdminOrEntrepreneur" @click="toggleCompaniesDropdown" class="header__profile-menu-item" style="color: #463a2e; cursor: pointer;">
              Ver empresas
            </a>
            <ul v-if="isAdminOrEntrepreneur && isCompaniesDropdownVisible">
              <li v-for="company in userCompanies" :key="company.companyID" @click="selectCompany(company.companyID)">
                {{ company.companyName }}
              </li>
            </ul>
            <a @click="goToLogout" href="/" class="header__profile-menu-item" style="color: #463a2e">Salir</a>
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
        <a v-if="isAdminOrEntrepreneur" href="/users-list">Lista de usuarios</a>
        <a v-if="isAdminOrEntrepreneur" href="/companies-list">Lista de empresas</a>
        <div>
                <a v-if="this.isAdmin" href="/pendingOrders">Pedidos pendientes</a>
                </div>
      </div>
    <div class="container mt-4">
    <div class="search-results text-center">
      <div class="results-container" v-if="searchResults.length">
        <div class="row justify-content-center">
          <div class="col-12 col-md-10">
            <div style="background-color: #f1d897;">
               <ProductList :products="searchResults" />
        
            </div>
          </div>
        </div>
      </div>
      <div v-else-if="searchQuery" class="no-results text-center mt-4">
        <div class="alert alert-warning" role="alert">
          <h4 class="alert-heading">No se encontraron resultados</h4>
          <p class="mb-0">No hay productos disponibles para "{{ searchQuery }}".</p>
          <hr>
          <p class="mb-0">Intenta con otra búsqueda.</p>
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
      <p style="display: block; text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;">&copy; Copyright by BichiWare Solutions 2024</p>
    </footer>
  </div>
</template>





<script>
  import ProductList from './ProductList.vue';
  import commonMethods from '@/mixins/commonMethods';
  import axios from "axios";
    export default {
        mixins: [commonMethods],
        components: {
            ProductList, 
        },
        mounted() {
            const searchTerm = this.$route.query.search;
            if (searchTerm) {
                this.searchQuery = searchTerm;
                this.fetchSearchResults();
            }
        },
      methods:{
        fetchSearchResults() {
        const perishableRequest = axios.get(this.$backendAddress + "api/products/search_perishable", {
                params: { searchTerm: this.searchQuery }
            });
        
            const nonPerishableRequest = axios.get(this.$backendAddress + "api/products/search_non-perishable", {
                params: { searchTerm: this.searchQuery }
            });
        
            Promise.all([perishableRequest, nonPerishableRequest])
                .then(([perishableResponse, nonPerishableResponse]) => {
                    this.searchResults = [
                        ...perishableResponse.data,
                        ...nonPerishableResponse.data
                    ];
                    console.log(this.searchResults);
                })
                .catch((error) => {
                    console.error("Error during combined product search:", error);
                });
              },
      }
  
    };

</script>




<style>
.no-results {
  max-width: 500px; 
  margin: 0 auto; 
}
.results-container {
  overflow-x: hidden;
  overflow-y: hidden;
  white-space: nowrap; 
}

.card {
  border-radius: 15px;
}</style>

