<?php

$db = new mysqli('mysql16.000webhost.com', 'a9307624_admin', 'dl4cag6179_401', 'a9307624_broogle');
if($db){
$db->autocommit(FALSE);
$stmt = $db->prepare('select m.id,m.name as Name ,m.description as Description,m.Rating as Rating,m.item_uses as Uses,m.item_buy as buy,it.itemtype as type, pr.price as Price_Range from Master m inner join ItemType it on m.item_type_id=it.id 
inner join PriceBracket pr on m.priceBracketId=pr.id' );
$stmt->execute();
$stmt->bind_result($id,$name,$desc,$rat,$use,$buy,$type,$price);
while($stmt->fetch())
{
$resul = array(
					"id" => $id,
					"name" => $name,
"desc"=>$desc,"rating"=>$rat,"uses"=>$use,"buy"=>$buy,"type"=>$type,"price"=>$price
			);	
break;
}
$stmt->close();
		
	
echo json_encode($result);}
else
{
echo "connection error";
}
?>