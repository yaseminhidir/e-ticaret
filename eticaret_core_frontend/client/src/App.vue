<template>
  <div id="app">
    <div class="header" id="home">
      <div class="container">
        <ul>
          <li class="col-md-3"></li>
          <li v-if="!$store.getters.IsLoggedIn" class="col-md-3">
            <router-link to="/login"
              ><i class="fa fa-unlock-alt" aria-hidden="true"></i> Log In
            </router-link>
          </li>
          <li v-else>
            <a @click="logOut" style="cursor:pointer"
              ><i class="fa fa-unlock-alt" aria-hidden="true"></i> Log Out
            </a>
          </li>

          <li v-if="!$store.getters.IsLoggedIn">
            <router-link to="/register"
              ><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Sign Up
            </router-link>
          </li>

         

          <li v-if="$store.getters.IsLoggedIn">
            <i class="fa fa-user" aria-hidden="true"></i>
            <router-link to="/profile">{{
              $store.state.user.firstName
            }}</router-link>
          </li>
        </ul>
      </div>
    </div>
    <!-- //header -->
    <!-- header-bot -->
    <div class="header-bot">
      <div class="header-bot_inner_wthreeinfo_header_mid">
        <div class="col-md-4 header-middle">
         
        </div>
        <!-- header-bot -->
        <div class="col-md-4 logo_agile">
          <h1>
            <router-link to="/"
              ><span>E</span>lite Shoppy
              <i
                class="fa fa-shopping-bag top_logo_agile_bag"
                aria-hidden="true"
              ></i
            ></router-link>
          </h1>
        </div>
        <!-- header-bot -->
        <div class="col-md-4 agileits-social top_content">
       
        </div>
        <div class="clearfix"></div>
      </div>
    </div>
    <top-menu></top-menu>

    <router-view />
    <div class="footer">
      <div class="footer_agile_inner_info_w3l">
        <div class="col-md-3 footer-left">
          <h2>
            <router-link to="/"><span>E</span>lite Shoppy </router-link>
          </h2>
          <p>
            Lorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed
            quia non numquam eius modi tempora.
          </p>
        
        </div>
        <div class="col-md-9 footer-right">
          <div class="sign-grds">
            <div class="col-md-4 sign-gd">
              <h4>Our <span>Information</span></h4>
              <ul>
                <li><router-link to="/">Home</router-link></li>
                <li v-for="category in categories" :key="category.id"><router-link :to="'/products/' + category.id">{{category.name}}</router-link></li>
               
              </ul>
            </div>

            <div class="col-md-5 sign-gd-two">
              <h4>Store <span>Information</span></h4>
              <div class="w3-address">
                <div class="w3-address-grid">
                  <div class="w3-address-left">
                    <i class="fa fa-phone" aria-hidden="true"></i>
                  </div>
                  <div class="w3-address-right">
                    <h6>Phone Number</h6>
                    <p>+1 234 567 8901</p>
                  </div>
                  <div class="clearfix"></div>
                </div>
                <div class="w3-address-grid">
                  <div class="w3-address-left">
                    <i class="fa fa-envelope" aria-hidden="true"></i>
                  </div>
                  <div class="w3-address-right">
                    <h6>Email Address</h6>
                    <p>
                      Email :<a href="mailto:example@email.com">
                        mail@example.com</a
                      >
                    </p>
                  </div>
                  <div class="clearfix"></div>
                </div>
                <div class="w3-address-grid">
                  <div class="w3-address-left">
                    <i class="fa fa-map-marker" aria-hidden="true"></i>
                  </div>
                  <div class="w3-address-right">
                    <h6>Location</h6>
                    <p>Broome St, NY 10002,California, USA.</p>
                  </div>
                  <div class="clearfix"></div>
                </div>
              </div>
            </div>

            <div class="clearfix"></div>
          </div>
        </div>
        <div class="clearfix"></div>
        <div class="agile_newsletter_footer">
      
        </div>
        
      </div>
    </div>
  </div>
</template>
<script>
axios;
import topmenu from "@/views/TopMenu.vue";
import axios from "axios";

class Emitter {
  constructor() {
    var delegate = document.createDocumentFragment();
    ["addEventListener", "dispatchEvent", "removeEventListener"].forEach(
      (f) => (this[f] = (...xs) => delegate[f](...xs))
    );
  }
}

export default {
  created() {
    window.eventEmitter = new Emitter();

    window.eventEmitter.addEventListener("cartData", this.CartDataReceived);
    this.$store.commit("addDefaultHeader");
    if (!this.$store.state.customerIdentify) {
      this.$store.commit("setCustomerIdentify", this.uuidv4());
    }
    this.load();
  },
  data() {
    return {
      cart: [],
       categories: [],
    };
  },
  components: {
    TopMenu: topmenu,
  },
  computed: {},
  methods: {
    CartDataReceived(eventData) {
      console.log("CartDataReceived",eventData);
      this.cart = eventData.detail;
    },
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetCategoriesTopMenu"
      );
      this.categories = result.data;
      console.log(this.categories);
    },
    async logOut() {
      var result = await axios.post(
        window.apiUrl + "AuthCustomer/LogOut"
      );

      this.$router.push("/login");
      this.$store.dispatch("logOut");
      console.log(result);
    },
    uuidv4() {
      return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
        (
          c ^
          (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
        ).toString(16)
      );
    },
  },
};
</script>
<style src="@vueform/slider/themes/default.css"></style>
