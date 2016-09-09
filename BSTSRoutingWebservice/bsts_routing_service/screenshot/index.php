<?php

// Use the first autoload instead if you don't want to install composer
require_once 'autoload.php';
require_once 'lib/Capture.php';

// example gmaps static : https://maps.googleapis.com/maps/api/staticmap?center=-6.896445,107.6345261&zoom=18&size=512x512
$url = 'http://localhost/gmaps/index.php?latitude=-6.896445&longitude=107.6345261';
$screen = new Capture($url);
$screen->setWidth(256);
$screen->setHeight(256);
//$imagePath = dirname(__FILE__) . DIRECTORY_SEPARATOR . 'image';
//$screen->output->setLocation($imagePath);

$fileLocation = 'test.jpg';
$screen->save($fileLocation);

$type = 'image/jpeg';
header('Content-Type:' . $type);
header('Content-Length: ' . filesize($fileLocation));
readfile($fileLocation);