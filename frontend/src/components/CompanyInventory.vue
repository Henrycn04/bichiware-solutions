<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>

        <div class="content">
            <div class="left-column">
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
        </div>
        <div class="right-column">
            <div class="tables">
                <h1><strong>Inventario</strong></h1><br>

                <h2>Productos no perecederos</h2>
                <div v-if="isLoadingNonPerishable">
                    <p>Cargando productos no perecederos...</p>
                </div>
                <div v-else class="table-container">
                    <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                        <thead>
                            <tr>
                                <th>Nombre</th>
                                <th>Imagen</th>
                                <th>Categoría</th>
                                <th>Precio</th>
                                <th>Descripción</th>
                                <th>Stock</th>
                                <th>Acciones</th>
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
                                <td>
                                    <button class="btn btn-secondary" @click="redirectToModify('product', product.productID)">
                                        <img :src="pencilIcon" alt="Modify" style="width: 20px; height: 20px;">
                                    </button>
                                    <button class="btn btn-secondary" @click="deleteNonPerishable(product.productID)">
                                        <img :src="trashIcon" alt="Eliminar" style="width: 20px; height: 20px;">
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div><br><br>

                <h2>Productos perecederos</h2>
                <div v-if="isLoadingPerishable">
                    <p>Cargando productos perecederos...</p>
                </div>
                <div v-else class="table-container">
                    <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista1">
                        <thead>
                            <tr>
                                <th>Nombre</th>
                                <th>Imagen</th>
                                <th>Categoría</th>
                                <th>Precio</th>
                                <th>Descripción</th>
                                <th>Días de despacho</th>
                                <th>Límite de producción</th>
                                <th>Acciones</th>
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
                                <td>
                                <div style="display: flex; align-items: center;">
                                    <button class="btn btn-light" @click="showDeliveries(product)">
                                        <img :src="arrowIcon" alt="Modify" style="width: 20px; height: 20px;">
                                    </button>
                                    <div style="display: flex; flex-direction: column; align-items: center; margin-left: 5px;">
                                        <button class="btn btn-secondary" @click="redirectToModify('product', product.productID)">
                                            <img :src="pencilIcon" alt="Modify" style="width: 20px; height: 20px;">
                                        </button>
                                        <button class="btn btn-secondary mt-2" @click="deletePerishable( product.productID)">
                                            <img :src="trashIcon" alt="Eliminar" style="width: 20px; height: 20px;">
                                        </button>
                                    </div>
                                </div>
                            </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div
      v-if="showDeliveryModal"
      class="modal fade show"
      style="display: block"
      tabindex="-1"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Entregas</h5>
            <button
              type="button"
              class="btn-close"
              @click="closeDeliveryModal"
            ></button>
          </div>
          <div class="modal-body">
            <ul class="list-group">
              <li
                v-for="delivery in deliveries"
                :key="delivery.id"
                class="list-group-item d-flex justify-content-between align-items-center border-0 rounded shadow-sm mb-2 no-hover"
              >
                <div class="d-flex flex-column">
                    <span class="fw-bold">Lote: {{ delivery.batchNumber }}</span>
                    <span class="text-muted">Expira: {{ delivery.expirationDate }}</span>
                    <span class="text-success">
                    Unidades Disponibles: 
                    <strong>{{ this.selectedProduct.productionLimit - delivery.reservedUnits }}</strong>
                    </span>
                </div>
                    <div class="d-flex align-items-center ml-auto">
                    <button class="btn btn-secondary me-1" @click="redirectToModify('delivery', delivery.batchNumber)">
                        <img :src="pencilIcon" alt="Modify" style="width: 20px; height: 20px;">
                    </button>
                    <button class="btn btn-secondary" @click="deleteDelivery(delivery.batchNumber)">
                        <img :src="trashIcon" alt="Eliminar" style="width: 20px; height: 20px;">
                    </button>
                </div>
                </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import axios from "axios";
    import Swal from 'sweetalert2';
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
                pencilIcon: require('@/assets/pencilIcon.png'),
                arrowIcon: require('@/assets/arrowIcon.png'),
                trashIcon: require('@/assets/trashIcon.png'),
                selectedProduct: null,
                showDeliveryModal: false,
                deliveries: [],
                batchNumber: 0,
                productID: 0,
                deliveryID: []
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
                axios.get(this.$backendAddress + "api/CompanyProducts/perishable", {
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
                axios.get(this.$backendAddress + "api/CompanyProducts/non-perishable", {
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
            redirectToModify(type,productID) {
                if (type === 'product') {
                    this.openProduct(productID);
                    this.$router.push("/modifyProductData");
                } else if (type === 'delivery') {
                    console.log(productID);
                    this.batchNumber=productID;
                    this.productID=this.selectedProduct.productID;
                    this.deliveryID[0]= this.productID;
                    this.deliveryID[1]= this.batchNumber;
                    this.openProduct(this.deliveryID);
                    this.$router.push("/modifyDeliveryData");
                }
            },
            deleteDelivery(deliveryBatch) {
                this.batchNumber=deliveryBatch;
                this.productID=this.selectedProduct.productID;
                this.deliveryID[0]= this.productID;
                this.deliveryID[1]= this.batchNumber;
                
            },
            deletePerishable(productID) {
                    Swal.fire({
                        title: '¿Está seguro?',
                        text: "Esta acción no se puede deshacer.",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'No, cancelar'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            axios.delete(this.$backendAddress + `api/UpdateProduct/delete-perishable/${productID}`)
                                .then((response) => {
                                    console.log(response);
                                    Swal.fire('Eliminado','El producto ha sido eliminado con éxito.','success')
                                    .then(() => { window.location.reload();});
                                })
                                .catch((error) => {
                                    console.log(error);
                                    Swal.fire('Error','Ocurrió un error al eliminar el producto.','error'
                                    );
                                });
                        } else {
                            console.log("Action canceled.");
                        }
                    });
                },
                deleteNonPerishable(productID) {
                    console.log(productID);
                    Swal.fire({
                        title: '¿Está seguro?',
                        text: "Esta acción no se puede deshacer.",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'No, cancelar'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            axios.delete(this.$backendAddress + `api/UpdateProduct/delete-non-perishable/${productID}`)
                                .then((response) => {
                                    console.log(response);
                                    Swal.fire('Eliminado','El producto ha sido eliminado con éxito.','success')
                                    .then(() => { window.location.reload();});
                                })
                                .catch((error) => {
                                    console.log(error);
                                    Swal.fire('Error','Ocurrió un error al eliminar el producto.','error');
                                });
                        } else {
                            console.log("Action canceled.");
                        }
                    });
                },
            showDeliveries(product) {
                this.selectedProduct = product;
                this.fetchDeliveries(product.productID);
                this.showDeliveryModal = true;
            },
            closeDeliveryModal() {
                this.showDeliveryModal = false;
            },
            async fetchDeliveries(productId) {
                try {
                    const response = await axios.get(this.$backendAddress + "api/products/getProductDeliveries", {
                    params: { searchTerm: productId }
                    });
                    console.log(response.data);
                    this.deliveries = response.data;
                } catch (error) {
                    console.error("Error fetching deliveries:", error);
                }
            },
        }
    }
</script>


<style scoped>
    .modal {
    background-color: rgba(0, 0, 0, 0.5);
    }
    .modal-body {
        max-height: 400px; 
        overflow-y: auto;  
    }
    .page-container {
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
    .left-column {
        background-color: #ffeec2; 
        max-width: 250px
    }

    .right-column {
        background-color: white;
    
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
        display: flex;
    }

    .sidebar {
        height: 100vh;
        width: 300px;
        background-color: #ffeec2;
        padding-top: 20px;
        flex-shrink: 0;
        flex-grow: 1;
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
    .table-container {
        max-height: 300px; 
        overflow-y: auto; 
        margin-bottom: 20px; 
    }

    .product-image {
        width: 50px; 
        height: auto;
    }
    .no-hover:hover {
        background-color: inherit;
    }

</style>