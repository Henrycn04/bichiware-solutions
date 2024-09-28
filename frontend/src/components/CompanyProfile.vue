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
                    <h1><strong>{{companyProfileData.companyName}}</strong></h1>
                    <h5>{{companyProfileData.companyCedula}}</h5>
                    <h5>{{companyProfileData.companyEmail}}</h5>
                    <h5>{{companyProfileData.companyPhoneNumber}}</h5>
                </div>
                <div>
                    <button>Añadir producto</button>
                    <button>Inventario</button>
                </div>
                <div>
                    <button>Añadir entrega</button>
                </div>
            </div>
            <div class="secondInfoContainer">
                <div>
                    <h2>Direcciones registradas</h2>
                    <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista">
                        <thead>
                            <tr>
                                <th>Provincia</th>
                                <th>Cantón</th>
                                <th>Distrito</th>
                                <th>Dirección exacta</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(address, index) of companyAddresses" :key="index">
                                <td>{{ companyProfileData.addresses.province }}</td>
                                <td>{{ companyProfileData.addresses.canton }}</td>
                                <td>{{ companyProfileData.addresses.district }}</td>
                                <td>{{ companyProfileData.addresses.exactAddress }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div>
                    <button>Ver pedidos activos</button>
                </div>
            </div>
            <div class="secondInfoContainer">
                <div>
                    <h2>Perfiles asociados</h2>
                    <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth" id="lista1">
                        <thead>
                            <tr>
                                <th>Nombre</th>
                                <th>Correo electrónico</th>
                                <th>Teléfono</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(member, index) of companyMembers" :key="index">
                                <td>{{ companyProfileData.members.username }}</td>
                                <td>{{ member.members.email }}</td>
                                <td>{{ member.members.phoneNumber }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div>
                    <button>Ver pedidos pendientes</button>
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
    export default {
        data() {
            return {
                companyID: 0, 
                companyProfileData: {
                    companyName: ' ',
                    companyCedula: ' ',
                    companyEmail: ' ',
                    companyPhoneNumber: ' ',
                    addresses: [{
                        province: ' ',
                        canton: ' ',
                        district: ' ',
                        exactAddress: ' '
                    }],
                    members: [{
                        username: ' ',
                        email: ' ',
                        phoneNumber: ' '
                    }]
                }
            };
        }, mounted() {
            this.companyID = this.$route.params.companyID;
            this.getUserCompanies();
        },
        methods: {
            getUserCompanies() {
                axios.get("https://localhost:7263/api/CompanyProfile/CompanyData", {
                    params: {
                        userID: this.companyID
                    }
                })
                    .then((response) => {
                        console.log(response.data);
                        this.userCompanies = response.data;
                        console.log(this.userCompanies);
                    })
                    .catch((error) => {
                        console.error("Error obtaining user companies:", error);
                    });
            }
        }
    }
</script>


<style scoped>
    .page-container {
        /*Toda la pantalla*/
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
        /*Empuja el footer hacia abajo*/
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
        grid-template-columns: fit-content(700px) fit-content(200px) fit-content(200px);
        gap: 400px;
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

    .dropdown {
        position: relative;
        display: inline-block;
    }



    .dropdown-content {
        display: none;
        position: absolute;
        background-color: #f9f9f9;
        min-width: 250px;
        box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
        z-index: 100;
    }

        .dropdown-content a {
            color: black;
            padding: 12px 16px;
            text-decoration: none;
            display: block;
        }

            .dropdown-content a:hover {
                background-color: #f1f1f1;
            }

    .dropdown:hover .dropdown-content {
        display: block;
    }

    .secondInfoContainer {
        padding-left: 30px;
        padding-right: 30px;
        padding-top: 20px;
        padding-bottom: 20px;
        font-family: 'League Spartan', sans-serif;
        display: grid;
        grid-template-columns: fit-content(700px) fit-content(200px);
        gap: 400px;
        align-items: center;
    }

</style>
