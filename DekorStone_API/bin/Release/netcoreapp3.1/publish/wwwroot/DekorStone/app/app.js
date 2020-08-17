
$( document ).ready(function() {
    $("#main-content").load('/dekorstone/kassa')
});
function Routing(obj) {
  alert($(obj).val())
  $(".nav-link").removeClass('active');
$(obj).addClass("active")

}
