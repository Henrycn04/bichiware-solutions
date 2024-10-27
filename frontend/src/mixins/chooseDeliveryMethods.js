import axios from "axios";
const MILLISECONDS_IN_A_SECOND = 1000;
const SECONDS_IN_A_MINUTE = 60;
const MINUTES_IN_AN_HOUR = 60;
const HOURS_IN_A_DAY = 24;
const DAYS_IN_A_WEEK = 7;

const MILLISECONDS_IN_A_DAY = MILLISECONDS_IN_A_SECOND * SECONDS_IN_A_MINUTE * MINUTES_IN_AN_HOUR * HOURS_IN_A_DAY;
export default {
    data() {
        return {
        productIDs:[],
        productsForDates:[],
        deliveriesForDates:[],
        possibleDates: [],
        availableDays: []
        }
    },
    methods: {
        extractPerishableProductIds(){
            let productIds=[];
            for (let i = 0; i < this.products.length; i++) {
                if(this.products[i].isPerishable)
                    productIds.push(parseInt(this.products[i].productID));
            }
            return productIds;

        },
       async getProductsDeliveryDays(){ 
           await axios.post(`${this.$backendAddress}api/SearchProduct/specificproducts`, { productIDs: this.productIDs })
                .then((response) => {
                       this.productsForDates = response.data; 
                                })
                .catch((error) => {
                    console.error("Error obtaining delivery days:", error);
                });
        },
        async getDeliveriesAvailiable(){
           await axios.post(`${this.$backendAddress}api/SearchDelivery/specificDeliveries/`, { productIDs: this.productIDs })
                .then((response) => {
                        this.deliveriesForDates= response.data;
                                })
                .catch((error) => {
                    console.error("Error obtaining deliveries:", error);
                });
        },
        restrictPosibleDatesToDeliver() {
            const availableDays = this.restrictPosibleDaysToDeliver();
            if(availableDays.length === 0){
                alert(`No hay días para las entregas que coincidan. Intente comprar los productos perecederos por aparte`);
                return false;
            }
            this.availableDays = availableDays;
            const LastPossibleDate = this.restrictLastPossibleDate(availableDays)
            if(LastPossibleDate === ""){
                alert(`No hay fechas para las entregas que coincidan. Intente comprar los productos perecederos por aparte`);
                return false;
            }
            return LastPossibleDate;

        },
        restrictPosibleDaysToDeliver() {
            const possibleDays = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
            const availableDays = [];
            
            for (let i = 0; i < possibleDays.length; i++) {
                const day = possibleDays[i];
                let allProductsHaveDay = true; 
                for (let j = 0; j < this.productsForDates.length; j++) {
                    const product = this.productsForDates[j];
                   if (product.deliveryDays) {
                        const deliveryDaysArray = product.deliveryDays.split(',').map(d => d.trim());
                        
                        
                        if (!deliveryDaysArray.includes(day)) {
                            allProductsHaveDay = false; 
                            break; 
                        }
                    } else {
                        allProductsHaveDay = false; 
                        break;
                    }
                }
                if (allProductsHaveDay) {
                    availableDays.push(day);
                }
            }  
            return availableDays;
        },
        restrictLastPossibleDate(availableDays) {
            const maxDates = this.getMaxDeliveryDates();
            return this.getClosestValidDate(maxDates, availableDays);
        },
        
        getMaxDeliveryDates() {
            const maxDatesByProductID = {};
            
            for (let i = 0; i < this.deliveriesForDates.length; i++) {
                const { productID, expirationDate } = this.deliveriesForDates[i];
                const date = new Date(expirationDate);
                
                if (!maxDatesByProductID[productID] || date > maxDatesByProductID[productID]) {
                    maxDatesByProductID[productID] = date;
                }
            }
            
            return Object.values(maxDatesByProductID).sort((a, b) => a - b);
        },
        
        getClosestValidDate(maxDates, availableDays) {
            const today = new Date();
            let closestDate = null;
            for (let i = 0; i < maxDates.length; i++) {
                const date = maxDates[i];
                if (date >= today) {
                    if (!closestDate || (date - today < closestDate - today)) {
                        closestDate = date;
                    }
                }
            }
            const daysOfWeek = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
            const isWithin7Days = (date) => (date - today) / MILLISECONDS_IN_A_DAY <= DAYS_IN_A_WEEK;
            const isDeliveryDay = (date) => availableDays.includes(daysOfWeek[date.getDay()]);

            if (!closestDate || !isWithin7Days(closestDate) || !isDeliveryDay(closestDate)) {
                for (let i = 0; i < maxDates.length; i++) {
                    const date = maxDates[i];
                    if (isDeliveryDay(date) && date >= today) {
                        closestDate = date;
                        break; 
                    }
                }
            }
            return closestDate || "";
        },
        restrictDatesByStock(cart) {
            let allProductsAdded = true;
            this.possibleDates = [];
            for (let i = 0; i < cart.length; i++) {
                const cartItem = cart[i];
                const productID = cartItem.productID;
                const quantityToPurchase = cartItem.currentCartQuantity;
                let productAdded = false;
                if(cartItem.isPerishable){
                    for (let j = 0; j < this.deliveriesForDates.length; j++) {
                        const delivery = this.deliveriesForDates[j];
                        if (delivery.productID === productID && 
                            (this.productsForDates[i].limit - delivery.reservedUnits) >= quantityToPurchase) {
                            const deliveryDate = new Date(delivery.expirationDate);
                            this.possibleDates.push({
                                date: deliveryDate,
                                batchNumber: delivery.batchNumber,
                                productID: delivery.productID
                            });
                            productAdded=true;
                        }
                        if(j+1 === this.deliveriesForDates.length && !productAdded ){
                            allProductsAdded = false;
                        }
                    }
                }
            }
            if( !allProductsAdded){
                this.possibleDates = [];
            } 
            else{
                this.sortProducts();
            }
        },
        sortProducts(){
            this.possibleDates.sort((first, second) => {
                const firstDate = new Date(first.date);
                const secondDate = new Date(second.date);
                return secondDate - firstDate;
            });
        }
    }
}