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
              <a class="nav-link" v-if="this.isAdminOrEntrepreneur"
                href="/users-list">Lista de usuarios</a>
            </li>
            <li class="nav-item">
              <a class="nav-link"  v-if="this.isAdminOrEntrepreneur"
                href="/companies-list">Lista de empresas</a>
            </li>
          </ul>
        </div>
      </nav>
      <div class="container-fluid bg-secondary py-5">
        <div class="container bg-light rounded-4 mb-3 pb-4">
          <div class="row bg-primary pt-3 rounded-top-4">
            <h1 class="display-6 text-center fw-bold ff-lspartan">Créditos</h1>
          </div>
          <table class="table table-primary my-3">
            <thead>
              <tr>
                <th>Nombre</th>
                <th>Correo</th>
                <th>Carnet</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(author, index) in authors" :key="index" class="table-secondary">
                <th>{{ author.name }}</th>
                <th>{{ author.email }}</th>
                <th>{{ author.id }}</th>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
        @Copyright BichiWare Solutions 2024
      </footer>
    </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  setup () { return {} },

  data()
  {
    return {
      authors: [
        { name: "André Salas Chinchilla", id: "C27058", email: "andre.salaschinchilla@ucr.ac.cr" },
        { name: "Daniel Mora Rodríguez", id: "C25200", email: "daniel.morarodriguez@ucr.ac.cr" },
        { name: "Felipe Bianchini Sanchez", id: "C21178", email: "felipe.bianchini@ucr.ac.cr" },
        { name: "Henry Campos Navarro", id: "C21636", email: "henry.camposnavarro@ucr.ac.cr" },
        { name: "José Mario Castro Chanto", id: "C21878", email: "jose.castrochanto@ucr.ac.cr" },
      ],
      isAdminOrEntrepreneur: false,
      searchQuery: '',
    }
  },


  mounted() 
  {

    var userType = Number(this.getUserType());
    this.isAdminOrEntrepreneur = userType === 3 || userType === 2 ;  
  },


  methods: {
    
    ...mapGetters(["getUserType", "isLoggedIn"]),

    performSearch() {
      this.$router.push({
                path: '/SearchPage',
                query: { search: this.searchQuery }
            });
    },
    accountClicked() {
      if (this.isLoggedIn())
      {
        window.location.href = "/userProfile";
      }
      else
      {

        window.location.href = "/login"
      }
    },
  }
}
</script>

<style lang="scss" scoped>

</style>