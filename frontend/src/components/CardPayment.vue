<template>
    <div style="height: 100vh;" class="bg-secondary">
      <div class="bg-primary pt-3 pb-3">
        <header class="header">
                  <a href="/orderconfirmation" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer; text-decoration: none ; color: #332f2b">Feria del Emprendedor</a>
          </header>     
      </div>
      <div class="container-fluid bg-secondary py-5">
        <div class="container bg-light rounded-4 mb-3 pb-4">
          <div class="row bg-primary pt-3 rounded-top-4">
            <h1 class="display-6 text-center fw-bold ff-lspartan">Método de pago: Tarjeta</h1>
          </div>
          <form @submit.prevent="redirect" class="mt-3 ff-lspartan">
            <label for="cardNumber" class="form-label mb-0">Número de la tarjeta</label>
            <div class="mb-3">
              <input
                id="cardNumber"
                class="form-control rounded-2 border-0 bg-secondary"
                maxlength="16" 
                pattern="\d{16}"
                placeholder="Número de la tarjeta"
                required
              >
            </div>
            <label for="cardHolder" class="form-label mb-0">Titular</label>
            <div class="mb-3">
              <input
                id="cardHolder"
                class="form-control rounded-2 border-0 bg-secondary"
                type="text"
                placeholder="Nombre del Titular de la Tarjeta"
                required
              >
            </div>
            <div class="mb-3">
              <label for="expirationDate" class="form-label mb-0">Fecha de vencimiento</label>
              <input
                id="expirationDate"
                class="form-control rounded-2 border-0 bg-secondary"
                type="date"
                placeholder=""
                required
              />
            </div>
            <div class="mb-3">
              <label for="securityCode" class="form-label mb-0">Código de seguridad</label>
              <input
                id="securityCode"
                class="form-control rounded-2 border-0 bg-secondary"
                maxlength="3" 
                pattern="\d{3}"
                placeholder="Código de seguridad"
                required
              />
            </div>
            <div class="d-grid gap-2">
              <button
                name="submitChange"
                class="btn fw-bold btn-primary ff-lspartan fs-5"
                type="submit"
              >
                Confirmar
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
  import { mapActions } from 'vuex';
  export default {
    methods: {
      ...mapActions(['paymentWasSuccesful']),
      redirect() {
        alert("Payment succesful.");
        this.paymentWasSuccesful(true);
        this.$router.push(`/orderconfirmation`);
      }
    }
  };
  </script>
  
  <style>
  </style>