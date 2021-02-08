$('.sl-menu').on('change', function() {
    if(this.value != 'user_setting'){
        $(location).attr('href', this.value+".html");
    }else{
        loadModal(this.value+".html");
    }

});

var menuClickFlag = false;

/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
    menuClickFlag = true;
}

// Close the dropdown if the user clicks outside of it
window.onclick = function(event) {
    if (!Element.prototype.matches) {
        if(!menuClickFlag){
            var dropdowns = document.getElementsByClassName("dropdown-content");
            var i;
            for (i = 0; i < dropdowns.length; i++) {
                var openDropdown = dropdowns[i];
                if (openDropdown.classList.contains('show')) {
                    openDropdown.classList.remove('show');
                }
            }
        }
        menuClickFlag = false;
    }else{
        if (!event.target.matches('.dropbtn')) {
            var dropdowns = document.getElementsByClassName("dropdown-content");
            var i;
            for (i = 0; i < dropdowns.length; i++) {
                var openDropdown = dropdowns[i];
                if (openDropdown.classList.contains('show')) {
                    openDropdown.classList.remove('show');
                }
            }
        }
    }

}
