<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="forms_background">
            <!--Previene que el forms se envie hasta que la condicion sea verdadera-->
            <!--En este caso espera a que todos los inputs obligatorios est?n llenos y a que se ingresen datos correctos-->
            <form @submit.prevent="checkBeforeSubmit">
                <h2 class="forms_header">Registro de empresa</h2>
                <div class="form_content_padding">
                    <div class="for_required_text">* Campo obligatorio</div>
                    <div>
                        <!--v-base: muestra un estilo (invoca una clase) solo si una variable es true-->
                        <!--formato: {'claseCSS': variableCondicional}-->
                        <label :class="{ 'errorInInputsLabel': companyNameNotEmpty}">Nombre de la empresa: *</label><br>
                        <!--V-model: asocia el input con una variable, entonces en esa variable queda almacenado lo que se escriba en ?l-->
                        <input v-model="formData.companyName" :class="{ 'errorInInputsInput': companyNameNotEmpty}">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': cedulaNotEmpty}">Cedula juridica: *</label><br>
                        <!--pattern es para aceptar solo inputs de acuerdo con una regex-->
                        <!--Si el texto no calza con la regez sale un warning-->
                        <!--ref es para referenciar una etiqueta desde javascript-->
                        <input v-model="formData.cedula" :class="{ 'errorInInputsInput': cedulaNotEmpty}" maxlength="10" pattern="\d{10}" ref="ced" placeholder="  Formato: 0123456789">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': emailAddressNotEmpty}">Correo electronico: *</label><br>
                        <input type="email" :class="{ 'errorInInputsInput': emailAddressNotEmpty}" v-model="formData.emailAddress" placeholder="  Formato: usuario@gmail.com" ref="email">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': phoneNumberNotEmpty}">Numero de telefono: *</label><br>
                        <input v-model="formData.phoneNumber" :class="{ 'errorInInputsInput': phoneNumberNotEmpty}" maxlength="8" pattern="\d{8}" ref="phoneNumb" placeholder="  Formato: 12345678">
                    </div>
                    <div class="address_input_button_container">
                        <label :class="{ 'errorInInputsLabel': addressNotEmpty}">Direccion: *</label>
                        <router-link to="/mapForAddress" class="map_button" style="display: none">Mapa</router-link>
                    </div>
                    <div class="address_container">
                        <label>Provincia</label>
                        <label>Canton</label>
                        <label>Distrito</label>
                        <select class="input_for_address" v-model="formData.provincia">
                            <option>San Jose</option>
                            <option>Alajuela</option>
                            <option>Cartago</option>
                            <option>Heredia</option>
                            <option>Guanacaste</option>
                            <option>Puntarenas</option>
                            <option>Limon</option>
                        </select>
                        <input class="input_for_address" v-model="formData.canton">
                        <input class="input_for_address" v-model="formData.distrito">
                    </div>
                    <div>
                        <label>Direccion exacta:</label><br>
                        <input v-model="formData.exactAddress">
                    </div><br>
                    <button type="submit">Registrar empresa</button>
                </div>
            </form>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import axios from "axios"
    import { useRouter } from 'vue-router';
    import { mapActions } from 'vuex';
    export default {
        data() {
            return {
                formData: {
                    companyName: "",
                    cedula: "",
                    emailAddress: "",
                    phoneNumber: "",
                    provincia: "",
                    canton: "",
                    distrito: "",
                    exactAddress: ""
                },
                companyNameNotEmpty: false,
                cedulaNotEmpty: false,
                emailAddressNotEmpty: false,
                phoneNumberNotEmpty: false,
                addressNotEmpty: false,
                conditionInputs: true,
                companyID: 0
            };
        },
        setup() {
            const router = useRouter();
            return { router };
        },
        methods: {
            ...mapActions(['openCompany']),
            async checkBeforeSubmit() {
                this.validateCompanyName();
                this.validateCedula();
                this.validateEmail();
                this.validatePhoneNumber();
                this.validateAddress();

                if (this.conditionInputs) {
                    alert('Form submitted.');
                    await this.saveCompany();
                    this.resetFormData();
                    this.openCompany(this.companyID);
                    this.router.push('/companyProfile'); 
                }
            },
            validateCompanyName() {
                this.companyNameNotEmpty = this.formData.companyName.trim() === '';
                if (this.companyNameNotEmpty) this.conditionInputs = false;
            },
            validateCedula() {
                const ced = this.$refs.ced;
                this.cedulaNotEmpty = this.formData.cedula.trim() === '' || !ced.validity.valid;
                if (this.cedulaNotEmpty) this.conditionInputs = false;
            },
            validateEmail() {
                const email = this.$refs.email;
                this.emailAddressNotEmpty = this.formData.emailAddress.trim() === '' || !email.validity.valid;
                if (this.emailAddressNotEmpty) this.conditionInputs = false;
            },
            validatePhoneNumber() {
                const phoneNum = this.$refs.phoneNumb;
                this.phoneNumberNotEmpty = this.formData.phoneNumber.trim() === '' || !phoneNum.validity.valid;
                if (this.phoneNumberNotEmpty) this.conditionInputs = false;
            },
            validateAddress() {
                const { provincia, canton, distrito, exactAddress } = this.formData;
                // el .some() revisa un array y retorna true/false si se cumple una condición para cualquier elementos del array
                this.addressNotEmpty = [provincia, canton, distrito, exactAddress].some(field => field.trim() === '');
                if (this.addressNotEmpty) this.conditionInputs = false;
            },
            resetFormData() {
                this.formData = {
                    companyName: "",
                    cedula: "",
                    emailAddress: "",
                    phoneNumber: "",
                    provincia: "",
                    canton: "",
                    distrito: "",
                    exactAddress: ""
                };
            },
            async saveCompany() {
                await axios.post("https://localhost:7263/api/CompanyData", {
                    companyName: this.formData.companyName,
                    cedula: this.formData.cedula,
                    emailAddress: this.formData.emailAddress,
                    phoneNumber: this.formData.phoneNumber,
                    provincia: this.formData.provincia,
                    canton: this.formData.canton,
                    distrito: this.formData.distrito,
                    exactAddress: this.formData.exactAddress
                })
                    .then((response) => {
                        console.log(response.data);
                        this.companyID = response.data;
                        console.log(this.companyID);
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            },
        }
    }
</script>

<style scoped>

    .page-container {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 10px 20px;
        background-color: #f07800;
        color: white;
        font-family: 'League Spartan', sans-serif;
        max-width: 100vw;
        box-sizing: border-box;
    }

    .header__home-link {
        text-decoration: none;
        color: #332f2b;
    }

    .header__actions {
        display: flex;
        align-items: center;
    }

    .header__profile-container {
        position: relative;
    }

    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }

    .footer_columns {
        display: flex;
        justify-content: space-between;
        padding: 1rem;
        text-align: center;
        box-sizing: border-box;
        color: #f2f2f2;
        margin: 0 0 -30px 0;
    }

    .footer__column {
        font-family: 'League Spartan', sans-serif;
        flex: 1;
        margin: 0 10px;
        color: #f2f2f2;
    }

        .footer__column strong {
            display: block;
            margin-bottom: 0.5rem;
            font-weight: bolder;
            font-size: large;
        }

        .footer__column a {
            font-family: 'Poppins', sans-serif;
            display: block;
            margin: 0.5rem 0;
            text-decoration: none;
            color: #f2f2f2;
        }

            .footer__column a:hover {
                text-decoration: underline;
            }

        .footer__column p {
            font-family: 'Poppins', sans-serif;
            margin: 0.5rem 0;
            text-decoration: none;
        }

            .footer__column p:hover {
                text-decoration: underline;
            }

    .forms_background {
        margin: 0;
        padding-bottom: 50px;
        padding-top: 50px;
        height: 100%;
        /*Para que la disposici?n de los elementos que contenga sea flexible*/
        display: flex;
        /*Alinear vertical y horizontalmente al centro*/
        justify-content: center;
        align-items: center;
        background-color: #ffeec2;
    }

    label {
        font-weight: bold;
    }

    form {
        font-family: 'League Spartan', sans-serif;
        color: black;
        background-color: white;
        display: block;
        width: 500px;
        line-height: 2;
    }

    .forms_header {
        font-family: 'League Spartan', sans-serif;
        margin: 0;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        text-align: center;
    }

    .form_content_padding {
        padding-top: 10px;
        padding-bottom: 10px;
        padding-left: 20px;
        padding-right: 10px;
    }

    .for_required_text {
        color: red;
    }

    input {
        background-color: #ffeec2;
        border-radius: 20px;
        height: 20px;
        width: 460px;
    }

    .address_container {
        background-color: white;
        /*Para crear una especie de tabla*/
        display: grid;
        /*3 columnas de igual tama?o*/
        grid-template-columns: 1fr 1fr 1fr;
        /*Separadas entre s? por 10 pixeles*/
        gap: 10px;
        align-items: center;
    }

    .input_for_address {
        background-color: #ffeec2;
        border-radius: 20px;
        height: 20px;
        width: 100px;
    }

    button {
        font-size: 18px;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: black;
        /*Permite redondear los bordes*/
        border-radius: 20px;
        width: 460px;
        text-align: center;
        font-weight: bold;
    }

    .address_input_button_container {
        display: flex;
    }

    .map_button {
        font-size: 15px;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: black;
        /*Permite redondear los bordes*/
        border-radius: 20px;
        width: 150px;
        text-align: center;
        font-weight: bold;
        margin-left: auto;
    }

    .errorInInputsLabel {
        color: red;
    }

    .errorInInputsInput {
        border-color: red;
        border-style: double;
    }

    .eraseRouterLinkStyle {
        color:inherit;
        text-decoration: none;
    }

</style>