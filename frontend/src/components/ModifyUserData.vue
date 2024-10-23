<template>
      <div style="height: 100vh;" class="bg-secondary">
    <div class="bg-primary pt-3 pb-3">
      <header class="header">
                <a href="/userProfile" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer; text-decoration: none ; color: #332f2b">Feria del Emprendedor</a>
        </header>     
    </div>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light rounded-4 mb-3 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Modificar Datos de Usuario</h1>
        </div>
        <form @submit.prevent="checkIfNewData" class="mt-3 ff-lspartan">
          <label for="userName" class="form-label mb-0">Nombre</label>
          <div class="mb-3">
            <input
              id="userName"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text"
              placeholder=""
              v-model="newData.name"
            >
          </div>
          <div class="mb-3">
            <label for="email" class="form-label mb-0">Correo electrónico</label>
            <input
              id="email"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text"
              placeholder=""
              v-model="newData.emailAddress"
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
              v-model="newData.phoneNumber"
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
    import { mapGetters } from 'vuex';
    export default {
        computed: {
            ...mapGetters(['isLoggedIn']),
            ...mapGetters(['getUserId']),
        },
        data() {
            return {
                newData: {
                    name: "",
                    emailAddress: "",
                    phoneNumber: ""
                },
                oldData: {
                    name: "",
                    emailAddress: "",
                    phoneNumber: ""
                },
                userID: -1,
                validName: true,
                validEmail: true,
                validPhone: true,
            }
        },
        mounted() {
            if(this.isLoggedIn) {
                this.userID = this.getUserId;
                this.getOldData();
            } else {
                alert("Para usar esta funcion debe accesar a una cuenta");
                this.$router.push('LandingPage')
            }
        },
        methods: {
            getOldData() {
                axios.get(this.$backendAddress + "api/UserData/getData", {
                params: {
                    userID: this.userID
                }}).then((response) => {
                    this.oldData.name = response.data.name;
                    this.oldData.emailAddress = response.data.emailAddress; 
                    this.oldData.phoneNumber = response.data.phoneNumber;
                    this.newData.name = response.data.name;
                    this.newData.emailAddress = response.data.emailAddress; 
                    this.newData.phoneNumber = response.data.phoneNumber;
                }).catch( (error) => {
                    console.log(error);
                    alert("Error al recibir datos del usuario, volviendo al perfil");
                    this.$router.push("UserProfile")
                });
            },
            checkIfNewData() {
                if ( this.newData.name === this.oldData.name &&
                     this.newData.emailAddress === this.oldData.emailAddress &&
                     this.newData.phoneNumber === this.oldData.phoneNumber
                 ) {
                    alert("No se dieron cambios en el perfil");
                    this.$router.push('UserProfile');
                } else {
                    this.checkEntries();
                }
            },
            checkEntries() {
                this.checkName();
                this.checkEmail();
                this.checkPhone();
                if(this.validName && this.validEmail && this.validPhone) {
                    this.updateData();
                } else {
                    alert("Datos invalidos, revise de nuevo");
                }
            },
            checkName(){
                this.validName = (/^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$/.test(this.newData.name)) && 
                                 (this.newData.name.trim().length <= 60);
                if (!this.validName) console.log("Error in name");
            },
            checkEmail() {
                this.validEmail = (/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(
                                    this.newData.emailAddress));
                if (!this.validEmail) console.log("Error in email");
            },
            checkPhone() {
                this.validPhone = (/^\d{8}$/.test(this.newData.phoneNumber))
                if (!this.validPhone) console.log("Error in phone");
            },
            updateData() {
                axios.post(this.$backendAddress + "api/UserData/updateData", {
                    name: this.newData.name,
                    emailAddress: this.newData.emailAddress,
                    phoneNumber: this.newData.phoneNumber,
                    UserID: this.userID
                }).then((response)=>{
                    console.log(response.data);
                    alert("Se actualizaron los datos correctamente");
                    this.$router.push("UserProfile");
                }).catch((error)=>{
                    console.log("Error al hacer update ", error);
                    alert("Error al actualizar datos, volviendo al perfil");
                    this.$router.push("UserProfile");
                })
            },
        },
    };
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
    .errorInInputsLabel{
        color: red
    }
</style>