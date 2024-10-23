<template>
    <div>
        <div class="d-flex flex-column min-vh-100 bg-light-custom">
            <header class="header py-2">
                <div class="header__brand text-center">
                    <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
                </div>
            </header>
            <div class="d-flex justify-content-center align-items-center flex-grow-1">
                <div class="card shadow" style="max-width: 400px; width: 100%;">
                    <div class="forms_header">
                        <h3><strong>Agregar Producto</strong></h3>
                    </div>
                    <form @submit.prevent="submitForm">
                        <div class="px-2 form-group">
                            <div class="text-danger mb-2">{{ errorMessage }}</div>

                            <label for="name">Nombre*:</label>
                            <input v-model="Product.name" type="text" id="name" class="form-control custom-input" required />

                            <label for="image">URL de la imagen:</label>
                            <input v-model="Product.image" type="url" id="image" class="form-control custom-input" />

                            <label for="category">Categoría*:</label>
                            <select v-model="Product.category" id="category" class="form-control custom-input" required>
                                <option value="" disabled selected>Selecciona una categoría</option>
                                <option value="Alimentos">Alimentos</option>
                                <option value="Electronicos">Electrónicos</option>
                                <option value="DecoracionCasa">Decoración Casa</option>
                                <option value="Automoviles">Automóviles</option>
                                <option value="DecoracionExteriores">Decoración Exteriores</option>
                                <option value="Ropa">Ropa</option>
                                <option value="Joyeria">Joyería</option>
                                <option value="Limpieza">Limpieza</option>
                            </select>

                            <label for="price">Precio*:</label>
                            <input v-model="Product.price" type="number" step="0.01" id="price" min="0" class="form-control custom-input" required />

                            <label for="description">Descripción:</label>
                            <textarea v-model="Product.description" id="description" class="form-control custom-input"></textarea>
                            
                            <div v-if="isPerishable">
                                <label>Días de entrega*:</label>
                                <div v-for="day in days" :key="day">
                                    <input 
                                        type="checkbox" 
                                        :value="day" 
                                        v-model="Product.deliveryDays" 
                                        :id="day">
                                    <label :for="day">{{ day }}</label>
                                </div>
                                <p v-if="!isValid && !Product.deliveryDays.length" class="text-danger mb-2">
                                    Debes seleccionar al menos un día.
                                </p>
                                
                                <label for="productionLimit">Límite de producción*:</label>
                                <input v-model="PerishableProductData.limit" type="number" id="productionLimit" min="0" class="form-control custom-input" required />
                            </div>

                            <div v-else>
                                <label for="stock">Stock*:</label>
                                <input v-model="nonPerishableProductData.stock" type="number" id="stock" min="0" class="form-control custom-input" required />
                            </div>
                        </div>
                        <div class="px-2 form-group">
                            <button type="submit" class="btn btn-primary w-100 mt-3">Agregar Producto</button>
                        </div>
                    </form>
                </div>
            </div>
            <footer class="footer py-3">
                <p class="text-center" style="font-family: 'Poppins', sans-serif; font-size: medium;">&copy; Copyright by BichiWare Solutions 2024</p>
            </footer>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { mapGetters } from 'vuex';
import '../assets/styles/productForms.css'; 

export default {
    mounted() {
        this.getProductData();
    },
    computed: {
        ...mapGetters(['getIdProduct']),
    },
    data() {
        return {
            days: ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'], 
            errorMessage: 'Campos requeridos *',
            isPerishable: false,
            Product: {
                name: String.empty,
                image: String.empty,
                category: String.empty,
                price: String.empty,
                description: String.empty,
            },
            PerishableProductData: {
                limit: String.empty,
                deliveryDays: String.empty
            },
            nonPerishableProductData: { 
                stock: String.empty
            },
            originalData: {}
        };
    },
    methods: {
        getProductData() {
            axios.get(this.$backendAddress + "api/ProductData", {
                params: {
                    productID: this.getIdProduct
                }
            })
            .then((response) => {
                this.originalData = response.data;
                this.isPerishable = this.originalData.stock === String.empty;
                this.fillProductData();
            })
            .catch((error) => {
                console.error("Error obtaining product data:", error);
            });
        },
        fillProductData() {
            this.Product.name = this.originalData.name || String.empty;
            this.Product.image = this.originalData.image || String.empty;
            this.Product.category = this.originalData.category || String.empty;
            this.Product.price = this.originalData.price || String.empty;
            this.Product.description = this.originalData.description || String.empty;
            this.nonPerishableProductData.stock = this.originalData.stock || String.empty;
            this.PerishableProductData.limit = this.originalData.limit || String.empty;
            this.PerishableProductData.deliveryDays = this.originalData.deliveryDays || String.empty;
        },
        async submitForm() {
            try {
                if (this.isPerishable) {
                    await axios.post(this.$backendAddress + "api/Products/Perishable", {
                        ...this.Product,
                        limit: this.PerishableProductData.limit
                    });
                } else {
                    await axios.post(this.$backendAddress + "api/Products/NonPerishable", {
                        ...this.Product,
                        stock: this.nonPerishableProductData.stock
                    });
                }
                alert("Producto agregado exitosamente.");
                this.$router.push('/products');
            } catch (error) {
                console.error("Error adding product:", error);
                this.errorMessage = "Error al agregar el producto. Intenta de nuevo.";
            }
        }
    }
}
</script>

<style>
</style>