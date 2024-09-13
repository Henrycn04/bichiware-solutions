<template>
  <header class="header">
    <nav class="navbar bg-body-tertiary">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Feria del Emprendedor</a>
      </div>
    </nav>
  </header>
  <div class="container mt-5 mb-5">
    <div class="container mt-5">
      <h1 class="display-5 text-center">Confirmación</h1>
      <div class="container">
        Se le ha enviado un correo con un código de 6 digitos para confirmar el registro de su nueva cuenta. Solamente debe colocarlo abajo y darle al botón <strong>Crear Cuenta</strong>.
      </div>
      <br>
      <form action="" class="text-center">
        <label class="form-label" for=""><strong>Código de Confirmación</strong></label>
        <br>
        <input class="form-control-sm" type="text">
      </form>
      <div class="container mt-5 mb-4">
        En caso de que no le haya llegado el código, utilize el botón inferior de renviar código.
      </div>
      <form action="" class="text-center d-grid gap-2">
        <input class="btn btn-secondary" type="button" value="Renviar Código">
        <input class="btn btn-primary" type="button" value="Crear Cuenta">
      </form>
    </div>
  </div>
  <footer class="text-center mt-3 p-2 fs-6 fst-italic bg-secondary">
    @Copyright BichiWare Solutions 2024
  </footer>
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