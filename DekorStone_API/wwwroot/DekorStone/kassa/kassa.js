$("#appTitle").text("DS - İdarəetmə sistemi (Kassa)");
$("#appBrand").text("Dekor Stone (Kassa)");
var json = localStorage.getItem("json")
var parsedJSON = JSON.parse(json)
$('#systemModal').modal('show');
$('#systemModalTitle').text("Yüklənir...");
$('#systemModalText').html(`<center><div class="spinner-border text-dark mx-auto" role="status">
  <span class="sr-only">Loading...</span>
</div></center>`);
$('#systemModalBtn').attr("hidden","");

if (parsedJSON !== null && parsedJSON !== "") {

  $("#fullName").text(parsedJSON.name + " " + parsedJSON.surname)


  //-----
var kassaSum = 0


  $.ajax({
    type: 'POST',
    url: "http://pullu.az:85/api/ds/user/get/daily/sum",
    data: {userToken:localStorage.getItem("userToken"), requestToken:localStorage.getItem("requestToken")},
    dataType: 'json',
    async: true,
    success: function (data,status,xhr) {   // success callback function
    //  var json = JSON.stringify(data)
    //alert(data.name)



  switch (data.status) {

  case 1:
$('#systemModal').modal('hide')

  if (typeof(Storage) !== "undefined") {

  localStorage.requestToken = data.requestToken
  } else {

  // Sorry! No Web Storage support..
  }

  //alert(JSON.parse(json).name)
  //document.cookie = "jsonData="+data;
  $("#list-tab").html('');
  data.data.forEach(element =>{
var newDate = element.cDate.split('T')[0];
//$(o).html(element.name);
    var html = `<div class="card">
  <div class="card-header" id="`+element.id+`">
    <h2 class="mb-0">
      <button class="btn  btn-block text-left" type="button" data-toggle="collapse" data-target="#id`+element.id+`" aria-expanded="true" aria-controls="collapseOne">
        <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-calendar-date" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
  <path d="M6.445 11.688V6.354h-.633A12.6 12.6 0 0 0 4.5 7.16v.695c.375-.257.969-.62 1.258-.777h.012v4.61h.675zm1.188-1.305c.047.64.594 1.406 1.703 1.406 1.258 0 2-1.066 2-2.871 0-1.934-.781-2.668-1.953-2.668-.926 0-1.797.672-1.797 1.809 0 1.16.824 1.77 1.676 1.77.746 0 1.23-.376 1.383-.79h.027c-.004 1.316-.461 2.164-1.305 2.164-.664 0-1.008-.45-1.05-.82h-.684zm2.953-2.317c0 .696-.559 1.18-1.184 1.18-.601 0-1.144-.383-1.144-1.2 0-.823.582-1.21 1.168-1.21.633 0 1.16.398 1.16 1.23z"/>
  <path fill-rule="evenodd" d="M1 4v10a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V4H1zm1-3a2 2 0 0 0-2 2v11a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V3a2 2 0 0 0-2-2H2z"/>
  <path fill-rule="evenodd" d="M3.5 0a.5.5 0 0 1 .5.5V1a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 .5-.5zm9 0a.5.5 0 0 1 .5.5V1a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 .5-.5z"/>
</svg>
      `  +newDate+`
      </button>
    </h2>
  </div>

  <div id="id`+element.id+`" class="collapse" aria-labelledby="headingOne" data-parent="#sumAccordion">
    <div class="card-body">
    <ul class="list-group">
<li class="list-group-item" >Tarix: `+newDate+`</li>
<li class="list-group-item">İlkin qalıq: `+element.dayStartMoney+` AZN</li>
<li class="list-group-item" id="income">Gəlir: `+element.income+` AZN</li>
<li class="list-group-item" id="outcome">Xərc: `+element.outcome+` AZN</li>
<li class="list-group-item">Son qalıq: `+element.dayEndMoney+` AZN</li>

</ul>
<button type="button" style="margin-top:1%" class="btn btn-warning btn-lg btn-block" data-toggle="modal" data-target="#staticBackdrop">Gün ərzində olunan sifarişlər
<svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-bag-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
  <path d="M1 4h14v10a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V4zm7-2.5A2.5 2.5 0 0 0 5.5 4h-1a3.5 3.5 0 1 1 7 0h-1A2.5 2.5 0 0 0 8 1.5z"/>
</svg</button>
    </div>
  </div>
</div>`
$("#sumAccordion").append(html);
  }
  );
  kassaSum =data.data[0].dayEndMoney
  $("#kassaSumText").text("Cəmi: "+kassaSum+" AZN");


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
      btn.disabled = false
      btn.innerText = 'Giriş'
  $('#warningModal').modal('show')
  $('#warningText').text('Xəta, internetə bağlı olduğunuzdan əmin olun');
      //  $('#alert').text('Error: ' + errorMessage);
    }
  });

}
else {
  $('#systemModalTitle').text("Sessiyanız başa çatıb");
  $('#systemModalText').html(`<p id="systemModalText">Zəhmət olmasa yenidən giriş edin</p>`);
  $('#systemModalBtn').removeAttr("hidden");
  //$('#systemModal').modal('show')
  //window.location.replace("file:///Users/rufat/Desktop/Dekor%20Stone/login/dekor_stone.html");
}
