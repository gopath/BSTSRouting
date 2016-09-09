<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

	require APPPATH.'/libraries/REST_Controller.php';
	require APPPATH.'/libraries/Dijkstra.php';
	
	class Route extends REST_Controller {

		public function __construct()
		{
			parent::__construct();
			$this->load->model('node_model');
			$this->load->model('report_model');
		}

		public function dijkstra($graph_array, $source, $target) {
			$vertices = array();
			$neighbours = array();
			foreach ($graph_array as $edge) {
				array_push($vertices, $edge[0], $edge[1]);
				$neighbours[$edge[0]][] = array("end" => $edge[1], "cost" => $edge[2]);
				$neighbours[$edge[1]][] = array("end" => $edge[0], "cost" => $edge[2]);
			}
			$vertices = array_unique($vertices);
		 
			foreach ($vertices as $vertex) {
				$dist[$vertex] = INF;
				$previous[$vertex] = NULL;
			}
		 
			$dist[$source] = 0;
			$Q = $vertices;
			while (count($Q) > 0) {
		 
				$min = INF;
				foreach ($Q as $vertex){
					if ($dist[$vertex] < $min) {
						$min = $dist[$vertex];
						$u = $vertex;
					}
				}
		 
				$Q = array_diff($Q, array($u));
				if ($dist[$u] == INF or $u == $target) {
					break;
				}
		 
				if (isset($neighbours[$u])) {
					foreach ($neighbours[$u] as $arr) {
						$alt = $dist[$u] + $arr["cost"];
						if ($alt < $dist[$arr["end"]]) {
							$dist[$arr["end"]] = $alt;
							$previous[$arr["end"]] = $u;
						}
					}
				}
			}
			$path = array();
			$u = $target;
			while (isset($previous[$u])) {
				array_unshift($path, $u);
				$u = $previous[$u];
			}
			array_unshift($path, $u);
			return $path;
		}
		
		public function test_get()
		{
			$model = $this->node_model->getNodes();
			if(empty($model)){
				$this->response(array('success' => false, 'message' => 'No Nodes Data', 'responseCode' => 406), 406);
			} else {
				$edge = array();
				foreach($model as $row){
					$temp = array($row->node_from, $row->node_to, intval($row->distance));
					array_push($edge, $temp);					
				}
				$path = $this->dijkstra($edge, "25432923", "25433399");
				$path = implode(";", $path).";";
				$this->response(array('success' => true, 'message' => 'Success', 'responseCode' => 200, 'data' => $path), 200);
			}    
		}
		
		public function routing_get($node_from="", $node_to="")
		{
			$source_from = $node_from;
			$source_destination = $node_to;
			
			$model = $this->node_model->getNodes();			
			
			$graph = new Graph();
			$result = mysql_query("select * from node_to_node");
			while ($row = mysql_fetch_array($result, MYSQL_ASSOC)) {        
				$graph->addedge($row["node_from"], $row["node_to"], $row["distance"]);				
				$graph->addedge($row["node_to"], $row["node_from"], $row["distance"]);				
			}
			
			list($distances, $prev) = $graph->paths_from($source_from);
			
			$rute = "";
			$rutes = $graph->paths_to($prev, $source_destination);
			$jarak = $graph->paths_to($distances, $source_destination);
			$path = array();
			foreach ($rutes as $value) {
				if ($value != $source_destination) {
					array_push($path, $value);
					$rute = $rute . $value . ",";					
				} else {
					array_push($path, $value);
					$rute = $rute . $value;
				}
			}
			
			$list_route = rtrim($rute, ",");
			//echo $list_route;
			$list_path = array();
			foreach($path as $node){
				array_push($list_path, $this->node_model->getLatLong($node));
			}
			//$list_path = $this->node_model->getNodeLatLong($list_route);
			// distance in meter
			$this->response(array('success' => true, 'message' => 'Success', 'responseCode' => 200, 'path' => $list_path, 'distance' => $jarak[0]), 200);
		}
		
		public function dijkstra_post()
		{
			if($this->validateRouting()){
				$node_from = post('node_from');
				$node_to = post('node_to');
				
				$model;
				$model_report = $this->report_model->GetReport();
				if(empty($model_report)){
					$model = $this->node_model->getNodes();				
				} else {
					$node_report = array();
					foreach($model_report as $node){
						array_push($node_report, $node->report_near_node);
					}
					$node_report = implode("," , $node_report);
					$model = $this->node_model->getNodeReports($node_report);				
				}
				
				if(empty($model)){
					$this->response(array('success' => false, 'message' => 'No Nodes Data', 'responseCode' => 406), 406);
				} else {
					$edge = array();
					foreach($model as $row){
						$temp = array($row->node_from, $row->node_to, intval($row->distance));
						array_push($edge, $temp);					
					}
					$path = $this->dijkstra($edge, $node_from, $node_to);
					$path = implode(",", $path);
					
					$this->response(array('success' => true, 'message' => 'Success', 'responseCode' => 200, 'data' => $path), 200);
				}
			} else {
				$this->response(array('success' => false, 'message' => 'Routing Parameter Not Completed', 'responseCode' => 400), 400);
			}
		}	

		public function validateRouting(){
			if(!post('node_from') || !post('node_to'))
				return false;
			else
				return true;
		}
		
		
	}

/* End of file route.php */
/* Location: ./application/controllers/route.php */