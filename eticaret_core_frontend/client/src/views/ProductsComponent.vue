<template>
  <div>
    <div class="page-head_agile_info_w3l">
      <div class="container">
        <!--/w3_short-->
        <div class="services-breadcrumb">
          <div class="agile_inner_breadcrumb">
            <ul class="w3_short">
              <li><router-link to="/">Home</router-link><i>|</i></li>
            </ul>
          </div>
        </div>
        <!--//w3_short-->
      </div>
    </div>
    <div class="banner-bootom-w3-agileits">
      <div class="container">
        <!-- mens -->
        <div class="col-md-3 products-left">
          <div class="filter-price">
            <h3>Filter By <span>Price</span></h3>
            <ul class="dropdown-menu6">
              <li>
                <div>
                  <div class="app-content">
                    <Slider :min="minPrice" :max="maxPrice" v-model="value" />
                  </div>
                </div>
              </li>
            </ul>
          </div>
          <div class="css-treeview">
            <h4>Categories</h4>
            <ul>
              <menu-category-item
                :category="parent"
                v-for="parent in categories"
                :key="parent.id"
              ></menu-category-item>
            </ul>
          </div>
          <div class="community-poll">
            <h4>Stock</h4>
            <div class="swit form">
              <div
                class="check_box"
                v-for="stockStatu in stockStatus"
                :key="stockStatu.id"
              >
                <div class="checkbox">
                  <label
                    ><input
                      type="checkbox"
                      name="checkbox"
                      checked=""
                      id="stockStatu.id"
                      :value="stockStatu.id"
                      v-model="selectedStatus"
                    /><i></i>{{ stockStatu.name }}</label
                  >
                </div>
              </div>
            </div>
          </div>
          <div class="community-poll">
            <h4>Brands</h4>
            <div style="padding:8px">
              <input
                type="text"
                class="searchBrand"
                placeholder="Search by name..."
                v-model="search"
                style="width:100%;padding:8px"
              />
            </div>
            <div class="swit form">
              <div
                class="check_box"
                v-for="manufacturer in BrandsFiltered"
                :key="manufacturer.id"
              >
                <div class="checkbox">
                  <label
                    ><input
                      type="checkbox"
                      :value="manufacturer.id"
                      v-model="selectedManufacturers"
                    /><i></i>{{ manufacturer.name }}</label
                  >
                </div>
              </div>
            </div>
          </div>
          <div class="community-poll">
            <button class="filterbutton" @click="filteredProducts">
              Filtrele
            </button>
          </div>
          <div class="clearfix"></div>
        </div>

        <div class="col-md-9 products-right">
          <h5>{{ category.name }}</h5>
          <div class="sort-grid">
            <div class="clearfix"></div>
          </div>
          <div v-if="!noProductsFound">
            <Products
              class="col-md-4  product-men"
              v-for="product in products"
              :key="product.id"
              :product="product"
            ></Products>
          </div>
          <div class="row " v-else>
            <div class="col-md-4">
 
            </div>
            <div class="col-md-4">  <h4 style="
    font-family: 'Open Sans', sans-serif;color: #0c0c0c;
    text-align: center;
    text-transform: uppercase;
    font-size: 1.5em;
    font-weight: 700;
    letter-spacing: 1px;
"> Ürün Bulunamadı </h4></div>
            <div class="col-md-4">

       
          
            </div>
          </div>
        
        </div>

        <div class="clearfix"></div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import Slider from "@vueform/slider/dist/slider.vue2.js";
import MenuCategoryItem from "./MenuCategoryItem.vue";
import Products from "./Products.vue";

export default {
  data() {
    return {
      products: [],
      categories: [],
      value: [0, 0],
      stockStatus: [],
      Manufacturers: [],
      selectedStatus: [],
      selectedManufacturers: [],
      resultCategories: [],
      search: "",
      maxPrice: 0,
      minPrice: 0,
      category: "",
      noProductsFound: false,
    };
  },

  props: {
    id: {},
  },

  async created() {
    this.load(true);

    this.bgStyle = {
      backgroundColor: "#fff",
      boxShadow: "inset 0.5px 0.5px 3px 1px rgba(0,0,0,.36)",
    };
    this.tooltipStyle = {
      backgroundColor: "#666",
      borderColor: "#666",
    };
    this.processStyle = {
      backgroundColor: "#999",
    };
  },
  methods: {
    async load(isFirstLoad) {
      if (isFirstLoad) {
        this.value = [0, 0];
      }
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetProducts",
        {
          CategoryId: this.id,
          SelectedStatus: this.selectedStatus,
          SelectedManufacturersId: this.selectedManufacturers,
          MinPrice: this.value[0],
          maxPrice: this.value[1], // burda onceki kategoriden kalan valuenin değeri filter a gidiyor ok sıfırlıcaz bi yerde nerde
        }
      );
      this.products = result.data.products;
      if (isFirstLoad) {
        this.maxPrice = result.data.maxPrice;
        this.minPrice = result.data.minPrice;
        this.value = [this.minPrice, this.maxPrice];
      }

      this.category = result.data.category;
      console.log(this.products);
      var resultStock = await axios.post(
        window.apiUrl + "CustomerProduct/GetStockStatus"
      );
      this.stockStatus = resultStock.data;
      var resultManufacturers = await axios.post(
        window.apiUrl + "CustomerProduct/GetManufacturers"
      );
      this.Manufacturers = resultManufacturers.data;
      var resultCategories = await axios.post(
        window.apiUrl + "CustomerProduct/GetCategories"
      );
      this.categories = resultCategories.data;
      console.log(this.categories);
      if (this.products.length == 0) {
        return (this.noProductsFound = true);
      }
    },
    async filteredProducts() {
      this.load(false);
    },
  },
  watch: {
    id() {
      //pare şimdi ne zaman yenilendiğini anlamak için buraya log koyucam oki
      console.log(this.id);
      this.load(true);
      //window.location.reload();
    },
  },
  computed: {
    BrandsFiltered() {
      return this.Manufacturers.filter((x) =>
        x.name.toLowerCase().includes(this.search.toLowerCase())
      );
    },
    /*
    FirstThreeProducts() {
      //ilk 5 ürünün sadece ilk resmini alma
      // resmi olmayan ürünlerin resimlerini devre dısı bırakmıs oldum filter(x=>x) ile. yip :D D:
      return this.products
        .slice(0, 3)
        .map((x) => x.productImages[0])
        .filter((x) => x);
    }, */
  },
  components: {
    Slider,
    MenuCategoryItem,
    Products,
  },
};
</script>
<style lang="scss">
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.searchBrand {
  border-color: #2fdab8;
  border-radius: 5px;
}

.filterbutton {
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

.app-content {
  padding: 40px 15px;
}
</style>
