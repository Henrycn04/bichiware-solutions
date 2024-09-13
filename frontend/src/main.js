import { createApp } from 'vue'
import App from './App.vue'
import router from './router/router' // Importa el router

const app = createApp(App)

app.use(router) // Usa el router en la aplicación

app.mount('#app')
