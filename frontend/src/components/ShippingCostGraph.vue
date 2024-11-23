<template>
    <ApexChart v-if="series.length && series[0].data.length" type="bar" height="350" :options="chartOptions" :series="series">
    </ApexChart>
</template>

<script>
import commonMethods from '@/mixins/commonMethods';
import axios from 'axios';
import VueApexCharts from 'vue3-apexcharts';
import { mapGetters, mapState } from 'vuex';

export default {
  mixins: [commonMethods],
  computed: {
    ...mapGetters(['isLoggedIn']),
    ...mapState(['userCredentials']),
  },
  components: {
    ApexChart: VueApexCharts,
  },
  data() {
    return {
      shippingCosts: null,
      series: [],
      categories: [],
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
                    text: "Mes-Año", 
                    style: {
                        fontSize: "14px",
                        fontWeight: "bold",
                        color: "#333",
                    },
                },
            },
            yaxis: {
                title: {
                    text: "Costos en colones", 
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
    getMonthlyShippingCosts(startDate, endDate) {
      axios.get(this.$backendAddress + 'api/Admin_EntrepreneurDashboard/GetMonthlyShippingCost?startDate=' + startDate + '&endDate=' + endDate)
        .then((response) => {
          if (typeof response.data === 'string') {
            this.shippingCosts = null;
          } else {
            this.shippingCosts = response.data
            this.createSeries();
          }
        })
        .catch((error) => {
          console.error('Error on obtaining shipping costs: ' + error);
        });
    },
    createSeries() {
        const seriesData = [];
        const categoriesData = [];
        this.shippingCosts.forEach((monthlyData) => {
            seriesData.push(monthlyData.cost);
            categoriesData.push(`${monthlyData.month}-${monthlyData.year}`); // Properly formatted category
        });

      this.series = [
        {
          name: 'Costo Mensual',
          data: seriesData,
        },
      ];
      this.chartOptions.xaxis.categories = categoriesData;
    },
  },
  mounted() {
    if (this.isLoggedIn && this.userTypeNumber === 3) {
      this.getMonthlyShippingCosts('1753-01-01', '9999-12-31');
    }
  },
};
</script>


<style>

</style>