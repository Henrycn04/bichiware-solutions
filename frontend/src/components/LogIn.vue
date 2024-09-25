
<template>
    <div class="d-flex flex-column min-vh-100 bg-light-custom">
        <header class="header py-2">
            <div class="header__brand text-center">
                <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
            </div>
        </header>

        <div class="d-flex justify-content-center align-items-center flex-grow-1">
            <div class="card shadow" style="max-width: 400px; width: 100%;">
                <div class="forms_header">
                    <h3><strong>Iniciar Sesión</strong></h3>
                </div>
                <form @submit.prevent="submitForm">
                    <div class="px-2 form-group">
                        <div v-if="errorMessage" class="text-danger mb-2">{{ errorMessage }}</div>
                        <label for="email">Correo electrónico:</label>
                        <input v-model="logInData.email" type="email" id="email" class="form-control custom-input" required />
                    </div>
                    <div class="px-2 form-group">
                        <label for="password">Contraseña:</label>
                        <input v-model="logInData.password" type="password" id="password" class="form-control custom-input" required />
                    </div>
                    <div>
                        <button type="submit" class="my-2 mb-3 btn btn-success btn-block custom-btn">Iniciar Sesión</button>
                        <strong @click="forgotPassword" class="px-2" style="text-decoration: underline; cursor: pointer; color: black;">¿Olvidó su contraseña?</strong>
                        <hr width="100%" color="black" class="my-2" />
                        <button @click="goToRegister" type="button" class="mb-3 btn btn-secondary btn-block custom-btn">Registrarse</button>

                    </div>
                </form>
            </div>
        </div>

        <footer class="footer py-3">
            <p class="text-center" style="font-family: 'Poppins', sans-serif; font-size: medium;">&copy; Copyright by BichiWare Solutions 2024</p>
        </footer>
    </div>
</template>


<script>
import axios from 'axios';
import { mapActions } from 'vuex';
export default{
    
    data() {
        return {
            logInData: { email: "", password: ""},
            errorMessage: '',
        };
        },
    methods: {
        ...mapActions(['logIn']),
        async submitForm() {
            try {
                const response = await axios.post("https://localhost:7263/api/login/search", this.logInData);
                console.log(response.data);
                if (response.data.success) {
                    // found the user
                    console.log('Inicio de sesión exitoso');
                    
                    const userProfile = await axios.post("https://localhost:7263/api/login/getData",
                     this.logInData);

                     console.log(userProfile.data);
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

                    // redirect to landing page
                    this.$router.push('/');
                } else {
                    // couldn't find the user
                    this.errorMessage = 'La contraseña o el correo son incorrectos';
                    console.log(this.errorMessage);
                    // Delete email and password data
                    this.logInData.email = '';
                    this.logInData.password = '';
                }
            } catch (error) {
                // show conection error information
                this.errorMessage = 'Error server conection';
            }
        },
        forgotPassword() {
            this.$router.push('/changePassword')
        },
        goToRegister() {
            this.$router.push('/register');
        }
    }
};
</script>

<style scoped>
      .bg-light-custom {
        background-color: #ffeec2;
    }
    .footer {
        display: block;
        justify-content: space-between;
        background-color: #9b6734;
        color: #f2f2f2;
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

    .forms_header {
        font-family: 'League Spartan', sans-serif;
        margin: 0;
        padding-top: 10px;
        padding-bottom: 10px;
        background-color: #f07800;
        color: #332f2b;
        text-align: center;
    }
    .header__home-link {
        text-decoration: none;
        color: #332f2b;
    }

    .header__brand h1 {
        margin: 0;
        font-size: 24px;
    }
    .custom-btn {
        background-color: #db6e00;
        color: white;
        border: none;
        display: block;
        margin: 0 auto;
       
}
    .custom-input {
        background-color: #ffeec2;
        border-radius: 8px;
        padding: 10px; 
    }


</style>