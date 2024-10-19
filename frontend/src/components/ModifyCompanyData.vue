<template>
    <div style="height: 100vh;" class="bg-secondary">
      <div class="bg-primary pt-3 pb-3">
        <a
          href="/"
          class="navbar-brand fw-bold ff-lspartan float-start"
          >Feria del Emprendedor
        </a>      
    </div>
      <div class="container-fluid bg-secondary py-5">
        <div class="container bg-light rounded-4 mb-3 pb-4">
          <div class="row bg-primary pt-3 rounded-top-4">
            <h1 class="display-6 text-center fw-bold ff-lspartan">Modificar Datos de Empresa</h1>
          </div>
          <form class="mt-3 ff-lspartan">
            <label for="companyName" class="form-label mb-0">Nombre de la empresa</label>
            <div class="mb-3">
              <input
                id="companyName"
                class="form-control rounded-2 border-0 bg-secondary"
                type="text"
                required
                placeholder="x"
              >
            </div>
            <label for="legalID" class="form-label mb-0">Cédula jurídica</label>
            <div class="mb-3">
              <input
                id="legalID"
                class="form-control rounded-2 border-0 bg-secondary"
                maxlength="10" 
                pattern="\d{10}"
                required
                placeholder="x"
              >
            </div>
            <div class="mb-3">
              <label for="email" class="form-label mb-0">Correo electrónico</label>
              <input
                id="email"
                class="form-control rounded-2 border-0 bg-secondary"
                type="text"
                required
                placeholder="x"
              />
            </div>
            <div class="mb-3">
              <label for="phoneNumber" class="form-label mb-0">Número de teléfono</label>
              <input
                id="phoneNumber"
                class="form-control rounded-2 border-0 bg-secondary"
                maxlength="8" 
                pattern="\d{8}"
                required
                placeholder="x"
              />
            </div>
            <div class="d-grid gap-2">
              <input
                name="submitChange"
                class="btn fw-bold btn-primary ff-lspartan fs-5"
                type="submit"
                value="Modificar datos"
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
    import {mapGetters} from "vuex"
    export default {
        computed: {
            ...mapGetters(['getIdCompany']),
        },
        data() {
            return {
                originalData: {
                    companyName: ' ',
                    cedula: ' ',
                    email: ' ',
                    phoneNumber: ' ',
                }
            };
        },
        methods: {
            getUserCompanyData() {
                axios.get("https://localhost:7263/api/CompanyProfileData/CompanyData", {
                    params: {
                        companyID: this.getIdCompany
                    }
                })
                    .then((response) => {
                        this.originalData = response.data;
                        console.log("Original company data: ", this.companyProfileData);
                    })
                    .catch((error) => {
                        console.error("Error obtaining original company data:", error);
                    });
            },
        }
    }

</script>

  <style></style>