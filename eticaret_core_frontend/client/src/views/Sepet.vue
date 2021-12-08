<template>
  <div>
    <div
      class="container"
      style="margin-top:30px;"
      v-show="cart.cartItems.length > 0"
    >
      <div class="col-md-6">
        <div
          class="panel panel-default"
          style="border: 1px solid rgb(153 153 153); "
        >
          <div
            class="panel-heading"
            style="border-bottom: 1px solid rgb(153 153 153);"
          >
            <h3 class="panel-title">Cart</h3>
          </div>

          <!-- <div
      class="agile_inner_drop_nav_info"
      
    > -->
          <CartProductComponent @cartLoaded="cartLoaded">

            
          </CartProductComponent>

          <div class="clearfix"></div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="row">
          <div
            class="panel-heading"
            style="border-bottom: 1px solid rgb(153 153 153);backgorund:transparent"
          >
            <h3 class="panel-title">Kayıtlı Adreslerim</h3>
          </div>

          <div class="col-sm-4" @click="push" style="cursor:pointer">
            <div
              class="address-div"
              style="display:flex;align-items:center;
    justify-content:center;"
            >
              <i class="fa fa-plus" aria-hidden="true"></i>
            </div>
          </div>
          <div class="col-sm-4" v-for="address in addresses" :key="address.id">
            <div class="address-div">
              <input type="radio" :value="address.id" v-model="addressId" />

              <b> {{ address.name }} </b>

              <p class="address">{{ address.address1 }}</p>

              <p class="address" style="white-space:nowrap">
                {{ address.zone }}/{{ address.city }}
              </p>

              <p class="address">{{ address.country }}</p>
            </div>
          </div>
        </div>
        <div class="row" style="margin-top:10px">
          <div class="panel panel-default">
            <div class="panel-heading">
              Sipariş Detayı
            </div>
            <div class="panel-body">
              <div class="div" v-if="SelectedAddress">
                <p>{{ SelectedAddress.address1 }}</p>
                <p>{{ SelectedAddress.zone }} / {{ SelectedAddress.city }}</p>
                <p>{{ SelectedAddress.country }}</p>
              </div>
              <div class="panel-footer" style="height:50px">
                <div class="col-sm-4"></div>
                <div class="col-sm-4"></div>
                <div class="col-sm-4">
                  <h6>Toplam tutar : {{ cart.cartTotalPrice }} TL</h6>
                
                </div>
                 
              </div>
               <div class="panel-footer" style="height:50px">
                <div class="col-sm-4"></div>
                <div class="col-sm-4"></div>
                <div class="col-sm-4">
               <button class="btn btn-success" @click="PlaceOrder">Siparişi Tamamla</button> </div></div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-show="cart.cartItems.length <= 0">
      Sepetinizde ürün bulunmamaktadır.
          <router-link to="/">Şimdi Alışverişe Başlayın</router-link>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import swal from "sweetalert2";
import CartProductComponent from "./CartProductComponent.vue";

export default {
  components: { CartProductComponent },
  data() {
    return {
      addresses: [],
      addressId: null,
      cart: { cartItems: [] },
    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProfile/GetProfileDetails"
      );
      this.addresses = result.data.addresses;
      console.log(this.addresses);
    },

    cartLoaded(cart) {
      this.cart = cart;
    },

    async PlaceOrder() {
      var result = await axios.post(
        window.apiUrl + "CustomerCard/PlaceOrder",
        {
          AddressId: this.addressId,
          CustomerIdentify: this.$store.state.customerIdentify,
        }
      );
      console.log(result);
      if (result.data.success == false) {
        swal.fire("error", result.data.message, "error");
      } else {
        swal.fire("success", result.data.message, "success");
        window.location.reload();
      }
    },
    push() {
      this.$router.push("/profile");
    },
  },
  computed: {
    SelectedAddress() {
      if (this.addressId != null) {
        return this.addresses.filter((x) => x.id == this.addressId)[0];
      } else return false;
    },
  },
};
</script>
<style lang="scss">
.panel {
  -webkit-box-shadow: 10px 10px 22px -4px rgba(0, 0, 0, 0.24);
  box-shadow: 10px 10px 22px -4px rgba(0, 0, 0, 0.24);
}
.address-div {
  background-color: rgb(102 102 102 / 9%);
  border: 1px solid rgb(153 153 153);
  padding: 10px;
  border-radius: 5px;
  height: 150px;
  margin-top: 10px;
}
.address {
  overflow: hidden;
  text-overflow: ellipsis;

  width: 100%;
}
.container {
  font-size: 10px;
}
</style>
