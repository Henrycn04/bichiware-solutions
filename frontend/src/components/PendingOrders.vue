<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="content">
            <div class="title-buttons_container">
                <h3><strong>Pedidos pendientes de revisión: </strong></h3>
                <div class="buttons_container">
                    <button @click="acceptOrder">
                        <strong>Confirmar</strong>
                    </button>
                    <button @click="rejectOrder">
                        <strong>Rechazar</strong>
                    </button>
                </div>
            </div>
            <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                <thead>
                    <tr>
                        <th>Seleccionar</th>
                        <th>ID Pedido</th>
                        <th>Nombre del cliente</th>
                        <th>Dirección de envío</th>
                        <th>Listado de productos y sus cantidades</th>
                        <th style="text-align: right">Monto total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(order, index) of orderData" :key="index">
                        <td>
                            <input type="checkbox" :checked="selectedOrders.includes(order)" @change="toggleSelection(order)">
                        </td>
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.clientName }}</td>
                        <td>{{ order.orderAddress }}</td>
                        <td>
                            <span v-for="(product, index) in order.orderProducts" :key="index" class="product-item">
                                <span class="product-name">{{ product.productName }}</span>
                                <span class="product-quantity"> {{ product.quantity }}</span>
                            </span>
                        </td>
                        <td style="text-align: right">₡ {{ order.totalAmount.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import axios from "axios"
    export default {
        data() {
            return {
                orderData: [],
                selectedOrders: []
            };
        }, mounted() {
            this.getOrdersData();
        },
        methods: {
            getOrdersData() {
                axios.get(this.$backendAddress + "api/Orders", {
                })
                    .then((response) => {
                        this.orderData = response.data;
                    });
            },
            toggleSelection(order) {
                const index = this.selectedOrders.indexOf(order);
                if (index > -1) {
                    this.selectedOrders.splice(index, 1);
                } else {
                    this.selectedOrders.push(order);
                }
            },
            acceptOrder() {
                console.log("Órdenes seleccionadas:", this.selectedOrders);
                // TODO Make a for that posts the orders one by one            
            },
            rejectOrder() {
                console.log("Órdenes seleccionadas:", this.selectedOrders);
            }
        }
    }
</script>

<style scoped>

    .page-container {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
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

    .header__home-link {
        text-decoration: none;
        color: #332f2b;
    }

    .content {
        flex-grow: 1; 
        padding:20px;
    }

    table {
        background-color: #ffeec2;
    }

    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }

    .title-buttons_container {
        display:flex;
        justify-content: space-between;
        align-items: center;
    }

    .buttons_container {
        display:flex;
        gap:30px;
    }

    button {
        padding: 10px 15px; 
        border: none; 
        border-radius: 5px; 
        background-color: #5a4d02; 
        color: white; 
    }

    button:hover {
        background-color: #8d6103; 
    }

    .product-item {
    display: flex;
    justify-content: space-between; 
    margin-bottom: 5px; 
    }

    .product-name {
        text-align: left; 
    }

    .product-quantity {
        text-align: right; 
        margin-left: auto; 
    }


</style>
