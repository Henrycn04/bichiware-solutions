<template>
    <div class="product-list">
      <div v-if="products.length === 0" class="no-results">
        <p>No se encontraron resultados.</p>
      </div>
      <div class="slider-container">
        <div v-for="product in products" :key="product.id" class="product-card">
          <div class="product-info">
            <h3 class="product-name">{{ product.name }}</h3>
            <p class="product-category">Categoría: {{ product.category }}</p>
            <p class="product-company">Compañía: {{ product.companyName }}</p>
            <div class="product-footer">
              <p class="product-price">Precio: ${{ product.price }}</p>
              <div class="product-controls">
                <input type="number" v-model="product.quantity" min="1" class="quantity-input" />
                <span class="product-stock">Stock: {{ product.stock }}</span>
                <button @click="addToCart(product)" class="add-to-cart-button">
                  🛒
                </button>
              </div>
              <div v-if="product.isPerishable" class="perishable-info">
                <span class="perishable-symbol">🌱</span>
                <button @click="showDeliveryOptions(product)">Seleccionar entrega</button>
              </div>
            </div>
          </div>
          <img :src="product.image" alt="Imagen del producto" class="product-image" />
        </div>
      </div>
  
      <!-- Modal para seleccionar entrega -->
      <div v-if="showDeliveryModal" class="modal">
        <div class="modal-content">
          <span @click="closeDeliveryModal" class="close">&times;</span>
          <h2>Opciones de entrega</h2>
          <ul>
            <li v-for="delivery in deliveries" :key="delivery.id">
              {{ delivery.date }} - {{ delivery.time }}
              <button @click="selectDelivery(delivery)">Seleccionar</button>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  export default {
    props: {
      products: {
        type: Array,
        required: true,
      },
    },
    data() {
      return {
        showDeliveryModal: false,
        deliveries: [],
        selectedProduct: null,
      };
    },
    methods: {
      addToCart(product) {
        console.log("Agregado al carrito:", product);
      },
      showDeliveryOptions(product) {
        this.selectedProduct = product;
        this.fetchDeliveries(product.id);
        this.showDeliveryModal = true;
      },
      closeDeliveryModal() {
        this.showDeliveryModal = false;
      },
      async fetchDeliveries(productId) {
        try {
          const response = await axios.get(`https://localhost:7263/api/Deliveries/${productId}`);
          this.deliveries = response.data;
        } catch (error) {
          console.error("Error fetching deliveries:", error);
        }
      },
      selectDelivery(delivery) {
        console.log("Entrega seleccionada:", delivery);
        this.closeDeliveryModal();
      },
    },
  };
  </script>
  
  <style scoped>
  .search-results {
    border-radius: 10px;
    padding: 20px;
  }
  
  .product-list {
    display: flex;
    overflow-x: auto; /* Para habilitar el desplazamiento horizontal */
  }
  
  .product-card {
    display: flex;
    background-color: #fff; /* Fondo blanco para las tarjetas */
    border: 1px solid #ddd;
    border-radius: 10px; /* Esquinas redondeadas */
    padding: 15px;
    margin-right: 20px; /* Espaciado entre tarjetas */
    min-width: 300px; /* Ancho mínimo para cada tarjeta */
    align-items: center; /* Centrar verticalmente */
  }
  
  .product-image {
    width: 100px; /* Tamaño de la imagen */
    height: auto;
    border-radius: 5px; /* Esquinas redondeadas para la imagen */
  }
  
  .product-info {
    margin-left: 20px; /* Espacio entre la imagen y la información del producto */
    display: flex;
    flex-direction: column;
    flex-grow: 1; /* Permitir que la información ocupe el espacio disponible */
  }
  
  .product-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  
  .quantity-input {
    width: 60px; /* Ancho del campo de cantidad */
    margin-right: 10px; /* Espaciado entre el campo y el stock */
  }
  
  .add-to-cart-button {
    background-color: #4caf50; /* Color del botón */
    color: white; /* Color del texto del botón */
    border: none;
    border-radius: 5px; /* Esquinas redondeadas */
    padding: 5px 10px; /* Relleno del botón */
    cursor: pointer; /* Cambiar el cursor al pasar por encima */
  }
  
  .add-to-cart-button:hover {
    background-color: #45a049; /* Color de fondo al pasar el mouse */
  }
  
  .perishable-info {
    display: flex;
    align-items: center;
  }
  
  .perishable-symbol {
    margin-right: 5px; /* Espacio entre el ícono y el botón */
  }
  
  .product-category,
  .product-company,
  .product-price,
  .product-stock {
    margin: 5px 0; /* Espaciado entre líneas de texto */
  }
  </style>