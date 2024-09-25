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
                        <select v-model="Product.category" type="text" id="category" class="form-control custom-input" required >
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

                        <div class="form-check my-3">
                            <input v-model="isPerishable" type="checkbox" class="form-check-input" id="isPerishable">
                            <label class="form-check-label" for="isPerishable">¿Es perecedero?</label>
                        </div>

                        <div v-if="isPerishable">
                            <label for="deliveryDays">Días de entrega*:</label>
                                <select v-model="PerishableProductData.deliveryDays" id="deliveryDays" class="form-control custom-input" required>
                                    <option disabled value="">Seleccione un día</option>
                                    <option value="Lunes">Lunes</option>
                                    <option value="Martes">Martes</option>
                                    <option value="Miércoles">Miércoles</option>
                                    <option value="Jueves">Jueves</option>
                                    <option value="Viernes">Viernes</option>
                                    <option value="Sábado">Sábado</option>
                                    <option value="Domingo">Domingo</option>
                                </select>

                            <label for="productionLimit">Límite de producción*:</label>
                            <input v-model="PerishableProductData.limit" type="number" id="productionLimit" min="0" class="form-control custom-input" required />

                            <label for="batchNumber">Número de lote*:</label>
                            <input v-model="Delivery.batchNumber" type="number" id="batchNumber" min="0" class="form-control custom-input" required />

                            <label for="expirationDate">Fecha de expiración*:</label>
                            <input v-model="Delivery.expirationDate" type="date" id="expirationDate" class="form-control custom-input" required />
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
import { mapGetters, mapActions } from 'vuex';
export default {
    computed: {
        ...mapGetters(['getIdCompany']),
    },
    data() {
        return {
            reservedUnitsDefault: 0,
            Product: { 
                name: "", 
                image: "", 
                category: "", 
                price: "",
                description: "", 
                companyID: "", 
                companyName: "",
            },
            nonPerishableProductData: { 
                companyID: "",
                companyName: "",
                name: "", 
                image: "", 
                category: "", 
                price: "",
                description: "", 
                stock: ""
            },
            PerishableProductData: { 
                companyID: "",
                companyName: "",
                name: "", 
                image: "", 
                category: "", 
                price: "",
                description: "", 
                deliveryDays: "", 
                limit: ""
            },
            Delivery: {
                productID: "",
                batchNumber: "",
                expirationDate: "",
                reservedUnits: this.reservedUnitsDefault,
            },
            errorMessage: 'Campos requeridos *',
            isPerishable: false,
        };
    },
    methods: {
        ...mapActions(['openCompany']), // THIS IS TO TEST THE FUNCTION
        async submitForm() {
            
            await this.openCompany(1); // THIS IS TO TEST THE FUNCTION
            this.Product.companyID = this.getIdCompany;
            console.log(this.Product.companyID);
            this.Product.companyName = (await axios.get("https://localhost:7263/api/CompanyData/getProductOwner", 
            this.Product.companyID)).data;
            if(this.isPerishable){
               
                this.PerishableProductData.companyID = this.Product.companyID;
                this.PerishableProductData.companyName = this.Product.companyName;
                this.PerishableProductData.name = this.Product.name; 
                this.PerishableProductData.image = this.Product.image; 
                this.PerishableProductData.category = this.Product.category; 
                this.PerishableProductData.price = this.Product.price;
                this.PerishableProductData.description = this.Product.description; 
                this.addPerishableData();
            }
            else{ 
                this.nonPerishableProductData.companyID = this.Product.companyID;
                this.nonPerishableProductData.companyName = this.Product.companyName;
                this.nonPerishableProductData.name = this.Product.name; 
                this.nonPerishableProductData.image = this.Product.image; 
                this.nonPerishableProductData.category = this.Product.category; 
                this.nonPerishableProductData.price = this.Product.price;
                this.nonPerishableProductData.description = this.Product.description; 
                this.addNonPerishableData();
            }
            this.goToCompanyProfile();
        },
        async addPerishableData() {
            try{
                const response = await axios.post("https://localhost:7263/api/product/addperishableproduct", this.PerishableProductData);
                console.log(response.data);
                this.addDeliveryData(response.data);
            } catch (error) {
                console.error("Error adding perishable data:", error);
            }
        },
        async addNonPerishableData() {
            try{
                const response = await axios.post("https://localhost:7263/api/product/addnonperishableproduct", this.nonPerishableProductData);
                console.log(response.data);
            } catch (error) {
                console.error("Error adding non-perishable data:", error);
            }

        },
        async addDeliveryData(idProduct) {
            this.Delivery.productID = idProduct;
            try{
                const response = await axios.post("https://localhost:7263/api/product/adddelivery", this.Delivery);
                console.log(response.data);
            } catch (error) {
                console.error("Error adding delivery data:", error);
            }

        },
        goToCompanyProfile() {
            this.$router.push('/companyProfile');
        }
    }
}
</script>

<style scoped>
      .bg-light-custom {
        background-color: #ffeec2;
    }
    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 10px 20px;
        background-color: #f07800;
        color: white;
        font-family: 'League Spartan', sans-serif;
        max-width: 100vw;
        box-sizing: border-box;
    }

    .forms_header {
        font-family: 'League Spartan', sans-serif;
        margin: 0;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: #332f2b;
        text-align: center;
    }
    .header__home-link {
        text-decoration: none;
        color: #332f2b;
    }
    .custom-input {
        background-color: #ffeec2;
        border-radius: 8px;
        padding: 10px; 
    }


</style>