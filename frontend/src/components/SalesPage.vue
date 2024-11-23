<template>
    <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
        <div v-if="userTypeNumber === 2" class="col-12 col-md-6 d-flex justify-content-center">
            <div class="w-100" style="max-width: 40%;"> <!-- Limitar el ancho al 30% -->
                <h5 for="companySelect" style="display: block; margin-top: 8px;">Seleccione su empresa</h5>
                <select  v-model="selectedCompany" @change="callQuery(selectedCompany)" id="companySelect" class="form-select">
                    <option v-for="company in userCompanies" :key="company.companyID" :value="company.companyID">
                        {{ company.companyName }}
                    </option>
                </select>
            </div>
        </div>
        <div class="container mt-4">
            <ApexChart 
                v-if="series.length && series[0].data.length" 
                type="bar" 
                height="350" 
                :options="chartOptions" 
                :series="series"
            ></ApexChart>
            <p v-else>No hay datos disponibles para mostrar.</p>

        </div>
    </div>
</template>

<script>
import commonMethods from '@/mixins/commonMethods';
import axios from "axios";
import { mapGetters, mapState } from 'vuex';
import VueApexCharts from "vue3-apexcharts";
export default {
    components: {
    ApexChart: VueApexCharts,
  },
mixins: [commonMethods],
computed: {
    ...mapGetters(['isLoggedIn']), 
    ...mapState(['userCredentials']),
},
data() {
    return {
        completedOrders: [],
        series: [],
        chartOptions: {
            chart: {
                height: 350,
                type: "bar",
            },
            plotOptions: {
                bar: {
                    columnWidth: "45%",
                    distributed: true,
                },
            },
            dataLabels: {
                enabled: false,
            },
            xaxis: {
                categories: [],
                labels: {
                    style: {
                        fontSize: "12px",
                    },
                },
                title: {
                    text: "Ganancia por mes", 
                    style: {
                        fontSize: "14px",
                        fontWeight: "bold",
                        color: "#333",
                    },
                },
            },
            yaxis: {
                title: {
                    text: "Ganancias en colones", 
                    style: {
                        fontSize: "14px",
                        fontWeight: "bold",
                        color: "#333",
                    },
                },
            },
        },
    };
},
methods: {
    callQuery(companyID) {
        console.log('Company ID seleccionado:', companyID);
        this.userId = this.userCredentials.userId; 
        const params = new URLSearchParams();
        params.append("UserID", this.userId);
        params.append("CompanyID", companyID);
        const url = `${this.$backendAddress}api/Admin_EntrepreneurDashboard/getLastYearEarnings?${params.toString()}`;
        this.getInitialOrders(url);
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
                    console.log(this.completedOrders);
                    this.processOrders();
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
            currency: 'USD'
        }).format(amount);
    },
    processOrders() {
      const totalsByMonth = {};
      this.completedOrders.forEach((order) => {
        const month = new Date(order.creationDate).toLocaleString("default", {
          month: "short",
        });
        if (!totalsByMonth[month]) {
          totalsByMonth[month] = 0;
        }
        totalsByMonth[month] += order.total;
      });

      this.series = [
        {
          data: Object.values(totalsByMonth),
        },
      ];
      this.chartOptions.xaxis.categories = Object.keys(totalsByMonth);
      console.log("La serie: ", this.series);
    },
},
mounted() {
    if (this.isLoggedInVar && this.userTypeNumber === 3) {
        this.userId = this.userCredentials.userId; 

        const params = new URLSearchParams();

        params.append("UserID", this.userId);

        const url = `${this.$backendAddress}api/Admin_EntrepreneurDashboard/getLastYearEarnings?${params.toString()}`;

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
