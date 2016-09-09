<html>
	<head>
		<title>Google Map with Traffic</title>
		<meta name="viewport" content="initial-scale=1.0">
		<meta charset="utf-8">
		<style>
		html, body {
			height: 100%;
			margin: 0;
			padding: 0;
		}
		#map {
			width: 512px;
			height: 512px;
		}
		.gmnoprint:not(.gm-bundled-control) {
			display: none;
		}
		.gm-bundled-control .gmnoprint {
			display: block;
		}
		a[href^="http://maps.google.com/maps"]{display:none !important}
		a[href^="https://maps.google.com/maps"]{display:none !important}
		a[href^="https://www.google.com/maps"]{display:none !important}
		</style>
  </head>
  <body>
	  <script type="text/javascript" src="jquery-1.11.3-jquery.min.js"></script>
	  <script type="text/javascript" src="html2canvas.js"></script>
	  <div id="map"></div>
		<script>
			// function to create map looks like google static map
			// example : http://maps.google.com/maps/api/staticmap?center=-6.896445,107.6345261&zoom=18&size=512x512
			var urlParams;
			(window.onpopstate = function () {
				var match,
					pl     = /\+/g,  // Regex for replacing addition symbol with a space
					search = /([^&=]+)=?([^&]*)/g,
					decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
					query  = window.location.search.substring(1);

				urlParams = {};
				while (match = search.exec(query))
				   urlParams[decode(match[1])] = decode(match[2]);
			})();
		
			var paramLat = urlParams["latitude"];
			var paramLng = urlParams["longitude"];
			var map;
			function initMap() {
				var latlng = new google.maps.LatLng(paramLat, paramLng);
				map = new google.maps.Map(document.getElementById('map'), {
					center: latlng,
					zoom: 18,
					disableDefaultUI: true
					/*,styles: [{
						"stylers": [{
							"visibility": "off"
						}]
					}]*/
				});
			  
				// add traffic layer into google map
				var trafficLayer = new google.maps.TrafficLayer();
				trafficLayer.setMap(map);
			  
				//after finish loading traffic layer and than capture the screen and save it	
				window.onload = function () {
					captureDiv();
				}
			}
			
			function captureDiv()
			{
				html2canvas([document.getElementById('map')], {   
					useCORS: true,
					onrendered: function(canvas)  
					{
						var img = canvas.toDataURL()
						$.post("save.php", {data: img}, function (file) {
						window.location.href =  "download.php?path="+ file});
						
						/*var a = document.createElement('a');
						// toDataURL defaults to png, so we need to request a jpeg, then convert for file download.
						a.href = canvas.toDataURL("image/jpeg").replace("image/jpeg", "image/octet-stream");
						a.download = 'test.jpg';
						a.click();*/
					}
				});
			}
			
			function captureFullPage()
			{
				html2canvas(document.body, {  
					useCORS: true,
					onrendered: function(canvas)  
					{
						var img = canvas.toDataURL()
						$.post("save.php", {data: img}, function (file) {
						window.location.href =  "download.php?path="+ file});   
					}
				});
			}

			/*function urlsToAbsolute(nodeList) {
				if (!nodeList.length) {
					return [];
				}
				var attrName = 'href';
				if (nodeList[0].__proto__ === HTMLImageElement.prototype
				|| nodeList[0].__proto__ === HTMLScriptElement.prototype) {
					attrName = 'src';
				}
				nodeList = [].map.call(nodeList, function (el, i) {
					var attr = el.getAttribute(attrName);
					if (!attr) {
						return;
					}
					var absURL = /^(https?|data):/i.test(attr);
					if (absURL) {
						return el;
					} else {
						return el;
					}
				});
				return nodeList;
			}

			function screenshotPage() {
				urlsToAbsolute(document.images);
				urlsToAbsolute(document.querySelectorAll("link[rel='stylesheet']"));
				var screenshot = document.documentElement.cloneNode(true);
				var b = document.createElement('base');
				b.href = document.location.protocol + '//' + location.host;
				var head = screenshot.querySelector('head');
				head.insertBefore(b, head.firstChild);
				screenshot.style.pointerEvents = 'none';
				screenshot.style.overflow = 'hidden';
				screenshot.style.webkitUserSelect = 'none';
				screenshot.style.mozUserSelect = 'none';
				screenshot.style.msUserSelect = 'none';
				screenshot.style.oUserSelect = 'none';
				screenshot.style.userSelect = 'none';
				screenshot.dataset.scrollX = window.scrollX;
				screenshot.dataset.scrollY = window.scrollY;
				var script = document.createElement('script');
				script.textContent = '(' + addOnPageLoad_.toString() + ')();';
				screenshot.querySelector('body').appendChild(script);
				var blob = new Blob([screenshot.outerHTML], {
					type: 'image/html'
				});
				return blob;
			}

			function addOnPageLoad_() {
				window.addEventListener('DOMContentLoaded', function (e) {
					var scrollX = document.documentElement.dataset.scrollX || 0;
					var scrollY = document.documentElement.dataset.scrollY || 0;
					window.scrollTo(scrollX, scrollY);
				});
			}
			
			function generate() {
				window.URL = window.URL || window.webkitURL;
				window.open(window.URL.createObjectURL(screenshotPage()));
			}
			*/

		</script>
		<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAvF5jPzgSOGj66KsFsHWmKcu6HYL1Y8dE&callback=initMap"></script>
	</body>
</html>