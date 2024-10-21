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
        <table class="table table-primary">
          <thead class="">
            <tr class="">
              <th scope="col">ID de Usuario</th>
              <th scope="col">Nombre</th>
              <th scope="col">Apellido</th>
              <th scope="col">Cédula</th>
              <th scope="col">Correo Electrónico</th>
              <th scope="col">Estado</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user) in userList" :key="user"
              class="table-secondary">
              <th scope="row">{{ user.userId }}</th>
              <td scope="row">{{ user.firstName }}</td>
              <td scope="row">{{ user.lastName }}</td>
              <td scope="row">{{ user.legalId }}</td>
              <td scope="row">{{ user.email }}</td>
              <td scope="row">{{ user.accountStatus }}</td>
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
      userList: [ ],
      isAdminOrEntrepreneur: false,
      searchQuery: '',
    };
  },


  methods: {
    ...mapGetters(['getUserId', "getUserType", "isLoggedIn"]),
  
    getAllUsers()
    {
      axios.get("https://localhost:7263/api/UserCompanyList/GetUsersList?userId=" + this.getUserId())
        .then((response) => {
          this.userList = response.data
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
    this.isAdminOrEntrepreneur = userType === 2 || userType === 3;  

    if (this.getUserId().length > 0)
    {
      this.getAllUsers();
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
