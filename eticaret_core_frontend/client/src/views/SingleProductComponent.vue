<template>
  <div>
    <SingleProduct v-if="product" :product="product"></SingleProduct>
  </div>
</template>

<script>
import axios from "axios";
import SingleProduct from "./SingleProduct.vue";

export default {
  data() {
    return {
      product: null,
    };
  },
  props: {
    id: {},
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetProductById",
        { Id: this.id }
      );
      this.product = result.data;
      console.log(this.product);
    },
  },
  watch: {
    id() {
      this.load();
    },
  },
  components: {
    SingleProduct,
  },
};
</script>
