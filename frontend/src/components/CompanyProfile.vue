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
                    <router-link to="/modifyCompanyData" class="eraseRouterLinkStyle"><a>Modificar datos de empresa</a></router-link>
                    <div style="height: 25%;"></div>
                    <router-link to="/deleteCompany" class="btn btn-danger m-3"><a>Eliminar Empresa</a></router-link>
                </div>
            </div>
            <div class="hstack gap-0 my-3">
                <div class="ms-auto btn-group" role="group" aria-label="Basic example">
                    <button type="button" class="btn btn-primary border-1 border-dark"   @click="deleteSelectedAddresses"
                    :disabled="selectedAddresses.length === 0" id="deleteButton">
                        <svg width="25px" height="25px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M5.73708 6.54391V18.9857C5.73708 19.7449 6.35257 20.3604 7.11182 20.3604H16.8893C17.6485 20.3604 18.264 19.7449 18.264 18.9857V6.54391M2.90906 6.54391H21.0909" stroke="#1C1C1C" stroke-width="1.7" stroke-linecap="round"/>
                        <path d="M8 6V4.41421C8 3.63317 8.63317 3 9.41421 3H14.5858C15.3668 3 16 3.63317 16 4.41421V6" stroke="#1C1C1C" stroke-width="1.7" stroke-linecap="round"/>
                        </svg>
                    </button>
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
                            <th>Seleccionar</th>
                            <th>Provincia</th>
                            <th>Cantón</th>
                            <th>Distrito</th>
                            <th>Dirección exacta</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(address, index) of companyProfileData.addresses" :key="index">
                            <td>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" :value="address" v-model="selectedAddresses" id="defaultCheck1">
                                </div>
                            </td>
                            <td>{{ address.province }}</td>
                            <td>{{ address.canton }}</td>
                            <td>{{ address.district }}</td>
                            <td>{{ address.exactAddress }}</td>
                            <td>
              <button
                type="button"
                class="btn btn-primary border-1 border-dark"
                @click="saveAddressId(address)"
              >
                <svg width="25px" height="25px" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M13 0L16 3L9 10H6V7L13 0Z" fill="#000000"/>
                  <path d="M1 1V15H15V9H13V13H3V3H7V1H1Z" fill="#000000"/>
                </svg>
              </button>
            </td>
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
                },
                selectedAddresses: [],
                selectedAddressesID: [],
            };
        }, mounted() {
            console.log(this.getIdCompany);
            this.getUserCompanyData();
            this.getCompanyProducts();
        },
        methods: {
            ...mapActions(['openProduct']),
            ...mapGetters(['getIdCompany']),
            ...mapActions(['setAddressId']),
            saveAddressId(address) {
                const addressID = {
                    addressID: address.addressID,
                    province: address.province,
                    canton: address.canton,
                    district: address.district,
                    exact: address.exactAddress,
                    latitude: address.latitude || 0,
                    longitude: address.longitude || 0,
                    userID: 0,
                    companyID: Number(this.getIdCompany),
                    isCompany: true
                };
                console.log('AddressID:', addressID);
                this.$store.commit('setAddressId', addressID);
                this.$router.push(`/modifyAddress`);
            },
            getUserCompanyData() {
                axios.get(this.$backendAddress + "api/CompanyProfileData/CompanyData", {
                    params: {
                        companyID: this.getIdCompany
                    }
                })
                    .then((response) => {
                        this.companyProfileData = response.data;
                    })
                    .catch((error) => {
                        console.error("Error obtaining user companies:", error);
                    });
            },
            getCompanyProducts() {
                axios.get(this.$backendAddress + "api/CompanyProfileData/CompanyProducts", {
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
            deleteSelectedAddresses() {
                for (var address of this.selectedAddresses) {
                    this.selectedAddressesID.push(address.addressID)
                }
                console.log('Deleting these addresses:', this.selectedAddressesID);
                axios({
                    url: this.$backendAddress + "api/AccountAddresses/DeleteAddresess",
                    method: 'post',
                    data: this.selectedAddressesID
                })
                .then(() => {
                    this.companyProfileData.addresses = this.companyProfileData.addresses.filter(
                    address => !this.selectedAddresses.includes(address)
                    );
                    this.selectedAddresses = [];
                })
                .catch(error => {
                    console.error("Error deleting addresses:", error);
                });
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
