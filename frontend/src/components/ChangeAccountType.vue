<template>
    <div class="page-container">
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
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin congue, tellus id ornare porttitor...
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
</template>

<script>
    import { useRouter } from 'vue-router';
    import axios from "axios";
    import { mapGetters } from 'vuex';
    export default {
        computed: {
            ...mapGetters(['isLoggedIn']), // Maps the getter isLoggedIn
            ...mapGetters(['getUserId']), // Maps getUserID
        },
        data() {
            return{
                ID: 0
            };
        },
        setup() {
            const router = useRouter();
            return { router };
        },
        methods: {
            accept() {
                this.ID = -1
                this.getUserID();
                this.ID = 4;
                if(this.ID !== -1) {
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
                    newType: 2
                }).then(function (response) {
                    console.log(response);
                    window.location.href ="/companyRegistration";
                }).catch(function (error) {
                    console.log(error);
                });
            },
            getUserID() {
                if(this.isLoggedIn) this.ID = this.getUserId;
                else this.ID = -1;
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
        height: 100px;
        overflow-y: auto;
        border: 2px solid #272727;
        padding: 10px;
        font-size: 12px;
        color: #000000;
        margin-bottom: 10px;
    }

</style>