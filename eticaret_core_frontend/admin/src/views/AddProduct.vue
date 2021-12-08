<template>
  <md-card>
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
    <md-card-header>
      <h2>Product Details</h2>
    </md-card-header>
    <md-card-content>
      <div class="md-layout md-gutter">
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Product Name</label>
            <md-input v-model="Product.name" required></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Price</label>
            <md-input v-model="Product.price" type="number" required></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Quantity</label>
            <md-input
              v-model="Product.quantity"
              type="number"
              required
            ></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label for="stock">Out Of Stock Status</label>
            <md-select
              v-model="Product.stockStatuId"
              name="Stock"
              id="Stock"
              required
            >
              <md-option
                v-for="StockStatu in StockStatus"
                :key="StockStatu.id"
                :value="StockStatu.id"
                >{{ StockStatu.name }}</md-option
              >
            </md-select>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Manufacturer</label>
            <md-select v-model="Product.manufacturerId" required>
              <md-option
                :value="Manufacturer.id"
                v-for="Manufacturer in Manufacturers"
                :key="Manufacturer.id"
              >
                {{ Manufacturer.name }}
              </md-option>
            </md-select>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Information</label>
            <md-input
              v-model="Product.information"
              type="text"
              required
            ></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-33">
          <md-field>
            <label>Discount(%)</label>
            <md-input v-model="Product.discount" type="number"></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-100">
          <label>Description*</label>
          <md-field>
            <wysiwyg v-model="Product.description" required />
          </md-field>
        </div>

        <div class="md-layout-item md-size-100 ">
          <h5>Categories</h5>
        </div>
        <div class="md-layout-item md-size-100">
          <div class="md-layout">
            <div
              class="md-layout-item md-size-33"
              v-for="Category in Categories"
              :key="Category.id"
            >
              <md-checkbox v-model="ProductsCategory" :value="Category.id">{{
                Category.fullName
              }}</md-checkbox>
            </div>
          </div>
        </div>
        <div class="md-layout-item md-size-100">
          <h5>Images</h5>
        </div>
        <div
          class="md-layout-item md-size-25"
          v-for="(image, index) in ProductImages"
          :key="index"
        >
          <img
            class="product-image"
            v-if="image.url"
            :src="image.url"
            alt="..."
          />
        </div>
        <md-field>
          <label>images</label>
          <md-file
            multiple
            accept="image/*"
            v-model="fileEvent"
            @change="UploadImages"
          />
        </md-field>
        <div class="md-layout-item md-size-25">
          <md-button class="md-fab md-mini md-primary" @click="SatirEkle()">
            <md-icon>add</md-icon>
          </md-button>
        </div>
        <div class="md-layout-item md-size-100">
          <h5>Meta</h5>
        </div>
        <div class="md-layout-item md-size-55">
          <md-field>
            <label>Meta Tag Title</label>
            <md-input v-model="Product.metaTagTitle"></md-input>
          </md-field>
        </div>
        <div class="md-layout-item md-size-55">
          <md-field>
            <label>Meta Tag Description</label>
            <md-textarea v-model="Product.metaTagDescription"></md-textarea>
          </md-field>
        </div>
        <div class="md-layout-item md-size-100">
          <md-field>
            <label>Meta Tag Keys</label>
            <md-textarea v-model="Product.metaTagKeys"></md-textarea>
          </md-field>
        </div>
      </div>
    </md-card-content>
    <md-card-actions>
      <md-button class="md-primary md-raised" @click="AddProduct">
        Save</md-button
      >
    </md-card-actions>
  </md-card>
</template>

<script>
import axios from "axios";
import swal from "sweetalert2";

export default {
  data() {
    return {
      showAlert: false,
      message: "",
      ProductId: 0,
      StockStatus: [],
      Categories: [],
      Manufacturers: [],
      Product: {
        information: "",
        quantity: 0,
        name: "",
        description: "",
        price: "",
        manufacturerId: "",
        stockStatuId: "",
        metaTagTitle: "",
        metaTagDescription: "",
        metaTagKeys: {},
        discount: "",
      },
      fileEvent: null,
      ProductsCategory: [],
      ProductImages: [{ url: "" }],
    };
  },
  props: {
    id: {},
  },
  async created() {
    this.load();
  },
  methods: {
    async UploadImages() {
      console.log(this.fileEvent.srcElement.files);
      var formData = new FormData();
      for (const file of this.fileEvent.srcElement.files) {
        formData.append("files", file);
      }
     var resultImage= await axios.post(window.apiUrl + "AdminProduct/UploadImage", formData);
     console.log(resultImage);
     for (const imageUrl of resultImage.data) {
        this.ProductImages.push({ url: window.apiUrl+imageUrl.replace("/","") });
     }
    },
    SatirEkle() {
      this.ProductImages.push({ url: "" });
    },
    async load() {
      var result = await axios.post(
        window.apiUrl + "AdminProduct/GetStockStatus"
      );
      this.StockStatus = result.data;
      var result2 = await axios.post(
        window.apiUrl + "AdminProduct/GetCategories"
      );
      this.Categories = result2.data;
      var resultManufacturers = await axios.post(
        window.apiUrl + "AdminProduct/GetManufacturers"
      );
      this.Manufacturers = resultManufacturers.data;
      if (this.id != 0) {
        var resultUrun = await axios.post(
          window.apiUrl + "AdminProduct/GetProductFromId",
          { Id: this.id }
        );
        console.log(resultUrun.data);
        this.Product = resultUrun.data;

        this.ProductImages = [];
        for (const Image of this.Product.productImages) {
          this.ProductImages.push(Image);
        }

        for (const cat of this.Product.productCategories) {
          this.ProductsCategory.push(cat.categoryId);
        }
      }
    },
    async AddProduct() {
      if (
        !(
          this.Product.name &&
          this.Product.quantity >= 0 &&
          this.Product.description &&
          this.Product.stockStatuId &&
          this.Product.price &&
          this.Product.manufacturerId
        )
      ) {
        swal.fire("Error", "Doldurulması zorunlu alanları doldurunuz", "error");
      } else {
        var resultproduct = await axios.post(
          window.apiUrl + "AdminProduct/AddProduct",
          this.Product
        );
        console.log(resultproduct);
        this.message = resultproduct.data.message;
        this.showAlert = true;
        this.ProductId = resultproduct.data.id;
        await axios.post(window.apiUrl + "AdminProduct/AddImage", {
          ProductId: this.ProductId,
          ProductImages: this.ProductImages,
        });
        await axios.post(window.apiUrl + "AdminProduct/ProductsCategory", {
          ProductId: this.ProductId,
          ProductsCategory: this.ProductsCategory,
        });
      }
    },
  },
};
</script>

<style lang="scss">
.product-image {
  max-height: 100px;
}
</style>
