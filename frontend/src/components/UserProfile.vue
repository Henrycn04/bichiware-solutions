<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="userInfoContainer">
            <div>
                <h1><strong>{{username}} {{lastname}}</strong></h1>
                <h5>{{email}}</h5>
                <h5>{{creationDate}}</h5>
            </div>
            <div class="buttonsContainer">
                <button>Direccion</button>
                <button>Informacion de pago</button>
                <button>Cambiar tipo de cuenta</button>
            </div>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>
    //import axios from "axios";
    import { mapState } from 'vuex';
    export default {
        data() {
            return {
                username: "Felipe",
                lastname: "Bianchini Sanchez",
                email: "felipebianchini@ucr.ac.cr",
                creationDate: "12/03/2004",
            };
        }, computed: {
            ...mapState(['userCredentials']),
            // en userCredentials.userId está el id
        },
        methods: {
        }
    }
</script>

<style scoped>
    html, body {
        margin: 0;
        padding: 0;
        height: 100%;
        background-color: #ffeec2;
    }

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

    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
    }

    .userInfoContainer {
        padding-top: 20px;
        padding-bottom: 20px;
        font-family: 'League Spartan', sans-serif;
        display: grid;
        grid-template-columns: fit-content(700px) fit-content(200px);
        gap: 300px;
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

</style>
