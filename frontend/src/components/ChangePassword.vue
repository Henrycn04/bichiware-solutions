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
          <h1 class="display-6 text-center fw-bold ff-lspartan">Cambiar Contraseña</h1>
        </div>
        <div v-if="failedChangingPassword"
          class="ff-poppins container d-flex justify-content-center my-3"
          style="height: 64px;"
        >
          <div
            class="d-flex justify-content-center align-items-center"
          >
            <svg fill="#dc3545" width="50px" height="50px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
              <g data-name="Layer 21" id="Layer_21">
                <path d="M27.25,22.68,18.51,7.37a2.9,2.9,0,0,0-5,0L4.75,22.68A2.88,2.88,0,0,0,7.25,27h17.5a2.88,2.88,0,0,0,2.5-4.32Zm-1.74,1.88a.85.85,0,0,1-.76.44H7.25a.85.85,0,0,1-.76-.44.87.87,0,0,1,0-.89L15.23,8.36a.89.89,0,0,1,1.54,0l8.75,15.31A.87.87,0,0,1,25.51,24.56Z"/>
                <path d="M16,13a1,1,0,0,0-1,1v5a1,1,0,0,0,2,0V14A1,1,0,0,0,16,13Z"/>
                <circle cx="16" cy="22" r="1"/>
              </g>
            </svg>
          </div>
          <div class="d-flex flex-column justify-content-center ps-2">
            <div class="text-danger">
              Contraseñas no concuerdan o el código de seguridad es incorrecto, intente de nuevo
            </div>
          </div>
        </div>
        <form
          method="dialog"
          class="mt-3 ff-lspartan"
          @submit="changePassword"
        >
          <label for="email" class="form-label mb-0">Correo Electrónico</label>
          <div class="d-flex gap-2">
            <input id="email"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text" required placeholder="Correo Electrónico"
              v-model="email"
              pattern="\w+@\w+\.\w+"
            >
            <input
              name="resendCode"
              class="btn btn-secondary ff-lspartan fs-5"
              type="button"
              value="Enviar Código"
              @click="resendCode"
            >
          </div>
          <div v-show="resendCodeMessage" class="form-text" ref="codeSentMessage">
            Su nuevo código fue enviado. Debe esperar 15 minutos antes de solicitar otro.
          </div>
          <hr class="hr">
          <div class="mb-3">
            <label for="newPassword" class="form-label mb-0">Contraseña Nueva</label>
            <input id="newPassword"
              class="form-control rounded-2 border-0 bg-secondary"
              type="password" required placeholder="Contraseña Nueva"
              pattern="(\w){8,}"
              v-model="newPassword"
            />
            <div id="newPasswordHelpBlock" class="form-text">
              Debe de tener como mínimo 8 caractéres y no debe contener caractéres especiales o emojis.
            </div>
          </div>
          <div class="mb-3">
            <label for="newPasswordConfirm" class="form-label mb-0">Confirmar Contraseña Nueva</label>
            <input id="newPasswordConfirm"
              class="form-control rounded-2 border-0 bg-secondary"
              type="password" required placeholder="Confirmar Contraseña Nueva"
              pattern="(\w){8,}"
              v-model="confirmNewPassword"
            />
          </div>
          <div class="mb-3">
            <label for="securityCode" class="form-label mb-0">Código de Seguridad</label>
            <input id="securityCode"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text" required placeholder="Código de Seguridad"
              maxlength="6"
              pattern="(\w){6}"
              v-model="securityCode"
            />
            <div id="securityCodeHelpBlock" class="form-text">
              En caso de que no le haya llegado el código de seguridad a su correo o desea uno nuevo, debe presionar el botón enivar código.
            </div>
          </div>
          <div class="d-grid gap-2">
            <input
              name="submitChange"
              class="btn fw-bold btn-primary ff-lspartan fs-5"
              type="submit"
              value="Cambiar Contraseña"
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
import CryptoJS from "crypto-js";
import { mapGetters } from 'vuex';

export default {
  data ()
  {
    return {
      email: '',
      failedChangingPassword: false,
      resendCodeMessage: false,
      newPassword: "",
      confirmNewPassword: "",
      securityCode: "",
      isAdminOrEntrepreneur: false,
    }
  },


  mounted()
  {
    var userType = this.getUserType();
    this.isAdminOrEntrepreneur = userType == 1 || userType == 2;      
  },


  methods:
  {
    ...mapGetters(["getUserType", "isLoggedIn"]),


    getDateTimeNow : function ()
    {
      var dateTimeFormatted = new Date();
      return dateTimeFormatted.toISOString();
    },


    hashData : function (data)
    {
      var hashedCode = CryptoJS.SHA512(data).toString().toUpperCase();
      return hashedCode;
    },

    
    resendCode () {
      var canResendCode = true;
      var dateTimeNow = new Date();
      var dateTimeLastRequestedCode = this.$store.getters.getDateTimeLastRequestedCode;

      if (dateTimeLastRequestedCode)
      {
        var minutesSinceLastCode = dateTimeNow.getMinutes() - dateTimeLastRequestedCode.getMinutes();
        canResendCode = minutesSinceLastCode > 15;

        if (!canResendCode) console.log("Debe esperar " + (15 - minutesSinceLastCode) + " minutos");
      }
      canResendCode = canResendCode && this.email.length > 0;

      if (canResendCode)
      {
        axios.post(this.$backendAddress + 'api/ChangePassword/SendConfirmationEmail?email=' + this.email)
        .then((response) =>
        {
          if (response.data)
          {
            this.$store.commit('setDateTimeLastRequestedCode', dateTimeNow);
            this.$refs.codeSentMessage.innerHTML = "Su nuevo código fue enviado. Debe esperar 15 minutos antes de solicitar otro.";
          }
        })
        .catch((error) => {
          if (error.response) {
            this.$refs.codeSentMessage.innerHTML = "Error " + error.response.status;
          } else if (error.request) {
            console.log(error.request);
            this.$refs.codeSentMessage.innerHTML = "Error conectando al servidor";
          } else {
            console.log('Error al preparar la consulta', error);
          }
        });
      }
      else
      {
        this.$refs.codeSentMessage.innerHTML = "No se pudo enviar el codigo, verifique que lo esta escribiendo correctamente o que siga un formato establecido";
      }
      this.resendCodeMessage = true;
    },


    updateNewPassword () {
      axios.post(this.$backendAddress + "api/ChangePassword/ChangePassword",
      {
        "email": this.email,
        "newPassword": this.newPassword,
        "securityCode": this.hashData(this.securityCode)
      })
      .then((response) =>
      {
        if (response.data)
        {
          console.log("Success");
          window.location.href = "/";
        }
        else
        {
          console.log("Didnt worked")
          this.failedChangingPassword = true;
        }
      });
    },


    validateInput () {
      return this.confirmNewPassword == this.newPassword;
    },

    
    changePassword () {
      this.resendCodeMessage = false;

      if (!this.validateInput())
      {
        console.log("Passwords are not equal")
        this.failedChangingPassword = true;
      }
      else
      {
        this.updateNewPassword();
      }
      console.log("change password was pressed");
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

<style lang="scss">
  
</style>
