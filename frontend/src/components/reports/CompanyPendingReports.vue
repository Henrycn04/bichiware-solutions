<template>
  <div class="container-fluid bg-secondary py-5">
    <div class="container-fluid bg-light rounded-4 pb-4">
      <div class="row bg-primary pt-3 rounded-top-4">
        <h1 class="display-6 text-center fw-bold ff-lspartan">Reportes de Pedidos Pendientes</h1>
      </div>
      <form
        method="dialog"
        class="mt-3 ff-poppins"
        @submit="loadReport"
      >
      <label for="companyName" class="form-label mb-0">Compañía</label>
      <div class="d-flex gap-2">
        <select id="companyName" required
          class="form-select rounded-2 border-0 bg-secondary"
          v-model="companyName"
        >
          <option v-for="company in companies" v-bind:key="company">{{ company.name }}</option>
        </select>
        <input name="loadReport"
          class="btn btn-secondary ff-lspartan fs-5"
          type="submit"
          value="Cargar"
        >
      </div>
      </form>
      <hr class="hr">
      <div v-show="showInfoMessage" class="alert alert-light ff-poppins" role="alert" ref="informationalMessage"></div>
      <div v-show="showAppliedFilters" class="alert border-0 bg-secondary">
        <div v-for="(filter, index) in filters" v-bind:key="filter" class="d-flex py-1">
          <div class="flex-fill align-self-center">
            Filtro por {{ filter.columnName }}
            <span v-if="filter.isString"> relacionado a {{ filter.from }}</span>
            <span v-else> de {{ filter.from }} hasta {{ filter.to }}</span>
          </div>
          <button name="deleteFilter"
            class="btn btn-danger"
            @click="removeFilter(index)"
          >
            <svg width="20px" height="20px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M4 6H20M16 6L15.7294 5.18807C15.4671 4.40125 15.3359 4.00784 15.0927 3.71698C14.8779 3.46013 14.6021 3.26132 14.2905 3.13878C13.9376 3 13.523 3 12.6936 3H11.3064C10.477 3 10.0624 3 9.70951 3.13878C9.39792 3.26132 9.12208 3.46013 8.90729 3.71698C8.66405 4.00784 8.53292 4.40125 8.27064 5.18807L8 6M18 6V16.2C18 17.8802 18 18.7202 17.673 19.362C17.3854 19.9265 16.9265 20.3854 16.362 20.673C15.7202 21 14.8802 21 13.2 21H10.8C9.11984 21 8.27976 21 7.63803 20.673C7.07354 20.3854 6.6146 19.9265 6.32698 19.362C6 18.7202 6 17.8802 6 16.2V6M14 10V17M10 10V17" stroke="#FFFFFF" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
          </button>
        </div>
      </div>
      <div class="d-flex flex-row gap-2 ff-poppins">
        <div class="bg-secondary flex-fill rounded-2 p-2 d-flex gap-2">
          <svg class="align-self-center" width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M3 4.6C3 4.03995 3 3.75992 3.10899 3.54601C3.20487 3.35785 3.35785 3.20487 3.54601 3.10899C3.75992 3 4.03995 3 4.6 3H19.4C19.9601 3 20.2401 3 20.454 3.10899C20.6422 3.20487 20.7951 3.35785 20.891 3.54601C21 3.75992 21 4.03995 21 4.6V6.33726C21 6.58185 21 6.70414 20.9724 6.81923C20.9479 6.92127 20.9075 7.01881 20.8526 7.10828C20.7908 7.2092 20.7043 7.29568 20.5314 7.46863L14.4686 13.5314C14.2957 13.7043 14.2092 13.7908 14.1474 13.8917C14.0925 13.9812 14.0521 14.0787 14.0276 14.1808C14 14.2959 14 14.4182 14 14.6627V17L10 21V14.6627C10 14.4182 10 14.2959 9.97237 14.1808C9.94787 14.0787 9.90747 13.9812 9.85264 13.8917C9.7908 13.7908 9.70432 13.7043 9.53137 13.5314L3.46863 7.46863C3.29568 7.29568 3.2092 7.2092 3.14736 7.10828C3.09253 7.01881 3.05213 6.92127 3.02763 6.81923C3 6.70414 3 6.58185 3 6.33726V4.6Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>
          <form
            method="dialog"
            class="d-flex flex-row flex-fill gap-2"
            @submit="applyFilter"
          >
            <select id="filterColumn" required
              class="form-select rounded-2 border-0 bg-light"
              @change="onSelectColumn"
              v-model="filterColumn"
              :disabled="this.data.length <= 0"
            >
              <option disabled>Columna Por Filtrar</option>
              <option v-for="column in columnsEsp" v-bind:key="column">{{ column }}</option>
            </select>
            <div v-if="isNumericFilter"
              class="d-flex flex-fill gap-2"
            >
              <div class="align-self-center">
                De
              </div>
              <input name="valueFrom" required
                type="text"
                placeholder="Valor"
                class="form-control rounded-2 border-0 bg-light"
                v-model="valueFrom"
              >
              <div class="align-self-center">
                Hasta
              </div>
              <input name="valueTo" required
                type="text"
                placeholder="Valor"
                class="form-control rounded-2 border-0 bg-light"
                v-model="valueTo"
              >
            </div>
            <div v-if="isStringFilter"
              class="d-flex flex-fill gap-2"
            >
              <input name="value" required
                type="text"
                placeholder="Valor"
                class="form-control rounded-2 border-0 bg-light"
                v-model="valueFrom"
              >
            </div>
            <div v-if="isDateFilter"
              class="d-flex flex-fill gap-2"
            >
              <div class="align-self-center">De</div>
              <input name="valueFrom" required
                type="date"
                class="form-control rounded-2 border-0 bg-light"
                v-model="valueFrom"
              >
              <div class="align-self-center">Hasta</div>
              <input name="valueTo" required
                type="date"
                class="form-control rounded-2 border-0 bg-light"
                v-model="valueTo"
              >
            </div>
            <input name="clearFilter"
              class="btn btn-light flex-fill"
              type="button"
              value="Limpiar"
              @click="clearFilters"
            >
            <input name="applyFilter"
              class="btn btn-primary flex-fill"
              type="submit"
              value="Aplicar"
            >
          </form>
        </div>
        <div>
          <button class="btn btn-success" @click="convertToPdf" style="margin-top: 40px;" >Descargar PDF</button>
        </div>
      </div>
      <div class="p-2 overflow-auto" style="height: 300px;">
        <table class="table table-primary" id="report">
          <thead>
            <tr class="table-primary">
              <th scope="col" v-for="(column, index) in columnsEsp" :key="column">
                <button class="btn btn-primary fw-bold ff-lspartan " @click="applyOrderBy(index)">
                  <svg width="20px" height="20px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" clip-rule="evenodd" d="M7 5C7.55228 5 8 5.44772 8 6V15.5858L10.2929 13.2929C10.6834 12.9024 11.3166 12.9024 11.7071 13.2929C12.0976 13.6834 12.0976 14.3166 11.7071 14.7071L7.70711 18.7071C7.31658 19.0976 6.68342 19.0976 6.29289 18.7071L2.29289 14.7071C1.90237 14.3166 1.90237 13.6834 2.29289 13.2929C2.68342 12.9024 3.31658 12.9024 3.70711 13.2929L6 15.5858V6C6 5.44772 6.44772 5 7 5ZM16.2929 5.29289C16.6834 4.90237 17.3166 4.90237 17.7071 5.29289L21.7071 9.29289C22.0976 9.68342 22.0976 10.3166 21.7071 10.7071C21.3166 11.0976 20.6834 11.0976 20.2929 10.7071L18 8.41421V18C18 18.5523 17.5523 19 17 19C16.4477 19 16 18.5523 16 18V8.41421L13.7071 10.7071C13.3166 11.0976 12.6834 11.0976 12.2929 10.7071C11.9024 10.3166 11.9024 9.68342 12.2929 9.29289L16.2929 5.29289Z" fill="#000000"/>
                  </svg> {{ column }}</button>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr class="table-secondary"
              v-for="(order) in dataFiltered" :key="order"
            >
              <td scope="col" v-for="(column) in columns" :key="column">{{ order[column] }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mapGetters } from 'vuex';
import jsPDF from 'jspdf';
export default {
  data ()
  {
    return {
      reportType: "",
      companies : [],
      companyName: "",
      isAdmin: false,
      isEnterpreneur: false,
      showInfoMessage: false,
      showAppliedFilters: false,

      isNumericFilter: false,
      isStringFilter: false,
      isDateFilter: true,

      valueFrom: "",
      valueTo: "",

      filterColumn: "",
      filters: [],
      orderBy: "",
      columnsEsp: ["Número", "Empresas", "Cantidad", "Fecha de Creación", "Fecha de Envío", "Estado", "Subtotal", "Coste de Envío", "Total"],
      columns: [] /* A list of strings */,
      data: [] /* a list of dictionaries corresponding each column name to a value */,
      dataFiltered: []
    }
  },


  mounted()
  {
    var userType = this.getUserType();
    this.isAdmin = userType == 3;
    this.isEnterpreneur = userType == 2;
    
    if (this.isEnterpreneur || this.isAdmin)
    {
      this.getCompanies()
    }
    else
    {
      this.displayMessage("Usted no esta autorizado para ver esta pagina");
    }
  },


  methods:
  {
    ...mapGetters(["getUserType", "isLoggedIn", "getUserId"]),
    convertToPdf() {
      const baseTable = document.getElementById("report");
      const tableHeight = baseTable.offsetHeight * 2;
      const tableWidth = baseTable.offsetWidth;
      const reportTable = baseTable.cloneNode(true);

      const svgs = reportTable.querySelectorAll("svg");
      svgs.forEach(oneSvg => oneSvg.remove());

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
          doc.save(`CompanyPendingOrdersReport${timeStamp}.pdf`);
      });
    },
    applyFilter() {
      if (this.validateFilter())
      {
        this.filters.push({
          columnName: this.filterColumn,
          isString:   this.isStringFilter,
          isNumeric:  this.isNumericFilter,
          isDate:     this.isDateFilter,
          from:       this.valueFrom,
          to:         this.valueTo
        })
        this.showAppliedFilters = true;
        this.filterData();
      }
    },

    applyOrderBy(columnIndex) {
      this.orderBy = this.columns[columnIndex];
      this.orderData();
    },

    clearFilters() {
      this.showAppliedFilters = false;
      this.filters = [];
      if (this.data.length > 0)
      {
        this.dataFiltered = JSON.parse(JSON.stringify(this.data))
      }
    },

    validateFilter() {
      var isValid = this.validateSelectedReport();

      if (isValid && this.isNumericFilter)
      {
        isValid = this.validateValidNumber()
      }
      else if (isValid && this.isDateFilter)
      {
        isValid = this.validateValidDate()
      }

      return isValid && this.validateAlreadyPlacedFilter();
    },

    validateAlreadyPlacedFilter() {
      for (var i = 0; i < this.filters.length; ++i)
      {
        if (this.filters[i].columnName == this.filterColumn && !this.filters[i].isString)
        {
          this.displayMessage("El filtro que quiere añadir ya esta colocado. Debe limpiar y volver a ingresarlo.");
          return false;
        }
      }
      return true;
    },

    validateValidDate() {
      if (Date.parse(this.valueFrom) - Date.parse(this.valueTo) > 0)
      {
        this.displayMessage("El rango de fechas no es correcto. Verifique que lo esta ingresando correctamente.");
        return false;
      }
      return true;
    },

    validateValidNumber() {
      if (this.valueFrom - this.valueTo > 0)
      {
        this.displayMessage("El rango de numeros no es correcto. Verifique que lo esta ingresando correctamente.");
        return false;
      }
      return true;
    },

    validateSelectedReport() {
      if (this.companyName.length <= 0)
      {
        this.displayMessage("Debe cargar un reporte antes.")
        return false;
      }
      return true;
    },

    loadReport() {
      this.showAppliedFilters = false;
      this.filters = [];
      this.getReport();
      this.displayMessage(`Se cargó el reporte de pedidos pendientes para la compañía ${this.companyName}.`)
    },

    getReport() {
      var company = this.companies[this.companies.findIndex((c) => { return c.name === this.companyName })];
      axios.get(this.$backendAddress + "api/Reports/getReport/pendingOrders" + `?UserID=${this.getUserId()}&CompanyID=${company.id}`)
      .then((response) => {
        this.data = response.data;
        this.columns = Object.getOwnPropertyNames(this.data[0]);
        if (this.data.length > 0)
        {
          this.dataFiltered = JSON.parse(JSON.stringify(this.data))
        }
      }).catch((error) => { this.manageBackendError(error) })
    },

    filterData() {
      this.dataFiltered = JSON.parse(JSON.stringify(this.data))
      for (var i = 0; i < this.filters.length; ++i) {
        var indexColumn = this.columnsEsp.indexOf(this.filters[i].columnName)
        var visitedColumns = 0;
        for (var j = 0; j < this.data.length; ++j) {
          var order = this.dataFiltered[visitedColumns];
          if (order == undefined)
          {
            break;
          }
          console.log(order);
          var value = order[this.columns[indexColumn]];
          console.log(`isNum ${this.filters[i].isNumeric}. isStr ${this.filters[i].isString}. isDate ${this.filters[i].isDate}`)
          if (this.filters[i].isNumeric)
          {
            if (value > this.filters[i].to || value < this.filters[i].from)
            {
              this.dataFiltered.splice(this.dataFiltered.indexOf(order), 1)
            }
            else
            {
              visitedColumns++;
            }
          }
          else if (this.filters[i].isString)
          {
            if (!value.includes(this.filters[i].from))
            {
              this.dataFiltered.splice(this.dataFiltered.indexOf(order), 1)
            }
            else
            {
              visitedColumns++;
            }
          }
          else
          {
            if (Date.parse(value) > Date.parse(this.filters[i].to)
             || Date.parse(value) < Date.parse(this.filters[i].from))
            {
              this.dataFiltered.splice(this.dataFiltered.indexOf(order), 1)
            }
            else
            {
              visitedColumns++;
            }
          }
        }
      }
    },
    
    orderData() {
      var pageData = this;
      this.dataFiltered.sort((a ,b) => {
        var isNumeric = !isNaN(a[pageData.orderBy]);
        if (!isNumeric && !isNaN(Date.parse(a[pageData.orderBy])))
        {
          let astr = Date.parse(a[pageData.orderBy]);
          let bstr = Date.parse(b[pageData.orderBy]);
          if (astr > bstr) {
            return 1;
          } else if (astr < bstr) {
            return -1;
          }
          return 0;
        }
        else if (!isNumeric)
        {
          let astr = a[pageData.orderBy].toUpperCase();
          let bstr = b[pageData.orderBy].toUpperCase();
          if (astr > bstr) {
            return 1;
          } else if (astr < bstr) {
            return -1;
          }
          return 0;
        }
        else
        {
          if (a[pageData.orderBy] > b[pageData.orderBy]) {
            return 1;
          } else if (a[pageData.orderBy] < b[pageData.orderBy]) {
            return -1;
          }
          return 0;
        }
      });
    },

    getCompanies() {
      axios.get(this.$backendAddress + "api/UserCompanyList/GetCompanies?userId=" + this.getUserId())
        .then((response) => {
          this.companies = response.data;
        }).catch((error) => { this.manageBackendError(error) });
    },
    
    removeFilter(indexColumn) {
      this.filters.splice(indexColumn, 1);
      this.filterData();

      if (this.filters.length <= 0)
      {
        this.showAppliedFilters = false;
      }
    },

    accountClicked() {
      if (this.isLoggedIn())
      {
        window.location.href = "/userProfile";
      }
      else
      {
        
        window.location.href = "/login"
      }
    },

    displayMessage(message) {
      if (this.$refs.informationalMessage != null)
      {
        this.$refs.informationalMessage.innerHTML = message;
        this.showInfoMessage = true;
      }
    },

    onSelectColumn() {
      var value = this.data[0][this.columns[this.columnsEsp.indexOf(this.filterColumn)]]
      this.isNumericFilter = !isNaN(value)
      if (!this.isNumericFilter && !isNaN(Date.parse(value)))
      {
        this.isDateFilter = true;
        this.isStringFilter = false;
      }
      else if (!this.isNumericFilter)
      {
        this.isDateFilter = false;
        this.isStringFilter = true;
      }
      else
      {
        this.isDateFilter = false;
        this.isStringFilter = false;
      }
      this.valueFrom = "";
      this.valueTo = "";
    },

    manageBackendError(error) {
      var errorMsg = "";
      var errorStatus = 0;
      if (error.response == undefined) {
        errorMsg = "No hay conexión con el servidor.";
        errorStatus = 408;
      } else if (error.response.data) {
        errorMsg = error.response.data;
        errorStatus = error.response.status;
      } else if (error.response.data.title) {
        errorMsg = error.response.data.title;
        errorStatus = error.response.status;
      } else if (error.request) {
        errorMsg = error.message;
        errorStatus = error.response.status;
      }
      
      console.log("ERROR " + errorStatus + " ----> " + errorMsg);
      this.displayMessage("Ocurrió un error. " + errorMsg)
    }
  }
}
</script>

<style lang="scss">
</style>
