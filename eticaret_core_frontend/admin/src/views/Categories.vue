<template>
  <div>
    <div class="row justify-content-end">
      <md-button class="md-fab md-mini md-primary" to="/addcategory/0">
        <md-icon>add</md-icon>
      </md-button>
      <md-button class="md-fab md-mini md-accent" @click="DeleteCategory">
        <md-icon>delete</md-icon>
      </md-button>
    </div>
    <md-table>
      <md-table-toolbar>
        <div class="md-toolbar-section-start">
          <h5>Categories</h5>
        </div>

        <md-field md-clearable class="md-toolbar-section-end">
          <md-input placeholder="Search by name..." v-model="search" />
        </md-field>
      </md-table-toolbar>

      <md-table-row>
        <md-table-cell>
          <md-checkbox
            v-model="DeleteCategories"
            v-if="Categories.length > 1"
            @change="checkAll"
          ></md-checkbox
        ></md-table-cell>
        <md-table-cell> <span class="text-muted">ID</span></md-table-cell>
        <md-table-cell
          ><span class="text-muted">Category Name</span></md-table-cell
        >
        <md-table-cell> <span class="text-muted">Action </span></md-table-cell>
      </md-table-row>

      <md-table-row v-for="cat in CategoriesFiltered" :key="cat.id">
        <md-table-cell>
          <md-checkbox
            v-model="DeleteCategories"
            :value="cat.id"
            md-selectable="multiple"
          ></md-checkbox
        ></md-table-cell>
        <md-table-cell>{{ cat.id }}</md-table-cell>
        <md-table-cell>{{ cat.fullName }}</md-table-cell>
        <md-table-cell>
          <md-button
            class="md-fab md-mini md-plain"
            :to="'/addcategory/' + cat.id"
          >
            <md-icon>edit</md-icon>
          </md-button></md-table-cell
        >
      </md-table-row>
    </md-table>
    <md-card v-if="CategoriesFiltered.length == 0">
      <md-card-header><h3>No categories found</h3></md-card-header>
      <md-card-content
        ><p>
          {{
            `No category found for this '${search}' query. Try a different search term or create a new category.`
          }}
        </p>
        <md-button class="md-primary md-raised" to="/addcategory/0"
          >Create New Category</md-button
        >
      </md-card-content>
    </md-card>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      Categories: [],
      search: "",
      DeleteCategories: [],
    };
  },
  computed: {
    CategoriesFiltered() {
      return this.Categories.filter((x) =>
        x.name.toLowerCase().includes(this.search.toLowerCase())
      );
    },
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "AdminProduct/GetCategories"
      );
      this.Categories = result.data;
    },

    async DeleteCategory() {
      await axios.post(
        window.apiUrl + "AdminProduct/DeleteCategory",
        this.DeleteCategories
      );
    },

    checkAll() {
      if (this.DeleteCategories.length >= this.Categories.length) {
        this.DeleteCategories = [];
      } else {
        for (var item of this.Categories) {
          this.DeleteCategories.push(item.id);
          console.log();
        }
      }

      this.$forceUpdate();
    },
  },
};
</script>
<style lang="scss" scoped>
.md-table + .md-table {
  margin-top: 16px;
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
