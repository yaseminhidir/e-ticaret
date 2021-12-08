<template>
  <div class="container">
    <div class="banner-bootom-w3-agileits">
      <div class="responsive_tabs_agileits">
        <div id="horizontalTab">
          <ul class="resp-tabs-list">
            <li>Profile Details</li>
            <li>Adresses</li>
            <li>Orders</li>
          </ul>
          <div class="resp-tabs-container">
            <!--/tab_one-->
            <div class="tab1">
              <div class="single_page_agile_its_w3ls">
                <h6>Profile Details</h6>
                <div class="panel panel-default">
                  <div class="panel-body">
                    <div class="modal-body modal-body-sub_agile">
                      <div>
                        <div class="styled-input agile-styled-input-top">
                          <input
                            type="text"
                            name="Name"
                            v-model="customer.firstName"
                            required=""
                          />
                          <label>Name</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="text"
                            name="Name"
                            v-model="customer.lastName"
                            required=""
                          />
                          <label> Last Name</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="email"
                            name="Email"
                            v-model="customer.email"
                            required=""
                          />
                          <label>Email</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="number"
                            class="form-number"
                            name="telephone"
                            v-model="customer.telephone"
                            required=""
                          />
                          <label>Phone</label>
                          <span></span>
                        </div>

                        <button
                          class="form-submit-button"
                          @click="updateProfile"
                        >
                          Save
                        </button>

                        <div class="clearfix"></div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <!--//tab_one-->
            <div class="tab1">
              <div class="single_page_agile_its_w3ls">
                <h6>Addresses</h6>

                <div
                  class="panel panel-default"
                  v-for="address in customer.addresses"
                  :key="address.id"
                >
                  <p style="margin-left:47px;    text-decoration: underline;">
                    Address 1
                  </p>
                  <div class="panel-body">
                    <div class="modal-body modal-body-sub_agile">
                      <div>
                        <div class="styled-input agile-styled-input-top">
                          <input
                            type="text"
                            name="Name"
                            v-model="address.address1"
                            required=""
                          />
                          <label>Address</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="text"
                            name="city"
                            v-model="address.city"
                            required=""
                          />
                          <label>City</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="text"
                            name="Country"
                            v-model="address.country"
                            required=""
                          />
                          <label>Country</label>
                          <span></span>
                        </div>
                        <div class="styled-input">
                          <input
                            type="text"
                            name="Zone"
                            v-model="address.zone"
                            required=""
                          />
                          <label>Zone</label>
                          <span></span>
                        </div>
                        <div class="col-md-5"></div>
                        <div class="col-md-5"></div>
                        <div class="col-md-2">
                          <button
                            class="form-submit-button-sil"
                            @click="sil(address)"
                          >
                            Sil
                          </button>
                        </div>
                        <div class="clearfix"></div>
                      </div>
                    </div>
                  </div>
                </div>
                <div style="margin-bottom:5px" class="col-md-5"></div>
                <div v-if="showSave" style="margin-bottom:5px" class="col-md-2">
                  <button class="form-submit-button" @click="save">
                    Save
                  </button>
                </div>
                <div style="margin-bottom:5px" class="col-md-5"></div>

                <button class="addnewbutton" @click="newAddress">
                  Add New Address
                </button>
              </div>
            </div>
            <div class="tab3">
              <div class="single_page_agile_its_w3ls">
                <div class="div" >
                  <div class="panel" v-for="order in orders" :key="order.id">
                    <div class="row">
                      <div class="col-sm-3">
                        Sipariş tarihi
                      </div>
                      <div class="col-sm-3">
                        Sipariş Özeti
                      </div>
                      <div class="col-sm-3">
                        Sipariş Tutarı
                      </div>
                      <div class="col-sm-3">
                        <router-link :to="'orderdetails/' + order.id " class="addnewbutton" >
                          Sipariş Detayı
                        </router-link>
                      </div>
                      <div class="col-sm-3">
                        {{ order.date }}
                      </div>
                      <div class="col-sm-3">{{ totalPrice(order) }} Ürün</div>
                      <div class="col-sm-3">{{ order.totalPrice }} TL</div>
                      <div class="col-3"></div>
                    </div>
                  </div>
                </div>
                 
              
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Vue from "vue";
import Swal from "sweetalert2";
import moment from "moment";

export default {
  data() {
    return {
      customer: {},
      showSave: false,
      orders: [],
      
    };
  },
  async created() {
    Vue.nextTick(() => {
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
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProfile/GetProfileDetails"
      );
      this.customer = result.data;
      console.log(this.customer);
      if (this.customer.addresses.length > 0) {
        this.showSave = true;
      } else {
        this.showSave = false;
      }
      var resultOrder = await axios.post(
        window.apiUrl + "CustomerCard/GetOrders"
      );
      console.log(resultOrder.data);
      this.orders = resultOrder.data;
      for (const order of this.orders) {
        order.date = moment(order.date).format("MM/DD/YYYY HH:mm:ss");
      }
    },
    totalPrice(order) {
      var toplam = 0;
      for (const product of order.orderDetails) {
        toplam = toplam + product.quantity;
      }
      return toplam;
    },
    newAddress() {
      this.customer.addresses.push({});
      this.showSave = true;
    },
    async sil(address) {
      var result = await axios.post(
        window.apiUrl + "CustomerProfile/DeleteAddress",
        address
      );
      console.log(result.data);
      Swal.fire("success", result.data.message, "success");
      this.load();
    },
    async updateProfile() {
      var resultUpdate = await axios.post(
        window.apiUrl + "AuthCustomer/Register",
        this.customer
      );

      console.log(resultUpdate);
      if (resultUpdate.data.message == false) {
        return Swal.fire("error", resultUpdate.data.message, "error");
      }
      Swal.fire("Güncellendi", resultUpdate.data.message, "Güncellendi");
      this.customer = resultUpdate.data.response;
    },
   
    async save() {
      var result = await axios.post(
        window.apiUrl + "CustomerProfile/AddAdresses",
        this.customer.addresses
      );
      if (result.data.success == false) {
        return Swal.fire("error", result.data.message, "error");
      }
      Swal.fire("success", result.data.message, "success");
      this.load();
      console.log(result.data);
    },
  },
};
</script>

<style lang="scss">
.form-submit-button-sil:hover {
  background: brown;
}
.form-submit-button-sil {
  transition: 0.5s all;
  -webkit-transition: 0.5s all;
  border: none;
  padding: 10px 40px 10px;
  font-size: 13px;
  outline: none;
  text-transform: uppercase;
  margin: 0 0 0 -4px;
  font-weight: 700;
  letter-spacing: 1px;
  background: #fc636b;
  color: #fff;
}
.addnewbutton {
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
</style>
