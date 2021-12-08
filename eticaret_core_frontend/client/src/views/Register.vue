<template>
  <div class="container">
    <div class="row">
      <div class="col-md-3"></div>
      <div class="col-md-6 modal-body modal-body-sub_agile">
        <div class="form-div">
          <h3 class="agileinfo_sign">Sign Up <span>Now</span> <br>   <small class="self">*Tüm alanların doldurulması zorunludur </small> </h3>
       
            <div class="styled-input agile-styled-input-top" style="margin-top:10px">
              <input
                type="text"
                name="Name"
                v-model="customer.firstName"
                required=""
              />
              <label>Name</label>
              <span></span>
            </div>
            <div class="styled-input">
              <input
                type="text"
                name="Name"
                v-model="customer.lastName"
                required=""
              />
              <label> Last Name</label>
              <span></span>
            </div>
            <div class="styled-input">
              <input
                type="email"
                name="Email"
                v-model="customer.email"
                required=""
              />
              <label>Email</label>
              <span></span>
            </div>
            <div class="styled-input">
              <input
                type="number"
                class="form-number"
                name="telephone"
                v-model="customer.telephone"
                required=""
              />
              <label>Phone</label>
              <span></span>
            </div>
            <div class="styled-input">
              <input
                type="password"
                name="password"
                v-model="customer.password"
                required=""
              />
              <label>Password</label>
              <span></span>
            </div>

            <button class="form-submit-button" @click="Register">
              Register
            </button>
         
          
          <div class="clearfix"></div>
        </div>
      </div>
      <div class="col-md-3"></div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import swal from 'sweetalert2';

export default {
  data() {
    return {
      customer: {
        firstName: "",
        lastName: "",
        telephone: "",
        email: "",
        password: "",
      },
    };
  },
  methods: {
    async Register() {
      var result = await axios.post(
        window.apiUrl + "AuthCustomer/Register",
        this.customer
      );
      console.log(result);
      if (result.data.success == false) {
        return swal.fire("Error", result.data.message, "error");
      }
      this.$router.push("/login");
      console.log(result);
    },
  },
};
</script>
<style lang="scss">
.form-submit-button {
  transition: 0.5s all;
  -webkit-transition: 0.5s all;
  border: none;
  padding: 10px 40px 10px;
  font-size: 14px;
  outline: none;
  text-transform: uppercase;
  margin: 0 0 0 -4px;
  font-weight: 700;
  letter-spacing: 1px;
  background: #111;
  color: #fff;
}
.self{
  color: red;
    font-size: x-small;
    /* margin-bottom: 10px; */
    text-transform: none;
}
.form-number {
  font-size: 14px;
  letter-spacing: 1px;
  color: #777;
  padding: 0.5em 1em 0.5em 0;
  border: 0;
  width: 100%;
  border-bottom: 1px solid #dcdcdc;
  background: none;
  -webkit-appearance: none;
  outline: none;
}
.form-submit-button:hover {
  background: #2fdab8;
}

.form-div {
  border: 1px solid gray;
  padding: 30px;
  border-radius: 5px;
  -webkit-box-shadow: 3px 4px 4px -1px rgb(0 0 0 / 56%);
  box-shadow: 3px 4px 4px -1px rgb(0 0 0 / 56%);
}
</style>
