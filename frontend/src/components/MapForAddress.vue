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
                  </svg>{{ isUserLoggedIn() ? "Cuenta" : "Acceder" }}
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
                src="../assets/AllIcon.png"
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
            <a v-if="this.isAdminOrEntrepreneur"
              href="/users-list">Lista de usuarios</a>
          </li>
          <li class="nav-item">
            <a v-if="this.isAdminOrEntrepreneur"
              href="/companies-list">Lista de empresas</a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="container-fluid bg-secondary py-5">
      <div class="container bg-light rounded-4 mb-3 pb-4">
        <div class="row bg-primary pt-3 rounded-top-4">
          <h1 class="display-6 text-center fw-bold ff-lspartan">Añade una dirección con el Mapa Interactivo</h1>
        </div>
        <div>
          <div style="height: 500px" class="p-4">
            <l-map ref="map" :options="mapOptions" :zoom="zoom" :maxZoom="maxZoom" :center="center" @update:zoom="zoomUpdate">
              <l-tile-layer :url="url" :attribution="attribution"></l-tile-layer>
              <l-marker :lat-lng="markerLatLng"></l-marker>
            </l-map>
          </div>
          <div class="px-4">
            <div>
              <form action="">
                <table class="table">
                  <thead class="table-primary">
                    <tr>
                      <th>Provincia: </th>
                      <th>Cantón: </th>
                      <th>Distrito: </th>
                      <th>Dirección Exacta: </th>
                    </tr>
                  </thead>
                  <tbody class="table-secondary">
                    <tr>
                      <td>{{ this.province }}</td>
                      <td>{{ this.canton }}</td>
                      <td>{{ this.district }}</td>
                      <td>
                        <input type="text" v-model="this.exact" class="form-control rounded-2 border-0 bg-secondary">
                      </td>
                    </tr>
                  </tbody>
                </table>
                <div class="text-end">
                  <input type="button" value="Borrar" class="btn btn-secondary ff-lspartan fs-5" @click="resetAddress">
                  <input type="button" value="Cancelar" class="btn btn-secondary ff-lspartan fs-5 mx-3" @click="goBack">
                  <input type="button" value="Aceptar" class="btn fw-bold btn-primary ff-lspartan fs-5" @click="submitAddress">
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
    <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
      @Copyright BichiWare Solutions 2024
    </footer>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import { LMap, LTileLayer, LMarker } from 'vue3-leaflet';
import axios from 'axios'
import 'leaflet'

const NOMINATIM_API = "https://nominatim.openstreetmap.org/reverse?format=jsonv2";

export default {
  components: {
    LMap,
    LTileLayer,
    LMarker
  },

	setup () {
    return {}
  },

  data()
  {
    return {
      isAdminOrEntrepreneur: false,
      map : null,
      province: "Provincia",
      canton: "Cantón",
      district: "Distrito",
      exact: "Dirección Exacta",
      url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
      attribution:
        '&copy; <a target="_blank" href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      zoom: 15,
      maxZoom: 18,
      center: [9.93271, -84.07966],
      markerLatLng: [9.93271, -84.07966],
      mapOptions: {
        zoomControl: true,
        attributionControl: true
      },
    }
  },

  methods:
  {
    ...mapGetters(["getUserType", "isLoggedIn"]),
    ...mapActions(["saveAddress", "setPrevPage"]),

    isUserLoggedIn() {
      try
      {
        return this.isLoggedIn() ? "Cuenta" : "Acceder";
      }
      catch
      {
        return "Acceder";
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

    zoomUpdate(zoom) {
      this.zoom = zoom;
    },

    onMapClick(event) {
      try
      {
        this.markerLatLng = event.latlng
        this.requestAddressToNominatim(event.latlng.lat, event.latlng.lng)
      }
      catch(e)
      {
        console.log(e);
      }
    },

    resetAddress() {
      this.province = "Provincia";
      this.canton = "Cantón";
      this.district = "Distrito";
      this.exact = "Dirección Exacta";
    },

    submitAddress() {
      if (this.province == "Provincia") {
        return;
      }
      this.saveAddress({
        "province": this.province,
        "canton": this.canton,
        "district": this.district,
        "exact": this.exact,
        "lat": this.markerLatLng.lat,
        "lon": this.markerLatLng.lng,
      })
      
      this.setPrevPage({ prev: window.location.href })
      window.history.back();
    },

    requestAddressToNominatim(lat, lon) {
      var url = NOMINATIM_API + "&lat=" + lat + "&lon=" + lon

      if (lat == undefined || lon == undefined)
      {
        throw new Error('Cannot get information because the coordinates were not provided');
      }

      axios.get(url).then((response) =>
      {
        var address = response.data.address;
        console.log(address);

        this.province = address.province != undefined ? address.province : address.state;
        if (this.province == undefined)
        {
          // The shitty site for getting this information for some reason doesn't detects the province of Limon.
          this.province = "Limón";
        }
        this.formatProvince();

        this.canton = address.county;
        this.formatCounty();

        this.district = address.village != undefined ? address.village :
          address.town != undefined ? address.town : address.city;

        // This thing is a pain.
        this.exact = "";
        this.exact += address.amenity != undefined ? (address.amenity + " ") : "" ;
        this.exact += address.office != undefined ? (address.office + " ") : "" ;
        this.exact += address.residential != undefined ? (address.residential + " ") : "";
        this.exact += address.neighbourhood != undefined ? (address.neighbourhood + " ") : "";
        this.exact += address.road != undefined ? (address.road + " ") : "";
        this.exact += address.farm != undefined ? (address.farm + " ") : "";
        this.exact += address.postcode;
      })
      .catch((error) =>
      {
        var errorMsg = "";
        if (error.response)
        {
          errorMsg = "Axios error with get RESPONSE to Nominatim " + error.response.status;
          throw new Error(errorMsg);
        }
        else if (error.request)
        {
          errorMsg = "Axios error with get REQUEST to Nominatim ";
          console.log(error.request);
          throw new Error(errorMsg);
        }
        else
        {
          errorMsg = "Other axios error";
          throw new Error(errorMsg);
        }
      });
    },

    formatCounty() {
      this.canton = this.canton.replace('Cantón de ','');
    },

    formatProvince() {
      this.province = this.province.replace(' Province', '');
    },
  },

  mounted() {
    // Because the map is not available immediately
    this.$nextTick(() => {
      this.$refs.map.on('click', this.onMapClick);

      
      var userType = this.getUserType();
      this.isAdminOrEntrepreneur = userType == 1 || userType == 2;  
    })
  },
}
</script>

<style lang="scss" scoped></style>
