<template>
            <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
                <div v-if="userTypeNumber === 2" class="col-12 col-md-6 d-flex justify-content-center">
                    <div class="w-100" style="max-width: 40%;">
                        <h5 for="companySelect" style="display: block; margin-top: 8px;">Seleccione su empresa</h5>
                        <select  v-model="selectedCompany" @change="callQuery(selectedCompany)" id="companySelect" class="form-select">
                            <option v-for="company in userCompanies" :key="company.companyID" :value="company.companyID">
                                {{ company.companyName }}
                            </option>
                        </select>
                    </div>
                </div>
                <div class="container mt-4">
                    <h2 class="mb-4">Reporte de Órdenes Completadas</h2>
                        <div class="filters mb-4 w-100">
                            <div class="row g-3">
                        <div class="col-12 col-md-3">
                        <label for="allCompanies">Empresas:</label>
                        <multiselect
                            v-model="this.filters.AllCompanies"
                            :options="this.allCompaniesList"
                            :multiple="true"
                            :close-on-select="false"
                            :clear-on-select="false"
                            placeholder="Seleccione empresas"
                        ></multiselect>
                        </div>

                        <div class="col-12 col-md-3">
                        <label for="quantity">Cantidad:</label>
                        <multiselect
                            v-model="this.filters.Quantity"
                            :options="this.quantitiesList"
                            :multiple="true"
                            :close-on-select="false"
                            :clear-on-select="false"
                            placeholder="Seleccione cantidades"
                        ></multiselect>
                        </div>

                            <div class="col-12 col-md-3">
                            <label for="startDate">Fecha de Creación (Inicio):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="startDate"
                                v-model="filters.StartDate"
                                :max="filters.EndDate"
                            />
                            </div>
                            <div class="col-12 col-md-3">
                            <label for="endDate">Fecha de Creación (Fin):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="endDate"
                                v-model="filters.EndDate"
                                :min="filters.StartDate"
                            />
                            </div>

                            <div class="col-12 col-md-3">
                            <label for="sentStartDate">Fecha de Envío (Inicio):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="sentStartDate"
                                v-model="filters.SentStartDate"
                                :max="filters.SentEndDate"
                            />
                            </div>
                            <div class="col-12 col-md-3">
                            <label for="sentEndDate">Fecha de Envío (Fin):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="sentEndDate"
                                v-model="filters.SentEndDate"
                                :min="filters.SentStartDate"
                            />
                            </div>
                            
                            <div class="col-12 col-md-3">
                            <label for="deliveredStartDate">Fecha de Entrega (Inicio):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="deliveredStartDate"
                                v-model="filters.DeliveredStartDate"
                                :max="filters.DeliveredEndDate"
                            />
                            </div>
                            <div class="col-12 col-md-3">
                            <label for="deliveredEndDate">Fecha de Entrega (Fin):</label>
                            <input
                                type="date"
                                class="form-control"
                                id="deliveredEndDate"
                                v-model="filters.DeliveredEndDate"
                                :min="filters.DeliveredStartDate"
                            />
                            </div>

                            <div class="col-12 col-md-3">
                                <label for="productCostRange" style="margin-bottom: 40px;">Costo del Producto:</label>
                                <div ref="sliderProduct" id="totalRangeSlider"></div>
                            </div>

                            <div class="col-12 col-md-3">
                                <label for="shippingCostRange" style="margin-bottom: 40px;">Costo de Envío:</label>
                                <div ref="sliderEnvio" id="totalRangeSlider"></div>
                            </div>

                            <div class="col-12 col-md-3">
                                <label for="totalRange" style="margin-bottom: 40px;">Total:</label>
                                <div ref="slider" id="totalRangeSlider"></div>
                            </div>
                        </div>

                        <div>
                            <button class="btn btn-success" @click="applyFilters" style="margin-top: 40px;" >Aplicar Filtros</button>
                            
                            <button class="btn btn-danger ms-2" @click="clearFilters" style="margin-top: 40px; margin-left: 10px;"  id="resetButton">Limpiar Filtros</button>
                        </div>                        
                    </div>

                    <table class="table table-bordered table-hover" id="report">
                    <thead class="thead-light">
                        <tr>
                        <th>
                            <div class="table-header">
                            <span>ID de Orden</span>
                            <button class="th_button" @click="sortColumn('orderID')">
                                    ↑↓
                            </button>
                        </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Empresas</span>
                            <button class="th_button" @click="sortColumn('allCompanies')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>                        
                            <div class="table-header">
                            <span>Cantidad</span>
                            <button class="th_button" @click="sortColumn('quantity')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Fecha de Creación</span>
                            <button class="th_button" @click="sortColumn('creationDate')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Fecha de Envío</span>
                            <button class="th_button" @click="sortColumn('sentDate')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Fecha de Entrega</span>
                            <button class="th_button" @click="sortColumn('deliveredDate')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Costo de Productos</span>
                            <button class="th_button" @click="sortColumn('productCost')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Costo de Envío</span>
                            <button class="th_button" @click="sortColumn('shippingCost')">
                                    ↑↓
                            </button>
                            </div>
                        </th>
                        <th>
                            <div class="table-header">
                            <span>Costo Total</span>
                            <button class="th_button" @click="sortColumn('total')">
                                    ↑↓
                            </button>
                        </div>
                        </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="order in filteredOrders" :key="order.OrderID">
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.allCompanies || 'N/A' }}</td>
                        <td>{{ order.quantity || 0 }}</td>
                        <td>{{ formatDate(order.creationDate) }}</td>
                        <td>{{ formatDate(order.sentDate) }}</td>
                        <td>{{ formatDate(order.deliveredDate) }}</td>
                        <td>{{ formatCurrency(order.productCost) }}</td>
                        <td>{{ formatCurrency(order.shippingCost) }}</td>
                        <td>{{ formatCurrency(order.total) }}</td>
                        </tr>
                    </tbody>
                    </table>
                    <div>
                        <button class="btn btn-success" @click="convertToPdf" style="margin-top: 40px;" >Descargar PDF</button>
                    </div>
                </div>
            </div>
</template>

<script>
import 'nouislider/dist/nouislider.css';
import noUiSlider from 'nouislider';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
    import commonMethods from '@/mixins/commonMethods';
    import axios from "axios";
    import { mapGetters, mapState } from 'vuex';
    import jsPDF from 'jspdf';
    export default {
        components: {
            Multiselect
            },
        mixins: [commonMethods],
        computed: {
            ...mapGetters(['isLoggedIn']), 
            ...mapState(['userCredentials']),
        },
        data() {
            return {
                completedOrders: [],
                filteredOrders: [],

                filters: {
                    AllCompanies: [],
                    Quantity: [],
                    StartDate: null,
                    EndDate: null,
                    SentStartDate: null,
                    SentEndDate: null,
                    DeliveredStartDate: null,
                    DeliveredEndDate: null,
                    StartProductCost: 0,
                    EndProductCost: 0,
                    StartShippingCost: 0,
                    EndShippingCost: 0,
                    StartTotal: 0,
                    EndTotal: 0
                },

                allCompaniesList: [],
                quantitiesList: [],
                maxProductCost: 0,
                maxShippingCost: 0,
                maxTotal: 0,
                minProductCost: 0,
                minShippingCost: 0,
                minTotal: 0
            };
        },
        methods: {
            convertToPdf() {
                const baseTable = document.getElementById("report");
                const tableHeight = baseTable.offsetHeight * 2;
                const tableWidth = baseTable.offsetWidth;
                const reportTable = baseTable.cloneNode(true);
                const buttons = reportTable.querySelectorAll(".th_button");
                buttons.forEach(button => button.remove());


                const doc = new jsPDF({
                    orientation: "p",
                    unit: "px",
                    format:  [tableWidth + 40, tableHeight + 100],
                });

                const margins = 20;
                const pdfHeight = tableHeight + 2 * margins;
                const pdfWidth = tableWidth + 2 * margins;
                doc.internal.pageSize.height = pdfHeight;
                doc.internal.pageSize.width = pdfWidth;
                doc.html(reportTable, {
                    x: margins,
                    y: margins,
                    width: tableWidth,
                }).then(() => {
                    const totalPages = doc.internal.getNumberOfPages();

                    for (let i = totalPages; i > 1; i--) {
                        doc.deletePage(i);
                    }

                    const timeStamp = new Date().toISOString().replace(/[:\-T.]/g, "-");
                    doc.save(`CompletedOrdersReport_${timeStamp}.pdf`);
                });

            },
            callQuery(companyID) {
                console.log('Company ID seleccionado:', companyID);
                this.userId = this.userCredentials.userId; 
                const params = new URLSearchParams();
                params.append("UserID", this.userId);
                params.append("CompanyID", companyID);
                const url = `${this.$backendAddress}api/Reports/getReport/completedOrders?${params.toString()}`;
                this.getInitialOrders(url);
            },
            
            applyFilters() {
                let filtered = [...this.completedOrders];
                if (Array.isArray(this.filters.AllCompanies) && this.filters.AllCompanies.length > 0) {
                    filtered = filtered.filter(order =>
                    typeof order.allCompanies === 'string' &&
                    this.filters.AllCompanies.some(company => order.allCompanies.includes(company))
                    );
                }
                
                if (Array.isArray(this.filters.Quantity) && this.filters.Quantity.length > 0) {
                    filtered = filtered.filter(order =>this.filters.Quantity.includes(order.quantity)
                    );
                }

                if (this.filters.StartDate) {
                    filtered = filtered.filter(order =>
                    order.creationDate !== null && new Date(order.creationDate) >= new Date(this.filters.StartDate)
                    );
                }
                if (this.filters.EndDate) {
                    filtered = filtered.filter(order =>
                    order.creationDate !== null && new Date(order.creationDate) <= new Date(this.filters.EndDate)
                    );
                }

                if (this.filters.SentStartDate) {
                    filtered = filtered.filter(order =>
                    order.sentDate !== null && new Date(order.sentDate) >= new Date(this.filters.SentStartDate)
                    );
                }
                if (this.filters.SentEndDate) {
                    filtered = filtered.filter(order =>
                    order.sentDate !== null && new Date(order.sentDate) <= new Date(this.filters.SentEndDate)
                    );
                }


                if (this.filters.DeliveredStartDate) {
                    filtered = filtered.filter(order =>
                    order.deliveredDate !== null && new Date(order.deliveredDate) >= new Date(this.filters.DeliveredStartDate)
                    );
                }
                if (this.filters.DeliveredEndDate) {
                    filtered = filtered.filter(order =>
                    order.deliveredDate !== null && new Date(order.deliveredDate) <= new Date(this.filters.DeliveredEndDate)
                    );
                }
                console.log(this.filters.StartProductCost);

                console.log(filtered);

                if (this.filters.StartProductCost >= 0) {
                    filtered = filtered.filter(order =>
                      order.productCost >= this.filters.StartProductCost
                    );
                }
                console.log(this.filters.EndProductCost);
                if (this.filters.EndProductCost >= 0) {
                    filtered = filtered.filter(order =>
                      order.productCost <= this.filters.EndProductCost
                    );
                }
                console.log(filtered);
                if (this.filters.StartShippingCost > 0) {
                    filtered = filtered.filter(order =>
                    order.shippingCost !== null && order.shippingCost >= this.filters.StartShippingCost
                    );
                }
                if (this.filters.EndShippingCost > 0) {
                    filtered = filtered.filter(order =>
                    order.shippingCost !== null && order.shippingCost <= this.filters.EndShippingCost
                    );
                }

                if (this.filters.StartTotal > 0) {
                    filtered = filtered.filter(order => order.total !== null && order.total >= this.filters.StartTotal);
                }
                if (this.filters.EndTotal > 0) {
                    filtered = filtered.filter(order => order.total !== null && order.total <= this.filters.EndTotal);
                }
                this.filteredOrders = filtered;
             
            },
            clearFilters() {
                this.filters = {
                    AllCompanies: [],
                    Quantity: [], 
                    StartDate: null,
                    EndDate: null,
                    SentStartDate: null,
                    SentEndDate: null,
                    DeliveredStartDate: null,
                    DeliveredEndDate: null,
                    StartProductCost: 0,
                    EndProductCost: 0,
                    StartShippingCost: 0,
                    EndShippingCost: 0,
                    StartTotal: 0,
                    EndTotal: 0
                };
                this.filteredOrders = [...this.completedOrders];
            },
            getInitialOrders(url) {
                axios.get(url)
                    .then((response) => {
                        if (typeof response.data === "string") {
                            console.warn(response.data, this.userCredentials.userId);
                            this.completedOrders = [];
                        } else {
                            console.warn(response.data);
                            this.completedOrders = response.data;
                            this.filteredOrders = [...this.completedOrders];
                            const allCompanies = this.completedOrders.map(order => order.allCompanies).filter(company => company !== null);
                            this.allCompaniesList = [...new Set(allCompanies)].sort((a, b) => a.localeCompare(b));
                            const allQuantities = this.completedOrders.map(order => order.quantity);
                            this.quantitiesList = [...new Set(allQuantities)].sort((a, b) => a - b);

                            ({ min: this.minProductCost, max: this.maxProductCost } = this.getMinMax(this.completedOrders, 'productCost'));
                            ({ min: this.minShippingCost, max: this.maxShippingCost } = this.getMinMax(this.completedOrders, 'shippingCost'));
                            ({ min: this.minTotal, max: this.maxTotal } = this.getMinMax(this.completedOrders, 'total'));
                            
                            this.createSlider(this.$refs.sliderProduct, this.minProductCost, this.maxProductCost, this.filters.StartProductCost, this.filters.EndProductCost);
                            this.$refs.sliderProduct.noUiSlider.on('update', (values) => {
                                this.filters.StartProductCost = parseInt(values[0], 10);
                                this.filters.EndProductCost = parseInt(values[1], 10);
                            });
                            
                            this.createSlider(this.$refs.sliderEnvio, this.minShippingCost, this.maxShippingCost, this.filters.StartShippingCost, this.filters.EndShippingCost);
                            this.$refs.sliderEnvio.noUiSlider.on('update', (values) => {
                                this.filters.StartShippingCost = parseInt(values[0], 10);
                                this.filters.EndShippingCost = parseInt(values[1], 10);
                            });
                           
                            this.createSlider(this.$refs.slider, this.minTotal, this.maxTotal, this.filters.StartTotal, this.filters.EndTotal);
                            this.$refs.slider.noUiSlider.on('update', (values) => {
                                this.filters.StartTotal = parseInt(values[0], 10);
                                this.filters.EndTotal = parseInt(values[1], 10);
                            });
                        }
                    })
                    .catch((error) => {
                        console.error("Error obtaining completed orders:", error);
                    });
            },

            formatDate(date) {
            if (!date) return 'N/A';
                const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                return new Date(date).toLocaleDateString(undefined, options);
            },
            formatCurrency(amount) {
                if (amount == null) return 'N/A';
                return new Intl.NumberFormat('es-ES', {
                    style: 'currency',
                    currency: 'CRC'
                }).format(amount);
            },
            getMinMax(orders, property) {
                if (!orders || orders.length === 0) {
                    console.warn('No orders available to process.');
                    return { min: null, max: null };
                }


                if (!orders.every(order => Object.prototype.hasOwnProperty.call(order, property))) {
                    console.warn(`La propiedad "${property}" no existe en todos los objetos de orders.`);
                    return { min: null, max: null };
                }

                let min = Infinity;
                let max = -Infinity;

                orders.forEach(order => {
                    const value = order[property];
                    if (value < min) {
                        min = value;
                    }
                    if (value > max) {
                        max = value;
                    }
                });

                this.min = min;
                this.max = max;
                
                return { min, max };
            },
            createSlider(sliderElement, minValue, maxValue) {
                maxValue = maxValue + 1;
                minValue = minValue -1;
                const slider = sliderElement;
                noUiSlider.create(slider, {
                    start: [minValue, maxValue],
                    connect: true,
                    range: {
                        'min': minValue,
                        'max': maxValue
                    },
                    step: 1,
                    tooltips: true,
                    format: {
                        to: (value) => Math.round(value),
                        from: (value) => Number(value)
                    }
                });

                const sliderBase = slider.querySelector('.noUi-base');
                const sliderConnect = slider.querySelector('.noUi-connect');

                if (sliderBase) {
                    sliderBase.style.background = '#e0e0e0';
                    sliderBase.style.transform = 'scaleX(0.9)';
                    slider.style.background = 'transparent';
                }

                if (sliderConnect) {
                    sliderConnect.style.background = '#f07800';
                }

                const sliderHandles = slider.querySelectorAll('.noUi-handle');

                sliderHandles.forEach(handle => {
                    handle.style.backgroundColor = '#000000';
                    handle.style.borderRadius = '15%';
                });

                sliderHandles.forEach(handle => {
                    handle.style.transform = 'scale(0.9)';
                });
                const resetButton = document.getElementById('resetButton');
                if (resetButton) {
                    resetButton.addEventListener('click', () => {
                        slider.noUiSlider.set([minValue, maxValue]);
                    });
                }
            },
            fixNulls(columnName) {
                for (var i = 0; i < this.filteredOrders.length; i++) {
                    if(this.filteredOrders[i][columnName] == null){
                        this.filteredOrders[i][columnName] = "N/A";
                    }
                }
            },
        sortColumn(columnName) {
            this.fixNulls(columnName);
            if(this.lastColumnSorted === columnName) this.ascendingSort = !this.ascendingSort;
            else this.ascendingSort = true;
            this.lastColumnSorted = columnName;
            console.log("Sorts by: " + columnName + ", ascending: " + this.ascendingSort)
            var temp;
            if (this.ascendingSort) {
                for (var i = 1; i < this.filteredOrders.length; i++) {
                    for (var j = 0; j < this.filteredOrders.length - i; j++) {
                        if(this.filteredOrders[j][columnName] > this.filteredOrders[j+1][columnName]) {
                            temp = this.filteredOrders[j+1];
                            this.filteredOrders[j+1] = this.filteredOrders[j];
                            this.filteredOrders[j] = temp;
                        }
                    }
                }
            } else {
                for (i = 1; i < this.filteredOrders.length; i++) {
                    for (j = 0; j < this.filteredOrders.length - i; j++) {
                        if(this.filteredOrders[j][columnName] < this.filteredOrders[j+1][columnName]) {
                            temp = this.filteredOrders[j+1];
                            this.filteredOrders[j+1] = this.filteredOrders[j];
                            this.filteredOrders[j] = temp;
                        }
                    }
                }
            }
          }
        },
        mounted() {
            if (this.isLoggedInVar && this.userTypeNumber === 3) {
                this.userId = this.userCredentials.userId; 

                const params = new URLSearchParams();

                params.append("UserID", this.userId);

                const url = `${this.$backendAddress}api/Reports/getReport/completedOrders?${params.toString()}`;

                this.getInitialOrders(url);
            }
        },
    };
</script>

<style>
.filters {
  border: 1px solid #ddd;
  padding: 15px;
  border-radius: 5px;
}
.table {
  margin-top: 20px;
  border-collapse: collapse;
}
.table th, .table td {
  text-align: left;
  padding: 8px;
}
.thead-light {
  background-color: #f8f9fa;
}
.col-10 {
    height: calc(50vh - 100px);
}
</style>
