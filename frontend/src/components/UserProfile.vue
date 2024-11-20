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
                    <router-link to="/addresses-list"><button class="eraseRouterLinkStyle">Direcci�n</button></router-link>
                    <button>Informacion de pago</button>
                    <router-link to="/changeAccountType"><button class="eraseRouterLinkStyle">Cambiar tipo de cuenta</button></router-link>
                    <router-link to="/modifyUserData"><button class="eraseRouterLinkStyle">Cambiar datos de cuenta</button></router-link>
                    <button class="delete-profile-button" @click="deleteProfile">Eliminar perfil</button>
                    <router-link to="/userReports"><button class="eraseRouterLinkStyle">Reportes de cuenta</button></router-link>
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
    import Swal from 'sweetalert2';
    import commonMethods from '@/mixins/commonMethods';
    import { mapState } from 'vuex';
    const companyError = 'RelatedCompaniesConflict';
    const orderError = 'RelatedOrdersInProgressConflict';
    export default {
        mixins: [commonMethods],
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
            },
            deleteProfile() {
                Swal.fire({
                    title: '�Est� seguro de querer eliminar su perfil?',
                    text: "Esta acci�n no se puede deshacer.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'S�, eliminar',
                    cancelButtonText: 'No, cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.executeProfileDeletion();
                    }
                });
            },

            executeProfileDeletion() {
                axios.delete(this.$backendAddress + `api/UserData/userProfile/${this.userCredentials.userId }`)
                .then((response) => {
                    console.log(response);
                    this.showSuccessMessage();
                })
                .catch((error) => {
                    console.error(error);
                    this.handleError(error);
                });
            },

            showSuccessMessage() {
                Swal.fire(
                    'Eliminado',
                    'El perfil ha sido eliminado con �xito.',
                    'success'
                ).then(() => {
                    this.goTologout();
                    this.goToHome();
                });
            },

            handleError(error) {
                if (error.response && error.response.data) {
                    const errorCode = error.response.data.error;

                    if (errorCode === companyError) {
                        this.showConflictErrorCompany();
                    }
                    else if(errorCode === orderError){
                        this.showConflictErrorOrder();
                    } else {
                        this.showGenericError();
                    }
                } else {
                    this.showConnectionError();
                }
            },

            showConflictErrorCompany() {
                Swal.fire(
                    'Error',
                    'No se puede eliminar el perfil porque tiene empresas donde es el �nico miembro.',
                    'warning'
                );
            },

            showConflictErrorOrder() {
                Swal.fire(
                    'Error',
                    'No se puede eliminar el perfil porque tiene ordenes en progreso.',
                    'warning'
                );
            },

            showGenericError() {
                Swal.fire(
                    'Error',
                    'Ocurri� un error al eliminar el perfil.',
                    'error'
                );
            },

            showConnectionError() {
                Swal.fire(
                    'Error',
                    'Ocurri� un error al conectar con el servidor.',
                    'error'
                );
            }
        },
        created() {
            this.getUserData();
        },
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

    .content {
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
        grid-template-columns: minmax(200px, 1fr) minmax(200px, 1fr);
        gap: 20px;
        align-items: center;
    }
    .buttonsContainer {
        display: grid;
        grid-template-columns: repeat(2, 1fr); 
        gap: 20px; 
        justify-items: start; 
        padding: 0 20px; 
        width: 100%;
        max-width: 800px; 
        margin: 0 auto; 
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
    .delete-profile-button {
        font-size: 20px;
        padding-top: 5px;
        padding-bottom: 5px;
        background-color: red;
        color: white;
        border-radius: 40px;
        width: 250px;
        height: 100px;
        text-align: center;
        font-weight: bold;
        cursor: pointer;
    }

    .delete-profile-button:hover {
        background-color: darkred;
    }

</style>
