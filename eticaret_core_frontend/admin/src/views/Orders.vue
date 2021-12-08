<template>
  <div>
    <md-table>
      <md-table-toolbar>
        <div class="md-toolbar-section-start">
          <h5>Orders</h5>
        </div>

        <md-field md-clearable class="md-toolbar-section-end">
          <md-input placeholder="Search by name..." v-model="search" />
        </md-field>
      </md-table-toolbar>

      <md-table-row>
        <md-table-head md-numeric>ID</md-table-head>

        <md-table-head>Customer Name</md-table-head>
           <md-table-head>Address Name</md-table-head>
        <md-table-head>Date</md-table-head>
        <md-table-head>Status</md-table-head>
        <md-table-head>Product Quantity</md-table-head>
        <md-table-head>Total Price</md-table-head>
        <md-table-head></md-table-head>
      </md-table-row>
      <md-table-row v-for="order in OrdersFiltered" :key="order.id">
        <md-table-cell md-numeric>{{ order.id }}</md-table-cell>
        <md-table-cell
          >{{ order.customer.firstName }}
          {{ order.customer.lastName }}</md-table-cell
        >
              <md-table-cell>{{ order.address.name }}</md-table-cell>
        <md-table-cell>{{ order.date }}</md-table-cell>
        <md-table-cell>{{ order.orderStatu.name }}</md-table-cell>
        <md-table-cell>{{ totalQuantity(order) }}</md-table-cell>
        <md-table-cell>{{ order.totalPrice }} TL</md-table-cell>
        <md-table-cell>
          <router-link :to="'orderdetail/' + order.id">
            Order Detail
          </router-link>
        </md-table-cell>
      </md-table-row>
    </md-table>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";

export default {
  data() {
    return {
      orders: [],
      search: "",
    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var resultOrder = await axios.post(
        window.apiUrl + "AdminOrder/GetOrders"
      );
      console.log(resultOrder.data);
      this.orders = resultOrder.data;
      for (const order of this.orders) {
        order.date = moment(order.date).format("MM/DD/YYYY HH:mm:ss");
      }
    },
    totalQuantity(order) {
      var toplam = 0;
      for (const product of order.orderDetails) {
        toplam = toplam + product.quantity;
      }
      return toplam;
    },
  },
  computed: {
    OrdersFiltered() {
      var self = this;
      return this.orders.filter((x) =>
        x.customer.firstName.toLowerCase().includes(self.search.toLowerCase())
      );
    },
  },
};
</script>

<style lang="scss">
.md-table-head-label {
  height: 28px;
  padding-right: 32px;
  padding-left: 24px;
  display: inline-block;
  position: relative;
  line-height: 28px;
  font-size: 11px;
}
.md-table-cell-container {
  padding: 6px 32px 6px 24px;
  font-size: 11px;
}
</style>
