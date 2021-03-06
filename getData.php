<?php
class TestAPI{

	function getStatusCodeMessage($status)
	{
		// these could be stored in a .ini file and loaded
		// via parse_ini_file()... however, this will suffice
		// for an example
		$codes = Array(
				100 => 'Continue',
				101 => 'Switching Protocols',
				200 => 'OK',
				201 => 'Created',
				202 => 'Accepted',
				203 => 'Non-Authoritative Information',
				204 => 'No Content',
				205 => 'Reset Content',
				206 => 'Partial Content',
				300 => 'Multiple Choices',
				301 => 'Moved Permanently',
				302 => 'Found',
				303 => 'See Other',
				304 => 'Not Modified',
				305 => 'Use Proxy',
				306 => '(Unused)',
				307 => 'Temporary Redirect',
				400 => 'Bad Request',
				401 => 'Unauthorized',
				402 => 'Payment Required',
				403 => 'Forbidden',
				404 => 'Not Found',
				405 => 'Method Not Allowed',
				406 => 'Not Acceptable',
				407 => 'Proxy Authentication Required',
				408 => 'Request Timeout',
				409 => 'Conflict',
				410 => 'Gone',
				411 => 'Length Required',
				412 => 'Precondition Failed',
				413 => 'Request Entity Too Large',
				414 => 'Request-URI Too Long',
				415 => 'Unsupported Media Type',
				416 => 'Requested Range Not Satisfiable',
				417 => 'Expectation Failed',
				500 => 'Internal Server Error',
				501 => 'Not Implemented',
				502 => 'Bad Gateway',
				503 => 'Service Unavailable',
				504 => 'Gateway Timeout',
				505 => 'HTTP Version Not Supported'
		);

		return (isset($codes[$status])) ? $codes[$status] : '';
	}

	// Helper method to send a HTTP response code/message

	function sendresponse($status = 200, $body = '', $content_type = 'text/html')
	{
		$status_header = 'HTTP/1.1 ' . $status . ' ' . TestAPI::getStatusCodeMessage($status);
		header($status_header);
		header('Content-type: ' . $content_type);
		echo $body;
	}
	private $db;

	// Constructor - open DB connection
	function __construct() {
		$this->db = new mysqli('mysql16.000webhost.com', 'a9307624_admin', 'dl4cag6179_401', 'a9307624_broogle');
		$this->db->autocommit(FALSE);
	}
	function __destruct() {
		$this->db->close();
	}
	function display(){
		if(isset($_GET['id']))
		{
			$id=$_GET['id'];
			$stmt = $this->db->prepare("select m.id,m.name as Name ,m.description as Description,m.Rating as Rating,m.item_uses as Uses,m.item_buy as buy,it.itemtype as type, pr.price as Price_Range from Master m inner join ItemType it on m.item_type_id=it.id inner join PriceBracket pr on m.priceBracketId=pr.id where m.imageRefId='".$id."'");
			$stmt->execute();
			
			$stmt->bind_result($id,$name,$desc,$rat,$use,$buy,$type,$price);
			while ($stmt->fetch()) {
				break;
			}
			$stmt->close();
			
			if ($id <= 0) {
				TestAPI::sendResponse(400, 'Invalid code');
				return false;
			}
			
			$result = array(
					"id" => $id,
					"name" => $name,
"desc"=>$desc,"rating"=>$rat,"uses"=>$use,"buy"=>$buy,"type"=>$type,"price"=>$price
);

			TestAPI::sendresponse(200, json_encode($result));
			return true;
		}
else{
TestAPI::sendResponse(400, 'Invalid code');
				return false;

}
		
	}
}

$api = new TestAPI();
$api->display();
?>