<template>
  <div>
    <md-card>
      <md-card-header>
        <h3>Manufacturer</h3>
      </md-card-header>
      <md-card-content>
        <md-field>
          <label>Manufacturer Name</label>
          <md-input v-model="Manufacturer.name" required></md-input>
        </md-field>
      </md-card-content>
      <md-card-actions>
        <md-button class="md-primary md-raised" @click="AddManufacturer"
          >Save</md-button
        >
      </md-card-actions>
    </md-card>
  </div>
</template>

<script>
import axios from "axios";
import Swal from "sweetalert2";

export default {
  data() {
    return {
      Manufacturer: {
        name: "",
   
      },
    };
  },
  props: {
    id: {},
  },
  async created() {
    if (this.id != 0) {
      var resultid = await axios.post(
        window.apiUrl + "AdminProduct/GetManufacturerFromId",
        { Id: this.id }
      );
      console.log(resultid);
      this.Manufacturer= resultid.data;
   
    }
  },
  methods: {
    async AddManufacturer() {
      var result = await axios.post(
        window.apiUrl + "AdminProduct/AddManufacturer",
        this.Manufacturer
      );
      console.log(result.data);
     
      if (result.data.success == false) {
        Swal.fire("Error", result.data.message, "error");
      } else {
        Swal.fire(result.data.message, "success");
      }
     
    },
  },
};
</script>
