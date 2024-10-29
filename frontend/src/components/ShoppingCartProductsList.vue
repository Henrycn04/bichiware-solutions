<template>
    <div class="container-fluid h-100 d-flex flex-column my-4">
      <div v-if="products.length === 0" class="alert alert-info text-center">
        No se encontraron resultados.
      </div>
  
      <div class="vertical-slider overflow-auto flex-grow-1">
        <div v-for="product in products" :key="product.productID" class="mb-3">
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
                <p class="fw-bold text-success">Precio:  ₡{{ product.productPrice }}</p>
                <p class="text-muted">Cantidad en el carrito: {{ product.currentCartQuantity }}</p> 
              </div>
  
              <div class="d-flex flex-column align-items-end">
                <div v-if="product.isPerishable" class="d-flex align-items-center">
                  <img
                    src="../assets/PerishableIcon.png" 
                    alt="Perecedero"
                    class="me-2"
                    style="width: 30px; height: 30px"
                  />
                </div>
                <div class="input-group mb-2 mt-2">
                  <button @click="decrementQuantity(product)" class="btn btn-secondary">-</button>
                  <input
                    type="number"
                    v-model="product.currentCartQuantity"
                    min="1"
                    class="form-control"
                    style="width: 60px; text-align: center;"
                    @focus="disableInput"
                    readonly 
                  />
                  <button @click="incrementQuantity(product)" class="btn btn-secondary">+</button>
                  <button @click="updateQuantity(product)" class="btn btn-success" style="background-color: #d57c23; border-color: #d57c23;">
                    🔄 Actualizar Cantidad
                  </button>
                  <button @click="removeFromCart(product)" class="btn btn-danger ms-2">🗑️ Eliminar</button>
                </div>
                <p v-if="!product.isPerishable">
                  Stock: {{ product.currentStock }} 
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
  import { mapGetters, mapState } from 'vuex';

  export default {
    props: {
      products: {
        type: Array,
        required: true,
      },
    },
    computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
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
      async updateQuantity(product) {
        const productToUpdate = {
                    userID: product.userID,
                    productID: product.productID,
                    isPerishable: product.productID % 2 === 0,
                    currentCartQuantity: product.currentCartQuantity
                };
                try {
                    const response = await axios.post(this.$backendAddress +"api/ShoppingCart/update", {
                        ...productToUpdate
                    });

                    if (response.status === 200) {
                        console.log('Product updated from cart successfully');
                        window.location.reload();
                    } else {
                        console.error('Error updating product from cart:', response.data);
                    }
                } catch (error) {
                    console.error('Error while updating product from cart:', error);
                }
        console.log("Updated quantity for:", product);
      },
      incrementQuantity(product) {
          if (!product.currentCartQuantity) {
            product.currentCartQuantity = 0;
          }
          if (product.isPerishable || (!product.isPerishable && product.currentCartQuantity < product.currentStock)) { // add this in search page
            product.currentCartQuantity++;
          } else {
            alert("No hay suficiente stock disponible");
          }
        },
      decrementQuantity(product) {
          if (product.currentCartQuantity > 1) {
            product.currentCartQuantity--;
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
        this.deliveryChosen = delivery;
        console.log("Selected Delivery:", delivery);
        this.closeDeliveryModal();
      },
      async removeFromCart(item) {
        const productToDelete = {
                    userID: item.userID,
                    productID: item.productID,
                    isPerishable: item.productID % 2 === 0
                };
                try {
                    const response = await axios.post(this.$backendAddress +"api/ShoppingCart/delete", {
                        ...productToDelete
                    });

                    if (response.status === 200) {
                        console.log('Product deleted from cart successfully');
                        window.location.reload();
                    } else {
                        console.error('Error deleting product from cart:', response.data);
                    }
                } catch (error) {
                    console.error('Error while deleting product from cart:', error);
                }
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
  