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
                        <h3><strong>Modificar Entrega</strong></h3>
                    </div>
                    <form @submit.prevent="checkIfThereIsNewData">
                        <div class="px-2 form-group">
                            <div class="text-danger mb-2">{{ errorMessage }}</div>

                            <label for="batchNumber"><strong>Número de lote*:</strong></label>
                            <input v-model="Delivery.BatchNumber" type="number" id="batchNumber" min="0" class="form-control custom-input" required />

                            <label for="expirationDate"><strong>Fecha de expiración*:</strong></label>
                            <input v-model="Delivery.ExpirationDate" type="date" id="expirationDate" class="form-control custom-input" required />
                        </div>

                        <div class="px-2 form-group">
                            <button type="submit" class="btn btn-primary w-100 mt-3"><strong>Confirmar</strong></button>
                        </div>
                        <br>
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
    computed: {
        ...mapGetters(['getIdProduct']),
    },
    data() {
        return {
            Delivery: {
                ProductID: 0,
                BatchNumber : 0,
                OldBatchNumber: 0,
                ExpirationDate: "",
            },
            originalData: {},
            errorMessage: 'Campos requeridos *',
        };
    },
    mounted() {
        this.getDeliveryData();
    },
    methods: {
    getDeliveryData() {
        console.log(this.getIdProduct[1]);
        console.log(this.getIdProduct[0]);
        axios.get(this.$backendAddress + "api/SearchDelivery/individualDelivery", {
            params: {
                productID:  parseInt(this.getIdProduct[0]),
                batchNumber: parseInt(this.getIdProduct[1])
            }
        })
        .then((response) => {
            this.originalData = response.data;
            console.log(this.originalData );
            this.initNewData();
            console.log("Original delivery data: ", this.originalData);
        })
        .catch((error) => {
            console.error("Error obtaining original delivery data:", error);
        });
        },
        initNewData() {
            this.Delivery.BatchNumber = this.originalData.batchNumber;
            const expirationDate = new Date(this.originalData.expirationDate);
            this.Delivery.ExpirationDate = expirationDate.toISOString().split('T')[0]; 
            this.Delivery.OldBatchNumber = this.originalData.batchNumber;
            this.Delivery.ProductID = this.originalData.productID;
        },
        checkIfThereIsNewData() {
            if (
                this.Delivery.BatchNumber !== this.originalData.BatchNumber ||
                this.Delivery.ExpirationDate !== this.originalData.ExpirationDate
            ) {
                this.submitNewDeliveryData();
            } else {
                alert("No hay cambios en los datos.");
                this.goToCompanyProfile();
            }
        },
        async submitNewDeliveryData() {
            console.log(this.Delivery);
            try {
                await axios.post(this.$backendAddress + "api/UpdateDelivery/update", this.Delivery);
                alert("Datos de entrega modificados exitosamente.");
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
    }
}
</script>

<style>
</style>