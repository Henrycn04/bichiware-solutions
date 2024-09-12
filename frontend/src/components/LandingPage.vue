<template>
    <header class="header">
        <div class="header__brand">
            <a href="/" class="header__home-link" style="font-size:x-large; font-weight: bold; cursor: pointer;">Feria del Emprendedor</a>
        </div>
        <div class="header__search">
            <input type="text"
                   v-model="searchQuery"
                   placeholder="Buscar"
                   @keydown.enter="performSearch" />
            <button @click="performSearch" class="header__search__button">
                <img src="../assets/SearchIcon.png" alt="Buscar" />
            </button>
        </div>
        <div class="header__actions">
            <a href="/login" class="header__login">Accesar</a>
            <div class="header__profile-container" ref="profileContainer">
                <button @click="toggleProfileMenu" class="header__profile">
                    <img src="../assets/ProfileIcon.png" alt="Perfil" />
                </button>
                <div v-if="isProfileMenuVisible" class="header__profile-menu">
                    <a href="/profile" class="header__profile-menu-item" style="color: #463a2e">Cuenta</a>
                    <a href="/register-company" class="header__profile-menu-item" style="color: #463a2e">Registrar empresa</a>
                    <a href="/exit-account" class="header__profile-menu-item" style="color: #463a2e">Salir</a>
                </div>
            </div>
            <button @click="goToCart" class="header__cart">
                <img src="../assets/CartIcon.png" alt="Carrito" />
            </button>
        </div>
    </header>
</template>




<script>
    export default {
        data() {
            return {
                searchQuery: '',
                isProfileMenuVisible: false
            }
        },
        methods: {
            performSearch() {
                // para el boton de buscar
                console.log('Buscando:', this.searchQuery);
            },
            goToProfile() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/profile');
            },
            goToCart() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/cart');
            },
            goToHome() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/');
            },
            toggleProfileMenu() { // activa y desactiva el cuadro que se despliega en el icono del perfil
                this.isProfileMenuVisible = !this.isProfileMenuVisible;
            },
            goToAccount() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/account');
                this.isProfileMenuVisible = false;
            },
            goToRegisterCompany() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                this.$router.push('/register-company');
                this.isProfileMenuVisible = false;
            },
            logout() {
                // no se usa pero es otra forma de redireccionar eventualmente en caso de ser necesario
                console.log('Logout');
                this.$router.push('/login');
                this.isProfileMenuVisible = false;
            },
            handleClickOutside(event) {
                // verifica si el clic fue fuera del contenedor del perfil
                const profileContainer = this.$refs.profileContainer;
                if (profileContainer && !profileContainer.contains(event.target)) {
                    this.isProfileMenuVisible = false;
                }
            }
        },
        mounted() {
            //annade el listener al montar el componente
            document.addEventListener('click', this.handleClickOutside);
        },
        beforeUnmount() {
            // remueve el listener antes de destruir el componente
            document.removeEventListener('click', this.handleClickOutside);
        }
    }
</script>




<style scoped>
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

    .header__brand h1 {
        margin: 0;
        font-size: 24px;
    }

    .header__search {
        display: flex;
        align-items: center;
        margin: 0 20px;
        flex-grow: 1;
    }

        .header__search input {
            flex-grow: 1;
            padding: 8px;
            border: none;
            border-radius: 15px 0 0 15px;
            font-family: 'League Spartan', sans-serif;
            box-sizing: border-box;
        }

            .header__search input:focus {
                outline: none;
                box-shadow: 0 0 3px rgba(255, 165, 0, 0.8);
                background-color: #fff8e1;
            }

    .header__search__button {
        padding: 8px;
        border: none;
        background-color: transparent;
        cursor: pointer;
        display: flex;
        align-items: center;
        margin-left: -20px;
    }

        .header__search__button img {
            width: 30px;
            height: 30px;
        }

    .header__actions {
        display: flex;
        align-items: center;
    }

        .header__actions a,
        .header__actions button {
            margin-left: 15px;
            color: white;
            text-decoration: none;
            background: none;
            border: none;
            cursor: pointer;
        }

        .header__actions img {
            width: 24px;
            height: 24px;
        }

        .header__actions a:hover,
        .header__actions button:hover {
            text-decoration: none;
        }

    .header__profile-container {
        position: relative;
    }

    .header__profile-menu {
        position: absolute;
        top: 100%;
        right: 0;
        background: white;
        color: #332f2b;
        border-radius: 5px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        display: flex;
        flex-direction: column;
        width: 150px;
        padding: 0;
        margin: 0;
        box-sizing: border-box;
    }

    .header__profile-menu-item {
        padding: 10px;
        text-decoration: none;
        color: #463a2e;
        display: block;
        margin: 0 0 0 -10px;
        border-radius: 5px;
    }

        .header__profile-menu-item:hover {
            background-color: #f5f5f5;
        }
</style>

