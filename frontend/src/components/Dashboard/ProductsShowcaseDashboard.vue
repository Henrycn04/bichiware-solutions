<template>
<div class="row row-cols-1 row-cols-md-2 g-4">
  <div class="col" v-for="product in products" v-bind:key="product">
    <div class="card bg-secondary ff-poppins">
      <div class="container overflow-hidden text-center bg-white border-bottom border-dark">
        <img style="height: 150px; width: auto;" class="align-middle card-img-top" :src=product.imageUrl alt="Product Image">
      </div>
      <div class="card-body">
        <h5 class="card-title">{{ product.name }}</h5>
        <p class="card-text">{{ product.company }}</p>
        <div class="d-flex gap-2">
          <div class="align-self-center flex-fill">
            <p class="card-text fs-4 fw-bold ff-lspartan align-self-center">₡ {{ product.price }}</p>
          </div>
          <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#buyProductModal">Comprar</button>
        </div>
      </div>
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
      <div class="alert alert-light text-danger" role="alert" v-text="errorMsg" v-show="errorMsg.length > 0"></div>
      <div class="modal-body">
        <form @submit="onBuyButtonPressed">
          <label class="form-label" for="AmountProduct">Especifique la cantiadad que desea comprar</label>
          <div class="input-group mb-3">
            <span class="input-group-text border-ternary bg-secondary ff-poppins">Cantidad: </span>
            <button class="btn btn-outline-ternary btn-secondary" type="button" id="button-addon2" @click="onIncrementAmount">+</button>
            <input name="AmountProduct" type="text" pattern="^([^0\-][0-9]*)" class="form-control border-ternary" aria-label="Amount" v-model="amountToBuy">
            <button class="btn btn-outline-ternary btn-secondary" type="button" id="button-addon2" @click="onDecrementeAmount">-</button>
          </div>
          <div class="d-flex gap-2">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" @click="onBuyDismiss">Cancelar</button>
            <button type="submit" class="btn btn-primary">Comprar</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</div>
</template>

<script>
export default {
  setup () {
    return {}
  },

  data() {
    return {
      products: [{ name: "Razer Laptop 32GB 2TB RTX 4080 Mobile", company: "Bichiware Solutions", price: 1000000, imageUrl: "https://assets3.razerzone.com/2qIEzE6PovR0jcvcBUI6HVJQVKM=/300x300/https%3A%2F%2Fhybrismediaprod.blob.core.windows.net%2Fsys-master-phoenix-images-container%2Fhaf%2Fh7b%2F9720377704478%2Fblade14-p10-black-500x500.png"}],
      amountToBuy: 0,
      errorMsg: ""
    }
  },

  computed() {

  },

  methods: {
    // backend conection
    getRandomProducts() {

    },

    onBuyButtonPressed(e) {
      if (!isNaN(this.amountToBuy) && this.amountToBuy > 0)
      {
        let hasBought = this.buyProduct();
        if (!hasBought) {
          this.errorMsg = "No hay suficiente stock";
          e.preventDefault();
        }
        alert("Bought")
      }
      else
      {
        this.errorMsg = "Debe ingresar una cantidad valida.";
        e.preventDefault();
      }
      this.amountToBuy = 0;
    },

    // backend conection
    buyProduct() {
      return false;
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
  },
}
</script>

<style scoped>

</style>