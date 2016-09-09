<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Tile extends CI_Controller {

	public function __construct() {
        parent::__construct();
        $this->load->library('curl');
    }
	 
	public function index()
	{
		$url = "http://localhost/gmaps/index.php?latitude=-6.896445&longitude=107.6345261";		
		$this->curl->create($url);
		$this->curl->option('useragent', 'Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 (.NET CLR 3.5.30729)');
		$this->curl->option('buffersize', 10);
		$this->curl->option('returntransfer', 1);
		$this->curl->option('followlocation', 1);
		$this->curl->option('HEADER', true);
		$this->curl->option('connecttimeout', 600);
		$data = $this->curl->execute();
		echo $data;
	}
}

/* End of file tile.php */
/* Location: ./application/controllers/tile.php */