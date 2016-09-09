<!DOCTYPE html>
<html>
<head>
	<title>Bandung</title>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
	<link rel="stylesheet" href="<?php echo base_url();?>assets/js/leaflet/leaflet.css" />
	<link rel="stylesheet" href="<?php echo base_url();?>assets/css/default.css" />
	<link href='<?php echo base_url();?>assets/js/jquery/jquery.autocomplete.css' rel='stylesheet' />

	<script type='text/javascript' src='<?php echo base_url();?>assets/js/jquery/jquery-1.8.2.min.js'></script>
    <script type='text/javascript' src='<?php echo base_url();?>assets/js/jquery/jquery.autocomplete.js'></script>
	<script type='text/javascript' src="<?php echo base_url();?>assets/js/leaflet/leaflet.js"></script>
	
    <!--[if lte IE 8]><link rel="stylesheet" href="leaflet.ie.css" /><![endif]-->
</head>
<body>
<style>
body {
    padding: 0;
    margin: 0;
}
html, body, #map {
    height: 100%;
}
</style>
	<div id="map">
		<div class="container">
		<p> <input type="text" id="node_from" class='autocomplete_from search' placeholder="Input from" ></p>
		<br/>
		<br/>
		<p> <input type="text" id="node_to" class='autocomplete_to search' placeholder="Input destination" > </p>		
		</div>
	</div>	
	<script>

		// init value
		var baseMapUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
		//map.locate({setView: true, maxZoom: 16});
		//mapnik -> http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png
		//hot -> http://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png		
		
		// init map
		var map = L.map('map', {
			center: [-6.91746, 107.61912],
			zoom: 13,
			zoomControl: false
		});
		
		// add osm layer into map
		L.tileLayer(baseMapUrl, {
			maxZoom: 19
		}).addTo(map);
		
		var zoomControl = L.control.zoom({
			position: 'bottomright'
        });
        map.addControl(zoomControl);
		
		// add popup when click layer on map
		/*
		var popup = L.popup();
		function onMapClick(e) {
			popup
				.setLatLng(e.latlng)
				.setContent("Lokasi : " + e.latlng.toString())
				.openOn(map);
		}
		map.on('click', onMapClick);
		*/
		
		// autocomplete
		// routing example -> http://localhost/bsts_routing/index.php/api/route/routing/2439053883/25433364
		var site = "<?php echo site_url();?>";
        $(function(){
            $('.autocomplete_from').autocomplete({
                serviceUrl: site + '/map/get_way',
                onSelect: function (suggestion) {
                    // save node from into session
					localStorage.setItem('node_from',suggestion.node_id);
					//alert(localStorage.getItem('node_from'));					
                }
            });
			$('.autocomplete_to').autocomplete({
                serviceUrl: site + '/map/get_way',
                onSelect: function (suggestion) {
                    // save node from into session
					localStorage.setItem('node_to',suggestion.node_id);
					//alert(localStorage.getItem('node_to'));
					
					getRoutePath(site + '/api/route/routing/' + localStorage.getItem('node_from') + '/' + localStorage.getItem('node_to'));								
                }
            });
        });	

		// ajax call api/route/routing/
		function AjaxCaller(){
			var xmlhttp=false;
			try{
				xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
			}catch(e){
				try{
					xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
				}catch(E){
					xmlhttp = false;
				}
			}

			if(!xmlhttp && typeof XMLHttpRequest!='undefined'){
				xmlhttp = new XMLHttpRequest();
			}
			return xmlhttp;
		}

		function getRoutePath(url){
			ajax = AjaxCaller(); 
			ajax.open("GET", url, true); 
			ajax.onreadystatechange=function(){
				if(ajax.readyState == 4){
					if(ajax.status == 200){
						var jsonData = JSON.parse(ajax.responseText);
						var arrayLoc = new Array();
						var arrayLat = new Array();
						var arrayLon = new Array();
						var arrayLatLon = new Array();
						for (var i = 0; i < jsonData.path.length; i++) {
							var lat = jsonData.path[i].latitude;
							// tambah tanda koma, handling response longitude dari database
							lat = lat.substring(0,2) + '.' + lat.substring(2,9);
							
							var lon = jsonData.path[i].longitude;
							// tambah tanda koma, handling response longitude dari database
							lon = lon.substring(0,3) + '.' + lon.substring(3,9);
							
							// save location into array 2 dimensional
							arrayLoc[i] = new Array(2);
							arrayLoc[i][0] = lat;
							arrayLoc[i][1] = lon;
							
							arrayLat[i] = lat;
							arrayLon[i] = lon;
							arrayLatLon[i] = new L.LatLng(arrayLat[i], arrayLon[i]);
							//alert(arrayLatLon);
						}						
						
						// add node_from & node_to marker
						L.marker([arrayLat[0], arrayLon[0]]).addTo(map);
						L.marker([arrayLat[jsonData.path.length-1], arrayLon[jsonData.path.length-1]]).addTo(map);
						
						// draw line
						var pathPolyline = new L.polyline(arrayLatLon, {
							color: '#0000FF',
							weight: 10,
							opacity: 0.75,
							smoothFactor: 1
						});
						pathPolyline.addTo(map);
						map.fitBounds(pathPolyline.getBounds());
						takeScreenshot(arrayLat, arrayLon);
					}
				}
			}
			ajax.send(null);
		}
		
		function getScreenshot(url, lat, lon){
			var parameter = "lat=" + lat + "&lon=" + lon;
			ajax = AjaxCaller(); 
			ajax.open("POST", url, true);
			ajax.setRequestHeader("Content-type", "application/x-www-form-urlencoded")
			ajax.send(parameter);
			ajax.onreadystatechange=function(){
				if(ajax.readyState == 4){
					if(ajax.status == 200){
						alert(ajax.responseText);
					}
				}
			}			
		}
		
		function takeScreenshot(arrayLatitude, arrayLongitude){
			//alert(arrayLatitude + " - " + arrayLongitude);
			var site = "<?php echo site_url();?>";
			var urlScreenshot = site + '/map/tile_screenshoot';
			getScreenshot(urlScreenshot, arrayLatitude, arrayLongitude);
		}
			
	</script>
	
</body>
</html>