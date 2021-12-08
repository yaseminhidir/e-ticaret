<template>
  <div class="home">
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
      <ol class="carousel-indicators">
        <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
        <li data-target="#myCarousel" data-slide-to="1" class=""></li>
    
      </ol>
      <div class="carousel-inner" role="listbox">
        <div class="item active">
          <div class="container">
            <div class="carousel-caption">
              <h3>The Biggest <span>Sale</span></h3>
              <p>Special for today</p>
              <router-link class="hvr-outline-out button2"  :to="'/products/' + categories[0].id"> {{categories[0].name}} </router-link>
            </div>
          </div>
        </div>
        <div class="item item2">
          <div class="container">
            <div class="carousel-caption">
              <h3>Summer <span>Collection</span></h3>
              <p>New Arrivals On Sale</p>
              <router-link class="hvr-outline-out button2" :to="'/products/' + categories[1].id">{{categories[1].name}} </router-link>
            </div>
          </div>
        </div>
        
      
      </div>
      <a
        class="left carousel-control"
        href="#myCarousel"
        role="button"
        data-slide="prev"
      >
        <span
          class="glyphicon glyphicon-chevron-left"
          aria-hidden="true"
        ></span>
        <span class="sr-only">Previous</span>
      </a>
      <a
        class="right carousel-control"
        href="#myCarousel"
        role="button"
        data-slide="next"
      >
        <span
          class="glyphicon glyphicon-chevron-right"
          aria-hidden="true"
        ></span>
        <span class="sr-only">Next</span>
      </a>
      <!-- The Modal -->
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Home",
  data() {
    return {
      categories: [],

    };
  },
  async created() {
    this.load();
  },
  methods: {
    async load() {
      var result = await axios.post(
        window.apiUrl + "CustomerProduct/GetCategoriesTopMenu"
      );
      this.categories = result.data;
     
      console.log(this.categories);
    },
  },
};
</script>
