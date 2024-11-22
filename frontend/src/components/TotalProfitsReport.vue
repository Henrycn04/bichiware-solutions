<template>
    <div v-if="isLoggedInVar && (userTypeNumber === 2 || userTypeNumber === 3)" class="logged-in-section">
      
      </div>
      <div class="container mt-4">
        <h2 class="mb-4">Reporte de Ganancias Totales</h2>
        <div class="filters mb-4 w-100">
          <div class="row g-3">
            <div class="col-12 col-md-3">
              <label for="allCompanies">Empresas:</label>
              <select multiple class="form-control" id="companies" v-model="selectedCompanies">
                <option v-for="company in allCompanies" :key="company.id" :value="company.id">
                  {{ company.name }}
                </option>
              </select>
            </div>
            <div class="col-12 col-md-3">
              <label for="years">Años:</label>
              <select multiple class="form-control" id="years" v-model="selectedYears">
                <option v-for="year in allYears" :key="year" :value="year">{{ year }}</option>
              </select>
            </div>
          </div>
     
        <table>
          <thead>
            <tr>
              <th @click="sortTable('companyName')">Emprendimiento</th>
              <th @click="sortTable('month')">Mes</th>
              <th @click="sortTable('totalPurchase')">Total de compra</th>
              <th @click="sortTable('totalShipping')">Total de envío</th>
              <th @click="sortTable('totalCost')">Costo total de la compra</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(profit, index) in sortedProfits" :key="index">
              <td>{{ profit.companyName }}</td>
              <td>{{ profit.month }}</td>
              <td>{{ formatCurrency(profit.totalPurchase) }}</td>
              <td>{{ formatCurrency(profit.totalShipping) }}</td>
              <td>{{ formatCurrency(profit.totalCost) }}</td>
            </tr>
            <tr v-if="sortedProfits.length > 0">
              <td colspan="2">Total por Año</td>
              <td>{{ formatCurrency(yearlyTotal.totalPurchase) }}</td>
              <td>{{ formatCurrency(yearlyTotal.totalShipping) }}</td>
              <td>{{ formatCurrency(yearlyTotal.totalCost) }}</td>
            </tr>
          </tbody>
        </table>
        <div>
          <button class="btn btn-success" @click="convertToPdf" style="margin-top: 40px;">Descargar PDF</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import jsPDF from 'jspdf';
  
  export default {
    data() {
      return {
        selectedCompanies: [],
        selectedYears: [],
        allCompanies: [
          { id: 1, name: 'Empresa 1' },
          { id: 2, name: 'Empresa 2' },
          { id: 3, name: 'Empresa 3' }
        ],
        allYears: [2020, 2021, 2022],
        profits: [
          { companyName: 'Empresa 1', month: 'Enero', totalPurchase: 100000, totalShipping: 5000, totalCost: 120000, companyId: 1, year: 2021 },
          { companyName: 'Empresa 2', month: 'Febrero', totalPurchase: 200000, totalShipping: 7000, totalCost: 230000, companyId: 2, year: 2021 },
          { companyName: 'Empresa 3', month: 'Marzo', totalPurchase: 150000, totalShipping: 6000, totalCost: 180000, companyId: 3, year: 2022 },
          { companyName: 'Empresa 1', month: 'Abril', totalPurchase: 120000, totalShipping: 5500, totalCost: 135000, companyId: 1, year: 2022 }
        ],
        sortedProfits: [],
        yearlyTotal: {
          totalPurchase: 0,
          totalShipping: 0,
          totalCost: 0
        }
      };
    },
    computed: {
      filteredProfits() {
        return this.profits.filter(profit => {
          return (
            (this.selectedCompanies.length === 0 || this.selectedCompanies.includes(profit.companyId)) &&
            (this.selectedYears.length === 0 || this.selectedYears.includes(profit.year))
          );
        });
      }
    },
    methods: {
      formatCurrency(amount) {
        return new Intl.NumberFormat('es-ES', { style: 'currency', currency: 'CRC' }).format(amount);
      },
      fetchData() {
        // Simulating a data fetch from an API
        setTimeout(() => {
          // Simulando que los datos ya fueron cargados
          this.sortedProfits = this.filteredProfits;
          this.calculateYearlyTotals();
        }, 1000);
      },
      sortTable(columnName) {
        const sorted = [...this.filteredProfits];
        sorted.sort((a, b) => {
          if (a[columnName] < b[columnName]) return -1;
          if (a[columnName] > b[columnName]) return 1;
          return 0;
        });
        this.sortedProfits = sorted;
        this.calculateYearlyTotals();
      },
      calculateYearlyTotals() {
        this.yearlyTotal = this.filteredProfits.reduce(
          (totals, profit) => {
            totals.totalPurchase += profit.totalPurchase;
            totals.totalShipping += profit.totalShipping;
            totals.totalCost += profit.totalCost;
            return totals;
          },
          { totalPurchase: 0, totalShipping: 0, totalCost: 0 }
        );
      },
      convertToPdf() {
        const reportTable = document.querySelector("table");
        const doc = new jsPDF();
        doc.html(reportTable, {
          callback: function (doc) {
            doc.save("totalProfitsReport.pdf");
          }
        });
      }
    },
    mounted() {
      // Aquí cargamos los datos de ejemplo directamente
      this.fetchData();
    }
  };
  </script>
  
  <style scoped>
  .table thead th {
    text-align: left;
    background-color: #b97a3a;
    font-weight: bold;
    padding: 4px 8px;
  }
  .table thead th span {
    color: white;
  }
  </style>