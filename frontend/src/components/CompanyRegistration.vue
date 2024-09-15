<template>
    <div class="page-container">
        <header class="header">
            <div class="header__brand">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>
        <div class="forms_background">
            <form>
                <h2 class="forms_header">Registro de empresa</h2>
                <div class="form_content_padding">
                    <div class="for_required_text">* Campo obligatorio</div>
                    <div>
                        <label>Nombre de la empresa: *</label><br>
                        <input>
                    </div>
                    <div>
                        <label>Cedula juridica: *</label><br>
                        <input>
                    </div>
                    <div>
                        <label style="font-family: 'League Spartan', sans-serif;">Correo electronico: *</label><br>
                        <input>
                    </div>
                    <div>
                        <label>Numero de telefono: *</label><br>
                        <input>
                    </div>
                    <div class="address_input_button_container">
                        <label>Direccion: *</label>
                        <router-link to="/mapForAddress" class="map_button">Mapa</router-link>
                    </div>
                    <div class="address_container">
                        <label>Provincia</label>
                        <label>Canton</label>
                        <label>Distrito</label>
                        <input class="input_for_address">
                        <input class="input_for_address">
                        <input class="input_for_address">
                    </div>
                    <div>
                        <label>Direccion exacta:</label><br>
                        <input>
                    </div><br>
                    <button>Crear cuenta</button>
                </div>
            </form>
        </div>
        <footer class="footer">
            <p style="display: block;text-align: center; font-family: 'Poppins', sans-serif; font-size: medium;"> &copy; Copyright by BichiWare Solutions 2024 </p>
        </footer>
    </div>
</template>

<script>

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

</style>
