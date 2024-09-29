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
          placeholder="Buscar"
          aria-label="Buscar"
        >
        <button
          class="btn btn-ternary btn-secondary"
          type="submit"
        >
        <svg fill="#000000" height="23px" width="23px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 183.792 183.792" xml:space="preserve">
          <path d="M54.734,9.053C39.12,18.067,27.95,32.624,23.284,50.039c-4.667,17.415-2.271,35.606,6.743,51.22  c12.023,20.823,34.441,33.759,58.508,33.759c7.599,0,15.139-1.308,22.287-3.818l30.364,52.592l21.65-12.5l-30.359-52.583  c10.255-8.774,17.638-20.411,21.207-33.73c4.666-17.415,2.27-35.605-6.744-51.22C134.918,12.936,112.499,0,88.433,0  C76.645,0,64.992,3.13,54.734,9.053z M125.29,46.259c5.676,9.831,7.184,21.285,4.246,32.25c-2.938,10.965-9.971,20.13-19.802,25.806  c-6.462,3.731-13.793,5.703-21.199,5.703c-15.163,0-29.286-8.146-36.857-21.259c-5.676-9.831-7.184-21.284-4.245-32.25  c2.938-10.965,9.971-20.13,19.802-25.807C73.696,26.972,81.027,25,88.433,25C103.597,25,117.719,33.146,125.29,46.259z"/>
        </svg>
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
              <a v-if="this.isAdminOrEntrepreneur"
                href="/users-list">Lista de usuarios</a>
            </li>
            <li class="nav-item">
              <a v-if="this.isAdminOrEntrepreneur"
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
    }
  },


  mounted() 
  {

    var userType = this.getUserType();
    this.isAdminOrEntrepreneur = userType == 1 || userType == 2;  
  },


  methods: {
    
    ...mapGetters(["getUserType", "isLoggedIn"]),

  
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