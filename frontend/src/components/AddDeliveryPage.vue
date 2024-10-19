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
                    <h3><strong>Agregar Entrega</strong></h3>
                </div>
                <form @submit.prevent="submitForm">
                    <div class="px-2 form-group">
                        <div class="text-danger mb-2">{{ errorMessage }}</div>

                        <label for="batchNumber"><strong>Número de lote*:</strong></label>
                        <input v-model="Delivery.batchNumber" type="number" id="batchNumber" min="0" class="form-control custom-input" required />

                        <label for="expirationDate"><strong>Fecha de expiración*:</strong></label>
                        <input v-model="Delivery.expirationDate" type="date" id="expirationDate" class="form-control custom-input" required />
                    </div>

                    <div class="px-2 form-group">
                        <button type="submit" class="btn btn-primary w-100 mt-3"><strong>Agregar Entrega</strong></button>
                        
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
import { mapGetters} from 'vuex';
export default {
    computed: {
        ...mapGetters(['getIdProduct']),
    },
    data() {
        return {
            Delivery: {
                productID: "",
                batchNumber: "",
                expirationDate: "",
                reservedUnits: 0,
            },
            errorMessage: 'Campos requeridos *',
        };
    },
    methods: {
        async submitForm() {
               
                // get the id of the product
                this.Delivery.productID = this.getIdProduct;
                console.log(this.Delivery.productID);
                // validate if there are other product with the same id and batch number
                const response = await axios.post("https://localhost:7263/api/adddelivery/searchdelivery", this.Delivery);

                if(!response.data.success){  
                    try{
                        const response = await axios.post("https://localhost:7263/api/adddelivery/adddelivery", this.Delivery);
                        console.log(response.data);
                    } catch (error) {
                        console.error("Error adding delivery data:", error);
                    }
                    this.goToCompanyProfile();
                } else {
                    this.errorMessage = "Ya existe un producto con la misma id y número de lote";
                    console.error("Error: there is already a delivery with that batch number and product ID");
                }
        },
        goToCompanyProfile() {
            // redirect to the company profile to add more products or deliverys
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