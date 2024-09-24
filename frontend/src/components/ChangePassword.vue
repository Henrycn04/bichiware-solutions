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
          <img
            src="../assets/SearchIcon.png"
            alt="Search" 
            class=""
            width=23
            height=23
          />
        </button>
        <div class="navbar-collapse collapse ">
          <ul class="navbar-nav ">
            <li class="nav-item">
              <a class="nav-link" href="/login">Acceder</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="/cart">Carrito</a>
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
            <a class="nav-link" href="/">
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
            <a class="nav-link">
              No perecederos
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link">
              Perecederos
            </a>
          </li>
          <li class="nav-itme">
            <a class="nav-link">
              Lista de usuarios
            </a>
          </li>
          <li class="nav-itme" disabled>
            <a class="nav-link">
              Lista de empresas
            </a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light rounded-4 mb-3 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Cambiar Contraseña</h1>
        </div>
        <div v-if="false"
          class="ff-poppins container d-flex justify-content-center"
          style="height: 64px;"
        >
          <div class="d-flex justify-content-center align-items-center">
            <img
              src="../assets/WarningSign.png"
              alt=""
              width="50"
              height="50"
              class="py-1"
            >
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
          class="mt-3 ff-lspartan"
          @submit="changePassword"
        >
          <div class="mb-3">
            <label for="newPassword" class="form-label mb-0">Contraseña Nueva</label>
            <input id="newPassword"
              class="form-control rounded-2 border-0 bg-secondary"
              type="password" required
              v-model="inputCode"
            />
            <div id="newPasswordHelpBlock" class="form-text">
              Help block
            </div>
          </div>
          <div class="mb-3">
            <label for="newPasswordConfirm" class="form-label mb-0">Confirmar Contraseña Nueva</label>
            <input id="newPasswordConfirm"
              class="form-control rounded-2 border-0 bg-secondary"
              type="password" required
              v-model="inputCode"
            />
          </div>
          <div class="mb-3">
            <label for="securityCode" class="form-label mb-0">Código de Seguridad</label>
            <input id="securityCode"
              class="form-control rounded-2 border-0 bg-secondary"
              type="text" required
              maxlength="6"
              minlenght="6"
              v-model="inputCode"
            />
            <div id="securityCodeHelpBlock" class="form-text">
              En caso de que no le haya llegado el código de seguridad a su correo o desea uno nuevo, debe presionar el botón reenivar código. Solo puede recibir uno cada 15 minutos.
            </div>
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
export default {
  
}
</script>

<style lang="scss">
  
</style>
