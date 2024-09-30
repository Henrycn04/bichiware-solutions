<template>
    <div class="page-container" v-if="normalUser">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="forms_background">
            <form>
                <h2 class="forms_header">Cambiar tipo de cuenta</h2>
                <div class = "form_content_padding">
                    <h3>Terminos y condiciones</h3>
                    <div class="terms-content">
                        Para registrarse como vendedor debe cumplir con los siguientes requisitos:<br>
                        &#x2022;Ser mayor de 18 años.<br>
                        &#x2022;Proporcionar información válida y precisa, incluyendo su nombre, dirección, teléfono y correo electrónico. <br>
                        &#x2022;Cumplir con todas las leyes y regulaciones aplicables a la venta de productos en línea en su jurisdicción. <br>
                        <br>
                        Usted es responsable de:<br>
                        &#x2022;Proporcionar información precisa y actualizada en todo momento.<br>
                        &#x2022;Mantener la confidencialidad de su nombre de usuario y contraseña.<br>
                        &#x2022;Informarnos inmediatamente si detecta cualquier uso no autorizado de su cuenta o cualquier otra violación de seguridad.<br>
                        <br>
                        Se debe comprometer a: <br>
                        &#x2022;Cumplir con todas las leyes, regulaciones y políticas que se aplican a su actividad de venta.<br>
                        &#x2022;Vender productos que sean de su propiedad o que tenga autorización legal para vender. <br>
                        &#x2022;Proporcionar descripciones precisas y detalladas de los productos que ofrece. <br>
                        &#x2022;Enviar los productos dentro del plazo establecido al realizar la venta y asegurarse de que los productos lleguen en condiciones adecuadas.<br>
                        &#x2022;Mantener actualizado su inventario y no ofrecer productos que no estén disponibles.<br>
                        &#x2022;Ofrecer un servicio al cliente adecuado y responder de manera oportuna a las consultas o reclamos de los compradores.<br>
                        <br>
                        &#x2022;Se prohibe la venta de productos que: <br>
                        &#x2022;Sean ilegales, falsificados, robados o que infrinjan los derechos de propiedad intelectual de terceros.<br>
                        &#x2022;Estén prohibidos por nuestras políticas de productos.<br>
                        &#x2022;No cumplan con las normativas de seguridad aplicables.<br>
                        &#x2022;No pueda enviar o entregar dentro de los plazos y términos acordados.<br>
                    </div>
                    <div class="button-container">
                        <CButton @click="accept" class="big-button">Aceptar</CButton>
                        <CButton @click="deny" class="big-button">Rechazar</CButton>
                    </div>
                </div>
            </form>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
    <div class="page-container" v-else-if="companyUser">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="forms_background">
            <form>
                <h2 class="forms_header">Cambiar tipo de cuenta</h2>
                <div class = "form_content_padding">
                    <h3>Funcionalidades perdidas al cambiar el tipo de cuenta:</h3>
                    <div class="terms-content">
                        Crear empresas <br>
                        Agregar Productos <br>
                        Vender productos <br>
                    </div>
                    <div class="button-container">
                        <CButton @click="becomeNormalUser" class="big-button">Aceptar</CButton>
                        <CButton @click="deny" class="big-button">Rechazar</CButton>
                    </div>
                </div>
            </form>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
    <div class="page-container" v-else-if="isAdmin">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>     
        <h2 class="notAvailable">Esta página no está disponible para administraddores</h2>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer> 
    </div>
    <div class="page-container" v-else>
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <h2 class="notAvailable">Esta pagina solo está disponible al hacer login</h2>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import { useRouter } from 'vue-router';
    import axios from "axios";
    import { mapGetters } from 'vuex';
    export default {
        computed: {
            ...mapGetters(['isLoggedIn']), // Maps the getter isLoggedIn
            ...mapGetters(['getUserId']), // Maps getUserID
            ...mapGetters(['getUserType']), // Maps getUserType 1 - Normal 2 - Company 3 - Admin
        },
        data() {
            return{
                ID: 0,
                companyUser: false,
                normalUser: false,
                type: 0,
                userType: 0
            };
        },
        setup() {
            const router = useRouter();
            return { router };
        },
        methods: {
            userChecks() {
                if(this.isLoggedIn) {
                    this.userType = this.getUserType;
                    this.normalUser = this.userType === 1;
                    this.companyUser = this.userType === 2;
                    this.isAdmin = this.userType === 3;
                }
            },
            accept() {
                this.ID = -1
                this.getUserID();
                if(this.ID !== -1) {
                    this.type = 2;
                    this.changeType();
                } else this.alertError();
            },
            deny(){
                window.alert("TyC rechazados\nSe procederá a la pantalla principal");
                this.$router.push('/');
            },
            alertError() {
                window.alert("Error al accesar a cuenta\nAsegurese de haber hecho login");
                this.$router.push('/');
            },
            changeType(){
                
                axios.post("https://localhost:7263/api/SetType",{
                    userID: this.ID,
                    newType: this.type
                }).then(function (response) {
                    console.log(response);
                    if(this.type === 2)window.location.href ="/companyRegistration";
                    else this.deleteCompanies();
                }).catch(function (error) {
                    console.log(error);
                });
            },
            deleteCompanies() {
                axios.post("https://localhost:7263/api/DeleteFromCompany",{
                    userID: this.ID,
                }).then(function (response) {
                    console.log(response);
                    window.location.href ="/";
                }).catch(function (error) {
                    console.log(error);
                });
            },
            getUserID() {
                if(this.isLoggedIn) this.ID = this.getUserId;
                else this.ID = -1;
            },
            becomeNormalUser() {
                this.ID = -1;
                this.getUserID;
                if (this.ID !== -1) {
                    this.type = 1;
                    this.changeType();
                } else this.alertError();
            }, 
        },
        created: function() {
            this.userChecks();
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

    .terms-box {
        background-color: #fff;
        padding: 15px;
        border-radius: 5px;
        border: 1px solid #ddd;
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

    h3{
        font-size: medium;
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

    CButton {
        font-size: 16px;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: black;
        border-radius: 20px;
        width: 500px;
        text-align: center;
        font-weight: bold;
    }

    .big-button {
        font-size: 18px;
        padding: 20px 30px;
        background-color: #f07800;
        color: black;
        border-radius: 25px;
        width: 200px;
        text-align: center;
        font-weight: bold;
        margin-right: 15px;
    }

    .button-container {
        display: flex;
        justify-content: center;
        margin-top: 20px;
    }
    .terms-content {
        height: 300px;
        overflow-y: auto;
        border: 2px solid #272727;
        padding: 10px;
        font-size: 12px;
        color: #000000;
        margin-bottom: 10px;
    }

    .notAvailable {
        justify-content: center;
        align-items: center;
        text-align: center;
        background-color: #ffeec2;
    }

</style>