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
            <a v-if="this.isAdmin || this.isEnterpreneur" class="nav-link"
              href="/users-list">Lista de usuarios</a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdmin || this.isEnterpreneur" class="nav-link"
              href="/companies-list">Lista de empresas</a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light rounded-4 mb-3 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Eliminar Empresa</h1>
        </div>
        <div class="ff-poppins text-center p-3">
          <span class="fs-5" ref="informationMessage2">¿Está seguro de eliminar la siguiente empresa? Esta acción es irreversible.</span><br>
          Asegurese de que la empresa no tenga pedidos pendientes.
        </div>
        <form
          method="dialog"
          class="mt-3 ff-lspartan"
          @submit="deleteCompany"
        >
          <label for="companyName" class="form-label mb-0">Nombre de la compañía *</label>
          <div class="d-flex gap-2">
            <select id="companyName"
              class="form-select rounded-2 border-0 bg-secondary"
              type="text" required placeholder="Nombre de la compañía"
              v-model="companyName"
              @change="validateCompany"
            >
              <option disabled value="">Seleccione una de sus empresas</option>
              <option
                v-for="company in companies"
                v-bind:key="company.id"
                :value="company.name"
              >
                {{ company.name }}
              </option>
            </select>
          </div>
          <div v-show="!canDeleteCompany" class="form-text pb-3" ref="informationMessage">
            Debe seleccionar una empresa.
          </div>
          <div class="d-flex gap-2 pt-4">
            <input
              name="submit"
              style="width: 25%;"
              class="btn fw-bold btn-danger ff-lspartan fs-5"
              type="submit"
              value="Eliminar"
            >
            <input
              name="cancel"
              style="width: 75%;"
              class="btn btn-secondary ff-lspartan fs-5"
              type="button"
              value="Cancel"
              @click="onCancelButtonClicked"
            >
          </div>
        </form>
      </div>
    </div>
    <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
      @Copyright BichiWare Solutions 2024
    </footer>
  </div>
</template>

<script>
import axios from "axios";
import { mapGetters } from 'vuex';

export default {
  data ()
  {
    return {
      isAdmin: false,
      isEnterpreneur: false,
      
      companies: [],
      companyId: '',
      canDeleteCompany: false,
    }
  },


  mounted()
  {
    var userType = this.getUserType();
    this.isAdmin = userType == 3;
    this.isEnterpreneur = userType == 2;
    if (this.validateUserType())
    {
      this.getUserCompanies();
    }
  },


  methods:
  {
    ...mapGetters(["getUserType", "isLoggedIn", "getUserId"]),


    validateUserType() {
      if (!this.isAdmin && !this.isEnterpreneur)
      {
        this.canDeleteCompany = false;
        this.$refs.informationMessage.innerHTML = "Usted no es un usuario empresario o administrador para utilizar esta función.";
        return false;
      }
      return true;
    },


    getUserCompanies()
    {
      axios.get(this.$backendAddress + "api/UserCompanyList/GetCompanies?userId=" + this.getUserId())
        .then((response) => {
          this.companies = response.data
        }).catch((error) => {
          var errorMsg = "";
          var errorStatus = 0;
          if (error.response == undefined) {
            errorMsg = "No hay conexión con el servidor.";
            errorStatus = 408;
          } else if (error.response.data.title) {
            errorMsg = error.response.data.title;
            errorStatus = error.response.status;
          } else if (error.response.data) {
            errorMsg = error.response.data;
            errorStatus = error.response.status;
          } else if (error.request) {
            errorMsg = error.message;
            errorStatus = error.response.status;
          }
          
          console.log("ERROR " + errorStatus + " ----> " + errorMsg);
          this.canDeleteCompany = false;
          this.$refs.informationMessage.innerHTML = "Ocurrió un error. " + errorMsg
        });
    },


    validateCompany() {
      if (this.companyName === "Bichiware Solutions")
      {
        this.canDeleteCompany = false;
        this.$refs.informationMessage.innerHTML = "La empresa " + this.companyName + " no se puede eliminar.";
        return false;
      }
      this.canDeleteCompany = true;
      return true;
    },


    deleteCompany()
    {
      if (this.validateCompany())
      {
        for (var i = 0; i < this.companies.length; i++)
        {
          console.log("Name: " + this.companies[i].name + " len: " + this.companies.length)
          if (this.companyName == this.companies[i].name)
          {
            this.companyId = this.companies[i].id;
          }
        }
        console.log(this.companyId);
        //console.log(this.companyName);
        axios.delete(this.$backendAddress + `api/UpdateCompany/DeleteCompany?companyId=${this.companyId }`)
        .then((response) => {
          if (response.data)
          {
            this.$refs.informationMessage2.innerHTML = "La eliminación se realizo con éxito."
            window.location.href = "/"
          }
        }).catch((error) => {
          var errorMsg = "";
          var errorStatus = 0;
          if (error.response == undefined) {
            errorMsg = "No hay conexión con el servidor.";
            errorStatus = 408;
          } else if (error.response.data.title) {
            errorMsg = error.response.data.title;
            errorStatus = error.response.status;
          } else if (error.response.data) {
            errorMsg = error.response.data;
            errorStatus = error.response.status;
          } else if (error.request) {
            errorMsg = error.message;
            errorStatus = error.response.status;
          }
          
          console.log("ERROR " + errorStatus + " ----> " + errorMsg);
          this.canDeleteCompany = false;
          this.$refs.informationMessage.innerHTML = "Ocurrió un error. " + errorMsg
        });
      }
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


    onCancelButtonClicked() {
      window.history.back()
    },
  }
}
</script>

<style lang="scss">
  
</style>
