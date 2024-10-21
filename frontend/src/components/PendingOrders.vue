<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="content">
            <div>
                <h3><strong>Pedidos pendientes de revisión: </strong></h3>
            </div>
            <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                <thead>
                    <tr>
                        <th>Seleccionar</th>
                        <th>ID Pedido</th>
                        <th>Nombre del cliente</th>
                        <th>Dirección de envío</th>
                        <th>Listado de productos y sus cantidades</th>
                        <th>Monto total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(order, index) of orderData" :key="index">
                        <td>
                            <input type="checkbox" v-model="selectedAddresses" :value="address">
                        </td>
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.clientName }}</td>
                        <td>{{ order.orderAddress }}</td>
                        <td>
                            <span v-for="(product, index) in order.orderProducts" :key="index">
                                {{ product.productName }} ({{ product.quantity }})<span v-if="index < order.orderProducts.length - 1">, </span>
                            </span>
                        </td>
                        <td>{{ order.totalAmount }}</td>
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
                orderData: []
            };
        }, mounted() {
            this.getOrdersData();
        },
        methods: {
            getOrdersData() {
                axios.get("https://localhost:7263/api/Orders", {
                })
                    .then((response) => {
                        console.log("Order data: ", response.data);
                        this.orderData = response.data;
                        console.log(this.orderData[0].orderID);
                    });
            }
        }
    }
</script>

<style scoped>

    .page-container {
        /*Toda la pantalla*/
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
        /*Empuja el footer hacia abajo*/
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

    .address_input_button_container {
        display: flex;
    }

    .map_button {
        font-size: 15px;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: black;
        border-radius: 20px;
        width: 150px;
        text-align: center;
        font-weight: bold;
        margin-left: auto;
    }

</style>
