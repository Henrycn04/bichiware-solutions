<template>
  <div style=" height: 100vh;" class="bg-secondary">
    <nav class="navbar navbar-expand-lg bg-primary">
        <div class="container-fluid">
          <a
            href="/"
            class="navbar-brand fw-bold ff-lspartan float-start"
          >Feria del Emprendedor</a>
          <input
            class="form-control me-2"
            type="search"
            v-model="searchQuery"
            placeholder="Buscar"
            aria-label="Buscar"
          >
          <button @click="performSearch" class="header__search__button">
                  <img src="../assets/SearchIcon.png" alt="Buscar" />
          </button>
         
          <div class="navbar-collapse collapse ">
            <ul class="navbar-nav ">
              <li class="nav-item">
                <a class="nav-link" @click="accountClicked">
                  <div class="d-flex my-3 ff-lspartan fw-bold">
                    <svg class="me-1" width="23px" height="23px" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path d="M8 7C9.65685 7 11 5.65685 11 4C11 2.34315 9.65685 1 8 1C6.34315 1 5 2.34315 5 4C5 5.65685 6.34315 7 8 7Z" fill="#000000"/>
                      <path d="M14 12C14 10.3431 12.6569 9 11 9H5C3.34315 9 2 10.3431 2 12V15H14V12Z" fill="#000000"/>
                    </svg>{{ isLoggedIn() ? "Cuenta" : "Acceder" }}
                  </div>
                </a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/cart">
                  <div class="d-flex my-3">
                    <svg class="me-1" width="23px" height="23px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path d="M6.29977 5H21L19 12H7.37671M20 16H8L6 3H3M9 20C9 20.5523 8.55228 21 8 21C7.44772 21 7 20.5523 7 20C7 19.4477 7.44772 19 8 19C8.55228 19 9 19.4477 9 20ZM20 20C20 20.5523 19.5523 21 19 21C18.4477 21 18 20.5523 18 20C18 19.4477 18.4477 19 19 19C19.5523 19 20 19.4477 20 20Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>
                  </div>
                </a>
              </li>
            </ul>
          </div>
        </div>
    </nav>
    <nav
      style="filter: brightness(85%);
            height: 40px;" 
      class="navbar navbar-expand-lg bg-primary"
    >
      <div class="container-fluid">
        <ul class="navbar-nav ff-lspartan fw-bold">
          <li class="nav-item">
            <a class="nav-link" href="/all-products">
              <img
                src="../assets/AllIcon.png"
                alt="AllProducts"
                class="me-2"
                width=20
                height=20
              />
              Todos los Productos
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/non-perishable-products">
              No perecederos
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/perishable-products">
              Perecederos
            </a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdminOrEntrepreneur" class="nav-link"
              href="/users-list">Lista de usuarios</a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdminOrEntrepreneur" class="nav-link"
              href="/companies-list">Lista de empresas</a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div 
        v-show="requestError == true"
        class="toast-container bg-primary rounded-3 position-fixed bottom-0 end-0 p-3 m-5"
        role="alert" aria-live="assertive" aria-atomic="true"
      >
        <div class="toast-header">
          <strong class="me-auto" ref="statusCode">Error</strong>
        </div>
        <div class="toast-body" ref="errorMessage">
          Hello, world! This is a toast message.
        </div>
      </div>
      <div class="hstack gap-0 my-3">
        <div class="ms-auto btn-group" role="group" aria-label="Basic example">
          <a role="button" class="btn btn-primary border-1 border-dark" href="/addAddress">
            <svg width="25px" height="25px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M4 12H20M12 4V20" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
          </a>
          <button type="button" class="btn btn-primary border-1 border-dark">
            <svg width="25px" height="25px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M5.73708 6.54391V18.9857C5.73708 19.7449 6.35257 20.3604 7.11182 20.3604H16.8893C17.6485 20.3604 18.264 19.7449 18.264 18.9857V6.54391M2.90906 6.54391H21.0909" stroke="#1C1C1C" stroke-width="1.7" stroke-linecap="round"/>
              <path d="M8 6V4.41421C8 3.63317 8.63317 3 9.41421 3H14.5858C15.3668 3 16 3.63317 16 4.41421V6" stroke="#1C1C1C" stroke-width="1.7" stroke-linecap="round"/>
            </svg>
          </button>
          <button type="button" class="btn btn-primary border-1 border-dark">
            <svg width="25px" height="25px" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M13 0L16 3L9 10H6V7L13 0Z" fill="#000000"/>
              <path d="M1 1V15H15V9H13V13H3V3H7V1H1Z" fill="#000000"/>
            </svg>
          </button>
        </div>
      </div>
      <table class="table table-primary">
        <thead class="">
          <tr class="">
            <th scope="col">Seleccionar</th>
            <th scope="col">Dirección Exacta</th>
            <th scope="col">Distrito</th>
            <th scope="col">Cantón</th>
            <th scope="col">Provincia</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(address, index) in addressList" :key="index"
            class="table-secondary">
            <td scope="row">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="defaultCheck1">
              </div>
            </td>
            <td scope="row">{{ address.exact }}</td>
            <td scope="row">{{ address.district }}</td>
            <td scope="row">{{ address.canton }}</td>
            <td scope="row">{{ address.province }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
      @Copyright BichiWare Solutions 2024
    </footer>
  </div>
</template>

<script>
import axios from 'axios';
import { mapGetters } from 'vuex';

export default {
  setup () { return {} },


  data ()
  {
    return {
      requestError: false,
      addressList: [ ],
      isAdminOrEntrepreneur: false,
      searchQuery: '',
    };
  },


  methods:
  {
    ...mapGetters(['getUserId', "getUserType", "isLoggedIn"]),


    getAddresses()
    {
      axios.get(this.$backendAddress + "api/AccountAddresses/GetUserAddresses?userId=" + this.getUserId())
        .then((response) => {
          this.addressList = response.data;
        }).catch((error) => {
          if (error.response) {
            this.$refs.errorMessage.innerHTML = error.response.data;
            this.$refs.statusCode.innerHTML = "Error " + error.response.status;
          } else if (error.request) {
            this.$refs.errorMessage.innerHTML = error.request;
          } else {
            console.log('Error al preparar la consulta', error.message);
          }
          this.requestError = true;
        });
      console.log("Gets addresses");
    },
    performSearch() {
      this.$router.push({
                path: '/SearchPage',
                query: { search: this.searchQuery }
            });
    },

    accountClicked() {
      if (this.isLoggedIn() == true)
      {
        window.location.href = "/userProfile";
      }
      else
      {

        window.location.href = "/login"
      }
    },
  },


  mounted() {
    
    var userType = Number(this.getUserType());
    this.isAdminOrEntrepreneur = userType === 3 || userType === 2;  

    if (this.getUserId().length > 0)
    {
      this.getAddresses();
    }
    else
    {
      this.$refs.errorMessage.innerHTML = "Debes ingresar a tu perfíl!";
      this.$refs.statusCode.innerHTML = "Fallo de autenticación";
      this.requestError = true;
    }
  },
}
</script>

<style lang="scss" scoped></style>
