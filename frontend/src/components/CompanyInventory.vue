<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>

        <div class="content">

            <div class="sidebarContainer">
                <div class="sidebar">
                    <router-link to="/companyProfile" class="eraseRouterLinkStyle"><a>Perfil</a></router-link>
                    <router-link to="/companyInventory" class="eraseRouterLinkStyle"><a>Inventario</a></router-link>
                    <router-link to="/add-product" class="eraseRouterLinkStyle"><a>Añadir producto</a></router-link>
                    <a @click="toggleProductsDropdown">Añadir entrega</a>
                    <ul v-if="isProductsDropdownVisible">
                        <li v-for="product in products" :key="product.productID" @click="selectProduct(product.productID)">
                            {{ product.productName }}
                        </li>
                    </ul>                    
                    <router-link to="/companyProfile" class="eraseRouterLinkStyle"><a>Ver pedidos activos</a></router-link>
                    <router-link to="/companyProfile" class="eraseRouterLinkStyle"><a>Ver pedidos pendientes</a></router-link>
                </div>
            </div>

            <div class="tables">
                <h1><strong>Inventario</strong></h1><br>

                <h2>Productos no perecederos</h2>
                <div v-if="isLoadingNonPerishable">
                    <p>Cargando productos no perecederos...</p>
                </div>
                <table v-else class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Imagen</th>
                            <th>Categoría</th>
                            <th>Precio</th>
                            <th>Descripción</th>
                            <th>Stock</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="product in nonPerishableProducts" :key="product.productID">
                            <td>{{ product.productName }}</td>
                            <td>
                                <img :src="product.imageURL" alt="Imagen del producto" class="product-image">
                            </td>
                            <td>{{ product.category }}</td>
                            <td>{{ product.price }}</td>
                            <td>{{ product.productDescription }}</td>
                            <td>{{ product.stock }}</td>
                        </tr>
                    </tbody>
                </table><br><br>

                <h2>Productos perecederos</h2>
                <div v-if="isLoadingPerishable">
                    <p>Cargando productos perecederos...</p>
                </div>
                <table v-else class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista1">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Imagen</th>
                            <th>Categoría</th>
                            <th>Precio</th>
                            <th>Descripción</th>
                            <th>Días de despacho</th>
                            <th>Límite de producción</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="product in perishableProducts" :key="product.productID">
                            <td>{{ product.productName }}</td>
                            <td>
                                <img :src="product.imageURL" alt="Imagen del producto" class="product-image">
                            </td>
                            <td>{{ product.category }}</td>
                            <td>{{ product.price }}</td>
                            <td>{{ product.productDescription }}</td>
                            <td>{{ product.deliveryDays }}</td>
                            <td>{{ product.productionLimit }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import axios from "axios";
    import {mapGetters, mapActions} from "vuex"
    export default {
        computed: {
            ...mapGetters(['getIdCompany']),
        },
        data() {
            return {
                products: [],
                isProductsDropdownVisible: false,
                isLoadingNonPerishable: true,
                isLoadingPerishable: true,
                perishableProducts: [],
                nonPerishableProducts: [], 
            };
        }, 
        mounted() {
            this.getCompanyProducts();
            this.getCompanyProductsDropdown();
        },
        methods: {
            ...mapActions(['openProduct']),
            getCompanyProducts() {
                // Load perishable products
                axios.get("https://localhost:7263/api/CompanyProducts/perishable", {
                    params: {
                        empresa: this.getIdCompany
                    }
                })
                .then((response) => {
                    this.perishableProducts = response.data;
                    this.isLoadingPerishable = false;
                    console.log(this.perishableProducts);
                })
                .catch((error) => {
                    console.error("Error obtaining perishable products:", error);
                    this.isLoadingPerishable = false;
                });

                // Load non-perishable products
                axios.get("https://localhost:7263/api/CompanyProducts/non-perishable", {
                    params: {
                        empresa: this.getIdCompany
                    }
                })
                .then((response) => {
                    this.nonPerishableProducts = response.data;
                    this.isLoadingNonPerishable = false;
                    console.log(this.nonPerishableProducts);
                })
                .catch((error) => {
                    console.error("Error obtaining non-perishable products:", error);
                    this.isLoadingNonPerishable = false;
                });
            },
            getCompanyProductsDropdown() {
                axios.get("https://localhost:7263/api/CompanyProfileData/CompanyProducts", {
                    params: {
                        companyID: this.getIdCompany
                    }
                })
                    .then((response) => {
                        this.products = response.data;
                    })
                    .catch((error) => {
                        console.error("Error obtaining company products:", error);
                    });
            },
            selectProduct(productID) {
                this.openProduct(productID);
                this.$router.push(`/add-delivery`);
            },
            toggleProductsDropdown() {
                this.isProductsDropdownVisible = !this.isProductsDropdownVisible;
            },
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

    .content {
    flex-grow: 1;
    display: flex;
    gap: 20px;
    flex-wrap: nowrap;
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

    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }

    .eraseRouterLinkStyle {
        color: black;
        text-decoration: none;
    }

    .sidebarContainer {
        height: 100%;
    }

    .sidebar {
        height: 100vh;
        width: 300px;
        background-color: #ffeec2;
        padding-top: 20px;
        /*Evita que se encoja*/
        flex-shrink: 0;
    }

        .sidebar a {
            padding: 10px 15px;
            font-size: 20px;
            display: block;
        }

        .sidebar h2 {
            padding: 10px 15px;
            display: block;
        }
            .sidebar a:hover {
                background-color: #c88646;
            }

        .tables {
            flex-grow: 1;
            padding: 10px;
            min-width: 300px;
            overflow: auto;
        }
        .product-image {
            width: 50px; 
            height: auto; 
        }

    li {
        list-style: none;
        cursor: pointer;
        user-select: none;
    }

        li:hover {
            background-color: #c88646;
        }

</style>
