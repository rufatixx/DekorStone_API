$("#appTitle").text("DS - İdarəetmə sistemi (Menu)");
$("#appBrand").text("Dekor Stone (Menu)");
var json = localStorage.getItem("json")
var parsedJSON = JSON.parse(json)
// $('#systemModal').modal('show');
// $('#systemModalTitle').text("Yüklənir...");
// $('#systemModalText').html(`<center><div class="spinner-border text-dark mx-auto" role="status">
//   <span class="sr-only">Loading...</span>
// </div></center>`);
// $('#systemModalBtn').attr("hidden","");

if (parsedJSON !== null && parsedJSON !== "") {

  $("#fullName").text(parsedJSON.name + " " + parsedJSON.surname)
  var today = new Date().toISOString().split('T')[0];
  $("#product-end-date")[0].setAttribute('min', today);

  //-----


}
else {
  $('#systemModalTitle').text("Sessiyanız başa çatıb");
  $('#systemModalText').html(`<p id="systemModalText">Zəhmət olmasa yenidən giriş edin</p>`);
  $('#systemModalBtn').removeAttr("hidden");
  //$('#systemModal').modal('show')
  //window.location.replace("file:///Users/rufat/Desktop/Dekor%20Stone/login/dekor_stone.html");
}

// end date picker settings
function newOrderModalClick(){
  $('#systemModal').modal('show');
  $('#systemModalTitle').text("Yüklənir...");
  $('#systemModalText').html(`<center><div class="spinner-border text-dark mx-auto" role="status">
    <span class="sr-only">Loading...</span>
  </div></center>`);
  $('#systemModalBtn').attr("hidden","");


  $.ajax({
    type: 'POST',
    url: "http://pullu.az:85/api/ds/get/models",
    data: {userToken:localStorage.getItem("userToken"), requestToken:localStorage.getItem("requestToken")},
    dataType: 'json',
    success: function (data,status,xhr) {   // success callback function
    //  var json = JSON.stringify(data)
    //alert(data.name)



  switch (data.status) {
  case 1:
$('#systemModal').modal('hide');
  $('#newOrderModal').modal('show')

  if (typeof(Storage) !== "undefined") {

  localStorage.requestToken = data.requestToken
  } else {

  // Sorry! No Web Storage support..
  }

  //alert(JSON.parse(json).name)
  //document.cookie = "jsonData="+data;
  $("#productModels").html('');
  data.data.forEach(element =>{

    var o = new Option(element.name, element.id);
  $(o).html(element.name);
  $("#productModels").append(o);
  }
  );



  //alert(getCookie("jsonData"))
  break;
  default:
  $('#systemModalTitle').text("Sessiyanız başa çatıb");
  $('#systemModalText').html(`<p id="systemModalText">Zəhmət olmasa yenidən giriş edin</p>`);
  $('#systemModalBtn').removeAttr("hidden");
  //window.location.replace("file:///Users/rufat/Desktop/Dekor%20Stone/login/dekor_stone.html");
  break;

  }

        //$('p').append(data.name + ' ' + data.surname);
    },
    error: function (jqXhr, textStatus, errorMessage) { // error callback

      $('#warningModal').modal('show')
      $('#warningText').text('Xəta, internetə bağlı olduğunuzdan əmin olun');
      //  $('#alert').text('Error: ' + errorMessage);
    }
  });
}
function AddOrder() {


      // Fetch all the forms we want to apply custom Bootstrap validation styles to
      var forms = document.getElementsByClassName('needs-validation');
      // Loop over them and prevent submission
      var validation = Array.prototype.filter.call(forms, function(form) {

          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
          }
          form.classList.add('was-validated');

      });



  var clientFullName = $('#clientFullName').val()
  var clientPhone = $('#clientPhone').val()
  var kvm = $('#kvm').val()
  var modelID = $( "#productModels" ).val();
  var price = $( "#price" ).val();
  var payment = $( "#payment" ).val();
  var note = $( "#note" ).val();
  var endDate = $( "#product-end-date" ).val();


if (clientFullName.length>0 && clientPhone.length>0 && kvm.length>0&&modelID.length>0&&price.length>0&&payment.length>0
  && note.length>0&&endDate.length>0) {
    var date = new Date(endDate);
  var isoEndDate = date.toISOString()
  $('#newOrderModal').modal('hide');
  $('#systemModal').modal('show');
  $('#systemModalTitle').text("Yüklənir...");
  $('#systemModalText').html(`<center><div class="spinner-border text-dark mx-auto" role="status">
    <span class="sr-only">Loading...</span>
  </div></center>`);
  $('#systemModalBtn').attr("hidden","");
  $.ajax({
    type: 'POST',
    url: "http://pullu.az:85/api/ds/user/add/order",
    data: {userToken:localStorage.getItem("userToken"), requestToken:localStorage.getItem("requestToken"),clientFullName:clientFullName,
  clientPhone:clientPhone, kvm:kvm, modelID: modelID, price:price, payment:payment, note:note,endDate:isoEndDate
},
    dataType: 'json',
    success: function (data,status,xhr) {   // success callback function
    //  var json = JSON.stringify(data)
    //alert(data.name)



switch (data.status) {
case 1:
$('#systemModal').modal('hide')
$('#warningModal').modal('show')
$('#warningText').text('Sifarişiniz əlavə olundu!');
var newKassaSum = parseFloat(kassaSum) + parseFloat(payment)
kassaSum = newKassaSum
$("#kassaSumText").text("Cəmi: "+kassaSum+" AZN");

if (typeof(Storage) !== "undefined") {
localStorage.requestToken = data.requestToken
} else {

// Sorry! No Web Storage support..
}

//alert(JSON.parse(json).name)
//document.cookie = "jsonData="+data;

//alert(getCookie("jsonData"))
break;
case 3:
$('#systemModalTitle').text("Sessiyanız başa çatıb");
$('#systemModalText').html(`<p id="systemModalText">Zəhmət olmasa yenidən giriş edin</p>`);
$('#systemModalBtn').removeAttr("hidden");
  break;
default:
  $('#systemModal').modal('hide');
  $('#warningModal').modal('show')
  $('#warningText').text('Xəta, biraz sonra yenidən cəhd edin');
break;

}

        //$('p').append(data.name + ' ' + data.surname);
    },
    error: function (jqXhr, textStatus, errorMessage) { // error callback
      btn.disabled = false
      btn.innerText = 'Giriş'
    $("#alert").removeAttr("hidden");
$('#alert').text('Xəta, internetə bağlı olduğunuzdan əmin olun');
      //  $('#alert').text('Error: ' + errorMessage);
    }
  });
    // alert(`
    // cfn: `+clientFullName+`
    // cp: `+clientPhone+`
    // kvm: `+kvm+`
    // price: `+price+`
    // payment: `+payment+`
    // note: `+note+`
    // productModel: `+productModel+`
    // productEndDate: `+isoEndDate+`
    //
    //   `)
}
}
function getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(';');
  for(var i = 0; i <ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}
