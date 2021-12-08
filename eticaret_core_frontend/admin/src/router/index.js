import Vue from 'vue'
import VueRouter from 'vue-router'
import AddProduct from '../views/AddProduct.vue'
import AddCategory from '../views/AddCategory.vue'
import ProductList from '../views/ProductList.vue'
import Manufacturers from "../views/Manufacturers.vue"
import AddManufacturer from "../views/AddManufacturer.vue"
import Orders from "../views/Orders.vue"
import OrderDetail from "../views/OrderDetail.vue"

Vue.use(VueRouter)

const routes = [
  {
    path: '/addproduct/:id',
    name: 'AddProduct',
    component: AddProduct,
    props:true,
  },
  {
    path: '/addcategory/:id',
    name: 'AddCategory',
    component: AddCategory,
    props:true,
  },
  {
    path: '/addmanufacturer/:id',
    name: 'AddManufacturer',
    component: AddManufacturer,
    props:true,
  },
  {
    path: '/orders',
    name: 'orders',
    component: Orders,
   //asdf
  },
  {
    path: '/productlist',
    name: 'ProductList',
    component: ProductList,
    // asdfghjkl
  },

  {
    path: '/orderdetail/:id',
    name: 'orderdetail',
    component: OrderDetail,
    props:true,
  
  },
  {
    path: '/manufacturers',
    name: 'Manufacturers',
    component: Manufacturers,
  
  },
  {
    path: '/categories',
    name: 'Categories',
    component: () => import(/* webpackChunkName: "Categories" */ '../views/Categories.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
