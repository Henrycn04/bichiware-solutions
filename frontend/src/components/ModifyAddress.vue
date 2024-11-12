<template>
    <div style="height: 100vh;" class="bg-secondary">
      <div class="bg-primary pt-3 pb-3">
        <header class="header">
          <a href="/userProfile" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer; text-decoration: none; color: #332f2b">Feria del Emprendedor</a>
        </header>     
      </div>
      <div class="container-fluid bg-secondary py-5">
        <div class="container bg-light rounded-4 mb-3 pb-4">
          <div class="row bg-primary pt-3 rounded-top-4">
            <h2 class="display-6 text-center fw-bold ff-lspartan">Modificar Dirección</h2>
          </div>

          <form @submit.prevent="checkBeforeSubmit" class="mt-3 ff-lspartan">
            <div class="mb-3">
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <div class="form-text text-muted">* Campo Obligatorio</div>
                    <div>
                        <router-link to="/mapForAddress" class="map_button btn btn-primary px-4">
                        Mapa
                        </router-link>
                    </div>
                    </div>


                <label :class="{ 'errorInInputsLabel': provinceNameNotEmpty }" class="form-label">Provincia*</label>
                
                <select class="form-control rounded-2 border-0 bg-secondary mb-2" v-model="address.province">
                            <option>San José</option>
                            <option>Alajuela</option>
                            <option>Cartago</option>
                            <option>Heredia</option>
                            <option>Guanacaste</option>
                            <option>Puntarenas</option>
                            <option>Limón</option>
                </select>

            </div>
            <div class="mb-3">
              <label :class="{ 'errorInInputsLabel': cantonNameNotEmpty }" class="form-label">Cantón*</label>
              <input class="form-control rounded-2 border-0 bg-secondary" v-model="address.canton">
            </div>
            <div class="mb-3">
              <label :class="{ 'errorInInputsLabel': districtNameNotEmpty }" class="form-label">Distrito*</label>
              <input class="form-control rounded-2 border-0 bg-secondary" v-model="address.district">
            </div>
            <div class="mb-3">
              <label :class="{ 'errorInInputsLabel': exactAddressNameNotEmpty }" class="form-label">Dirección Exacta*</label>
              <input class="form-control rounded-2 border-0 bg-secondary" v-model="address.exact">
            </div>
            <div class="d-grid gap-2">
              <button class="btn fw-bold btn-primary ff-lspartan fs-5" type="submit">
                Guardar Cambios
              </button>
            </div>
          </form>
        </div>
      </div>
      <footer class="fixed-bottom text-center fs-7 p-2 fst-italic bg-ternary text-light ff-poppins">
        @Copyright BichiWare Solutions 2024
      </footer>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  import { mapGetters, mapActions } from 'vuex';
  
  export default {
    computed: {
        ...mapGetters(['isLoggedIn']), // Maps the getter isLoggedIn
        ...mapGetters(['getUserId']), // Maps getUserID
    },
    data() {
      return {
        address: {
            addressID: 0,
            province: "",
            canton: "",
            district: "",
            exact: "",
            latitude: "",
            longitude: "",
            userID: 0,
            companyID: 0,
            isCompany: false
        },

                provinceNameNotEmpty: false,
                cantonNameNotEmpty: false,
                districtNameNotEmpty: false,
                exactAddressNameNotEmpty: false,
                conditionInputs: true,
                ID: 0,
      };
    },
    mounted() {
        this.loadAddress();
        try
            {
                if (this.$store.getters.getPreviousPage.prev == window.location.origin + "/mapForAddress")
                {
                    this.loadData();
                    console.log("2:", this.address);
                }
            }
            catch (e)
            {
                console.log("Error:", e);
            }
            console.log("1:", this.address);
    },
    methods: {
        ...mapActions(['setPrevPage']),
        ...mapGetters(['getAddressId']),
        ...mapGetters(['getSavedAddress']),

      loadAddress() {
        const savedAddressID = this.getAddressId();
        if (savedAddressID) {
          this.address = { ...savedAddressID };
        }
      },
        loadData() {
            // As of Vue 3.0, the getter's result is not cached as the computed property does.
            // This is a known issue that requires Vue 3.1 to be released. You can learn more at PR #1878.
            // The getter is not called after it previously was called by this reason.
            var savedAddress = this.getSavedAddress();
            this.address.province = savedAddress.province;
            this.address.canton = savedAddress.canton;
            this.address.district = savedAddress.district;
            this.address.exact = savedAddress.exact;
            this.address.latitude = savedAddress.lat;
            this.address.longitude = savedAddress.lon;
            this.setPrevPage("");
        },
            checkBeforeSubmit() {
                this.conditionInputs = true;
                this.ID = -1
                this.getUserID();
                if(this.ID !== -1) {
                    this.checkProvince();
                    this.checkCanton();
                    this.checkDistrict();
                    this.checkAddress();
                    if (this.conditionInputs) this.updateAddress();
                } else this.alertError();
            },
            alertError() {
                window.alert("Error al accesar a cuenta\nAsegurese de haber hecho login");
                this.$router.push('/');
            },
            checkProvince() {
                this.provinceNameNotEmpty = this.address.province.trim() === '';
                if (this.provinceNameNotEmpty) {
                    this.conditionInputs = false;
                    console.log("Error in Province");
                }
            },
            checkCanton() {
                this.cantonNameNotEmpty = this.address.canton.trim() === '';
                if (this.cantonNameNotEmpty) {
                    this.conditionInputs = false;
                    console.log("Error in canton");
                }
            },
            checkDistrict() {
                this.districtNameNotEmpty = this.address.district.trim() === '';
                if (this.districtNameNotEmpty) {
                    this.conditionInputs = false;
                    console.log("Error in district");
                }
            },
            checkAddress() {
                this.exactAddressNameNotEmpty = this.address.exact.trim() === '';
                if (this.districtNameNotEmpty) {
                    this.conditionInputs = false;
                    console.log("Error in exactAddress");
                }
            },
      updateAddress() {
        axios.post(this.$backendAddress + "api/UpdateAddress/update", this.address)
          .then(() => {
            alert("Dirección actualizada correctamente.");
            if (this.address.isCompany) {
                this.$router.push("/companyProfile");
            } else {
                this.$router.push("/userProfile");
            }
          })
          .catch((error) => {
            console.log(error);
            alert("Error al actualizar la dirección.");
          });
      },
      getUserID() {
                if(this.isLoggedIn) this.ID = this.getUserId;
                else this.ID = -1;
            },
    },
  };
  </script>
  
  <style>
  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 5px 10px;
    background-color: #f07800;
    color: white;
    font-family: 'League Spartan', sans-serif;
    max-width: 100vw;
    box-sizing: border-box;
  }
  .errorInInputsLabel {
    color: red;
  }
  .map_button {
        font-size: 15px;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: black;
        border-radius: 20px;
        width: 150px;
        text-align: center;
        font-weight: bold;
        margin-left: auto;
    }
  </style>
  