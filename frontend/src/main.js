import { createApp } from 'vue'
import App from './App.vue'
import router from './router/router' // Importa el router

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

import bootstrap from 'bootstrap'
import store from './store/store'
// import 'bootstrap/dist/css/custom.css'
import '../scss/custom.css'




const app = createApp(App);
app.use(router) // Usa el router en la aplicación
app.use(bootstrap)
app.use(store)
app.config.globalProperties.$backendAddress = 'https://localhost:7263/';


app.mount('#app'); 
