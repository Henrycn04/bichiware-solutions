<template>
  <div class="container-fluid h-100 d-flex flex-column my-4">
    <div v-if="orders.length === 0" class="alert alert-info text-center">
      No se encontraron órdenes.
    </div>

    <div class="vertical-slider overflow-auto flex-grow-1">
      <div v-for="order in orders" :key="order.orderID" class="mb-3">
        <div class="card h-100 shadow-sm card-background">
          <div class="card-body">
            <h5 class="card-title">Orden #{{ order.orderID }}</h5>
            <p class="card-text">
              <strong>ID Usuario:</strong> {{ order.userID }} <br />
              <strong>ID Dirección:</strong> {{ order.addressID }} <br />
              <strong>ID Costo de Envío:</strong> {{ order.feeID }} <br />
              <strong>Impuesto:</strong> ₡{{ order.tax }} <br />
              <strong>Costo de Envío:</strong> ₡{{ order.shippingCost }} <br />
              <strong>Costo del Producto:</strong> ₡{{ order.productCost }} <br />
              <strong>Estado de la Orden:</strong> {{ order.orderStatus }} <br />
              <strong>Fecha de Entrega:</strong> {{ order.deliveryDate }}
            </p>
          </div>

          <div class="card-footer">
            <h6 class="fw-bold">Productos:</h6>
            <div v-for="product in order.products" :key="product.productID" class="mb-2">
              <p class="mb-1">
                <strong>Producto:</strong> {{ product.productName }} <br />
                <strong>Cantidad:</strong> {{ product.quantity }} <br />
                <strong>Precio Unitario:</strong> ₡{{ product.productPrice }} <br />
                <strong>Número de Lote:</strong> {{ product.batchNumber || 'N/A' }} <br />
                <strong>Tipo de Producto:</strong> {{ product.productType }}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    orders: {
      type: Array,
      required: true,
    },
  },
};
</script>


<style scoped>
.container-fluid {
  height: 100vh; 
  background-color: #ffeec2;
}

.vertical-slider {
  max-height: calc(70vh - 70px); 
  display: flex;
  flex-direction: column;
  gap: 15px;
  border: none;
  overflow-y: auto; 
}
.card {
  min-width: 250px; 
  background-color: #f1d897; 
}

.overflow-auto {
  overflow-y: auto; 
}

.img-fluid {
  max-height: 200px; 
  object-fit: cover;
}

.modal {
  background-color: rgba(0, 0, 0, 0.5);
}

.card-background {
  background-color: #f1d897; 
}

.text-justify {
  text-align: justify;
}
input[type=number]::-webkit-inner-spin-button,
input[type=number]::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

input[type=number] {
  -moz-appearance: textfield;
}
</style>
