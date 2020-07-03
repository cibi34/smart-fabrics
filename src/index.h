const char MAIN_page[] PROGMEM = R"=====(
<!DOCTYPE html>
<html>
<style>
.card{
    max-width: 400px;
     min-height: 250px;
     background: #02b875;
     padding: 30px;
     box-sizing: border-box;
     color: #FFF;
     margin:20px;
     box-shadow: 0px 2px 18px -4px rgba(0,0,0,0.75);
}
</style>
<body>

<div class="card">
  <h4>Smart Fabric Dashboard</h4><br>
  <h1>Sensor Value:<span id="T0Value">0</span></h1><br>
  <span id="vis">-</span> <br>
</div>
<script>

setInterval(function() {
  // Call a function repetatively with 2 Second interval
  getData();
}, 500); //500mSeconds update rate


function getData() {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
        var touchVal = this.responseText
        document.getElementById("T0Value").innerHTML = touchVal;
        if(touchVal < 25){
            console.log("Touch detected");
            document.getElementById("vis").innerHTML = "touched";
        }else{
            document.getElementById("vis").innerHTML = "-";
        }
    }
  };
  xhttp.open("GET", "readTouch", true);
  xhttp.send();
}
</script>
</body>
</html>
)=====";