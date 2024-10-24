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
                        <h3><strong>Modificar Producto</strong></h3>
                    </div>
                    <form @submit.prevent="checkIfThereIsNewData">
                        <div class="px-2 form-group">
                            <div class="text-danger mb-2">{{ errorMessage }}</div>

                            <label for="name">Nombre*:</label>
                            <input v-model="Product.name" type="text" id="name" class="form-control custom-input" required />

                            <label for="image">URL de la imagen:</label>
                            <input v-model="Product.image" type="url" id="image" class="form-control custom-input" />

                            <label for="price">Precio*:</label>
                            <input v-model="Product.price" type="number" step="0.01" id="price" min="0" class="form-control custom-input" required />

                            <label for="description">Descripción:</label>
                            <textarea v-model="Product.description" id="description" class="form-control custom-input"></textarea>
                            <div>
                                <label for="weight">Peso (Kg)*:</label>
                                <input v-model="Product.weight" type="number" id="weight" min="0" class="form-control custom-input" required />
                            </div>
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
                              
                                
                                <label for="productionLimit">Límite de producción*:</label>
                                <input v-model="PerishableProductData.limit" type="number" id="productionLimit" min="0" class="form-control custom-input" required />
                            </div>

                            <div v-else>
                                <label for="stock">Stock*:</label>
                                <input v-model="nonPerishableProductData.stock" type="number" id="stock" min="0" class="form-control custom-input" required />
                            </div>
                        </div>
                        <div class="px-2 form-group">
                            <button type="submit" class="btn btn-primary w-100 mt-3">Confirmar</button>
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
                productID:0,
                name: String.empty,
                image: String.empty,         
                price: 0.0,
                description: String.empty,
                deliveryDays: [],
                weight: 0.0
            },
            PerishableProductData: {
                productID: 0,
                name: String.empty,
                image: String.empty,
                price: 0.0,
                description: String.empty,
                limit: String.empty,
                deliveryDays: String.empty,
                weight: 0.0
            },
            nonPerishableProductData: { 
                productID: 0,
                name: String.empty,
                image: String.empty,
                price: String.empty,
                description: String.empty,
                stock: 0.0,
                weight: 0.0
            },
            originalData: {}
        };
    },
    methods: {
        getProductData() {
            console.log(this.getIdProduct);
            axios.get(this.$backendAddress + "api/SearchProduct/individualproduct", {
                params: {
                    ID: parseInt(this.getIdProduct)
                }
            })
            .then((response) => {     
                this.originalData = response.data;
                this.isPerishable = this.originalData.stock === null;
                this.initNewData();
            })
            .catch((error) => {
                console.error("Error obtaining product data:", error);
            });
        },
        initNewData() {
            this.Product.productID = this.originalData.productID;
            this.Product.name = this.originalData.name || String.empty;
            this.Product.image = this.originalData.image || String.empty;
            this.Product.price = this.originalData.price || 0.0;
            this.Product.description = this.originalData.description || String.empty;
            this.Product.weight = this.originalData.weight || 0.0;
            this.Product.deliveryDays = this.originalData.deliveryDays.split(',') || [];
            if(this.isPerishable){
                this.PerishableProductData.productID = this.Product.productID;
                this.PerishableProductData.name = this.Product.name || String.empty;
                this.PerishableProductData.image = this.Product.image || String.empty;
                this.PerishableProductData.price = this.Product.price || String.empty;
                this.PerishableProductData.description = this.Product.description || String.empty;
                this.PerishableProductData.limit = this.originalData.limit || String.empty;
                this.PerishableProductData.weight = this.Product.weight || 0.0;
            }else{
                this.nonPerishableProductData.productID = this.Product.productID;
                this.nonPerishableProductData.name = this.Product.name || String.empty;
                this.nonPerishableProductData.image = this.Product.image || String.empty;
                this.nonPerishableProductData.price = this.Product.price || 0.0;
                this.nonPerishableProductData.description = this.Product.description || String.empty;
                this.nonPerishableProductData.stock = this.originalData.stock || String.empty;
                this.nonPerishableProductData.weight = this.Product.weight || 0.0;
             }

        },
        checkIfThereIsNewData() {
      
            this.updateData();
            let hasNewData = false;

                if (this.isPerishable) {
                    console.log(this.Product.deliveryDays.length);
                    if (this.Product.deliveryDays.length === 1 && this.Product.deliveryDays[0]==="") {
                            alert("Debes seleccionar al menos un día de entrega.");
                            return; 
                    }
                    hasNewData = 
                        this.PerishableProductData.name !== this.originalData.name ||
                        this.PerishableProductData.image !== this.originalData.image ||
                        this.PerishableProductData.price !== this.originalData.price ||
                        this.PerishableProductData.description !== this.originalData.description ||
                        this.PerishableProductData.limit !== this.originalData.limit ||
                        this.PerishableProductData.deliveryDays!== this.originalData.deliveryDays ||
                        this.Product.weight !== this.originalData.weight;
                } else {
                    hasNewData = 
                        this.nonPerishableProductData.name !== this.originalData.name ||
                        this.nonPerishableProductData.image !== this.originalData.image ||
                        this.nonPerishableProductData.price !== this.originalData.price ||
                        this.nonPerishableProductData.description !== this.originalData.description ||
                        this.nonPerishableProductData.stock !== this.originalData.stock ||
                        this.Product.weight !== this.originalData.weight;
                }

                if (hasNewData) {
                    console.log(hasNewData);
                    this.submitNewProductData();
                } else {
                    alert("No hay cambios en los datos.");
                    this.goToCompanyProfile();
                }
            },
        updateData(){
            if(this.isPerishable){
                this.PerishableProductData.name = this.Product.name;
                this.PerishableProductData.image = this.Product.image;
                if (!this.Product.image) {
                    this.PerishableProductData.image = "";
                }
                this.PerishableProductData.price = this.Product.price;
                this.PerishableProductData.description = this.Product.description;
                this.PerishableProductData.weight = this.Product.weight;
                this.PerishableProductData.deliveryDays = this.Product.deliveryDays.filter(day => day.trim() !== '').join(',')
                
            }else{
                this.nonPerishableProductData.name = this.Product.name;
                this.nonPerishableProductData.image = this.Product.image;
                if (!this.Product.image) {
                    this.nonPerishableProductData.image = "";
                }
                this.nonPerishableProductData.price = this.Product.price;
                this.nonPerishableProductData.description = this.Product.description;
                this.nonPerishableProductData.stock = this.originalData.stock;
                this.nonPerishableProductData.weight = this.Product.weight;
             }
        },
        
        async submitNewProductData() {
            try {
                if (this.isPerishable) {
                    console.log(this.PerishableProductData);
                    await axios.post(this.$backendAddress + "api/UpdateProduct/update-perishable", this.PerishableProductData);
                } else {
                    await axios.post(this.$backendAddress + "api/UpdateProduct/update-non-perishable", this.nonPerishableProductData);
                }
                alert("Producto actualizado exitosamente.");
                this.goToCompanyProfile();
            } catch (error) {
                if (error.response) {
                    alert("Error: " + error.response.data); 
                } else {
                    alert("Error al actualizar los datos de entrega. Por favor, inténtalo de nuevo más tarde.");
                }
                console.error("Error updating delivery data:", error);
            }
        },
        goToCompanyProfile() {
            this.$router.push('/companyProfile');
        }
    },
}

</script>

<style>
</style>