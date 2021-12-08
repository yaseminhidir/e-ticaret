<template>
  <div>
  <div v-if="showAlert" class="alert alert-success alert-dismissible fade show" role="alert">
  {{message}}
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>

    <md-field :class="messageClass">
      <label>Name</label>
      <md-input v-model="category.name" required></md-input>
      <span class="md-error">There is an error</span>
    </md-field>
    <md-field>
      <label>Parent Categories</label>

      <md-select v-model="category.parentId" name="category" id="category">
        <md-option
          v-for="Category in Categories"
          :key="Category.id"
          :value="Category.id"
          >{{ Category.fullName }}</md-option
        >
      </md-select>
    </md-field>
    <md-button class="md-primary" @click="AddCategory">{{
      category.id == 0 ? "Add" : "Save"
    }}</md-button>
  </div>
</template>
<script>
import axios from "axios";
export default {
  // v-modelden dolayı
  // v-model hedef data ya ama aynı zamanda senin data kaynagın yani kaegori listesi aslında
  // ordan gelen değer bi sayi kategori.id geliyor ordan onu oyle bişeye atamak gerek mesela datada yeni bi alan
  //evet sfdf gory ama sen geldiğinde v-model i sildim yine olmadı silersen hiç seçmez hmmmmmmmmmmmmmm
  //ben category id sini parentid sütununa nasıl yazcam şimdi gosteriyim
  //mantıklı dfdfdfdfdfhasdhahd
  // tadaaa kaydetti şimdi selecte bakalım :Dokişş
  data() {
    return {
      hasMessages: false,
      showAlert:false,
     message:"",
      category: {
        id: 0,
        name: "",
        parentId: null,
      },
      Categories: [],
    };
  },
  props: {
    id: {},
  },
  async created() {
    this.load();
  },
  computed: {
    messageClass() {
      return {
        "md-invalid": this.hasMessages,
      };
    },
  },
  methods: {
    async AddCategory() {
      if (this.category.name) {
       var resultmessage= await axios.post(
          window.apiUrl + "AdminProduct/AddCategory",
          this.category
        );
        this.showAlert=true;
       console.log(resultmessage.data);
       this.message=resultmessage.data.mesaj;
        this.load();
      }

      if (!this.category.name) {
        this.hasMessages=true;
        return this.messageClass;
      }
    },
    async load() {
      var result = await axios.post(
        window.apiUrl + "AdminProduct/GetCategories"
      );
      this.Categories = result.data;

      if (this.id != 0) {
        var result2 = await axios.post(
          window.apiUrl + "AdminProduct/GetCategoryFromId",
          { Id: this.id }
        );
        this.category = result2.data;
      }
    },
  },
};
</script>
