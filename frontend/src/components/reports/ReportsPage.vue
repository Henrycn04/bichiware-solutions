<template>
  <div style=" height: 100vh;" class="bg-secondary">
    <nav class="navbar navbar-expand-lg bg-primary">
      <div class="container-fluid">
        <a
          href="/"
          class="navbar-brand fw-bold ff-lspartan float-start"
        >Feria del Emprendedor</a>
        <input
          class="form-control me-2"
          type="search"
          placeholder="Buscar"
          aria-label="Buscar"
        >
        <button
          class="btn btn-ternary btn-secondary"
          type="submit"
        >
        <svg fill="#000000" height="23px" width="23px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 183.792 183.792" xml:space="preserve">
          <path d="M54.734,9.053C39.12,18.067,27.95,32.624,23.284,50.039c-4.667,17.415-2.271,35.606,6.743,51.22  c12.023,20.823,34.441,33.759,58.508,33.759c7.599,0,15.139-1.308,22.287-3.818l30.364,52.592l21.65-12.5l-30.359-52.583  c10.255-8.774,17.638-20.411,21.207-33.73c4.666-17.415,2.27-35.605-6.744-51.22C134.918,12.936,112.499,0,88.433,0  C76.645,0,64.992,3.13,54.734,9.053z M125.29,46.259c5.676,9.831,7.184,21.285,4.246,32.25c-2.938,10.965-9.971,20.13-19.802,25.806  c-6.462,3.731-13.793,5.703-21.199,5.703c-15.163,0-29.286-8.146-36.857-21.259c-5.676-9.831-7.184-21.284-4.245-32.25  c2.938-10.965,9.971-20.13,19.802-25.807C73.696,26.972,81.027,25,88.433,25C103.597,25,117.719,33.146,125.29,46.259z"/>
        </svg>
        </button>
        <div class="navbar-collapse collapse ">
          <ul class="navbar-nav ">
            <li class="nav-item">
              <a class="nav-link" @click="accountClicked">
                <div class="d-flex my-3 ff-lspartan fw-bold">
                  <svg class="me-1" width="23px" height="23px" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M8 7C9.65685 7 11 5.65685 11 4C11 2.34315 9.65685 1 8 1C6.34315 1 5 2.34315 5 4C5 5.65685 6.34315 7 8 7Z" fill="#000000"/>
                    <path d="M14 12C14 10.3431 12.6569 9 11 9H5C3.34315 9 2 10.3431 2 12V15H14V12Z" fill="#000000"/>
                  </svg>{{ isLoggedIn() ? "Cuenta" : "Acceder" }}
                </div>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="/cart">
                <div class="d-flex my-3">
                  <svg class="me-1" width="23px" height="23px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M6.29977 5H21L19 12H7.37671M20 16H8L6 3H3M9 20C9 20.5523 8.55228 21 8 21C7.44772 21 7 20.5523 7 20C7 19.4477 7.44772 19 8 19C8.55228 19 9 19.4477 9 20ZM20 20C20 20.5523 19.5523 21 19 21C18.4477 21 18 20.5523 18 20C18 19.4477 18.4477 19 19 19C19.5523 19 20 19.4477 20 20Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                  </svg>
                </div>
              </a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
    <nav
      style="filter: brightness(85%);
             height: 40px;" 
      class="navbar navbar-expand-lg bg-primary"
    >
      <div class="container-fluid">
        <ul class="navbar-nav ff-lspartan fw-bold">
          <li class="nav-item">
            <a class="nav-link" href="/all-products">
              <img
                src="../../assets/AllIcon.png"
                alt="AllProducts"
                class="me-2"
                width=20
                height=20
              />
              Todos los Productos
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/non-perishable-products">
              No perecederos
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/perishable-products">
              Perecederos
            </a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdmin || this.isEnterpreneur" class="nav-link"
              href="/users-list">Lista de usuarios</a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdmin || this.isEnterpreneur" class="nav-link"
              href="/companies-list">Lista de empresas</a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div class="container-fluid bg-light rounded-4 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Reportes</h1>
        </div>
        <form
          method="dialog"
          class="mt-3 ff-lspartan"
          @submit="loadReport"
        >
          <label for="reportType" class="form-label mb-0">Tipo de Reporte</label>
          <div class="d-flex gap-2">
            <select id="reportType" required
              class="form-select rounded-2 border-0 bg-secondary"
              v-model="reportChoosen"
            >
              <option v-for="report in reports" v-bind:key="report">{{ report }}</option>
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
          <div v-for="filter in filters" v-bind:key="filter" class="d-flex py-1">
            <div class="flex-fill align-self-center">
              Filtro por {{ filter.columnName }}
              <span v-if="filter.isString"> relacionado a {{ filter.value }}</span>
              <span v-else> de {{ filter.from }} hasta {{ filter.to }}</span>
            </div>
            <button name="deleteFilter"
              class="btn btn-danger"
              @click="removeFilter(filter.name)"
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
              >
                <option disabled>Columna Por Filtrar</option>
                <option v-for="column in columns" v-bind:key="column">{{ column }}</option>
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
          <div class="d-flex">
            <button class="btn btn-success text-align-center" @click="downloadPdf">Descargar PDF</button>
          </div>
        </div>
        <div class="p-2 overflow-auto" style="height: 300px;">
          <table class="table table-primary">
            <thead>
              <tr class="table-primary">
                <th scope="col" v-for="(column) in columns" :key="column">
                  <button class="btn btn-primary fw-bold ff-lspartan " @click="applyOrderBy(column)">
                    <svg width="20px" height="20px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path fill-rule="evenodd" clip-rule="evenodd" d="M7 5C7.55228 5 8 5.44772 8 6V15.5858L10.2929 13.2929C10.6834 12.9024 11.3166 12.9024 11.7071 13.2929C12.0976 13.6834 12.0976 14.3166 11.7071 14.7071L7.70711 18.7071C7.31658 19.0976 6.68342 19.0976 6.29289 18.7071L2.29289 14.7071C1.90237 14.3166 1.90237 13.6834 2.29289 13.2929C2.68342 12.9024 3.31658 12.9024 3.70711 13.2929L6 15.5858V6C6 5.44772 6.44772 5 7 5ZM16.2929 5.29289C16.6834 4.90237 17.3166 4.90237 17.7071 5.29289L21.7071 9.29289C22.0976 9.68342 22.0976 10.3166 21.7071 10.7071C21.3166 11.0976 20.6834 11.0976 20.2929 10.7071L18 8.41421V18C18 18.5523 17.5523 19 17 19C16.4477 19 16 18.5523 16 18V8.41421L13.7071 10.7071C13.3166 11.0976 12.6834 11.0976 12.2929 10.7071C11.9024 10.3166 11.9024 9.68342 12.2929 9.29289L16.2929 5.29289Z" fill="#000000"/>
                    </svg> {{ column }}</button>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr class="table-secondary"
                v-for="(order) in data" :key="order"
              >
                <td scope="col" v-for="(column) in columns" :key="column">{{ order[column] }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
      @Copyright BichiWare Solutions 2024
    </footer>
  </div>
</template>

<script>
// import axios from "axios";
import { mapGetters } from 'vuex';

export default {
  data ()
  {
    return {
      reportType: "",
      isAdmin: false,
      isEnterpreneur: false,
      showInfoMessage: false,
      showAppliedFilters: false,

      isNumericFilter: false,
      isStringFilter: false,
      isDateFilter: true,

      valueFrom: "",
      valueTo: "",

      reports: ["Reporte de Pedidos Pendientes (Negocio)",
                "Reporte de Pedidos Cancelados (Negocio)",
                "Reporte de Pedidos Completados (Negocio)",
                "Ganancias Totales",
                "Reporte de Pedidos Pendientes (Cliente)",
                "Reporte de Pedidos Cancelados (Cliente)",
                "Reporte de Pedidos Completados (Cliente)",
               ],
      reportChoosen: "",
      filterColumn: "",
      filters: [],
      orderBy: "",
      columns: ["Name", "Numero", "Fecha"] /* A list of strings */,
      data: [
              {
                "Name": "Daniel",
                "Numero": "2",
                "Fecha": "2023-12-22"
              },
              {
                "Name": "Roger",
                "Numero": "3",
                "Fecha": "2000-11-09"
              },
              {
                "Name": "Kevin",
                "Numero": "4",
                "Fecha": "2023-01-01"
              }
            ] /* a list of dictionaries corresponding each column name to a value */
    }
  },


  mounted()
  {
    var userType = this.getUserType();
    this.isAdmin = userType == 3;
    this.isEnterpreneur = userType == 2;    
  },


  methods:
  {
    ...mapGetters(["getUserType", "isLoggedIn"]),
   
    applyFilter() {
      if (this.validateFilter())
      {
        if (this.isStringFilter)
        {
          this.filters.push({
            columnName:  this.filterColumn,
            isString:    this.isStringFilter,
            value:       this.valueFrom,
          })
        }
        else
        {
          this.filters.push({
            columnName: this.filterColumn,
            isString:   this.isStringFilter,
            from:       this.valueFrom,
            to:         this.valueTo
          })
        }
        this.showAppliedFilters = true;
        this.getReportWithFilters();
      }
    },

    applyOrderBy(columnName) {
      this.orderBy = columnName;
      if (this.filters.length > 0)
      {
        this.getReportWithFilters();
      }
    },

    clearFilters() {
      this.showAppliedFilters = false;
      this.filters = [];
      this.getReport();
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
        if (this.filters[i].columnName == this.filterColumn)
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
      if (this.reportChoosen.length <= 0)
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

      this.displayMessage(`Se cargó el ${this.reportChoosen}`)
    },

    // TODO: This is for the conection between backend
    getReport() {

    },

    // TODO: This is for the conection between backend
    getReportWithFilters() {

    },
    
    downloadPdf() {
      if (this.validateSelectedReport())
      {
        this.displayMessage("Ocurrió un error. Esta funcion aun no esta implementada.");
      }
    },

    removeFilter(columnName) {
      this.filters.splice(this.filters.indexOf(columnName), 1);
      this.getReportWithFilters();

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
      this.$refs.informationalMessage.innerHTML = message;
      this.showInfoMessage = true;
    },

    onSelectColumn() {
      var value = this.data[1][this.filterColumn]
      this.isNumericFilter = !isNaN(value)
      const dateRegex = /^(\d{4})-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$/;
      if (!this.isNumericFilter && dateRegex.test(value))
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
    }
  }
}
</script>

<style lang="scss">
</style>
