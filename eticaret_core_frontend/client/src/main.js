import Vue from 'vue'

import App from './App.vue'
import router from './router'
import store from './store'
import VueCompositionAPI from '@vue/composition-api'

Vue.use(VueCompositionAPI)

Vue.config.productionTip = false
//window'un linkine bak, içinde localhost geçiyorsa
if(window.location.href.indexOf("//localhost")!=-1){ 
window.apiUrl="https://localhost:44304/";
}
else{
  window.apiUrl="http://eticaretapi.yaseminhidir.com/"
}
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
