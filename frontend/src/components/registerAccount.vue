<template>
    <header class="header">
        <div class="header__brand">
            <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
        </div>
    </header>
    <div class="content">
        <div class="forms_background">
            <form @submit.prevent="checkInput">
                <h2 class="forms_header">Registro de cuenta</h2>
                <div class="form_content_padding">
                    <div class="for_required_text">* Campo obligatorio</div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': nameNotEmpty}">
                                        Nombre*</label><br>
                        <input v-model="dataInput.userName"
                                :class="{ 'errorInInputsInput': nameNotEmpty}">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': lastNameNotEmpty}">
                                Apellidos*</label><br>
                        <input v-model="dataInput.userLastName"
                                :class="{ 'errorInInputsInput': lastNameNotEmpty}">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': emailAddressNotEmpty}">
                                        Correo electronico*</label><br>
                        <input type="email" 
                                :class="{ 'errorInInputsInput': emailAddressNotEmpty}"
                                v-model="dataInput.emailAddress" 
                                placeholder="  Formato: usuario@gmail.com" ref="email">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': cedulaNotEmpty}">
                                        Cedula*</label><br>
                        <input v-model="dataInput.cedula" 
                                :class="{ 'errorInInputsInput': cedulaNotEmpty}" 
                                maxlength="9" pattern="\d{9}" ref="ced" 
                                placeholder="  Formato: 123456789">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': passNotEmpty}">
                                        Contraseña*</label><br>
                        <input v-model="dataInput.password" 
                                :class="{ 'errorInInputsInput': passNotEmpty}"
                                type="password">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': passNotEmpty}">
                                        Contraseña (Confirmar)*</label><br>
                        <input v-model="dataInput.passwordC" 
                                :class="{ 'errorInInputsInput': passNotEmpty}"
                                type="password">
                    </div>
                    <div>
                        <label :class="{ 'errorInInputsLabel': phoneNumberNotEmpty}">
                                        Numero de telefono: *</label><br>
                        <input v-model="dataInput.phoneNumber" 
                                :class="{ 'errorInInputsInput': phoneNumberNotEmpty}"
                                maxlength="8" pattern="\d{8}" ref="phoneNumb" 
                                placeholder="  Formato: 12345678">
                    </div>
                    <div class="address_input_button_container">
                        <label :class="{ 'errorInInputsLabel': addressNotEmpty}">
                                        Direccion: *</label>
                        <router-link to="/register" class="map_button" style="display: none">
                                        Mapa</router-link>
                    </div>
                    <div class="address_container">
                        <label>Provincia</label>
                        <label>Canton</label>
                        <label>Distrito</label>
                        <select class="input_for_address" 
                                v-model="dataInput.province">
                            <option>San Jose</option>
                            <option>Alajuela</option>
                            <option>Cartago</option>
                            <option>Heredia</option>
                            <option>Guanacaste</option>
                            <option>Puntarenas</option>
                            <option>Limon</option>
                        </select>
                        <input class="input_for_address" 
                                v-model="dataInput.canton">
                        <input class="input_for_address" 
                                v-model="dataInput.district">
                    </div>
                    <div>
                        <label>Direccion exacta:</label><br>
                        <input v-model="dataInput.exactAddress">
                    </div><br>
                    <button type="submit" @click="checkValues">Crear cuenta</button>
                </div>
            </form>
            
        </div>
    </div>
    <footer class="footer">
        <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
    </footer>
</template>

<script>
    import axios from "axios";
    import { useRouter } from 'vue-router';
    import { mapActions } from 'vuex';
    import CryptoJS from "crypto-js";
    export default {
        data() {
            return {
                dataInput: {
                    userName: "", userLastName: "", emailAddress: "", cedula: "",
                    password: "", passwordC: "", phoneNumber: "", province: "",
                    canton:"", district:"", exactAddress:""
                },
                nameNotEmpty: false,
                lastNameNotEmpty: false,
                emailAddressNotEmpty: false,
                cedulaNotEmpty: false,
                passNotEmpty: false,
                pass2NotEmpty: false,
                phoneNumberNotEmpty: false,
                addressNotEmpty: false,
                validInputs: true,
                logInData: { email: "", password: ""},
                backendHost: null
            };
        },
        created() {
            this.backendHost = this.$backendAddress;
        },
        setup() {
            const router = useRouter();
            return { router };
        },
        methods: {
            checkValues() {
                console.log("checking inputs");
                this.validInputs = true;
                this.checkName();
                this.checkLastName();
                this.checkEmail();
                this.checkCedula();
                this.checkPassword();
                this.checkPhoneNumber();
                this.checkAddress();
                if (this.validInputs) this.registerUser();
            },
            checkName() {
                this.nameNotEmpty = this.dataInput.userName.trim() === '';
                if (this.nameNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in name");
                }
            },
            checkLastName() {
                this.lastNameNotEmpty = this.dataInput.userLastName.trim() === '';
                if (this.lastNameNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in Last name");
                }
            },
            checkEmail() {
                const emailRef = this.$refs.email;
                this.emailAddressNotEmpty = this.dataInput.emailAddress.trim() === '' || !emailRef.validity.valid;
                if (this.emailAddressNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in email");
                }
            },
            checkCedula() {
                const ced = this.$refs.ced;
                this.cedulaNotEmpty = this.dataInput.cedula.trim() === '' || !ced.validity.valid;
                if (this.cedulaNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in cedula");
                }
            },
            checkPassword() {
                this.passNotEmpty = this.dataInput.password.trim() === '' ||
                                    this.dataInput.passwordC === '' ||
                                    this.dataInput.password !== this.dataInput.passwordC ||
                                    this.dataInput.password.length < 8;
                if (this.passNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in password");
                }
            },
            checkPhoneNumber() {
                const phoneNum = this.$refs.phoneNumb;
                this.phoneNumberNotEmpty = this.dataInput.phoneNumber.trim() === '' || !phoneNum.validity.valid;
                if (this.phoneNumberNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in phone number");
                }
            },
            checkAddress() {
                const { province, canton, district, exactAddress } = this.dataInput;
                // .some() checks all the array and return true if at least one passes the check
                this.addressNotEmpty = [province, canton, district, exactAddress].some(field => field.trim() === '');
                if (this.addressNotEmpty) {
                    this.validInputs = false;
                    console.log("Error in address");
                }
            },
            ...mapActions(['logIn']),
            async completeLogIn() {
                this.logInData.email = this.dataInput.emailAddress;
                this.logInData.password = CryptoJS.SHA512(this.dataInput.password).toString().toUpperCase();
                var searchString = this.backendHost + "api/login/search"
                var getDataString = this.backendHost + "api/login/getData"
                try {
                    const response = await axios.post(searchString, this.logInData);
                    console.log(response.data);
                    if (response.data.success) {
                        const userProfile = await axios.post(getDataString, this.logInData);
                        console.log("USer ID: ", userProfile.data.userId);
                        this.logIn({
                                profile: {
                                    UserId: userProfile.data.userId,
                                    UserType: userProfile.data.userType,
                                    LoginDate: userProfile.data.loginDate
                                },
                                credentials: {
                                    userId: userProfile.data.userId,
                                    timeOfLogIn: userProfile.data.loginDate,
                                    userType: userProfile.data.userType
                                }
                            });
                            window.alert("Registro exitoso\nPasando a la pagina de verificación")
                            window.location.href = "/confirmation"; 
                    } else {
                        // couldn't find the user
                        window.alert("Error en accesar post registro\nVolviendo al sitio principal");
                    }
                } catch (error) {
                    // show conection error information
                    window.alert("Error al conectarse con el servidor\nVolviendo al sitio principal");
                }
            },
            registerUser() {
                console.log("Tries to register");
                console.log(this.dataInput);
                var registerString = this.backendHost + "api/registerUser"
                axios.post(registerString, {
                    Name: this.dataInput.userName,
                    lastName: this.dataInput.userLastName,
                    email: this.dataInput.emailAddress,
                    cedula: this.dataInput.cedula,
                    password: CryptoJS.SHA512(this.dataInput.password).toString().toUpperCase(),
                    phoneNumber: this.dataInput.phoneNumber,
                    province: this.dataInput.province,
                    canton: this.dataInput.canton,
                    district: this.dataInput.district,
                    exactAddress: this.dataInput.exactAddress
                }).then( (response) => {
                    console.log(response);
                    console.log("Attempting log in");
                    this.completeLogIn();
                }).catch(function (error) {
                    console.log(error);
                    window.alert("Error al registrar el usuario\nPasando a la pagina principal")
                    window.location.href = "/";
                });
            },
        },
    };
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