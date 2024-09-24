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
            alt="Buscar" 
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
          </ul>
        </div>
      </div>
    </nav>
  </div>
</template>

<script>
export default {
  
}
</script>

<style lang="scss">
  
</style>
