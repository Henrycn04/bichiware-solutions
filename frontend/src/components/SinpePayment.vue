<template>
    <div style="height: 100vh;" class="bg-secondary">
      <div class="bg-primary pt-3 pb-3">
        <header class="header">
            <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer; text-decoration: none ; color: #332f2b">Feria del Emprendedor</a>
        </header>     
      </div>
      <div class="container-fluid bg-secondary py-5 col-md-6 mx-auto">
        <div class="container bg-light rounded-4 mb-3 pb-4">
          <div class="row bg-primary pt-3 rounded-top-4">
            <h1 class="display-6 text-center fw-bold ff-lspartan">Método de pago: Sinpe</h1>
          </div>
          <form class="mt-3 ff-lspartan">
            <div style="display: flex; justify-content: center; align-items: center;">
                <div style="max-width: 600px; text-align: justify; font-size: large;">
                    Realice el sinpe móvil al número de teléfono 12345678.<br>
                    Escriba en la descripción de la transacción su nombre completo.<br>
                    Posteriormente, adjunte en el espacio indicado el comprobante de pago.<br><br>
                </div>
            </div>

            <div class="mb-3">
            <label class="form-label mb-2 fw-bold fs-4" style="padding: 12px;">Adjunte el comprobante</label>
            <input 
                type="file"
                accept="image/*"
                class="form-control form-control-lg"
                style="padding: 12px; font-size: 1.2rem;"
                required
            />
            </div>

            <br>
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
  </script>
  
  <style>
  </style>