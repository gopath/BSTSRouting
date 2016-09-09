<?php
	class Node_model extends CI_Model  {

		public function tableName()
		{
			return 'node_to_node';
		}
				
		public function getNodes()
		{
			$results = array();
			$this->db->select('*');
			$this->db->from($this->tableName());
			$query = $this->db->get();
			if ($query->num_rows() > 0){
				$results = $query->result();
			}
			$query->free_result();
			return $results;
		}

		public function getNodeReports($reports)
		{
			$results = array();
			$this->db->select('*');
			$this->db->from($this->tableName());
			$this->db->where('node_to not in(' .  $reports . ')');
			$query = $this->db->get();
			if ($query->num_rows() > 0){
				$results = $query->result();
			}
			$query->free_result();
			return $results;
		}
		
		public function getNodeCoordinate($data){
			$results = array();
			$query = $this->db->query("SELECT a.way_id, a.node_id, c.latitude, c.longitude, b.v as way_name FROM way_nodes a, way_tags b, nodes c WHERE a.way_id = b.way_id and a.node_id = c.node_id and b.k = 'name' and b.v LIKE '". $data ."%' group by b.way_id");
			if ($query->num_rows() > 0){
				$results = $query->result();
			}
			$query->free_result();
			return $results;
		}
		
		public function getNodeLatLong($data){
			$results = array();
			$query = $this->db->query("SELECT a.node_id, a.latitude, a.longitude FROM nodes a WHERE a.node_id IN (". $data .")");
			if ($query->num_rows() > 0){
				$results = $query->result();
			}
			$query->free_result();
			return $results;
		}
		
		public function getLatLong($data){
			$results = array();
			$query = $this->db->query("SELECT a.node_id, a.latitude, a.longitude FROM nodes a WHERE a.node_id =". $data);
			if ($query->num_rows() == 1){
				$results = $query->row();
			}
			$query->free_result();
			return $results;
		}
		
		public function getWay(){
			$results = array();
			$query = $this->db->query("SELECT a.way_id, a.node_id FROM way_nodes a LIMIT 5");
			if ($query->num_rows() > 0){
				$results = $query->result();
			}
			$query->free_result();
			return $results;
		}
		
	}			
?>