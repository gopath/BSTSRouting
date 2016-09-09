<?php  if ( ! defined('BASEPATH')) exit('No direct script access allowed');
	

	if ( ! function_exists('post'))
	{
		function post($post) 
		{
			return isset($_POST[$post]) ? $_POST[$post] : '';
		}
	}
	
	if ( ! function_exists('get'))
	{
		function get($get) 
		{
			return isset($_GET[$get]) ? $_GET[$get] : '';
		}
	}

/* End of file postget.php */
/* Location: ./application/helpers/postget_helper.php */