<template>
  <div v-if="cart.cartItems.length > 0">
    <div
      class="panel-body cart"
      v-for="cartItem in cart.cartItems"
     
      :key="cartItem.Id"
    >
      <div
        class="col-sm-2 multi-gd-img1 multi-gd-text"
        style=" margin-right:10px;padding:0px"
      >
        <router-link :to="'/product/' + cartItem.product.id">
          <img
            v-if="cartItem.product.productImages.length > 0"
            :src="cartItem.product.productImages[0].url"
          />
          <img
            v-else
            src="https://bt.altinbas.edu.tr/wp-content/uploads/2017/09/cropped-noimage.jpg"
          />
        </router-link>
      </div>
      <div class="col-sm-9">
        <div class="row">
          <ul class="multi-column-dropdown">
            <li>
              <h5 class="mt-0 text-left">{{ cartItem.product.name }}</h5>
              <p class="text-left" v-html="cartItem.product.description"></p>
             
              <table class="table table-responsive" style="width:3px">
                <thead>
                  <tr>
                    <th>
                   Adet:
                   </th>
                    <th>
                      <button
                        @click="AddToCart(cartItem.product, 1)"
                        style="background: transparent;border: none;"
                      >
                        <i class="fa fa-plus" aria-hidden="true"></i>
                      </button>
                    </th>
                      <th>
                    <span> {{ cartItem.quantity }}</span>
                    </th>
                    <th>
                      <button
                  @click="AddToCart(cartItem.product, -1)"
                  style="background: transparent;border: none;"
                >
                  <i class="fa fa-minus" aria-hidden="true"></i>
                </button>
                    </th>
                  </tr>
                </thead>
              </table>
      
            </li>
          </ul>
        </div>
        <div class="row">
          <span
            class="item_price"
            style="color:rgb(47, 218, 184)"
            v-if="cartItem.product.calculatedPrice"
          >
            <b> {{ cartItem.product.calculatedPrice }} TL</b>
          </span>
          <span class="item_price" v-else style="color:rgb(47, 218, 184)">
            <b> {{ cartItem.product.price }} TL </b>
          </span>

          <del v-if="cartItem.product.calculatedPrice" style="color:red"
            >{{ cartItem.product.price }} TL
          </del>
        </div>
      </div>

      <div class="clearfix"></div>
    </div>
    <div class="col-sm-4"></div>
    <div class="col-sm-4"></div>
  </div>
  <div v-else  >
    <div class="panel-body cart">
     <div class="col-sm-12">
       Sepetinizde ürün bulunmamaktadır.
      </div>
     
   
  </div>
    <div class="panel-footer">
 <router-link to="/">
    Şimdi Alışverişe Başlayın</router-link>
       </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      cart: { cartItems: [] },
    };
  },
  async created() {
    window.eventEmitter.addEventListener("cartChanged", this.CartLoad);

    this.CartLoad();
  },
  methods: {
    async CartLoad() {
      var resultCart = await axios.post(
        window.apiUrl + "CustomerCard/GetCartItems",
        { CustomerIdentify: this.$store.state.customerIdentify }
      );
      this.cart = resultCart.data;
      window.eventEmitter.dispatchEvent(
        new CustomEvent("cartData", { detail: this.cart })
      );
      this.$emit("cartLoaded", this.cart);
    },
    async AddToCart(product, quantity) {
      var result = await axios.post(
        window.apiUrl + "CustomerCard/AddToCart",
        {
          Quantity: quantity,
          Product: product,
          CustomerIdentify: this.$store.state.customerIdentify,
        }
      );
      window.eventEmitter.dispatchEvent(new Event("cartChanged"));
      console.log(result);
    },
  },
};
</script>
<style lang="scss">

.cart{    border:1px solid rgb(153, 153, 153);display:flex;padding:5px; margin:10px;border-radius:5px;
    justify-content: center
}
  </style>
