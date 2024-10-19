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
                <h1><strong>{{companyProfileData.companyName}}</strong></h1><br>
                <h5>Cédula jurídica: {{companyProfileData.cedula}}</h5>
                <h5>Correo Electrónico: {{companyProfileData.email}}</h5>
                <h5>Número de teléfono: {{companyProfileData.phoneNumber}}</h5><br><br>
                <h2>Direcciones registradas</h2>
                <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                    <thead>
                        <tr>
                            <th>Provincia</th>
                            <th>Cantón</th>
                            <th>Distrito</th>
                            <th>Dirección exacta</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(address, index) of companyProfileData.addresses" :key="index">
                            <td>{{ address.province }}</td>
                            <td>{{ address.canton }}</td>
                            <td>{{ address.district }}</td>
                            <td>{{ address.exactAddress }}</td>
                        </tr>
                    </tbody>
                </table><br><br>
                <h2>Perfiles asociados</h2>
                <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista1">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Correo electrónico</th>
                            <th>Teléfono</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(member, index) of companyProfileData.members" :key="index">
                            <td>{{ member.username }}</td>
                            <td>{{ member.email }}</td>
                            <td>{{ member.phoneNumber }}</td>
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
                companyProfileData: {
                    companyName: ' ',
                    cedula: ' ',
                    email: ' ',
                    phoneNumber: ' ',
                    addresses: [{
                        province: ' ',
                        canton: ' ',
                        district: ' ',
                        exactAddress: ' '
                    }],
                    members: [{
                        username: ' ',
                        email: ' ',
                        phoneNumber: ' '
                    }]
                }
            };
        }, mounted() {
            console.log(this.getIdCompany);
            this.getUserCompanyData();
            this.getCompanyProducts();
        },
        methods: {
            ...mapActions(['openProduct']),
            getUserCompanyData() {
                axios.get("https://localhost:7263/api/CompanyProfileData/CompanyData", {
                    params: {
                        companyID: this.getIdCompany
                    }
                })
                    .then((response) => {
                        this.companyProfileData = response.data;
                        console.log(this.companyProfileData);
                    })
                    .catch((error) => {
                        console.error("Error obtaining user companies:", error);
                    });
            },
            getCompanyProducts() {
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
        /*All the web page*/
        min-height: 100vh;
        display: flex;
        flex-direction: column;
    }

    .content {
        /*Pushes the footer to the botton*/
        flex-grow: 1;
        display: flex;
        gap: 20px;
        /*Let the elements accomodate themselves if there is space*/
        flex-wrap: wrap;

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
        /*prevents shrinking*/
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
            /*Effect when the cursor is over the element*/
            .sidebar a:hover {
                background-color: #c88646;
            }

    .tables {
        /*Let the content use all the space available*/
        flex-grow: 1;
        padding: 10px;
        /*prevents screen overflow*/
        min-width: 0;
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
