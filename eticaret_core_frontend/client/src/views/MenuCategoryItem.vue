<template>
  <li v-if="category.inverseParent && category.inverseParent.length > 0">
    <input type="checkbox" checked="checked" :id="'item-' + category.id" /><label
      :for="'item-' + category.id"
      ><i class="fa fa-long-arrow-right" aria-hidden="true"></i>
     <router-link :to="'/products/'+ category.id">   {{ category.name }} </router-link></label
    >
    <ul>
     <menu-category-item :category="child" v-for="child in category.inverseParent" :key="child.id"></menu-category-item>    
    </ul>
  </li>

  <li v-else>
    <router-link :to="'/products/'+ category.id"> 
      {{ category.name }} </router-link>
  </li>
</template>
<script>
export default {
  name: "menu-category-item",
  props: {
    category: {},
  },

};
</script>

<style lang="scss">
ul.tree-list-pad li a {
    margin-left: 17px;
}
</style>