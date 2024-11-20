<template>
    <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
        <div v-if="userTypeNumber === 2" class="col-12 col-md-6 d-flex justify-content-center">
        </div>
        <div class="container mt-4">
            <h2 class="mb-4">Reporte de ordenes en progreso</h2>
            <div class="filters mb-4 w-100">
                <div class="row g-3">
                    <div class="col-12 col-md-3">
                    <label for="allCompanies">Empresas:</label>
                        <input
                            type="text"
                            class="form-control"
                            id="Companies"
                            v-model="request.companies"
                        />
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="quantity">Cantidad minima:</label>
                        <input
                            type="number"
                            class="form-control"
                            id="QuantityMin"
                            v-model="request.minQuantity"
                        />
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="quantityMin">Cantidad maxima:</label>
                        <input
                            type="number"
                            class="form-control"
                            id="QuantityMax"
                            v-model="request.maxQuantity"
                        >
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="startDate">Fecha de Creación (Inicio):</label>
                    <input
                        type="date"
                        class="form-control"
                        id="startDate"
                        v-model="request.creationStartDate"
                        :max="request.creationEndDate"
                    />
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="endDate">Fecha de Creación (Fin):</label>
                    <input
                        type="date"
                        class="form-control"
                        id="endDate"
                        v-model="request.creationEndDate"
                        :min="request.creationStartDate"
                    />
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="sentStartDate">Fecha de Envío (Inicio):</label>
                    <input
                        type="date"
                        class="form-control"
                        id="sentStartDate"
                        v-model="request.sentStartDate"
                        :max="request.sentEndDate"
                    />
                    </div>
                    <div class="col-12 col-md-3">
                    <label for="sentEndDate">Fecha de Envío (Fin):</label>
                    <input
                        type="date"
                        class="form-control"
                        id="sentEndDate"
                        v-model="request.sentEndDate"
                        :min="request.sentStartDate"
                    />
                    </div>
                    <div class="col-12 col-md-3">
                        <label for="productCostRange" style="margin-bottom: 40px;">Costo del Producto:</label>
                        <div ref="sliderProduct" id="productRangeSlider"></div>
                    </div>
                    <div class="col-12 col-md-3">
                        <label for="shippingCostRange" style="margin-bottom: 40px;">Costo de Envío:</label>
                        <div ref="sliderEnvio" id="shippingRangeSlider"></div>
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
                <th>ID de Orden</th>
                <th>Empresas</th>
                <th>Cantidad</th>
                <th>Fecha de Creación</th>
                <th>Fecha de Envío</th>
                <th>Estado de la orden</th>
                <th>Costo de Productos</th>
                <th>Costo de Envío</th>
                <th>Costo Total de la orden</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="order in filteredOrders" :key="order.orderID">
                <td>{{ order.orderID }}</td>
                <td>{{ order.companies || 'N/A' }}</td>
                <td>{{ order.quantity || 0 }}</td>
                <td>{{ formatDate(order.creationDate) }}</td>
                <td>{{ formatDate(order.sentDate) }}</td>
                <td>{{ translateStatus(order.status)}}</td>
                <td>{{ order.productCost }}</td>
                <td>{{ order.deliveryCost }}</td>
                <td>{{ formatCurrency(order.totalCost) }}</td>
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
    import commonMethods from '@/mixins/commonMethods';
    import axios from "axios";
    import jsPDF from 'jspdf';
    import { mapGetters, mapState } from 'vuex';
    export default {
        mixins: [commonMethods],
        computed: {
            ...mapGetters(['isLoggedIn']),
            ...mapGetters(['getUserId']),
            ...mapState(['userCredentials']),
        },
        data() {
            return {
                request: {
                    userID: -1,
                    requestType: -1,
                    creationStartDate: "",
                    creationEndDate: "",
                    sentStartDate: "",
                    sentEndDate: "",
                    deliveredStartDate: "",
                    deliveredEndDate: "",
                    cancelledStartDate: "",
                    cancelledEndDate: "",
                    cancelledBy: -1,
                    minShippingCost: -1.0,
                    maxShippingCost: -1.0,
                    minProductCost: -1.0,
                    maxProductCost: -1.0,
                    minTotalCost: -1.0,
                    maxTotalCost: -1.0,
                    minQuantity: -1,
                    maxQuantity: -1,
                    orderID: -1,
                    companyName: ""
                },
                filteredOrders: null,
                minCost: 0.0,
                maxCost: 1000000.0,
            };
        },
        methods: {
            ...mapGetters(['getUserId', "isLoggedIn"]),
            convertToPdf() {
                const reportTable = document.getElementById("report");
                const tableHeight = reportTable.scrollHeight; // Full content height
                const tableWidth = reportTable.scrollWidth;   // Full content width

                // Initialize jsPDF with default settings
                const doc = new jsPDF({
                    orientation: "p",
                    unit: "px",
                    format:  [tableWidth + 40, tableHeight + 40], // Default page size, but the content will scale dynamically
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

                    // Remove unnecessary blank pages, if any
                    for (let i = totalPages; i > 1; i--) {
                        doc.deletePage(i);
                    }

                    // Save the PDF
                    const timeStamp = new Date().toISOString().replace(/[:\-T.]/g, "-");
                    doc.save(`InProgressClientReport_${timeStamp}.pdf`);
                });

            },
            clearFilters() {
                this.request = {
                    userID: this.getUserId,
                    requestType: 2,
                    creationStartDate: "",
                    creationEndDate: "",
                    sentStartDate: "",
                    sentEndDate: "",
                    deliveredStartDate: "",
                    deliveredEndDate: "",
                    cancelledStartDate: "",
                    cancelledEndDate: "",
                    cancelledBy: 0,
                    minShippingCost: -1.0,
                    maxShippingCost: -1.0,
                    minProductCost: -1.0,
                    maxProductCost: -1.0,
                    minTotalCost: -1.0,
                    maxTotalCost: -1.0,
                    minQuantity: -1,
                    maxQuantity: -1,
                    orderID: -1,
                    companyName: ""
                };
                this.getOrders();
            },
            getOrders() {
                axios.post(this.$backendAddress + "api/Reports/getReport/clientReport", 
                this.request)
                .then((response) => {
                    this.filteredOrders = response.data;
                    console.log(this.filteredOrders);
                    this.setTable();
                }).catch((error) => {
                    console.log(error);
                });
            },
            setTable(){

            },
            applyFilters() {
                this.request.userID = this.getUserId;
                this.request.requestType = 2;
                this.getOrders();
            },
            createSlider(sliderElement, minValue, maxValue) {
                maxValue = maxValue + 1;
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
            createSliders() {
                this.createSlider(this.$refs.sliderProduct, this.minCost, this.maxCost, this.request.minProductCost, this.request.maxProductCost);
                this.$refs.sliderProduct.noUiSlider.on('update', (values) => {
                    this.request.minProductCost = parseFloat(values[0], 10);
                    this.request.maxProductCost = parseFloat(values[1], 10);
                });
                            
                this.createSlider(this.$refs.sliderEnvio, this.minCost, this.maxCost, this.request.minShippingCost, this.request.maxShippingCost);
                this.$refs.sliderEnvio.noUiSlider.on('update', (values) => {
                    this.request.minShippingCost = parseFloat(values[0], 10);
                    this.request.maxShippingCost = parseFloat(values[1], 10);
                });
                
                this.createSlider(this.$refs.slider, this.minCost, this.maxCost, this.request.minTotalCost, this.request.maxTotalCost);
                this.$refs.slider.noUiSlider.on('update', (values) => {
                    this.request.minTotalCost = parseFloat(values[0], 10);
                    this.request.maxTotalCost = parseFloat(values[1], 10);
                });
            },
            formatCurrency(amount) {
                if (amount == null) return 'N/A';
                return new Intl.NumberFormat('es-ES', {
                    style: 'currency',
                    currency: 'CRC'
                }).format(amount);
            },
            formatDate(date) {
            if (!date) return 'N/A';
                const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                return new Date(date).toLocaleDateString(undefined, options);
            },
            translateStatus(status) {
                if (status == 1) return "No confirmado";
                else if (status == 2) return "Confirmado";
                else if (status == 4) return "Enviado";
                else return "N/A";
            }
        },
        mounted() {
            if (this.isLoggedIn === true) {
                this.$nextTick(() => {
                    this.createSliders();
                });
                this.clearFilters();    
            } else {
                window.location.href = "/login";
            }
        },
    };
</script>

<style scoped>
</style>