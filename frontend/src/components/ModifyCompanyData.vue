<template>
  <div style="height: 100vh;" class="bg-secondary">
    <div class="bg-primary pt-3 pb-3">
      <header class="header">
                <a href="/companyProfile" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer; text-decoration: none ; color: #332f2b">Feria del Emprendedor</a>
        </header>     
    </div>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light rounded-4 mb-3 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Modificar Datos de Empresa</h1>
        </div>
        <form @submit.prevent="checkIfThereIsNewData" class="mt-3 ff-lspartan">
          <label for="companyName" class="form-label mb-0">Nombre de la empresa</label>
          <div class="mb-3">
            <input
              id="companyName"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text"
              placeholder=""
              v-model="newName"
            >
          </div>
          <label for="legalID" class="form-label mb-0">Cédula jurídica</label>
          <div class="mb-3">
            <input
              id="legalID"
              class="form-control rounded-2 border-0 bg-secondary"
              maxlength="9" 
              pattern="\d{9}"
              placeholder=""
              v-model="newLegalID"
            >
          </div>
          <div class="mb-3">
            <label for="email" class="form-label mb-0">Correo electrónico</label>
            <input
              id="email"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text"
              placeholder=""
              v-model="newEmail"
            />
          </div>
          <div class="mb-3">
            <label for="phoneNumber" class="form-label mb-0">Número de teléfono</label>
            <input
              id="phoneNumber"
              class="form-control rounded-2 border-0 bg-secondary"
              maxlength="8" 
              pattern="\d{8}"
              placeholder=""
              v-model="newPhoneNumber"
            />
          </div>
          <div class="d-grid gap-2">
            <button
              name="submitChange"
              class="btn fw-bold btn-primary ff-lspartan fs-5"
              type="submit"
            >
              Modificar datos
            </button>
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
  import { mapGetters } from "vuex";

  export default {
    computed: {
      ...mapGetters(['getIdCompany']),
    },
    data() {
      return {
        newName: '',
        newLegalID: '',
        newEmail: '',
        newPhoneNumber: '',
        originalData: {
          id: 0,
          name: '',
          phoneNumber: '',
          email: '',
          legalId: '',
        }
      };
    },
    mounted() {
      this.getUserCompanyData();
    },
    methods: {
      getUserCompanyData() {
        axios.get(this.$backendAddress + "api/CompanyProfileData/CompanyMainData", {
          params: {
            companyID: this.getIdCompany
          }
        })
        .then((response) => {
          this.originalData = response.data;
          this.writePlaceholders();
          this.initNewData();
          console.log("Original company data: ", this.originalData);
        })
        .catch((error) => {
          console.error("Error obtaining original company data:", error);
        });
      },
      initNewData() {
        this.newName = this.originalData.name;
        this.newLegalID = this.originalData.legalId;
        this.newEmail = this.originalData.email;
        this.newPhoneNumber = this.originalData.phoneNumber;
      },
      writePlaceholders() {
        document.getElementById("companyName").placeholder = this.originalData.name;
        document.getElementById("legalID").placeholder = this.originalData.legalId;
        document.getElementById("email").placeholder = this.originalData.email;
        document.getElementById("phoneNumber").placeholder = this.originalData.phoneNumber;
      },
      checkIfThereIsNewData() {
        if (
          this.newName !== this.originalData.name ||
          this.newLegalID !== this.originalData.legalId ||
          this.newEmail !== this.originalData.email ||
          this.newPhoneNumber !== this.originalData.phoneNumber
        ) {
          this.submitNewCompanyData();
        } else {
          alert("There´s no change in the data.");
          this.$router.push('CompanyProfile' );
        }
      },
      async submitNewCompanyData() {
        await axios.post(this.$backendAddress + "api/CompanyProfileData", {
          Id: this.originalData.id,
          Name: this.newName,
          LegalId: this.newLegalID,
          Email: this.newEmail,
          PhoneNumber: this.newPhoneNumber,
          })
          .catch((error) => {
              console.log(error);
          });
        alert("Forms submitted succesfully.");
        this.$router.push('CompanyProfile' );
      },
    }
  }
</script>

<style>
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 5px 10px;
        background-color: #f07800;
        color: white;
        font-family: 'League Spartan', sans-serif;
        max-width: 100vw;
        box-sizing: border-box;
    }

</style>