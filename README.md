# BSTSRouting
C# Routing Engine and Traffic Jam Data Extraction from Google Map for BSTS

1. To use the MySQL database, SQL database file is in the folder "BSTSRoutingWebservice / database" with the name "bsts_routing.sql"

2. For web service copy existing folder in the folder "bsts_routing_service" to hosting or on XAMMP localhost / WAMP
   > Setting database -> bsts_routing_service\application\config\database.php
     hostname = 'localhost';
     username = 'root';
     password = '';
     database = 'bsts_routing';
	 
   > How to access google map tile with traffic jam:
     http://localhost/bsts_routing_service/tilegen/v2/index.php?latitude=-6.9131786&longitude=107.6474137
	 
   > How to access API routing:
     http://localhost/bsts_routing_service/index.php/api/route/routing/2439053883/25433364
	 *note: 2439053883 -> node ID source , 25433364 -> node ID destination
   
3. For visual studio project, setting the database connection:
   server = "localhost";
   database = "bsts_routing";
   uid = "root";
   password = "";           
   
