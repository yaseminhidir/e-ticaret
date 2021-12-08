<template>
  <div class="container" style="margin-top:10px" v-if="!loading">
    <md-card v-if="!showForm">
      <md-card-header>
        <div class="md-layout">
          <div class="md-layout-item">
            <div class="md-title">
              <b> Adres Bilgileri </b>
            </div>
          </div>
          <div class="md-layout-item"></div>
          <div class="md-layout-item end">
            <div class="md-title">
              <md-button
                class="md-dense md-primary"
                @click="showForm = !showForm"
                ><md-icon>
                  edit
                </md-icon></md-button
              >
            </div>
          </div>
        </div>
        <hr />
      </md-card-header>
      <md-card-content>
        <p>{{ order.address.name }} Adresi</p>
        <hr />
        <p>{{ order.address.address1 }}</p>
        <hr />
        <p>
          {{ order.address.zone }} /
          {{ order.address.city }}
        </p>
        <hr />
        <p>{{ order.address.country }}</p>
        <hr />
      </md-card-content>
    </md-card>
    <md-card v-if="showForm">
      <md-card-header>
        <div class="md-title">Adres Bilgileri</div>
        <hr />
      </md-card-header>
      <md-card-content>
        <md-field>
          <label for="name">Address Name</label>
          <md-input name="name" v-model="order.address.name" />
        </md-field>
        <md-field>
          <label for="address1">Address</label>
          <md-input name="address1" v-model="order.address.address1" />
        </md-field>
        <md-field>
          <label for="zone">Zone</label>
          <md-input name="zone" v-model="order.address.zone" />
        </md-field>
        <md-field>
          <label for="city">City</label>
          <md-input name="city" id="city" v-model="order.address.city">
          </md-input>
        </md-field>
        <md-field>
          <label for="country">Country</label>
          <md-input name="country" v-model="order.address.country" />
        </md-field>
      </md-card-content>
      <md-card-actions>
        <md-button class="md-primary" @click="SaveAddress">Save</md-button>
      </md-card-actions>
    </md-card>
    <md-card>
      <md-card-header>
        <div class="md-title">
          <b> Sipariş Detayları </b>
          <hr />
        </div>
      </md-card-header>

      <md-card-content>
        <p>Toplam Ürün: {{ TotalQuantity }} Adet</p>
        <hr />
        <p>Toplam Tutar: {{ order.totalPrice }} TL</p>
        <hr />
      </md-card-content>
    </md-card>

    <md-card>
      <md-card-header>
        <div class="md-title">
          <b> Statu </b>
          <hr />
        </div>
        <div class="md-layout" v-if="!showSelect">
          <div class="md-layout-item">
            {{ order.orderStatu.name }}
          </div>
          <div class="md-layout-item"></div>
          <div class="md-layout-item end">
            <md-button
              class="md-dense md-primary"
              @click="showSelect = !showSelect"
              ><md-icon>
                edit
              </md-icon></md-button
            >
          </div>
        </div>
        <div class="md-layout" v-if="showSelect">
          <div class="md-layout-item">
            <md-field>
              <label for="statu" style="font-size:11px">Statu</label>
              <md-select name="statu" id="statu" v-model="selectedStatu">
                <md-option
                  v-for="statu in orderStatus"
                  :key="statu.id"
                  :value="statu.id"
                  >{{ statu.name }}</md-option
                >
              </md-select>
            </md-field>
          </div>
          <div class="md-layout-item"></div>
          <div class="md-layout-item end" style="align-items:center">
            <md-button class="md-dense md-primary" @click="UpdateStatu"
              >SAVE</md-button
            >
          </div>
        </div>
      </md-card-header>
    </md-card>
    
    
    <div class="card-expansion">
      <md-card v-for="order in order.orderDetails" :key="order.id" :style="order.isCanceled ? 'background: rgb(128 128 128 / 16%)' : 'background:none'">
        <md-card-header>
          <md-card-media>
            <router-link :to="'/product/' + order.product.id">
              <img
                v-if="order.product.productImages.length > 0"
                :src="order.product.productImages[0].url"
              />
              <img
                v-else
                src="https://bt.altinbas.edu.tr/wp-content/uploads/2017/09/cropped-noimage.jpg"
              />
            </router-link>
          </md-card-media>
          <md-card-header-text>
            <div class="md-title">{{ order.product.name }}</div>
            <div class="md-subhead" v-html="order.product.description"></div>

            <p class="text-left">Adet: {{ order.quantity }}</p>
            <p class="text-left">Date: {{ order.date }}</p>
            <span
              class="item_price"
              style="color:rgb(47, 218, 184)"
              v-if="order.product.calculatedPrice"
            >
              <b> {{ order.product.calculatedPrice }} TL</b>
            </span>
            <span class="item_price" v-else style="color:rgb(47, 218, 184)">
              <b> {{ order.product.price }} TL </b>
            </span>

            <del v-if="order.product.calculatedPrice" style="color:red"
              >{{ order.product.price }} TL
            </del>
          </md-card-header-text>
        </md-card-header>

        <md-card-actions>
          <div v-if="!order.isCanceled">
            <md-button @click="CancelProduct(order)">Cancel</md-button>
          </div>
             <div v-else>
            <b style="color:red"> İPTAL EDİLDİ</b> 
          </div>
        </md-card-actions>
      </md-card>
    </div>

    <div class="clearfix"></div>
  </div>

  <div v-else></div>
</template>
<script>
import axios from "axios";
import moment from "moment";

export default {
  props: {
    id: {},
  },
  data() {
    return {
      order: {},
      loading: true,
      showForm: false,
      showSelect: false,
      selectedStatu: "",
      orderStatus: [],
    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "AdminOrder/GetOrderById",
        { Id: this.id }
      );
      console.log(result.data);
      this.order = result.data;
      for (const orderDetail of this.order.orderDetails) {
        this.order.date = moment(this.order.date).format("MM/DD/YYYY HH:mm:ss");
        orderDetail.date = this.order.date;
      }
      var resultStatu = await axios.post(
        window.apiUrl + "AdminOrder/GetOrderStatus"
      );
      this.orderStatus = resultStatu.data;
      console.log(resultStatu);
      this.loading = false;
    },

    async UpdateStatu() {
      var resultStatu = await axios.post(
        window.apiUrl + "AdminOrder/UpdateStatu",
        { StatuId: this.selectedStatu, Id: this.order.id }
      );
      this.showSelect = false;
      this.load();
      console.log(resultStatu);
    },
    async SaveAddress() {
      var resultAddress = await axios.post(
        window.apiUrl + "AdminOrder/EditAddress",
        this.order.address
      );
      this.showForm = false;
      console.log(resultAddress);
    },
    async CancelProduct(orderDetail) {
      var resultCancel = await axios.post(
        window.apiUrl + "AdminOrder/CancelProduct",
       orderDetail
      );
      this.cancel=resultCancel.data.IsCanceled;
      console.log(resultCancel);
    },
  },
  computed: {
    TotalQuantity() {
      var toplam = 0;
      for (const product of this.order.orderDetails) {
        toplam = toplam + product.quantity;
      }
      return toplam;
    },
  },
};
</script>

<style lang="scss">
.md-card-content {
  padding: 16px;
  font-size: 11px !important;
  line-height: 22px;
}
.md-field.md-has-value .md-input,
.md-field.md-has-value .md-textarea {
  font-size: 11px !important;
}
.md-card .md-title {
  font-size: 16px !important;
  letter-spacing: 0;
  line-height: 32px;
}
.md-app-content .md-card {
  margin-right: 16px;
  margin-left: 16px;
  margin-top: 20px;
  overflow: visible;
}
.md-list-item-container:not(.md-list-item-default):not([disabled])
  > .md-list-item-content {
  -webkit-user-select: none;
  user-select: none;
  cursor: pointer;
  font-size: 11px !important;
}
</style>
