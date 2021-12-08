<template>
  <div>
    <div class="men-pro-item simpleCart_shelfItem">
      <div class="men-thumb-item">
        <img
          :src="product.productImages[0].url"
          alt=""
          class="pro-image-front"
          v-if="product.productImages.length > 0"
        />
        <img
          src="https://bt.altinbas.edu.tr/wp-content/uploads/2017/09/cropped-noimage.jpg"
          alt=""
          class="pro-image-front"
          v-else
        />

        <img
          v-if="product.productImages.length > 0"
          :src="product.productImages[0].url"
          alt=""
          class="pro-image-back"
        />
        <img
          src="https://bt.altinbas.edu.tr/wp-content/uploads/2017/09/cropped-noimage.jpg"
          alt=""
          class="pro-image-back"
          v-else
        />

        <div class="men-cart-pro">
          <div class="inner-men-cart-pro">
            <router-link
              :to="'/product/' + product.id"
              class="link-product-add-cart"
              >Quick View</router-link
            >
          </div>
        </div>
        <span class="product-new-top">New</span>
      </div>
      <div class="item-info-product ">
        <h4>
          <router-link :to="'/product/' + product.id">{{
            product.name
          }}</router-link>
        </h4>
        <h4 v-if="product.manufacturer">
          <router-link :to="'/product/' + product.id">{{
            product.manufacturer.name
          }}</router-link>
        </h4>
        <h4 v-else>
          <router-link :to="'/product/' + product.id">Noname</router-link>
        </h4>
        <div class="info-product-price">
          <span class="item_price" v-if="product.calculatedPrice">
            {{ product.calculatedPrice }} TL
          </span>
          <span class="item_price" v-else> {{ product.price }} TL </span>
          <del v-if="product.calculatedPrice">{{ product.price }}</del>
        </div>
        <div
          class="snipcart-details top_brand_home_details item_add single-item hvr-outline-out button2"
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
  </div>
</template>

<script>
import axios from "axios";
export default {
  props: {
    product: {},
  },
  
  methods: {
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
<style lang="scss">
.button {
  font-size: 13px;
  color: #fff;
  background: #2fdab8;
  text-decoration: none;
  position: relative;
  border: none;
  border-radius: 0;
  width: 100%;
  text-transform: uppercase;
  padding: 0.5em 0;
  outline: none;
  letter-spacing: 1px;
  font-weight: 600;
}
.men-thumb-item .pro-image-front {
    opacity: 1;
    visibility: visible;
    height: 300px;
    width: auto;
    max-width: 100%;
    object-fit: cover;
   
}
.men-pro-item {
   
    height: 100%;
}

.product-men {
 
    height: 458px;
}
img.pro-image-back {
  opacity: 1;
    visibility: visible;
    height: 300px;
    width: auto;
    max-width: 100%;
    object-fit: cover;
}
.item_price {
  font-size: 13px;
  color: #fc636b;

  text-decoration: none;
  position: relative;
  border: none;
  border-radius: 0;
  width: 100%;
  text-transform: uppercase;
  padding: 0.5em 0;
  outline: none;
  letter-spacing: 1px;
  font-weight: 600;
}
</style>
