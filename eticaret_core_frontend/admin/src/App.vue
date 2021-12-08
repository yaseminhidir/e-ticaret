<template>
  <div id="app">
  
    <div class="page-container">
      <md-app>
        <md-app-toolbar class="md-primary">
          <span class="md-title">My Title</span>
        </md-app-toolbar>

        <md-app-drawer md-permanent="full">
          <md-toolbar class="md-transparent" md-elevation="0">
            E-Ticaret
          </md-toolbar>

          <md-list>
            <md-list-item>
              <md-icon>production_quantity_limits</md-icon>
              <span class="md-list-item-text">
                <router-link to="/productlist"> Products </router-link></span
              >
            </md-list-item>

            <md-list-item>
              <md-icon>category</md-icon>
              <span class="md-list-item-text">
                <router-link to="/categories"> Categories</router-link>
              </span>
            </md-list-item>

            <md-list-item>
              <md-icon>delete</md-icon>
              <span class="md-list-item-text">
                <router-link to="/manufacturers">
                  Manufacturers</router-link
                ></span
              >
            </md-list-item>
            <md-list-item>
              <md-icon>shopping_cart</md-icon>
              <span class="md-list-item-text">
                <router-link to="/orders"> Orders</router-link></span
              >
            </md-list-item>
            <md-list-item>
              <md-icon>error</md-icon>
              <span class="md-list-item-text">Spam</span>
            </md-list-item>
          </md-list>
        </md-app-drawer>

        <md-app-content>
          <router-view />
        </md-app-content>
      </md-app>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      Admin: {
        Username: "",
        Password: "",
      },
      
    };
  },
 
};
</script>

<style lang="scss" scoped>
.md-list-item-text{
  font-size:11px;
}
.app{
font-size:10px
}
.md-theme-default a:not(.md-button) {
    color: rgba(0,0,0,0.87);
    /* color: #448aff; */
    /* color: var(--md-theme-default-primary-on-background, #448aff); */
}
.md-app {
  max-height: 400px;
  border: 1px solid rgba(#000, 0.12);
  min-height: 100vh;
}
.md-field:last-child {
  margin-bottom: 40px;
}
// Demo purposes only
.md-drawer {
  width: 230px;
  max-width: calc(100vw - 125px);
}
#app {
  font-size: 10px;
}
#nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
