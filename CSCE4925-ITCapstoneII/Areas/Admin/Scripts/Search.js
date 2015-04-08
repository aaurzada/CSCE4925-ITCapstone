          
    var searchvisible = 0;            
$("#search-menu").click(function(e){ 
    //This stops the page scrolling to the top on a # link.
    e.preventDefault();
    if (searchvisible ===0) {
        //Search is currently hidden. Slide down and show it.
        $("#search-form").slideDown(200);
        $("#s").focus(); //Set focus on the search input field.
        searchvisible = 1; //Set search visible flag to visible.
    } else {
        //Search is currently showing. Slide it back up and hide it.
        $("#search-form").slideUp(200);
        searchvisible = 0;
    }
});

