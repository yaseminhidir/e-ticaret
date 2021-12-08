import axios from "axios";
import Vue from "vue";
import Vuex from "vuex";
import VuexPersistence from "vuex-persist";

Vue.use(Vuex);
const vuexLocal = new VuexPersistence({
  storage: window.localStorage,
});

export default new Vuex.Store({
  state: {
    user: null,
    userToken: null,
    sepet:[],
  },
  mutations: {
    setUser(state, payload) {
      state.user = payload.user;
      state.userToken = payload.userToken;
    },
setCustomerIdentify(state,payload){
state.customerIdentify=payload;
},
    addDefaultHeader(state) {
      if (state.userToken) {
        axios.defaults.headers["Authentication"] = "Bearer " + state.userToken; //headera token ekledik, backendte action filterda kullanabilmek için.
      } else {
        delete axios.defaults.headers.Authentication;
      }
    },
    
  },
  getters: {
    IsLoggedIn: (state) => {
      /* if(user =! null){
        return true;
      }
      else{
        return false;
      } */

      return state.user != null;
    },
  },
  actions: {
    loggedIn({ commit }, payload) {
      commit("setUser", payload);
      commit("addDefaultHeader");
    },
    logOut({ commit }) {
      commit("setUser", { user: null, userToken: null });
      commit("addDefaultHeader");
    },
  },
  plugins: [vuexLocal.plugin],
});
