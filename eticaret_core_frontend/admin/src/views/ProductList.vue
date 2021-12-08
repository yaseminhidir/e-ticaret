<template>
  <div>
    <div
      v-if="showAlert"
      class="alert alert-success alert-dismissible fade show"
      role="alert"
    >
      {{ message }}
      <button
        type="button"
        class="close"
        data-dismiss="alert"
        aria-label="Close"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
   <div class="row justify-content-end">
        <md-button class="md-fab md-mini md-primary" to="/addproduct/0">
          <md-icon>add</md-icon>
        </md-button>
        <md-button class="md-fab md-mini md-accent " @click="DeleteProduct">
          <md-icon>delete</md-icon>
        </md-button>
      </div>
    <div class="md-layout md-gutter">
      <md-card class="md-layout-item md-size-100" v-if="showfilter">
        <md-card-content>
          <md-field class="md-toolbar-section-start">
            <label for="categories">Categories</label>
            <md-select md-dense multiple v-model="SelectedCategories">
              <md-option
                v-for="category in Categories"
                :key="category.id"
                :value="category.id"
                >{{ category.fullName }}</md-option
              >
            </md-select>
          </md-field>
          <md-field class="md-toolbar-section-start">
            <label for="manufacturers">Manufacturers</label>
            <md-select md-dense multiple v-model="SelectedManufacturers">
              <md-option
                v-for="manufacturer in Manufacturers"
                :key="manufacturer.id"
                :value="manufacturer.id"
                >{{ manufacturer.name }}</md-option
              >
            </md-select>
          </md-field>
          <md-field>
            <label for="stockMin"> Min Stock </label>
            <md-input v-model.number="stockMin" type="number"></md-input>
          </md-field>
          <md-field>
            <label for="stockMax"> Max Stock </label>
            <md-input v-model.number="stockMax" type="number"></md-input>
          </md-field>
          <md-button class="md-raised" @click="Filter">Filtrele</md-button>
        </md-card-content>
      </md-card>
   

      <div class="md-layout-item md-size-100">
        <md-table>
          <md-table-toolbar>
            <div class="md-toolbar-section-start">
              <h5>Products</h5>
              <small @click="showfilter = !showfilter">Filter</small>
            </div>

            <md-field md-clearable class="md-toolbar-section-end">
              <md-input placeholder="Search by name..." v-model="search" />
            </md-field>
          </md-table-toolbar>

          <md-table-row>
            <md-table-cell>
              <md-checkbox
                v-model="DeleteProducts"
                v-if="Products.length > 1"
                @change="checkAll"
              ></md-checkbox
            ></md-table-cell>
            <md-table-cell> <span class="text-muted">ID</span></md-table-cell>
           
            <md-table-cell
              ><span class="text-muted">Product Name</span></md-table-cell
            >
            <md-table-cell
              ><span class="text-muted">Quantity</span></md-table-cell
            >
            <md-table-cell
              ><span class="text-muted">Stock Statu</span></md-table-cell
            >
            <md-table-cell
              ><span class="text-muted">Date Added</span></md-table-cell
            >
            <md-table-cell
              ><span class="text-muted">Date Modified</span></md-table-cell
            >
            <md-table-cell>
              <span class="text-muted">Price</span></md-table-cell
            >
            <md-table-cell>
              <span class="text-muted">Action </span></md-table-cell
            >
          </md-table-row>

          <md-table-row v-for="Product in ProductsFiltered" :key="Product.id">
            <md-table-cell>
              <md-checkbox
                v-model="DeleteProducts"
                :value="Product.id"
                md-selectable="multiple"
              ></md-checkbox
            ></md-table-cell>
            <md-table-cell>{{ Product.id }}</md-table-cell>
           
            <md-table-cell>{{ Product.name }}</md-table-cell>
            <md-table-cell>{{ Product.quantity }}</md-table-cell>
            <md-table-cell>{{ Product.stockStatu.name }}</md-table-cell>
            <md-table-cell>{{ Product.dateAdded }}</md-table-cell>

            <md-table-cell>{{ Product.dateModified }}</md-table-cell>
            <md-table-cell>{{ Product.price }}</md-table-cell>
            <md-table-cell>
              <md-button
                class="md-fab md-mini md-plain"
                :to="'/addproduct/' + Product.id"
              >
                <md-icon>edit</md-icon>
              </md-button></md-table-cell
            >
          </md-table-row>
        </md-table>
        <md-card v-if="ProductsFiltered.length == 0">
          <md-card-header><h4>No products found</h4></md-card-header>
          <md-card-content
            ><p>
              {{
                `No product found for this '${search}' query. Try a different search term or create a new product.`
              }}
            </p>
            <md-button class="md-primary md-raised" to="/addproduct/0"
              >Create New Product</md-button
            >
          </md-card-content>
        </md-card>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import moment from "moment";
export default {
  data() {
    return {
      showfilter: false,
      showAlert: false,
      message: "",
      Products: [],
      search: "",
      DeleteProducts: [],
      Categories: [],
      Manufacturers: [],
      SelectedManufacturers: [],
      SelectedCategories: [],
      stockMin: "",
      stockMax: "",
    };
  },
  methods: {
    checkAll() {
      if (this.DeleteProducts.length >= this.Products.length) {
        this.DeleteProducts = [];
      } else {
        for (var item of this.Products) {
          this.DeleteProducts.push(item.id);
          console.log(this.DeleteProducts);
        }
      }

      this.$forceUpdate();
    },
    async load() {
      //this.SelectedCategories
      //this.SelectedManufacturers
      var result = await axios.post(
        window.apiUrl + "AdminProduct/GetProducts",
        {
          ManufacturerIds: this.SelectedManufacturers,
          CategoryIds: this.SelectedCategories,
          stockMin: this.stockMin,
          stockMax: this.stockMax,
        }
      );
      //dateAdded dateModified
      for (const Product of result.data) {
        Product.dateAdded = moment(Product.dateAdded).format(
          "DD-MM-YYYY HH:mm:ss"
        );
        Product.dateModified = moment(Product.dateModified).format(
          "DD-MM-YYYY HH:mm:ss"
        );
      }
      this.Products = result.data;
      console.log(this.Products);
      var resultCategories = await axios.post(
        window.apiUrl + "AdminProduct/GetCategories"
      );
      this.Categories = resultCategories.data;
      var resultManufacturers = await axios.post(
        window.apiUrl + "AdminProduct/GetManufacturers"
      );
      console.log(resultManufacturers);
      this.Manufacturers = resultManufacturers.data;
      console.log(this.Manufacturers);
    },
    async Filter() {
      this.load();
      // iki filtre alanı oluştur, birisi min stok, diğeri max stok. textboxla sayı girilcek.
      //stok>min , stok<max, qquerye ekle. min stok varsa, max stok varsa.
    },
    async DeleteProduct() {
      var resultDelete = await axios.post(
        window.apiUrl + "AdminProduct/DeleteProduct",
        this.DeleteProducts
      );
      this.showAlert = true;
      this.message = resultDelete.data.message;
      console.log(resultDelete);
      this.load();
    },
  },
  async created() {
    this.load();
  },
  computed: {
    ProductsFiltered() {
      var self = this;
      return this.Products.filter((x) =>
        x.name.toLowerCase().includes(self.search.toLowerCase())
      );
    },
  },
};
</script>
<style lang="scss">
.product-image {
  max-height: 40px;
}
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
