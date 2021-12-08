<template>
  <div>
    <div class="page-head_agile_info_w3l">
      <div class="container">
        <h3><span> </span></h3>
        <!--/w3_short-->
        <div class="services-breadcrumb">
          <div class="agile_inner_breadcrumb">
            <ul class="w3_short">
              <li><a href="index.html">Home</a><i>|</i></li>
              <li
                v-for="category in product.productCategories"
                :key="category.id"
              >
                {{ category.category.fullName }}
              </li>
            </ul>
          </div>
        </div>
        <!--//w3_short-->
      </div>
    </div>
    <div class="banner-bootom-w3-agileits">
      <div class="container">
        <!---728x90--->

        <div class="col-sm-4">
          <div class="grid images_3_of_2">
            <div class="flexslider" v-if="!hideImages">
              <ul class="slides">
                <li
                  :data-thumb="image.url"
                  v-for="image in product.productImages"
                  :key="image.id"
                >
                  <div class="thumb-image">
                    <img
                      :src="image.url"
                      data-imagezoom="true"
                      class="img-responsive"
                    />
                  </div>
                </li>
              </ul>
              <div class="clearfix"></div>
            </div>
          </div>
        </div>
        <div class="col-sm-8 ">
          <h3>{{ product.name }}</h3>
          <p>
            <span class="item_price">{{ product.price }} TL</span>
            <del>- $900</del>
          </p>
          <div class="row">
            <div class="col-md-4" style="padding:0px">
              <div
                class="snipcart-details top_brand_home_details item_add single-item hvr-outline-out button2 "
              >
                <div v-if="product.stockStatuId < 3">
                  <button
                    type="submit"
                    name="submit"
                    value="Add to cart"
                    class="button"
                    @click="AddToCart"
                  >
                    ADD TO CARD
                  </button>
                </div>
                <div v-else style="background:rgb(238, 238, 238);padding:5px">
                  <span> Stok bulunmamaktadır </span>
                </div>
              </div>
            </div>
          </div>

          <div class="responsive_tabs_agileits">
            <div id="horizontalTab">
              <ul class="resp-tabs-list">
                <li>Description</li>

                <li>Information</li>
              </ul>
              <div class="resp-tabs-container">
                <!--/tab_one-->
                <div class="tab1">
                  <div class="single_page_agile_its_w3ls">
                    <p
                      v-html="product.description"
                      class="w3ls_para"
                      v-if="product.description"
                    ></p>
                    <p v-else>No description</p>
                  </div>
                </div>
                <!--//tab_one-->

                <div class="tab2">
                  <div class="single_page_agile_its_w3ls">
                    <p
                      v-html="product.information"
                      v-if="product.information"
                    ></p>
                    <p class="w3ls_para" v-else>
                      No information
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="clearfix"></div>

        <!---728x90--->
        <!-- /new_arrivals -->

        <!-- //new_arrivals -->
        <!--/slider_owl-->

        <div class="w3_agile_latest_arrivals">
          <h3 class="wthree_text_info">MOST <span>SELLERS</span></h3>

          <products
            class="col-md-3 product-men"
            v-for="mostSeller in mostSellers"
            :key="mostSeller.id"
            :product="mostSeller"
          ></products>

          <div class="clearfix"></div>
          <!--//slider_owl-->
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Vue from "vue";
import Products from "./Products.vue";
import axios from "axios";

export default {
  components: { Products },

  async created() {
    Vue.nextTick(() => {
      console.log("yuklendi", this.product);

      window.$("#horizontalTab").easyResponsiveTabs({
        type: "default", //Types: default, vertical, accordion
        width: "auto", //auto or any width like 600px
        fit: true, // 100% fit in a container
        closed: "accordion", // Start closed if in accordion view
        activate: function() {
          // Callback function if tab is switched
          var $tab = window.$(this);
          var $info = window.$("#tabInfo");
          var $name = window.$("span", $info);
          $name.text($tab.text());
          $info.show();
        },
      });
      window.$("#verticalTab").easyResponsiveTabs({
        type: "vertical",
        width: "auto",
        fit: true,
      });
    });
    this.load();
  },
  data() {
    return {
      mostSellers: [],
      hideImages: false,
    };
  },
  props: {
    product: {},
  },
  watch: {
    product() {
      this.load();
    },
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetMostSellers"
      );
      this.mostSellers = result.data;
      var self = this;
      Vue.nextTick(() => {
        self.hideImages = true;
        setTimeout(() => {
          self.hideImages = false;
          self.$forceUpdate();
          Vue.nextTick(() => {
            window.$(".flexslider").flexslider({
              animation: "slide",
              controlNav: "thumbnails",
            });
          });
        }, 10);
      });
    },
    async AddToCart() {
      var result = await axios.post(window.apiUrl + "CustomerCard/AddToCart", {
        Quantity: 1,
        Product: this.product,
        CustomerIdentify: this.$store.state.customerIdentify,
      });
      console.log(result);
      window.eventEmitter.dispatchEvent(new Event("cartChanged"));
    },
  },
};
</script>
<style>
* {
  box-sizing: border-box;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto,
    Oxygen-Sans, Ubuntu, Cantarell, "Helvetica Neue", sans-serif;
}

#w3lDemoBar.w3l-demo-bar {
  top: 0;
  right: 0;
  bottom: 0;
  z-index: 9999;
  padding: 40px 5px;
  padding-top: 70px;
  margin-bottom: 70px;
  background: #0d1326;
  border-top-left-radius: 9px;
  border-bottom-left-radius: 9px;
}

#w3lDemoBar.w3l-demo-bar a {
  display: block;
  color: #e6ebff;
  text-decoration: none;
  line-height: 24px;
  opacity: 0.6;
  margin-bottom: 20px;
  text-align: center;
}

#w3lDemoBar.w3l-demo-bar span.w3l-icon {
  display: block;
}

#w3lDemoBar.w3l-demo-bar a:hover {
  opacity: 1;
}

#w3lDemoBar.w3l-demo-bar .w3l-icon svg {
  color: #e6ebff;
}
#w3lDemoBar.w3l-demo-bar .responsive-icons {
  margin-top: 30px;
  border-top: 1px solid #41414d;
  padding-top: 40px;
}
#w3lDemoBar.w3l-demo-bar .demo-btns {
  border-top: 1px solid #41414d;
  padding-top: 30px;
}
#w3lDemoBar.w3l-demo-bar .responsive-icons a span.fa {
  font-size: 26px;
}
#w3lDemoBar.w3l-demo-bar .no-margin-bottom {
  margin-bottom: 0;
}
.toggle-right-sidebar span {
  background: #0d1326;
  width: 50px;
  height: 50px;
  line-height: 50px;
  text-align: center;
  color: #e6ebff;
  border-radius: 50px;
  font-size: 26px;
  cursor: pointer;
  opacity: 0.5;
}
.pull-right {
  float: right;
  position: fixed;
  right: 0px;
  top: 70px;
  width: 90px;
  z-index: 99999;
  text-align: center;
}
/* ============================================================
RIGHT SIDEBAR SECTION
============================================================ */

#right-sidebar {
  width: 90px;
  position: fixed;
  height: 100%;
  z-index: 1000;
  right: 0px;
  top: 0;
  margin-top: 60px;
  -webkit-transition: all 0.5s ease-in-out;
  -moz-transition: all 0.5s ease-in-out;
  -o-transition: all 0.5s ease-in-out;
  transition: all 0.5s ease-in-out;
  overflow-y: auto;
}

/* ============================================================
RIGHT SIDEBAR TOGGLE SECTION
============================================================ */

.hide-right-bar-notifications {
  margin-right: -300px !important;
  -webkit-transition: all 0.3s ease-in-out;
  -moz-transition: all 0.3s ease-in-out;
  -o-transition: all 0.3s ease-in-out;
  transition: all 0.3s ease-in-out;
}

@media (max-width: 992px) {
  #w3lDemoBar.w3l-demo-bar a.desktop-mode {
    display: none;
  }
}
@media (max-width: 767px) {
  #w3lDemoBar.w3l-demo-bar a.tablet-mode {
    display: none;
  }
}
@media (max-width: 568px) {
  #w3lDemoBar.w3l-demo-bar a.mobile-mode {
    display: none;
  }
  #w3lDemoBar.w3l-demo-bar .responsive-icons {
    margin-top: 0px;
    border-top: none;
    padding-top: 0px;
  }
  #right-sidebar,
  .pull-right {
    width: 90px;
  }
  #w3lDemoBar.w3l-demo-bar .no-margin-bottom-mobile {
    margin-bottom: 0;
  }
}
</style>
