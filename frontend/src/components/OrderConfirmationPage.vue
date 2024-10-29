<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
            <div class="header__search">
                <input type="text"
                       v-model="searchQuery"
                       placeholder="Buscar"
                       @keydown.enter="performSearch" />
                <button @click="performSearch" class="header__search__button">
                    <img src="../assets/SearchIcon.png" alt="Buscar" />
                </button>
            </div>
            <div class="header__actions">
                <a @click="isLoggedIn ? null : goToLogin()" class="header__login">
                {{ isLoggedIn ? '' : 'Accesar' }}
                </a>
                <div v-if="isLoggedIn" class="header__profile-container" ref="profileContainer">
                    <button @click="toggleProfileMenu" class="header__profile">
                        <img src="../assets/ProfileIcon.png" alt="Perfil" />
                    </button>
                    <div v-if="isProfileMenuVisible" class="header__profile-menu">
                        <router-link to="/userProfile" class="header__profile-menu-item" style="color: #463a2e">Cuenta</router-link>
                        <div v-if="isAdminOrEntrepreneur">
                        <router-link to="/companyRegistration" class="header__profile-menu-item" style="color: #463a2e">Registro empresa</router-link>
                        </div>
                        <a v-if="isAdminOrEntrepreneur" @click="toggleCompaniesDropdown" class="header__profile-menu-item" style="color: #463a2e; cursor: pointer">
                            Ver empresas
                        </a>
                        <ul v-if="isAdminOrEntrepreneur && isCompaniesDropdownVisible">
                            <li v-for="company in userCompanies" :key="company.companyID" @click="selectCompany(company.companyID)">
                                {{ company.companyName }}
                            </li>
                        </ul>
                        <a @click=goTologout href="/" class="header__profile-menu-item" style="color: #463a2e">Salir</a>
                    </div>  
                    <button @click="goToCart" class="header__cart">
                        <img src="../assets/CartIcon.png" alt="Carrito" />
                    </button>
                </div> 
            </div>
        </header>
        <main class="main-content">
            <div class="subheader">
                <a href="/all-products" class="element_button">
                    <img src="../assets/AllIcon.png" style="width: 24px; height: 24px; cursor: pointer;" alt="Todos" />
                    <div>&nbsp; Todos los productos</div>
                </a>
                <a href="/non-perishable-products">No perecederos</a>
                <a href="/perishable-products">Perecederos</a>
                <a v-if="this.isAdminOrEntrepreneur" href="/users-list">Lista de usuarios</a>
                <a v-if="this.isAdminOrEntrepreneur" href="/companies-list">Lista de empresas</a>
                <div>
                    <a v-if="this.isAdmin" href="/pendingOrders">Pedidos pendientes</a>
                </div>
            </div>
            <div class="row">
           
                <div class="col-md-8 product-list">
                    <div class="product-list__scroll">
                        <div v-for="(product, index) in products" :key="index" class="product-item">
                            {{ product.productName }} - Cantidad: {{ product.currentCartQuantity }} - Precio Unitario ₡{{ product.productPrice }}
                            - Peso Unitario: {{ product.weight }} - Peso Total: {{ product.currentCartQuantity * product.weight }} Kg
                            - Precio Total: ₡{{ product.productPrice * product.currentCartQuantity }}
                        </div>
                        <div class="product-item">
                            IVA: {{this.IVA= this.calculateIVA()  }}
                        </div>
                        <div class="product-item">
                            Costo de envío:  ₡{{ this.shippingCost }}
                        </div>
                        <div class="product-item">
                            Total:  ₡{{this.totalPrice= this.calculateTotalPrice() }}
                        </div>
                    </div>
                    
                    <div class="text-center mt-4">
                        <button class="confirm-button" @click="confirmSelection">Confirmar</button>
                    </div>
                </div>

                <div class="col-12 col-md-4 buttons d-flex flex-column justify-content-center align-items-stretch">
                    <div v-if="isPaymentOptionsVisible" class="payment-options d-flex">
                        <button class="btn btn-custom mb-2 me-2" @click="selectPaymentMethod('Tarjeta')">Tarjeta</button>
                        <button class="btn btn-custom mb-2" @click="selectPaymentMethod('Sinpe')">Sinpe Móvil</button>
                    </div>
                    <button class="btn btn-primary btn-lg mb-4" @click="paymentOptions">Forma de Pago</button>
                    <button class="btn btn-primary btn-lg mb-4" @click="dateOptions">Fecha de entrega</button>
                    <div v-if="showDates">
                        <div v-if="availableDates.length > 0">
                            <div class="available-dates-scroll">
                                <ul>
                                    <li
                                        v-for="(dateInfo, index) in availableDates"
                                        :key="index"
                                        class="available-date-item"
                                        @click="selectDate(dateInfo.date)" 
                                    >
                                        {{ dateInfo.capitalizedDayName }} - {{ dateInfo.date.toLocaleDateString('es-ES') }}
                                    </li>
                                </ul>
                            </div>
                        <br>
                    </div>
                    </div>
                    <div class="dropdown">
                        <button class="btn btn-primary btn-lg mb-4 dropdown-toggle" type="button" id="addressDropdown" data-bs-toggle="dropdown" aria-expanded="false" style="max-width: 100%; white-space: normal; overflow-wrap: break-word;">
                            {{ selectedAddressText || "Dirección de entrega" }}
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="addressDropdown">
                            <li v-for="address in addressList" :key="address.addressID">
                                <a class="dropdown-item" href="#" @click.prevent="selectAddress(address)" style="max-width: 100%; white-space: normal; overflow-wrap: break-word;">
                                    {{ address.province }}; {{ address.canton }}; {{ address.district }}; {{ address.exact }}
                                </a>
                            </li>
                        </ul>
                    </div>

                </div>
                
            </div>
        </main>
        <footer class="footer">
            <div class="footer_columns">
                <div class="footer__column">
                    <strong>Contacto</strong>
                    <router-link to="/email">Correo</router-link>
                </div>
                <div class="footer__column">
                    <strong>Inscripción</strong>
                    <a href="/login">Inicio de sesión</a>
                    <a href="/register">Registro</a>
                </div>
                <div class="footer__column">
                    <strong>Créditos</strong>
                    <a href="/creators">Creadores</a>
                </div>
            </div>
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
import axios from "axios";
import commonMethods from '@/mixins/commonMethods';
import chooseDeliveryMethods from '@/mixins/chooseDeliveryMethods';
import { mapGetters, mapActions } from 'vuex';
const IVA_VALUE = 0.13;
export default {
    mixins: [commonMethods,chooseDeliveryMethods],
    computed: {
        ...mapGetters(['getSuccesfulPayment', 'getUserId']), 
    },
    data() {
        return {
            products: [],
            isPaid: false,
            isDirectionSelected: false,
            isDateSelected: false,
            isPaymentOptionsVisible: false,
            totalPrice:0,
            orderID: 0,
            shippingCost: 0,
            IVA: 0,
            availableDates:[],
            selectedDate: null,
            addressID: 0, 
            feeID: 1,
            showDates:false,
            perishableProductDataList: [],
            nonPerishableProductDataList: [],
            TotalWeight: 0,
            addressList: [ ],
            selectedAddressText: "",
        };
    },
    mounted() {
        this.getAllCartProducts(); 
        if (this.getUserId > 0)
        {
            this.getAddresses();
        }
            else
        {
            this.$refs.errorMessage.innerHTML = "Debes ingresar a tu perfíl!";
            this.$refs.statusCode.innerHTML = "Fallo de autenticación";
            this.requestError = true;
        }
    },
    methods: {
        ...mapActions(['paymentWasSuccesful']),
        getAddresses() {
            axios.get(this.$backendAddress + "api/AccountAddresses/GetUserAddresses?userId=" + this.getUserId)
                .then((response) => {
                    this.addressList = response.data;
                    }).catch((error) => {
                    if (error.response) {
                        this.$refs.errorMessage.innerHTML = error.response.data;
                        this.$refs.statusCode.innerHTML = "Error " + error.response.status;
                    } else if (error.request) {
                        this.$refs.errorMessage.innerHTML = error.request;
                    } else {
                        console.log('Error al preparar la consulta', error.message);
                    }
                    this.requestError = true;
                    });
                console.log("Gets addresses");
        },
        selectAddress(address) {
            this.addressID = address.addressID;
            this.selectedAddressText = `${address.province}; ${address.canton}; ${address.district}; ${address.exact}`;
            console.log('ID de dirección seleccionada:', this.addressID);
            this.isDirectionSelected = true;
            this.CalculateShippingCost();
        },
        paymentOptions() {
            this.isPaymentOptionsVisible = !this.isPaymentOptionsVisible; 
        }, 
        selectPaymentMethod(type){
            if(type === "Tarjeta"){
                this.$router.push('/card-payment');
            }
            else if(type === "Sinpe"){
                this.$router.push('/sinpe-payment');
            }
        },
        getAllCartProducts() {
                axios.get(`${this.$backendAddress}api/ShoppingCart/getAllCartProducts/${this.userCredentials.userId}`)
                .then((response) => {
                    if (typeof response.data === "string") {
                        console.warn(response.data);
                        this.products = [];
                    } else {
                        console.warn(response.data);
                        this.products = response.data;
                        this.TotalWeight = this.calculateTotalWeight();
                    }                })
                .catch((error) => {
                    console.error("Error obtaining cart products:", error);
                });
            },
        calculateTotalWeight() {
            return this.products.reduce((acc, product) => acc + (product.currentCartQuantity * product.weight), 0);
        },
        calculateTotalPriceWithOutTaxes() {
            let totalPrice = 0;
            for (let i = 0; i < this.products.length; i++) {
                totalPrice += this.products[i].productPrice * this.products[i].currentCartQuantity;
            }
            return totalPrice;
        },
        calculateIVA() {
            return (this.calculateTotalPriceWithOutTaxes() * IVA_VALUE);
        },
        CalculateShippingCost(){
            try {
                    axios.post(this.$backendAddress + "api/addOrder/calculateShippingCost", {
                            addressId:  this.addressID,
                            weight: this.TotalWeight,
                        }).then((response) => {
                            this.shippingCost = response.data;
                            console.log('Shipping cost calculated success', this.shippingCost);
                    }).catch((error) => {
                    if (error.response) {
                        this.$refs.errorMessage.innerHTML = error.response.data;
                        this.$refs.statusCode.innerHTML = "Error " + error.response.status;
                    } else if (error.request) {
                        this.$refs.errorMessage.innerHTML = error.request;
                    } else {
                        console.log('Error al preparar la consulta', error.message);
                    }
                    this.requestError = true;
                    });

                } catch (error) {
                    console.error("Error calculating shipping cost emails:", error);
                }
            
        },
        calculateTotalPrice() {
            return this.calculateTotalPriceWithOutTaxes() + this.calculateIVA() + this.shippingCost;
        },
        async sendRealizationEmail()
        {
            const xd = {
                orderId:        this.orderID,
                addressId:      this.addressID,
                userId:         parseInt(this.userCredentials.userId),
                tax:            this.IVA
                };
                console.log("Esto es lo que mando: ", xd);
            await axios.post(this.$backendAddress + "api/addOrder/sendRealizationEmails", {
                ...xd
            }).catch((error) => {
                console.error("Error at order realization email" + error);
            });
        },
        async dateOptions() {
            this.showDates=!this.showDates;
            this.productIDs = this.extractPerishableProductIds();
            if(this.productIDs.length === 0){
                this.showAllAvailableDates();
            }else{
                await this.getProductsDeliveryDays();
                await this.getDeliveriesAvailiable();
                let firstDay = this.restrictPosibleDatesToDeliver();
            if(firstDay){
            this.restrictDatesByStock(this.products);
            if(this.possibleDates.length === 0){
                alert(`No hay fechas para las entregas que coincidan con suficientes productos. Intente comprar los productos perecederos por aparte`);
            }else {
                this.showAvailableDates(firstDay);
            } 
        }
        }        
        },
        showAllAvailableDates(){
            const today = new Date(); 
            const oneYearLater = new Date(); 
            oneYearLater.setFullYear(today.getFullYear() + 1); 
            const dates = []; 
            let currentDate = new Date(today);
            while (currentDate <= oneYearLater) {
                const dayName = currentDate.toLocaleDateString('es-ES', { weekday: 'long' });
                const capitalizedDayName = dayName.charAt(0).toUpperCase() + dayName.slice(1)
                dates.push({ date: new Date(currentDate), capitalizedDayName }); 
                currentDate.setDate(currentDate.getDate() + 1);
            }
            this.availableDates = dates;
        },
        showAvailableDates(firstDay) {
            let notFirstDay = false;
            const today = new Date();
            let newfirstDay = new Date(firstDay);
            if (!this.possibleDates.some(date => date.date.getTime() === firstDay.getTime())) {
                for(let i = 0; i < this.possibleDates.length; i ++ ){
                    notFirstDay = false;
                    newfirstDay = this.possibleDates[i].date;
                    if(newfirstDay < firstDay){
                        newfirstDay  = this.possibleDates[i].date;
                        break;
                    }
                    notFirstDay = true;
                }
            }
           
            this.availableDates = [];
            while (today <= newfirstDay) {
                const dayName = today.toLocaleDateString('es-ES', { weekday: 'long' });
                const capitalizedDayName = dayName.charAt(0).toUpperCase() + dayName.slice(1);
                if (this.availableDays.includes(capitalizedDayName)) {
                    this.availableDates.push({ date: new Date(today), capitalizedDayName });
                }
                today.setDate(today.getDate() + 1);
            }
            if(this.availableDates.length === 0 || notFirstDay){
                alert(`No hay fechas para las entregas que no caduquen antes del primer posible dia de entrega. Intente comprar los productos perecederos por aparte`);
            }
        },
        selectDate(date) {
            this.selectedDate = date;
            console.log("Selected Date:", this.selectedDate); 
            this.isDateSelected = true;

        },
        async confirmSelection() {
            this.isPaid = this.getSuccesfulPayment;
            const falseConditions = [];
            if (!this.isPaid) falseConditions.push('Forma de Pago');
            if (!this.isDirectionSelected) falseConditions.push('Dirección de entrega');
            if (!this.isDateSelected) falseConditions.push('Fecha de entrega');

            if (falseConditions.length > 0) {
                alert(`No ha completado la siguiente informacion: ${falseConditions.join(', ')}`);
            } else {
                const finishConfirmation = this.createOrder();
                if(finishConfirmation){
                    alert(`Compra realizada con éxito`);
                }
                  
            }
        },

        deleteCartProducts(){ 
            try {
                    axios.post(this.$backendAddress + "api/ShoppingCart/deleteall",
                        {
                            userID:  parseInt(this.userCredentials.userId),
                            productID: "0",
                            isPerishable: false
                        }).then(() => {
                            console.log('Delete success');               
                        })
                        .catch((error) => {
                            console.error("Error obtaining cart products:", error);
                        });
                    
                } catch (error) {
                    console.error("Error deleting products form cart:", error);
                }

        },
        async createOrder(){
                await this.addOrder();
                console.log("LA ORDEN:", this.orderID);
                let perishableProducts = [];
                let nonPerishableProducts = [];
                let productIndex = 0;
                for (let i = 0; i < this.products.length; i ++){
                    if(this.products[i].isPerishable){
                        perishableProducts[productIndex ]=this.products[i];
                        productIndex++;
                    }
                }
                productIndex = 0;
                for (let i = 0; i < this.products.length; i ++){
                    if(!this.products[i].isPerishable){
                        nonPerishableProducts[productIndex]=this.products[i];
                        productIndex++;
                    }
                }
                if(nonPerishableProducts.length > 0){
                    await this.addNonPerishableProductToOrder(nonPerishableProducts);
                }
                if(perishableProducts.length > 0){
                    await this.addPerishableProductToOrder(perishableProducts);
                }
                this.deleteCartProducts();
                this.sendRealizationEmail();
                return true;
        },
        async addOrder(){
            try {
                const orderData = {
                    UserID: parseInt(this.userCredentials.userId),
                    AddressID: this.addressID, 
                    FeeID: this.feeID, 
                    Tax: this.IVA,
                    ShippingCost: this.shippingCost,
                    ProductCost: this.totalPrice,
                    DeliveryDate: this.selectedDate 
                };
                const response = await axios.post(this.$backendAddress + "api/addOrder/add", orderData);
                this.orderID = response.data;
                    console.log('Order added: ', response.data);
                } catch (error) {
                    console.error("Error adding order:", error);
                }

        },
        async addNonPerishableProductToOrder(nonperishableProducts) {
            try {
                console.log(this.perishableProductDataList);
                this.createListOfNonPerishableProducts(nonperishableProducts)
                if (this.nonPerishableProductDataList.length > 0) {
                    await axios.post(this.$backendAddress + "api/addOrder/addnonperishable", this.nonPerishableProductDataList);
                    console.log('Non perishable products to order success');
                } else {
                    console.log('Not non perishable products founded to add to order.');
                }
            } catch (error) {
                console.error("Error adding non perishable products to order:", error);
            }
        },
        createListOfNonPerishableProducts(nonPerishableProducts){
            for (let i = 0; i < nonPerishableProducts.length; i++) {
                const product = nonPerishableProducts[i];
                const perishableProductData = {
                    ProductID: product.productID,
                    OrderID: this.orderID,
                    ProductName: product.productName,
                    Quantity: product.currentCartQuantity,
                    ProductPrice: product.productPrice
                };
            this.nonPerishableProductDataList.push(perishableProductData);
            }

        },
        async addPerishableProductToOrder(perishableProducts) {
            try {
                this.createListOfPerishableProducts(perishableProducts)
                if (this.perishableProductDataList.length > 0) {
                    await axios.post(this.$backendAddress + "api/addOrder/addperishable", this.perishableProductDataList);
                    console.log('Perishable products to order success');
                } else {
                    console.log('Not perishable products founded to add to order.');
                }
            } catch (error) {
                console.error("Error adding perishable products to order:", error);
            }
        },
        createListOfPerishableProducts(perishableProducts){
            for (let i = 0; i < perishableProducts.length; i++) {
            const product = perishableProducts[i];
            let batchNumber = null;
            for (let j = 0; j < this.possibleDates.length; j++) {
                const possibleDate = this.possibleDates[j];
                if (this.selectedDate <= possibleDate.date && product.productID === possibleDate.productID) {
                    batchNumber = possibleDate.batchNumber; 
                    break; 
                }
            }
            if (batchNumber === null) {
                console.error(`Batch number not founded ${product.productName}`);
                continue; 
            }

            const perishableProductData = {
                ProductID: product.productID,
                OrderID: this.orderID,
                BatchNumber: batchNumber, 
                ProductName: product.productName,
                Quantity: product.currentCartQuantity,
                ProductPrice: product.productPrice
            };
            this.perishableProductDataList.push(perishableProductData);
            }
            console.log(this.perishableProductDataList);
        },
        
    }
}
</script>

<style>
.dropdown-item:hover {
    background-color: rgba(0, 0, 0, 0.1);
    color: #000;
}


.product-list {
    border: 1px solid #ccc;
    padding: 10px;
    border-radius: 8px; 
    background-color: #f9f9f9; 
}

.product-list__scroll {
    max-height: 300px;
    min-height: 300px;
    overflow-y: auto;
}

.product-item {
    padding: 15px; 
    border-bottom: 1px solid #ddd;
    transition: background-color 0.3s; 
}

.product-item:hover {
    background-color: #eaeaea;
}

.confirm-button {
    margin-top: 20px; 
    padding: 10px 20px;
    background-color: #d57c23; 
    color: white;
    border: none;
    cursor: pointer;
    font-weight: bold;
    border-radius: 5px; 
}

.buttons .btn {
    width: 100%; 
    padding: 15px; 
    font-size: 1.2rem; 
    margin-bottom: 20px; 
    background-color: #705438; 
    color: white;
    border-radius: 5px; 
}

.available-dates-scroll {
    max-height: 100px; 
    overflow-y: auto; 
    border: 1px solid #ccc; 
    border-radius: 8px; 
    background-color: #f9f9f9; 
}

.available-date-item {
    padding: 10px; 
    border-bottom: 1px solid #ddd; 
    transition: background-color 0.3s; 
}

.available-date-item:hover {
    background-color: #eaeaea; 
}

@media (min-width: 768px) {
    .buttons {
        padding: 0 50px; 
    }
}
</style>