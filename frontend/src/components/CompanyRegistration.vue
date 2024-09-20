<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="forms_background">
            <!--Previene que el forms se envíe hasta que la condición sea verdadera-->
            <!--En este caso espera a que todos los inputs obligatorios estén llenos y a que se ingresen datos correctos-->
            <form @submit.prevent="conditonInputs">
                <h2 class="forms_header">Registro de empresa</h2>
                <div class="form_content_padding">
                    <div class="for_required_text">* Campo obligatorio</div>
                    <div>
                        <!--v-base: muestra un estilo (invoca una clase) solo si una variable es true-->
                        <!--formato: {'claseCSS': variableCondicional}-->
                        <label :class="{ 'errorInInputsLabel': companyNameNotEmpty}">Nombre de la empresa: *</label><br>
                        <!--V-model: asocia el input con una variable, entonces en esa variable queda almacenado lo que se escriba en él-->
                        <input v-model="formData.companyName" :class="{ 'errorInInputsInput': companyNameNotEmpty}">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': cedulaNotEmpty}">Cedula juridica: *</label><br>
                        <!--pattern es para aceptar solo inputs de acuerdo con una regex-->
                        <!--Si el texto no calza con la regez sale un warning-->
                        <!--ref es para referenciar una etiqueta desde javascript-->
                        <input v-model="formData.cedula" :class="{ 'errorInInputsInput': cedulaNotEmpty}" maxlength="9" pattern="\d{9}" ref="ced" placeholder="  Formato: 123456789">
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
                        <router-link to="/mapForAddress" class="map_button">Mapa</router-link>
                    </div>
                    <div class="address_container">
                        <label>Provincia</label>
                        <label>Canton</label>
                        <label>Distrito</label>
                        <input class="input_for_address" v-model="formData.provincia">
                        <input class="input_for_address" v-model="formData.canton">
                        <input class="input_for_address" v-model="formData.distrito">
                    </div>
                    <div>
                        <label>Direccion exacta:</label><br>
                        <input v-model="formData.exactAddress">
                    </div><br>
                    <button type="submit" @click="checkBeforeSubmit">Crear cuenta</button>
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
    export default {
        data() {
            return {
                formData: {
                    companyName: '',
                    cedula: '',
                    emailAddress: '',
                    phoneNumber: '',
                    provincia: '',
                    canton: '',
                    distrito: '',
                    exactAddress: ''
                },
                companyNameNotEmpty: false,
                cedulaNotEmpty: false,
                emailAddressNotEmpty: false,
                phoneNumberNotEmpty: false,
                addressNotEmpty: false,
                conditionInputs: false
            };
        },
        methods: {
            checkBeforeSubmit() {
                /*Se referencia a los inputs email, phoneNumber y cedula para revisar que su contenido sea correcto*/
                const email = this.$refs.email;
                const phoneNum = this.$refs.phoneNumb;
                const ced = this.$refs.ced;
                /*Se revisa que todos los inputs estén llenos y que tengan contenidos correctos*/
                /*El método trim() elimina cambios de línea y espacios en blanco*/
                if (this.formData.companyName.trim() === '') {
                    this.companyNameNotEmpty = true;
                    this.conditionInputs = false;
                } else {
                    this.companyNameNotEmpty = false;
                }
                if (this.formData.cedula.trim() === '' || !ced.validity.valid) {
                    this.cedulaNotEmpty = true;
                    this.conditionInputs = false;
                } else {
                    this.cedulaNotEmpty = false;
                }
                if (this.formData.emailAddress.trim() === '' || !email.validity.valid) {
                    this.emailAddressNotEmpty = true;
                    this.conditionInputs = false;
                } else {
                    this.formData.emailAddressNotEmpty = false;
                }
                if (this.formData.phoneNumber.trim() === '' || !phoneNum.validity.valid) {
                    this.phoneNumberNotEmpty = true;
                    this.conditionInputs = false;
                } else {
                    this.phoneNumberNotEmpty = false;
                }
                if (this.formData.provincia.trim() === '' || this.formData.canton.trim() === '' || this.formData.distrito.trim() === '' || this.formData.exactAddress.trim() === '') {
                    this.addressNotEmpty = true;
                    this.conditionInputs = false;
                } else {
                    this.addressNotEmpty = false;
                }
                if ((!this.companyNameNotEmpty && !this.cedulaNotEmpty && !this.emailAddressNotEmpty && !this.phoneNumberNotEmpty && !this.addressNotEmpty) && (email.validity.valid && ced.validity.valid && phoneNum.validity.valid)) {
                    this.conditionInputs = true;
                    this.formData.companyName = '', this.formData.cedula = '', this.formData.emailAddress = '', this.formData.phoneNumber = '', this.formData.provincia = '', this.formData.canton = '', this.formData.distrito = '', this.formData.exactAddress = '';
                    alert('Form submitted.');
                }
            },
            saveCompany() {
                console.log("Datos: ", this.formData);
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
        /*Para que la disposición de los elementos que contenga sea flexible*/
        display: flex;
        /*Alinear vertical y horizontalmente al centro*/
        justify-content: center;
        align-items: center;
        background-color: #ffeec2;
    }

    label {
        font-weight:bold;
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
        height:20px;
        width:460px;
    }

    .address_container {
        background-color: white;
        /*Para crear una especie de tabla*/
        display: grid;
        /*3 columnas de igual tamaño*/
        grid-template-columns: 1fr 1fr 1fr;
        /*Separadas entre sí por 10 pixeles*/
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
        display:flex;
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
        font-weight:bold;
        margin-left:auto;
    }

    .errorInInputsLabel {
        color:red;
    }

    .errorInInputsInput {
        border-color: red;
        border-style: double;
    }

</style>
