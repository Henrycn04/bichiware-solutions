<template>
    <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
        <div class="container mt-4">
            <h2 class="mb-4">Reporte de Gancias Totales</h2>  
        <div class="row justify-content-center">
            <div class="row justify-content-center">
            <div class="row justify-content-between align-items-center" style="background-color: #f07800; padding: 10px 15px;">
                <div class="col-12 col-md-4 d-flex align-items-center">
                    <h5 class="text-white mb-0 me-2" style="font-size: 0.9rem;">Empresas</h5>
                    <multiselect
                        v-model="selectedCompanies"
                        :options="availableCompanies"
                        :multiple="true"
                        :close-on-select="false"
                        :clear-on-select="false"
                        :preserve-search="true"
                        placeholder="Seleccione empresas"
                        label="companyName"
                        track-by="companyID"
                        class="form-control"
                        @update:modelValue="onSelectCompanies"
                        style="font-size: 0.9rem; width: 100%; max-width: none;">
                    </multiselect>
                </div>

                <div class="col-12 col-md-4 d-flex align-items-center">
                    <h5 class="text-white mb-0 me-2" style="font-size: 0.9rem;">Años</h5>
                    <multiselect
                        v-model="selectedYears"
                        :options="availableYears"
                        :multiple="true"
                        :close-on-select="false"
                        :clear-on-select="false"
                        :preserve-search="true"
                        placeholder="Seleccione años"
                        class="form-control"
                        @update:modelValue="onSelectYears"
                        style="font-size:0.9rem; width: 100%; max-width: none;">
                    </multiselect>
                </div>
                <div class="col-12 col-md-3 text-end mt-3 mt-md-0 d-flex flex-column">
                    <button class="btn btn-success me-3 btn-sm mb-2" @click="applyFilters" style="font-size: 0.9rem;">Aplicar Filtros Generales</button>
                    <button class="btn btn-danger me-3 btn-sm" @click="clearFilters" id="resetButton" style="font-size: 0.9rem;">Limpiar Filtros Generales</button>
                </div>

            </div>
        </div>
        </div>
        <div class="container mt-4">
        <div class="d-flex align-items-center col-md-6">
        <h5 class="mb-0 me-3" style="font-size: 0.9rem;">Empresas</h5>
        <multiselect
            v-model="columnFilters.companyName"
            :options="selectedCompanies"
            :multiple="true"
            :close-on-select="false"
            :clear-on-select="false"
            :preserve-search="true"
            placeholder=""
            label="companyName"
            track-by="companyID"
            class="form-control"
            style="font-size: 0.9rem; width: 100%; max-width: none;">
        </multiselect>
    </div>


    <div class="d-flex align-items-center col-md-6 mt-3 mt-md-0">
        <h5 class="mb-0 me-5" style="font-size: 0.9rem;">Mes</h5>
        <multiselect
            v-model="columnFilters.month"
            :options="availableMonths"
            :multiple="true"
            :close-on-select="false"
            :clear-on-select="false"
            :preserve-search="true"
            placeholder=""
            class="form-control"
            style="font-size: 0.9rem; width: 100%; max-width: none;">
        </multiselect>
    </div>

    <div class="row mb-4">

        <div class="col-12 col-md-3">
            <label for="sliderProduct" style="margin-bottom: 10px;">Total de compra:</label>
            <div ref="sliderProduct" id="sliderProduct"></div>
        </div>

        <div class="col-12 col-md-3">
            <label for="sliderEnvio" style="margin-bottom: 10px;">Costo de envío:</label>
            <div ref="sliderEnvio" id="sliderEnvio"></div>
        </div>

        <div class="col-12 col-md-3">
            <label for="sliderTotal" style="margin-bottom: 10px;">Costo total de la compra:</label>
            <div ref="sliderTotal" id="sliderTotal"></div>
        </div>


        <div>
            <button class="btn btn-success" @click="applyColumnFilters" style="margin-top: 40px;" >Aplicar Filtros de columnas</button>
                            
            <button class="btn btn-danger ms-2" @click="clearColumnFilters" style="margin-top: 40px; margin-left: 10px;"  id="resetButton">Limpiar Filtros de columnas</button>
        </div>
    </div>
</div>
        <table id="report" class="table table-bordered table-hover">
            <thead class="thead-light">
                <tr>
                    <th>
                        <div class="table-header">
                            <span>Emprendimiento</span>
                            <button class="th_button" @click="sortColumn('companyName')">
                                ↑↓
                            </button>
                        </div>

                    </th>
                    <th>
                        <div class="table-header">
                            <span>Mes</span>
                            <button class="th_button" @click="sortColumn('month')">
                                ↑↓
                            </button>
                        </div>
                    </th>
                    <th>
                        <div class="table-header">
                            <span>Total de compra</span>
                            <button class="th_button" @click="sortColumn('totalPrice')">
                                ↑↓
                            </button>
                        </div>
                    </th>
                    <th>
                        <div class="table-header">
                            <span>Total de envío</span>
                            <button class="th_button" @click="sortColumn('totalShippingCost')">
                                ↑↓
                            </button>
                        </div>
                    </th>
                    <th>
                        <div class="table-header">
                            <span>Costo total de la compra</span>
                            <button class="th_button" @click="sortColumn('totalOrderPrice')">
                                ↑↓
                            </button>
                        </div>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr v-if="preparedOrdersData.length === 0">
                    <td colspan="5" class="text-center">No hay registros disponibles.</td>
                </tr>
                <tr
                    v-for="(order, index) in preparedOrdersData"
                    :key="index"
                    :class="{'table-warning': order.isTotalRow}"
                >
                    <td>{{ order.companyName || 'N/A' }}</td>
                    <td>{{ order.month }}</td>
                    <td>{{ formatCurrency(order.totalPrice) }}</td>   
                    <td>{{ formatCurrency(order.totalShippingCost) }}</td>
                    <td>{{ formatCurrency(order.totalOrderPrice) }}</td>
                    
                </tr>
            </tbody>
        </table>
        <div>
        <button class="btn btn-success" @click="convertToPdf" style="margin-top: 40px;" >Descargar PDF</button>
    </div>
    <br>
    </div>
    
</div>  
</template>

<script>
import 'nouislider/dist/nouislider.css';
import noUiSlider from 'nouislider';
import Multiselect from 'vue-multiselect';
import jsPDF from 'jspdf';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import commonMethods from '@/mixins/commonMethods';
import Swal from 'sweetalert2';
import axios from "axios";
import { mapGetters, mapState } from 'vuex';
const totalString = "Todos";
const totalSliders = 3;
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
            selectedCompanies: [],
            selectedCompanyIds:[],
            selectedYears: [],
            availableCompanies: [],
            availableYears: [],
            availableMonths: [],
            filteredOrders: [],
            preparedOrdersData: [],
            sliders:[],
            columnFilters: {
                companyName: '',
                month: '',
                StartProductCost: 0,
                EndProductCost: 0,
                StartShippingCost: 0,
                EndShippingCost: 0,
                StartTotal: 0,
                EndTotal: 0
            },
        };
    },
    methods: {
        applyFilters() {
              this.selectedCompanyIds=[];
              this.sliders.forEach(sliderInstance => {
                sliderInstance.destroy();  
            });
            this.sliders = [];
                this.selectedCompanies = this.selectedCompanies.filter(company => company.companyID !== 0);
                this.selectedYears = this.selectedYears.filter(year => year !== totalString);
            for (let i = 0; i < this.selectedCompanies.length; i++) {
                this.selectedCompanyIds[i]=this.selectedCompanies[i].companyID;
            }
            const requestData = {
                years: this.selectedYears,
                companyIDs: this.selectedCompanyIds,
            };
            console.log(requestData);
            axios
                .post(this.$backendAddress + "api/Reports/total-profits", requestData)
                .then((response) => {
                    this.filteredOrders = response.data;
                    this.preparedOrdersData=this.prepareOrders();
                    for (let i = 0; i < this.filteredOrders.length; i++) {
                        if (!this.availableMonths.includes(this.filteredOrders[i].month)) {
                            this.availableMonths[i]=this.filteredOrders[i].month;
                        }
                    }
                    ({ min: this.minProductCost, max: this.maxProductCost } = this.getMinMax(this.filteredOrders, 'totalPrice'));
                    ({ min: this.minShippingCost, max: this.maxShippingCost } = this.getMinMax(this.filteredOrders, 'totalShippingCost'));
                    ({ min: this.minTotal, max: this.maxTotal } = this.getMinMax(this.filteredOrders, 'totalOrderPrice'));
                    this.createSlider(this.$refs.sliderProduct, this.minProductCost, this.maxProductCost, this.columnFilters.StartProductCost, this.columnFilters.EndProductCost);
                     this.$refs.sliderProduct.noUiSlider.on('update', (values) => {
                         this.columnFilters.StartProductCost = parseInt(values[0], 10);
                        this.columnFilters.EndProductCost = parseInt(values[1], 10);
                    });
                            
                    this.createSlider(this.$refs.sliderEnvio, this.minShippingCost, this.maxShippingCost, this.columnFilters.StartShippingCost, this.columnFilters.EndShippingCost);
                       this.$refs.sliderEnvio.noUiSlider.on('update', (values) => {
                        this.columnFilters.StartShippingCost = parseInt(values[0], 10);
                        this.columnFilters.EndShippingCost = parseInt(values[1], 10);
                    });
                           
                    this.createSlider(this.$refs.sliderTotal, this.minTotal, this.maxTotal, this.columnFilters.StartTotal, this.columnFilters.EndTotal);
                        this.$refs.sliderTotal.noUiSlider.on('update', (values) => {
                        this.columnFilters.StartTotal = parseInt(values[0], 10);
                        this.columnFilters.EndTotal = parseInt(values[1], 10);
                    });
                })
                .catch((error) => {
                    Swal.fire('Error','Ocurrió un error al aplicar filtros.','error');
                    console.log(error)
                });
        },
        clearFilters() {
            this.columnFilters.companyName= '',
            this.columnFilters.month= '',
            this.columnFilters.StartProductCost= 0,
            this.columnFilters.EndProductCost= 0,
            this.columnFilters.StartShippingCost= 0,
            this.columnFilters.EndShippingCost= 0,
            this.columnFilters.StartTotal= 0,
            this.columnFilters.EndTotal= 0
            this.selectedCompanies = [];
            this.selectedYears = [];
            this.filteredOrders = [];
            this.selectedCompanyIds = [];
            this.preparedOrdersData = []; 
        },

        applyColumnFilters() {
            let change = false;
            let filteredData = this.filteredOrders; 
            if (this.columnFilters.month.length != 0) {
                filteredData = filteredData.filter(order =>
                    this.columnFilters.month.includes(order.month)
                );
                change = true;
            }

            if (this.columnFilters.companyName.length != 0) {
                filteredData = filteredData.filter(order => {
                    const companyNames = this.columnFilters.companyName.map(filter => filter.companyID);
                    return companyNames.includes(order.companyID);
                });
                change = true;
            }


            if (this.columnFilters.StartProductCost >= 0) {
                filteredData = filteredData.filter(order =>
                order.totalPrice !== null &&  Math.ceil(parseFloat(order.totalPrice)) >= parseFloat(this.columnFilters.StartProductCost)
                );
                change = true;
            }

            if (this.columnFilters.EndProductCost >= 0) {
                filteredData = filteredData.filter(order =>
                    order.totalPrice !== null && Math.ceil(parseFloat(order.totalPrice)) <= parseFloat( this.columnFilters.EndProductCost)
                );
                change = true;
            }

       
            if (this.columnFilters.StartShippingCost > 0) {
                filteredData = filteredData.filter(order =>
                    order.totalShippingCost !== null && Math.ceil(parseFloat(order.totalShippingCost)) >= parseFloat( this.columnFilters.StartShippingCost)
                );
                change = true;
            }

            if (this.columnFilters.EndShippingCost > 0) {
                filteredData = filteredData.filter(order =>
                    order.totalShippingCost !== null && Math.ceil(parseFloat(order.totalShippingCost)) <= parseFloat( this.columnFilters.EndShippingCost)
                );
                change = true;
            }

          
            if (this.columnFilters.StartTotal > 0) {
                filteredData = filteredData.filter(order =>
                    order.totalOrderPrice !== null && Math.ceil(parseFloat(order.totalOrderPrice)) >= parseFloat(this.columnFilters.StartTotal)
                );
                change = true;
            }

            if (this.columnFilters.EndTotal > 0) {
                filteredData = filteredData.filter(order =>
                    order.totalOrderPrice !== null && Math.ceil(parseFloat(order.totalOrderPrice)) <= parseFloat(this.columnFilters.EndTotal)
                );
                change = true;
            }


            if (change) {
                this.preparedOrdersData = this.calculateTotalsAndSpecialRows(
                    this.groupOrdersByYear(filteredData)
                );
            }
        },

        clearColumnFilters(){
                this.columnFilters.companyName= '',
                this.columnFilters.month= '',
                this.columnFilters.StartProductCost= 0,
                this.columnFilters.EndProductCost= 0,
                this.columnFilters.StartShippingCost= 0,
                this.columnFilters.EndShippingCost= 0,
                this.columnFilters.StartTotal= 0,
                this.columnFilters.EndTotal= 0
                this.applyFilters();

        },
        formatCurrency(amount) {
            if (amount == null) return 'N/A';
            return new Intl.NumberFormat('es-CR', {
                style: 'currency',
                currency: 'CRC'
            }).format(amount);
        },


        groupOrdersByYear(orders) {
            return orders.reduce((acc, order) => {
                if (!acc[order.year]) acc[order.year] = [];
                acc[order.year].push(order);
                return acc;
            }, {});
        },

        calculateTotalsAndSpecialRows(groupedOrders) {
            const sortedOrders = [];
            for (const year of Object.keys(groupedOrders).sort()) {
                const yearOrders = groupedOrders[year];
                const totalOrderPrice = yearOrders.reduce((sum, o) => sum + o.totalOrderPrice, 0);
                const totalShippingCost = yearOrders.reduce((sum, o) => sum + o.totalShippingCost, 0);
                const totalPrice = yearOrders.reduce((sum, o) => sum + o.totalPrice, 0);

                sortedOrders.push(...yearOrders); 
                sortedOrders.push({ 
                    year,
                    companyName: `Totales`,
                    month: `${year}`,
                    totalOrderPrice,
                    totalShippingCost,
                    totalPrice,
                    isTotalRow: true,
                });
            }
            return sortedOrders;
        },

        prepareOrders() {
            if (this.filteredOrders.length === 0) {
                Swal.fire('Error', 'No se encontraron registros para los filtros seleccionados.', 'error');
                return [];
            }
            const groupedOrders = this.groupOrdersByYear(this.filteredOrders);
            return this.calculateTotalsAndSpecialRows(groupedOrders);
        },

        sortColumn(columnKey) {
            this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
            const sortMultiplier = this.sortOrder === 'asc' ? 1 : -1;

            const groupedOrders = this.groupOrdersByYear(this.filteredOrders);

            this.preparedOrdersData = Object.keys(groupedOrders)
                .sort() 
                .reduce((sortedOrders, year) => {
                    const yearOrders = groupedOrders[year].filter(order => !order.isTotalRow);
                    yearOrders.sort((a, b) => {
                        if (a[columnKey] < b[columnKey]) return -1 * sortMultiplier;
                        if (a[columnKey] > b[columnKey]) return 1 * sortMultiplier;
                        return 0;
                    });

                    return [...sortedOrders, ...yearOrders];
                }, []);

            this.preparedOrdersData = this.calculateTotalsAndSpecialRows(
                this.groupOrdersByYear(this.preparedOrdersData)
            );
        },

        getCompanies() {
            axios
                .get(this.$backendAddress + "api/UserCompanyList/GetCompanies?userId=" + this.userCredentials.userId)
                .then((response) => {
                    this.availableCompanies = [
                        { companyID: 0, companyName: totalString },
                        ...response.data.map(company => ({
                            companyID: company.id,
                            companyName: company.name
                        }))
                    ];
                })
                .catch(() => {
                    Swal.fire('Error','Ocurrió un error al conseguir las empresas.','error');
                });
        },

        getYears() {
            axios
                .get(this.$backendAddress + "api/Orders/years")
                .then((response) => {
                    this.availableYears = [totalString, ...response.data];
                })
                .catch(() => {
                    Swal.fire('Error','Ocurrió un error al conseguir los años.','error');
                });
        },

        onSelectCompanies() {
            const isAllSelected = this.selectedCompanies.some(company => company.companyID === 0);
            
            if (isAllSelected) {
                this.selectedCompanies = this.availableCompanies.map(company => ({
                    companyID: company.companyID,
                    companyName: company.companyName
                }));
            }
        },

        onSelectYears() {
            if (this.selectedYears.includes(totalString)) {
                this.selectedYears = this.availableYears;
            }
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
                if(this.sliders.length < totalSliders){
                    maxValue = maxValue + 1;
                    const slider = sliderElement;
                    const sliderInstance=noUiSlider.create(slider, {
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

                    this.sliders.push(sliderInstance);}
            },
            convertToPdf() {
                const baseTable = document.getElementById("report");
                const tableHeight = baseTable.scrollHeight;
                const tableWidth = baseTable.scrollWidth;
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
    },

    mounted() {
        this.getCompanies();
        this.getYears();
    }
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
.thead-light {
background-color: #f8f9fa;
}
.col-10 {
height: calc(50vh - 100px);
}
.table thead th {
        text-align: left;
        background-color: #b97a3a;
        font-weight: bold;
        padding: 4px 8px;
    }
    .table thead th span{
        color: white;
    }
    .table-header {
        display: flex; 
        align-items: center;
        justify-content: space-between;
    }
  .th_button {
    width: auto;
    height: auto;
    padding-top: 2px;
    padding-bottom: 2px;
    padding-left: 6px;
    padding-right: 6px;
    margin: 0;
    border: none;
    background-color: #b97a3a; 
    color: white;
    text-align: center;
    font-weight: bold;
    cursor: pointer;
  }
  th {
    margin: 0;
  }
  thead input {
    width: 100%;
    box-sizing: border-box;
    margin-top: 5px;
    font-size: 0.85rem;
    padding: 3px;
}

</style>