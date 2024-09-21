<template>
  <div style=" height: 100vh;" class="bg-secondary">
    <nav class="navbar bg-primary">
      <div class="container-fluid">
        <span class="navbar-brand fw-bold ff-lspartan" href="#">Feria del Emprendedor</span>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light pt-4 pb-4 rounded-4 d-flex flex-column mb-3">
        <h1 class="display-6 text-center fw-bold ff-lspartan">Confirmación</h1>
        <div v-if="!resentCode && !wrongInput"
          class="ff-poppins container">
          Se le ha enviado un correo con un código de 6 digitos para confirmar el registro de su nueva cuenta. Solamente debe colocarlo abajo y darle al botón <strong>Crear Cuenta</strong>.
        </div>
        <div v-else-if="resentCode"
          class="ff-poppins container text-center">
          Se le ha enviado un <span class="fw-bold">nuevo</span> código. Escribalo abajo y dele al botón Crear Cuenta.
        </div>
        <div v-else
          class="ff-poppins container d-flex justify-content-center"
          style="height: 64px;">
          <div
            class="d-flex justify-content-center align-items-center"
          >
            <img
            src="../assets/WarningSign.png"
            alt=""
            width="50"
            height="50"
            class="py-1">
          </div>
          <div
            class="d-flex flex-column justify-content-center ps-2">
            <span
              class="text-danger">
              El código que ingreso no es correcto. Los códigos tienen una duración de 15 minutos. Vuelva a intentarlo o pida uno nuevo.
            </span>
          </div>
        </div>
        <form
          method="dialog"
          class="text-center mt-4"
          @submit="validateCode"
        >
          <label
            class="form-label ff-lspartan fs-5"
            for="confirmationCode"><strong>Código de Confirmación</strong></label>
          <br>
          <input
            id="confirmationCodeInput"
            name="confirmationCode"
            class="form-control-sm rounded-2 border-0 bg-secondary p-2 mt-1 text-center fs-5"
            type="text"
            placeholder="XXXXXX"
            maxlength="6"
            minlength="6"
            pattern="(\w){6}"
            required
            v-model="inputCode"
          />
          <div class="container mt-5 mb-4 ff-poppins text-center">
            En caso de que no le haya llegado el código, utilize el botón inferior de renviar código.
          </div>
          <div class="d-grid gap-2">
            <input
                name="resendCode"
                class="btn btn-secondary ff-lspartan fs-5"
                type="button"
                value="Renviar Código"
                @click="resendCode"
              >
              <input
                name="createAccount"
                class="btn fw-bold btn-primary ff-lspartan fs-5"
                type="submit"
                value="Crear Cuenta"
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
  import CryptoJS from "crypto-js";
  import axios from "axios";

  export default {
    setup()
    {
      return { }
    },

    data()
    {
      return {
        wrongInput: false,
        resentCode: false,
        inputCode: null,
        userId: '',
        errorSendingEmail: false,
      }
    },


    mounted()
    {
      this.userId = this.$store.getters.getUserId;
    },

    methods: {
      getDateTimeNow : function ()
      {
        var dateTimeFormatted = new Date();
        return dateTimeFormatted.toISOString();
      },


      hashInput : function ()
      {
        var hashedCode = CryptoJS.SHA512(this.inputCode).toString().toUpperCase();
        return hashedCode;
      },


      validateCode()
      {
        this.resentCode = false;
        axios.post("https://localhost:7263/api/AccountActivation/ActivateAccount",
          {
            "userId": this.userId,
            "confirmationCode": this.hashInput(),
            "dateTimeLastCode": this.getDateTimeNow()
          })
          .then((response) =>
          {
            if (response.data)
            {
              window.location.href = "/";
            } else {
              this.wrongInput = true;
            }
          });
      },


      resendCode()
      {
        axios.post('https://localhost:7263/api/AccountActivation/RequestConfirmationEmail?userId=' + this.userId)
          .then((response) =>
          {
            if (response.data)
            {
              this.resentCode = true;
            }
            else
            {
              this.errorSendingEmail = true;
            }
          });
      },
    }
  }
</script>

<style lang="scss" scoped>
</style>