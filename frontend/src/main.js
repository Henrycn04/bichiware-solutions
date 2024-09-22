import { createApp } from 'vue'
import App from './App.vue'
import router from './router/router' // Importa el router
import bootstrap from 'bootstrap'
import store from './store/store'
// import 'bootstrap/dist/css/custom.css'
import '../scss/custom.css'


const app = createApp(App)

app.use(router) // Usa el router en la aplicación
app.use(bootstrap)
app.use(store)

app.mount('#app')
