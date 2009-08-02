<?php
$array = split('\\*',$_SERVER['argv'][0]);
if (count($array) == 2 || count($array) == 3) {
  if (count($array) == 2) $url = sprintf('http://www.wowarmory.com/%s?%s', $array[0], $array[1]);
  else $url = sprintf('http://%s.wowarmory.com/%s?%s', $array[0], $array[1], $array[2]);
  echo file_get_contents($url, false, stream_context_create(array('http'=>array('method'=>"GET",'header'=>"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4\r\n"))));
}
?>