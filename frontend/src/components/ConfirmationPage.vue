<template>
  <div class="d-flex flex-column" style="">
    <nav class="navbar bg-primary">
      <div class="container-fluid">
        <a class="navbar-brand fw-bold ff-poppins" href="#">Feria del Emprendedor</a>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5 h-100">
      <div class="container bg-light pt-4 pb-4 rounded-4 h-100 d-flex flex-column mb-3">
        <h1 class="display-6 text-center fw-bold ff-poppins">Confirmación</h1>
        <div class="ff-poppins container">
          Se le ha enviado un correo con un código de 6 digitos para confirmar el registro de su nueva cuenta. Solamente debe colocarlo abajo y darle al botón <strong>Crear Cuenta</strong>.
        </div>
        <form action="" class="text-center">
          <label class="form-label ff-poppins" for=""><strong>Código de Confirmación</strong></label>
          <br>
          <input
            class="form-control-sm rounded-2 border-0 bg-secondary p-2 mt-1 text-center fs-5"
            type="text"
            placeholder="xxx-xxx"
            maxlength="6" />
        </form>
        <div class="container mt-5 mb-4 ff-poppins text-center">
          En caso de que no le haya llegado el código, utilize el botón inferior de renviar código.
        </div>
        <form action="" class="text-center d-grid gap-2">
          <input
            class="btn btn-secondary ff-poppins"
            type="button"
            value="Renviar Código"
          >
          <input class="btn fw-bold btn-primary ff-poppins"
            type="button"
            value="Crear Cuenta"
          >
        </form>
      </div>
    </div>
    <footer class="text-center p-2 fst-italic bg-ternary text-light ff-poppins">
      @Copyright BichiWare Solutions 2024
    </footer>
  </div>
</template>

<script>
export default {
  setup () {
    return {}
  }
}
</script>

<style lang="scss" scoped>
</style>