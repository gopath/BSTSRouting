<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

require_once getcwd() . '\screenshot\autoload.php';
require_once getcwd() . '\screenshot\lib\Capture.php';

class Map extends CI_Controller {

	/**
	 * Index Page for this controller.
	 *
	 * Maps to the following URL
	 * 		http://example.com/index.php/welcome
	 *	- or -  
	 * 		http://example.com/index.php/welcome/index
	 *	- or -
	 * Since this controller is set as the default controller in 
	 * config/routes.php, it's displayed at http://example.com/
	 *
	 * So any other public methods not prefixed with an underscore will
	 * map to /index.php/welcome/<method_name>
	 * @see http://codeigniter.com/user_guide/general/urls.html
	 */
	 
	public function __construct() {
        parent::__construct();
        $this->load->model('node_model'); 
        $this->load->helper(array('url')); 
    }
	 
	public function index()
	{
		$this->load->view('map_bandung');
	}
	
	public function get_wayfrom() {
        $nfrom = $this->input->post('node_from',TRUE); 
        $query = $this->node_model->getNodeCoordinate('merdeka');
		
        $result = array();
        foreach ($query as $d) {
            $result[] = array(
                'way_id' => $d->way_id, 
                'node_id' => $d->node_id,
                'way_name' => $d->way_name
            );
        }
        echo json_encode($result);
    }
	
	public function get_wayto() {
        $nto = $this->input->post('node_to',TRUE); 
        $query = $this->node_model->getNodeCoordinate('merdeka');
		
        $result = array();
        foreach ($query as $d) {
            $result[] = array(
                'way_id' => $d->way_id, 
                'node_id' => $d->node_id,
                'way_name' => $d->way_name
            );
        }
        echo json_encode($result);
    }
	
	public function get_way() {
		$keyword = $this->uri->segment(3);
        $query = $this->node_model->getNodeCoordinate($keyword);
		
        $result = array();
        foreach ($query as $d) {
			$result['query'] = $keyword;			
            $result['suggestions'][] = array(
                'value' => $d->way_name, 
                'node_id' => $d->node_id,
				'way_name' => $d->way_id,
				'latitude' => $d->latitude,
				'longitude' => $d->longitude
            );
        }
        echo json_encode($result);
    }
	
	public function tile_screenshoot(){
		$lat = $_POST['lat'];
		$lon = $_POST['lon'];
		
		$lat = explode(',', $lat);
		$lon = explode(',', $lon);
		
		$image_url = base_url() . 'screenshot/image/';
		for ($i = 0; $i < count($lat); ++$i) {
			
			$url = 'http://localhost/gmaps/index.php?latitude=' . $lat[$i] . '&longitude=' . $lon[$i];
			$screen = new Capture($url);
			$screen->setWidth(256);
			$screen->setHeight(256);
			$imagePath = 'screenshot/image';
			$screen->output->setLocation($imagePath);

			// create epoc time for image filename
			date_default_timezone_set('Asia/Jakarta');
			$epoch = new DateTime(date('Y/m/d H:i:s'));				
			$fileLocation = $epoch->format('U') . '_' . $i . '.jpg';
			$screen->save($fileLocation);
			$response = array('image_id' => $epoch->format('U') . '_' . $i, 'image_url' => $image_url . $fileLocation);
			echo json_encode($response);
		}
		
		/*
		$url = 'http://localhost/gmaps/index.php?latitude=-6.896445&longitude=107.6345261';
		$screen = new Capture($url);
		$screen->setWidth(256);
		$screen->setHeight(256);
		$imagePath = 'screenshot/image';
		$screen->output->setLocation($imagePath);

		$fileLocation = '123.jpg';
		$screen->save($fileLocation);
		*/
	}
	
}

/* End of file map.php */
/* Location: ./application/controllers/map.php */