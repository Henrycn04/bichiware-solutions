<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="content">
            <div class="userInfoContainer">
                <div>
                    <h1><strong>{{userData.userName}}</strong></h1>
                    <h5>{{userData.email}}</h5>
                    <h5>Fecha de creacion: {{userData.creationDate}}</h5>
                </div>
                <div class="buttonsContainer">
                    <router-link to="/addresses-list"><button class="eraseRouterLinkStyle">Direccion</button></router-link>
                    <button>Informacion de pago</button>
                    <router-link to="/changeAccountType"><button class="eraseRouterLinkStyle">Cambiar tipo de cuenta</button></router-link>

                </div>
            </div>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    import axios from "axios";
    import { mapState } from 'vuex';
    export default {
        data() {
            return {
                userData: {
                    userName: "",
                    email: "",
                    creationDate: ""
                }
            };
        },
        computed: {
            ...mapState(['userCredentials']),
        },
        methods: {
            getUserData() {
                axios.get(this.$backendAddress + "api/UserProfile", {
                    params: {
                        userID: this.userCredentials.userId 
                    }
                })
                    .then((response) => {
                        console.log(response.data);
                        this.userData = response.data;
                    })
                    .catch((error) => {
                        console.error("Error obtaining user data:", error);
                    });
            }
        },
        created() {
            this.getUserData();
        },
    }
</script>


<style scoped>
    .page-container {
        /*All the web page*/
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

    .content {
        /*pushes the footer to the botton of the web page*/
        flex-grow: 1;
    }

    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }

    .userInfoContainer {
        padding-left: 30px;
        padding-right: 30px;
        padding-top: 20px;
        padding-bottom: 20px;
        font-family: 'League Spartan', sans-serif;
        display: grid;
        grid-template-columns: fit-content(700px) fit-content(200px);
        gap: 500px;
        align-items: center;
    }

    .buttonsContainer {
        display: grid;
        grid-template-columns: fit-content(200px) fit-content(200px) fit-content(200px);
        gap: 50px;
    }

    button {
        font-size: 20px;
        padding-top: 5px;
        padding-bottom: 5px;
        background-color: #f07800;
        color: black;
        border-radius: 40px;
        width: 250px;
        height: 100px;
        text-align: center;
        font-weight: bold;
    }

    .eraseRouterLinkStyle {
        color: black;
        text-decoration: none;
    }

</style>
