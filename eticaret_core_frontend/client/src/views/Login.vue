<template>
  <div class="container">
    <div class="row">
      <div class="col-md-3"></div>
      <div class="col-md-6 modal-body modal-body-sub_agile">
        <div class="form-div">
          <h3 class="agileinfo_sign">Log In <span>Now</span> <br> <small class="self">*Tüm alanların doldurulması zorunludur </small></h3>

          <div class="styled-input">
            <input type="email" name="Email" v-model="email" required="" />
            <label>Email</label>
            <span></span>
          </div>
          <div class="styled-input">
            <input
              type="password"
              name="password"
              v-model="password"
              required=""
            />
            <label>Password</label>
            <span></span>
          </div>

          <button class="form-submit-button" @click="Login">
            Login
          </button>

        
          <div class="clearfix"></div>
          <router-link class="aclass" to="/register">
            Don't have an account?</router-link
          >
        </div>
      </div>
      <div class="col-md-3"></div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import swal from "sweetalert2";
export default {
  data() {
    return {
      email: "",
      password: "",
    };
  },
  methods: {
    async Login() {
      if (!(this.email && this.password)) {
        return swal.fire("Error", "Lütfen tüm alanları doldurunuz", "error");
      }
      var result = await axios.post(
        window.apiUrl + "AuthCustomer/Login",
        { email: this.email, password: this.password }
      );
      console.log(result);
      if (result.data.success == false) {
        return swal.fire("Error", result.data.message, "error");
      }
      
      this.$store.dispatch("loggedIn", {
        userToken: result.data.response.token,
        user: result.data.response.customer,
      });
       this.$router.push("/")
    },
   
  },
};
</script>

<style lang="scss">
.self{
  color: red;
    font-size: x-small;
    /* margin-bottom: 10px; */
    text-transform: none;
}
.aclass {
  font-size: 0.875em;
  color: #212121;
  letter-spacing: 1px;
}
</style>
