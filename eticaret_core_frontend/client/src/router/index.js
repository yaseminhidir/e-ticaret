import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import register from '../views/Register.vue'
import login from '../views/Login.vue'
import Products from '../views/ProductsComponent'
import SingleProductComponent from '../views/SingleProductComponent'
import sepet from '../views/Sepet.vue'
import profile from '../views/Profile.vue'
import orderdetails from '../views/OrderDetails.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },

  {
    path: '/products/:id',
    name: 'Products',
    props:true,
    component: Products
  },
 
  {
    path: '/product/:id',
    name: 'product',
    props:true,
    component: SingleProductComponent
  },
  {
    path: '/sepet',
    name: 'sepet',
   
    component: sepet
  },
  {
    path: '/register',
    name: 'register',
    component: register
  },
  {
    path: '/login',
    name: 'login',
    component: login
  },
  {
    path: '/profile',
    name: 'profile ',
    component: profile 
  },
  {
    path: '/orderdetails/:id',
    name: 'orderdetails',
    props:true,
    component: orderdetails
  },
 
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
