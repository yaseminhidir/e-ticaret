<template>
  <div class="ban-top">
    <div class="container" style="float:left">
      <div class="top_nav_left">
        <nav class="navbar navbar-default">
          <div class="container-fluid">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
              <button
                type="button"
                class="navbar-toggle collapsed"
                data-toggle="collapse"
                data-target="#bs-example-navbar-collapse-1"
                aria-expanded="false"
              >
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
              </button>
            </div>
            <!-- Collect the nav links, forms, and other content for toggling -->
            <div
              class="collapse navbar-collapse menu--shylock"
              id="bs-example-navbar-collapse-1"
            >
              <ul class="nav navbar-nav menu__list">
                <li class="active menu__item menu__item--current">
                  <router-link class="menu__link" to="/"
                    >Home <span class="sr-only">(current)</span>
                  </router-link>
                </li>

                <li
                  class="dropdown menu__item"
                  v-for="parent in categories"
                  :key="parent.id"
                >
                  <a
                    href="#"
                    class="dropdown-toggle menu__link"
                    data-toggle="dropdown"
                    role="button"
                    aria-haspopup="true"
                    aria-expanded="false"
                    >{{ parent.name }} <span class="caret"></span
                  ></a>
                  <ul class="dropdown-menu multi-column columns-3">
                    <div class="agile_inner_drop_nav_info">
                      <div class="col-sm-6 multi-gd-img1 multi-gd-text ">
                        <router-link :to="'/products/' + parent.id"
                          ><img :src="parent.imageUrl" alt=" "
                        /></router-link>
                      </div>
                      <div class="col-sm-3 multi-gd-img">
                        <ul class="multi-column-dropdown">
                          <li
                            v-for="child in parent.inverseParent"
                            :key="child.id"
                          >
                            <router-link :to="'/products/' + child.id">{{
                              child.name
                            }}</router-link>
                          </li>
                        </ul>
                      </div>

                      <div class="clearfix"></div>
                    </div>
                  </ul>
                </li>
              </ul>
            </div>
          </div>
        </nav>
      </div>
    </div>

    <div class="dropdown menu__item" style=" height: 74px;   display: flex">
      <button
        style="margin-left: auto;
    margin-top: auto;
    margin-bottom: auto;
    margin-right:10px"
        class="w3view-cart dropdown-toggle stylebtn"
        type="button"
        id="dropdownMenu1"
        data-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded="true"
        value=""
        @click="LoadCart"
      >
        <i class="fa fa-cart-arrow-down" aria-hidden="true"></i>
        <span
          class="badge badge-danger"
          style=" position: absolute;
    top: 9px;
    right: 0px;"
          >{{ cartItemCount }}</span
        >
      </button>
      <ul class="dropdown-menu dropdown-menu-left ">
        <CartProductComponent
          @cartLoaded="cartChanged"
          ref="cartCommonReference"
        >
        </CartProductComponent>

        <div class="col-md-3"></div>
        <div class="col-md-3"></div>
        <div class="col-md-6" style="justify-content: end;display:flex">
          <router-link to="/sepet" v-if="cartItemCount > 0">
            Sepete Git
          </router-link>
        </div>
      </ul>
    </div>
    <div class="clearfix"></div>
  </div>
</template>

<script>
import axios from "axios";
import CartProductComponent from "./CartProductComponent.vue";
export default {
  data() {
    return {
      categories: [],
      cartItemCount: 0,
    };
  },
  components: { CartProductComponent },
  async created() {
    this.load();
  },
  methods: {
    cartChanged(cart) {
      var count = 0;
      for (const cartItem of cart.cartItems) {
        count += cartItem.quantity;
      }
      this.cartItemCount = count;
    },
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetCategoriesTopMenu"
      );
      this.categories = result.data;
      console.log(this.categories);
    },
    LoadCart() {
      this.$refs.cartCommonReference.CartLoad();
    },
  },
};
</script>

<style lang="scss">
.stylebtn {
  background-color: transparent;
}
.dropdown-menu-left {
  position: absolute;
  top: 100%;
  right: 0 !important;
  left: unset !important;
  z-index: 1000;
  display: none;
  float: left;
  width: 250px !important;
  padding: 5px 0;
  margin: 2px 0 0;
  font-size: 12px;
  text-align: left;
  list-style: none;
  background-color: #fff;
  -webkit-background-clip: padding-box;
  background-clip: padding-box;
  border: 1px solid #ccc;
  border: 1px solid rgba(0, 0, 0, 0.15);
  border-radius: 4px;
  -webkit-box-shadow: 0 6px 12px rgb(0 0 0 / 18%);
  box-shadow: 0 6px 12px rgb(0 0 0 / 18%);
}
</style>
