import { createApp } from 'vue'
import App from './App.vue'
import router from './router/router' // Importa el router
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';


const app = createApp(App)

app.use(router) // Usa el router en la aplicación

app.mount('#app')
