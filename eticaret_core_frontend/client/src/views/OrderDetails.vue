<template>
  <div class="container" style="margin-top:10px" v-if="!loading">
  
    <div class="col-sm-6">
      <div class="panel panel-default">
        <div class="panel-heading">
          <b> Adres Bilgileri </b>
        </div>
        <div  class="panel-body" style="height:250px">
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
        </div>
      </div>
    </div>
    <div class="col-sm-6">
      <div class="panel panel-default">
        <div class="panel-heading">
          <b> Ödeme Detayları </b>
        </div>
        <div class="panel-body" style="height:250px">
          <div class="col-sm-4">
            <p>Toplam Ürün:</p>
          </div>
          <div class="col-sm-4"></div>
          <div class="col-sm-4" style="display:flex; justify-content:end">
            <p>{{ TotalQuantity }} Adet</p>
          </div>
          <hr />
          <div class="col-sm-4">
            <p>Toplam Tutar:</p>
          </div>
          <div class="col-sm-4"></div>
          <div class="col-sm-4" style="display:flex; justify-content:end">
            {{ order.totalPrice }} TL
          </div>
        </div>
      </div>
    </div>
    <div class="col-sm-12">
      <div class="panel panel-default">
        <div class="panel-heading"><div class="row">
 <div class="col-sm-6">
            order Detail
          </div>
          <div class="col-sm-6"  style="display:flex; justify-content:end;color:green">
            {{ order.orderStatus}}
          </div>
        </div>
         
        </div>

        <div
          class="panel-body"
          v-for="order in order.orderDetails"
          style="border:1px solid rgb(153, 153, 153);display:flex;padiding:5px; margin:10px;border-radius:5px;
    justify-content: center;"
          :key="order.id"
        >
          <div
            class="col-sm-2 multi-gd-img1 multi-gd-text"
            style=" margin-right:10px;padding:0px"
          >
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
          </div>
          <div class="col-sm-9">
            <div class="row">
              <ul class="multi-column-dropdown">
                <li>
                  <h5 class="mt-0 text-left">{{ order.product.name }}</h5>
                  <p class="text-left">{{ order.product.description }}</p>
                  <p class="text-left">Adet: {{ order.quantity }}</p>
                  <p class="text-left">Date: {{ order.date }}</p>
                </li>
              </ul>
            </div>
            <div class="row">
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
            </div>

          
          </div>

          <div class="clearfix"></div>
        </div>
      </div>
    </div>
    
  </div>

  <div v-else> </div>
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
    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerCard/GetOrderById",
        { Id: this.id }
      );
      console.log(result.data);
      this.order = result.data;
      for (const orderDetail of this.order.orderDetails) {
        this.order.date = moment(this.order.date).format("MM/DD/YYYY HH:mm:ss");
        orderDetail.date = this.order.date;
      }
      this.loading=false;
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
