#pragma strict

// this script spawns cubes on clicking and allows to move them around for fun
// also adds few block on level

var object : Rigidbody;

function Start () {
	for(var i = 0; i<2; i++) {
		var obj : Rigidbody = Instantiate(object, Vector3(Random.Range(-2,2), 1, Random.Range(-2,2)), Quaternion(0,0,0,0));
		obj.gameObject.GetComponent.<Renderer>().material.color = Color(Random.value, Random.value, Random.value);
	}
}

var dragObject : Rigidbody = null;
var isDragging : boolean = false;

var tmpDX = 0.0;
var tmpDY = 0.0;

function OnGUI() {
	GUI.Label(Rect(100,100,200,100), "dx: " + tmpDX + ", " + tmpDY);
}

function Update () {
	// quit on back
	if(Input.GetKey(KeyCode.Home) || Input.GetKey("escape")) {
		Application.Quit();
	}
	if(Input.GetKey("menu")) {
		Application.LoadLevel("test1");
	}
	
	if(Input.GetMouseButton(0)) {
		var hit : RaycastHit;
		var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, hit, 100.0)) {
			if(hit.rigidbody != null && hit.rigidbody.gameObject.name == "Cube(Clone)") {
				isDragging = true;
				dragObject = hit.rigidbody;
			}
		}
	} else {
		isDragging = false;
		dragObject = null;
	}
	
	if(isDragging && dragObject != null) {
		var force = 50; // 1000;
		
		if(Input.touchCount > 0) {
			var touch : Touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved) {
				force = 5;
				tmpDX = touch.deltaPosition.x;
				tmpDY = touch.deltaPosition.y;
			} else {
				tmpDX = tmpDY = 0;
			}
		} else {
			tmpDX = Input.GetAxis("Mouse X");
			tmpDY = Input.GetAxis("Mouse Y");
		}
		//dragObject.AddForce(Input.GetAxis("Mouse X") * Time.deltaTime * force, 0, Input.GetAxis("Mouse Y") * Time.deltaTime * force, ForceMode.Impulse);
		//dragObject.AddForce(tmpDX*Time.deltaTime*force, 0, tmpDY*Time.deltaTime*force);
		dragObject.AddForce(tmpDX*Time.deltaTime*force, 0, tmpDY*Time.deltaTime*force, ForceMode.Impulse);
		return;
	}
	
	// spawn on tap
	if(Input.GetMouseButtonDown(0) && !isDragging) {
		var obj : Rigidbody = Instantiate(object, hit.point + Vector3(0,1,0), Quaternion(0,0,0,0));
		obj.gameObject.GetComponent.<Renderer>().material.color = Color(Random.value, Random.value, Random.value);
	}
}