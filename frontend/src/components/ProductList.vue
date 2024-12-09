<template>
  <div class="container-fluid h-100 d-flex flex-column my-4">
    <div v-if="products.length === 0" class="alert alert-info text-center">
      No se encontraron resultados.
    </div>


    <div class="vertical-slider overflow-auto flex-grow-1">
      <div v-for="product in products" :key="product.id" class="mb-3">
        <div class="card h-100 shadow-sm card-background">
          <div class="row g-0">

            <div class="col-md-4">
              <img
                :src="product.imageURL"
                alt="Imagen del producto"
                class="card-img img-fluid"
              />
            </div>
            <div class="col-md-8">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">{{ product.productName }}</h5>
                <p class="card-text text-justify"> 
                  <strong>Descripción:</strong> {{ product.productDescription }} <br />
                  <strong>Categoría:</strong> {{ product.category }} <br />
                  <strong>Compañía:</strong> {{ product.companyName }} <br />
                </p>
              </div>
            </div>
          </div>

       
          <div class="card-footer d-flex justify-content-between align-items-center">
            <div class="d-flex flex-column">
              <p class="fw-bold text-success">Precio:  ₡{{ product.price }}</p>
            </div>

            <div class="d-flex flex-column align-items-end">
              <div v-if="product.productionLimit > 0" class="d-flex align-items-center">
                <img
                  src="../assets/PerishableIcon.png" 
                  alt="Perecedero"
                  class="me-2"
                  style="width: 30px; height: 30px"
                />
                <button
                  @click="showDeliveryOptions(product)"
                  class="btn btn-warning btn-sm"
                   style="background-color: #d57c23; border-color: #d57c23;"
                >
                  Entrega
                </button>
              </div>
              <div class="input-group mb-2 mt-2">
              <button @click="decrementQuantity(product)" class="btn btn-secondary">-</button>
              <input
                type="number"
                v-model="product.quantity"
                min="1"
                class="form-control"
                style="width: 60px; text-align: center;"
                @focus="disableInput"
              />
              <button @click="incrementQuantity(product)" class="btn btn-secondary">+</button>
              <button @click="addToCart(product)" class="btn btn-success" style="background-color: #d57c23; border-color: #d57c23;">
                🛒 Agregar al carrito
              </button>
              </div>
        
              <p v-if="selectedProduct && selectedProduct.productID === product.productID
              && product.productionLimit" class="text-muted">
                Stock: {{ product.stock = selectedProduct.productionLimit - (deliveryChosen ? deliveryChosen.reservedUnits : 0) }}
              </p>
              <p v-else>
                Stock: {{ product.stock }}
              </p>
          
            </div>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="showDeliveryModal"
      class="modal fade show"
      style="display: block"
      tabindex="-1"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Opciones de entrega</h5>
            <button
              type="button"
              class="btn-close"
              @click="closeDeliveryModal"
            ></button>
          </div>
          <div class="modal-body">
            <ul class="list-group">
              <li
                v-for="delivery in deliveries"
                :key="delivery.id"
                class="list-group-item d-flex justify-content-between align-items-center border-0 rounded shadow-sm mb-2"
              >
                <div class="d-flex flex-column">
                  <span class="fw-bold">Lote: {{ delivery.batchNumber }}</span>
                  <span class="text-muted">Expira: {{ delivery.expirationDate }}</span>
                  <span class="text-success">
                    Unidades Disponibles: 
                    <strong>{{ this.selectedProduct.productionLimit - delivery.reservedUnits }}</strong>
                  </span>
                </div>
                <button
                  @click="selectDelivery(delivery)"
                  class="btn btn-primary btn-sm"
                >
                  Seleccionar
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Swal from "sweetalert2";
import {  mapState, } from 'vuex';
export default {
  computed: {
            ...mapState(['userCredentials']),
        },
  props: {
    products: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      deliveryChosen: null,
      showDeliveryModal: false,
      deliveries: [],
      selectedProduct: null,
   
    };
  },
  
  methods: {
    async addToCart(item) {
      if (this.negativeOrZeroQuantity(item)) {
        return; 
      }
      if(this.notEnoughStock(item)){
        return;
      }
      console.log(item);
      const productToAdd = {
                    userID: Number(this.userCredentials.userId),
                    productID: item.productID,
                    productName: item.productName,
                    productPrice: item.price,
                    quantity: item.quantity || 1,
                    imageURL: item.imageURL,
                    isPerishable: item.productID % 2 === 0
                };

                try {
                    const response = await axios.post(this.$backendAddress +`api/ShoppingCart/add`, {
                        ...productToAdd
                    });

                    if (response.status === 200) {
                      this.addedToCartSuccess(item)
                        item.quantity = 1;
                    } else {
                        console.error('Error adding product to cart:', response.data);
                    }
                } catch (error) {
                    console.error('Error while adding product to cart:', error);
                }
    },
    negativeOrZeroQuantity(item){
      if (item.quantity <= 0) {
        Swal.fire({
          icon: "error",
          title: "Cantidad inválida",
          text: "La cantidad no puede ser 0 o negativa.",
          confirmButtonColor: "#d33", 
          confirmButtonText: "Entendido",
        });
        return true; 
      }
        return false
    },
    notEnoughStock(item){
      if (item.quantity > item.stock || !item.stock) {
      Swal.fire({
        icon: "error",
        title: "Cantidad inválida",
        text: "No hay suficiente stock.",
        confirmButtonColor: "#d33", 
        confirmButtonText: "Entendido",
      });
      return true; 
    }
    return false;
    },
    incrementQuantity(product) {
        if(!product.quantity){
          product.quantity = 0;
        }
        product.quantity++;
        if ( this.notEnoughStock(product)) {
           product.quantity--;
            return;
        } 
      },
      addedToCartSuccess(item) {
        Swal.fire({
          icon: "success",
          title: "Producto añadido",
          text: `${item.productName} ha sido añadido al carrito.`,
          confirmButtonColor: "#3085d6", 
          confirmButtonText: "Entendido",
        });
      },
    decrementQuantity(product) {
        if (product.quantity > 1) {
          product.quantity--;
        }
      },
    showDeliveryOptions(product) {
      this.selectedProduct = product;
      this.fetchDeliveries(product.productID);
      this.showDeliveryModal = true;
    },
    closeDeliveryModal() {
      this.showDeliveryModal = false;
    },
    async fetchDeliveries(productId) {
      try {
        const response = await axios.get(this.$backendAddress + "api/products/getProductDeliveries", {
          params: { searchTerm: productId }
        });
        this.deliveries = response.data;
      } catch (error) {
        console.error("Error fetching deliveries:", error);
      }
    },
    selectDelivery(delivery) {
      this.deliveryChosen=delivery;
      console.log("Selected Delivery:", delivery);
      this.closeDeliveryModal();
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

.modal-body {
    max-height: 400px; 
    overflow-y: auto;  
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