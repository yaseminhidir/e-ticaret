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
    <div class="md-layout md-gutter">
      <div class="md-layout-item md-size-100 md-layout md-alignment-top-right">
        <div class="md-layout-item md-size-10">
          <md-button class="md-fab md-mini md-primary" to="/addmanufacturer/0">
            <md-icon>add</md-icon>
          </md-button>
          <md-button @click="Delete" class="md-fab md-mini md-accent ">
            <md-icon>delete</md-icon>
          </md-button>
        </div>
      </div>
      <div class="md-layout-item md-size-100">
        <md-table>
          <md-table-toolbar>
            <div class="md-toolbar-section-start">
              <h5>Manufacturer</h5>
            </div>

            <md-field md-clearable class="md-toolbar-section-end">
              <md-input placeholder="Search by name..." v-model="search" />
            </md-field>
          </md-table-toolbar>

          <md-table-row>
            <md-table-cell>
              <md-checkbox
                v-if="Manufacturers.length > 1"
                v-model="DeleteManufacturers"
                @change="checkAll"
              ></md-checkbox
            ></md-table-cell>
            <md-table-cell> <span class="text-muted">ID</span></md-table-cell>
            <md-table-cell
              ><span class="text-muted">Manufacturer Name</span></md-table-cell
            >
            <md-table-cell
              ><span class="text-muted">Action</span></md-table-cell
            >
          </md-table-row>

          <md-table-row
            v-for="Manufacturer in ManufacturersFiltered"
            :key="Manufacturer.id"
          >
            <md-table-cell>
              <md-checkbox
                v-model="DeleteManufacturers"
                :value="Manufacturer.id"
                md-selectable="multiple"
              ></md-checkbox
            ></md-table-cell>
            <md-table-cell>{{ Manufacturer.id }}</md-table-cell>

            <md-table-cell>{{ Manufacturer.name }}</md-table-cell>

            <md-table-cell>
              <md-button class="md-fab md-mini md-plain" :to="'/addmanufacturer/' +  Manufacturer.id">
                <md-icon>edit</md-icon>
              </md-button ></md-table-cell
            >
          </md-table-row>
        </md-table>
        <md-card v-if="ManufacturersFiltered.length == 0">
          <md-card-header><h4>No manufacturers found</h4></md-card-header>
          <md-card-content
            ><p>
              {{
                `No manufacturer found for this '${search}' query. Try a different search term or create a new manufacturer.`
              }}
            </p>
            <md-button class="md-primary md-raised" to="/addmanufacturer/0"
              >Create New
            </md-button>
          </md-card-content>
        </md-card>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      Manufacturers: [],
      DeleteManufacturers: [],
      search: "",
      message:"",
      showAlert:false,
    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "AdminProduct/GetManufacturers"
      );
      console.log(result.data);
      this.Manufacturers = result.data;
    },
    async Delete(){
var result2= await axios.post(window.apiUrl + "AdminProduct/DeleteManufacturers", this.DeleteManufacturers);
console.log(result2);
this.message=result2.data.message;
this.showAlert = true;
this.load();
    },
    checkAll() {

      if (this.DeleteManufacturers.length >= this.Manufacturers.length) {
        this.DeleteManufacturers = [];
      } else {
        for (var item2 of this.Manufacturers) {
          this.DeleteManufacturers.push(item2.id);
          console.log(this.DeleteManufacturers);
        }
      }

      this.$forceUpdate();
    },
  },
  computed: {
    ManufacturersFiltered() {
      var self = this;
      return this.Manufacturers.filter((x) =>
        x.name.toLowerCase().includes(self.search.toLowerCase())
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