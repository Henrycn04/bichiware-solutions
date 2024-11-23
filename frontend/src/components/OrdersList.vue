<template>
  <div class="container-fluid h-100 d-flex flex-column">
    <div v-if="orders.length === 0" class="alert alert-info text-center">
      No se encontraron órdenes.
    </div>

    <div class="vertical-slider overflow-auto flex-grow-1">
      <div v-for="order in orders" :key="order.orderID" class="mb-3">
        <div class="card h-100 shadow-sm card-background">
          <div class="card-body">
            <h5 class="card-title">Orden #{{ order.orderID }}</h5>
            <p class="card-text">
              <span v-if="CheckIfUserIsEntrepreneur()">
                <strong>Nombre de la empresa:</strong> {{ order.companyName }}<br />
              </span>
              <strong>Fecha entrega:</strong> {{ order.deliveryDate }} <br />
              <strong>Costo total:</strong> ₡{{ calculateTotal(order) }} <br />
              <strong>Estado:</strong> 
              {{ order.orderStatus === 1 ? 'Pendiente de revisión' : 
                order.orderStatus === 2 ? 'Confirmado' : 
                order.orderStatus === 4 ? 'Enviado' : 'Otro Estado' }}<br />
              <strong>Cantidad de Productos:</strong> {{ order.products.length }}
            </p>
            <button 
              v-if="!CheckIfUserIdAdmin()" 
              @click="cancelOrder(order.orderID)" 
              class="btn btn-danger ms-2">
              Cancelar pedido
            </button>
          </div>
          <div class="card-footer">
            <h6 class="fw-bold">Productos:</h6>
            <div v-for="product in order.products" :key="product.productID" class="d-flex justify-content-between mb-2">
              <span><strong>{{ product.productName }}</strong></span>
              <span>Cantidad: {{ product.quantity }}</span>
              <span>Precio: ₡{{ product.productPrice }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
  import { mapGetters, mapState } from 'vuex';
  import axios from "axios"
  export default {
    props: {
      orders: {
        type: Array,
        required: true,
      }
    },     
    computed: {
      ...mapGetters(['isLoggedIn']),
      ...mapState(['userCredentials']),
    },
    methods: {
        CheckIfUserIdAdmin() {
          return this.userCredentials.userType == 3;
        }, 
        CheckIfUserIsEntrepreneur() {
          return this.userCredentials.userType == 2;
        },
        calculateTotal(order) {
          return order.productCost + order.shippingCost + order.tax;
        },
        async cancelOrder(orderID) {
          const confirmOrderModel = {
            OrderID: orderID, 
          };
          if (this.userCredentials.userId == 1) {
            const response = await axios.post(this.$backendAddress + "api/CancelOrders/CancelOrderByUser", confirmOrderModel)
              .catch((error) => {
                  console.error("Error sending order ID:", error);
              });
            alert(response.data);
          } else if (this.userCredentials.userId == 2) {
            const response = await axios.post(this.$backendAddress + "api/CancelOrders/CancelOrderByEntrepreneur", confirmOrderModel)
              .catch((error) => {
                  console.error("Error sending order ID:", error);
              });
            alert(response.data);
          }
        }
      }
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
