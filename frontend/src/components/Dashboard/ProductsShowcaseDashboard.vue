<template>
<div class="row row-cols-1 row-cols-md-2 g-4">
  <div class="col" v-for="product in products" v-bind:key="product">
    <div class="card bg-secondary ff-poppins">
      <div class="container overflow-hidden text-center bg-white border-bottom border-dark">
        <img style="height: 150px; width: auto;" class="align-middle card-img-top" :src=product.image>
      </div>
      <div class="card-body">
        <h5 class="card-title">{{ product.name }}</h5>
        <p class="card-text">{{ product.companyName }}</p>
        <div class="d-flex gap-2">
          <div class="align-self-center flex-fill">
            <p class="card-text fs-4 fw-bold ff-lspartan align-self-center">₡ {{ product.price }}</p>
          </div>
          <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#buyProductModal" @click="assignProductToBuy(product)">Comprar</button>
        </div>
      </div>
    </div>
    <div class="modal fade ff-poppins" id="buyProductModal" tabindex="-1" ref="amountModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5" id="exampleModalLabel">¿Desea comprar este producto?</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="alert alert-light" role="alert" v-text="errorMsg" v-show="errorMsg.length > 0"></div>
          <div class="modal-body">
            <form @submit="onBuyButtonPressed">
              <div v-if="isPerishable" class="mb-2">
                <label class="form-label" for="BatchProduct">Especifique el lote del producto</label>
                <select required class="form-select border-ternary" id="inputGroupSelect02">
                  <option v-for="batch in batches" v-bind:key="batch">
                    Lote {{ batch.batchNumber }}: {{ productToBuy.limit - batch.reservedUnits }} disponibles. Expira {{ batch.expirationDate }}
                  </option>
                </select>
              </div>
              <label class="form-label" for="AmountProduct">Especifique la cantidad que desea comprar</label>
              <div class="input-group mb-3">
                <span class="input-group-text border-ternary bg-secondary ff-poppins">Cantidad: </span>
                <button class="btn btn-outline-ternary btn-secondary" type="button" @click="onIncrementAmount">+</button>
                <input name="AmountProduct" type="text" pattern="^([^0\-][0-9]*)" class="form-control border-ternary" aria-label="Amount" v-model="amountToBuy">
                <button class="btn btn-outline-ternary btn-secondary" type="button" @click="onDecrementeAmount">-</button>
              </div>
              <div class="d-flex gap-2">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" @click="onBuyDismiss">Atrás</button>
                <button type="submit" class="btn btn-primary" id="modalBuyButton">Comprar</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import axios from "axios";
import { mapGetters } from "vuex";

export default {
  setup () {
    return {}
  },

  data() {
    return {
      products: [],
      batches: [],
      amountToBuy: 0,
      productToBuy: null,
      errorMsg: "",
      isPerishable: false,
    }
  },

  mounted() {
    this.getRandomProducts();
  },

  methods: {
    ...mapGetters(['getUserId']),

    async getRandomProducts() {
      axios.get(this.$backendAddress + "api/ClientDashboard/getPorductsForShowcase")
        .then((response) => {
          this.products = response.data
          console.log(this.products[0].productID % 2 == 0)
        }).catch((error) => {
          var Msg = "";
          var errorStatus = 0;
          if (error.response == undefined) {
            Msg = "No hay conexión con el servidor.";
            errorStatus = 408;
          } else if (error.response.data.title) {
            Msg = error.response.data.title;
            errorStatus = error.response.status;
          } else if (error.response.data) {
            Msg = error.response.data;
            errorStatus = error.response.status;
          } else if (error.request) {
            Msg = error.message;
            errorStatus = error.response.status;
          }
          
          console.log("ERROR " + errorStatus + " ----> " + Msg);
          this.errorMsg = "Ocurrió un error. " + Msg
        });
    },

    onBuyButtonPressed(e) {
      e.preventDefault();
      if (!isNaN(this.amountToBuy) && this.amountToBuy > 0)
      {
        if (!this.isPerishable && this.amountToBuy > this.productToBuy.stock)
        {
          this.errorMsg = "No hay suficiente stock";
        }
        this.buyProduct();
      }
      else
      {
        this.errorMsg = "Debe ingresar una cantidad valida.";
      }
      this.amountToBuy = 0;
    },

    async fetchDeliveries(productId) {
      axios.get(this.$backendAddress + "api/products/getProductDeliveries", {
          params: { searchTerm: productId }
        })
        .then((response) => {
          this.batches = response.data
        }).catch((error) => {
          var Msg = "";
          var errorStatus = 0;
          if (error.response == undefined) {
            Msg = "No hay conexión con el servidor.";
            errorStatus = 408;
          } else if (error.response.data.title) {
            Msg = error.response.data.title;
            errorStatus = error.response.status;
          } else if (error.response.data) {
            Msg = error.response.data;
            errorStatus = error.response.status;
          } else if (error.request) {
            Msg = error.message;
            errorStatus = error.response.status;
          }
          
          console.log("ERROR " + errorStatus + " ----> " + Msg);
          this.errorMsg = "Ocurrió un error. " + Msg
        });
    },

    buyProduct() {
      axios.post(this.$backendAddress + `api/ShoppingCart/add`, {
          userID: this.getUserId(),
          productID: this.productToBuy.productID,
          productName: this.productToBuy.name,
          productPrice: this.productToBuy.price,
          quantity: this.amountToBuy || 1,
          imageURL: this.productToBuy.image,
          isPerishable: this.isPerishable
        }).then((response) => {
          if (response.status == 200 || response.status == 204)
          {
            this.errorMsg = "Se agregó el producto al carrito satisfactoriamente, puede cerrar esta ventana.";
          }
        }).catch((error) => {
          var Msg = "";
          var errorStatus = 0;
          if (error.response == undefined) {
            Msg = "No hay conexión con el servidor.";
            errorStatus = 408;
          } else if (error.response.data.title) {
            Msg = error.response.data.title;
            errorStatus = error.response.status;
          } else if (error.response.data) {
            Msg = error.response.data;
            errorStatus = error.response.status;
          } else if (error.request) {
            Msg = error.message;
            errorStatus = error.response.status;
          }
          
          console.log("ERROR " + errorStatus + " ----> " + Msg);
          this.errorMsg = "Ocurrió un error. " + Msg
        });
    },

    onIncrementAmount() {
      this.amountToBuy += 1;
    },

    onDecrementeAmount() {
      this.amountToBuy -= 1;
    },

    onBuyDismiss() {
      this.amountToBuy = 0;
    },

    assignProductToBuy(product) {
      this.productToBuy = product;
      this.isPerishable = product.productID % 2 == 0
      if (this.isPerishable) {
        this.fetchDeliveries(product.productID)
      }
    },
  },
}
</script>

<style scoped>

</style>