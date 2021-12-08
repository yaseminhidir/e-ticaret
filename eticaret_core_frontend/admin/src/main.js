import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

import VueMaterial from "vue-material";
import "vue-material/dist/vue-material.min.css";
import "vue-material/dist/theme/default.css";

import wysiwyg from "vue-wysiwyg";
import "./assets/app.scss";

Vue.use(wysiwyg, {}); // config is optional. more below
Vue.use(VueMaterial);
Vue.config.productionTip = false;
//window'un linkine bak, içinde localhost geçiyorsa
if (window.location.href.indexOf("//localhost") != -1) {
  window.apiUrl = "https://localhost:44304/";
} else {
  window.apiUrl = "http://eticaretapi.yaseminhidir.com/";
}
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
