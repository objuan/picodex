// mFollow3 ported to Unity : mgear : http://unitycoder.com/blog
// attach this script to camera, then assign some object(with linerenderer) as "object"

// original : http://processing.org/learning/topics/follow3.html
// All Examples Written by Casey Reas and Ben Fry  
// unless otherwise stated.  

// arrays
private var x:float[];
private var y:float[];

// how many objects
public var objects:int = 20;
// prefab obj
public var segmentObj:GameObject;

private var angle1:float = 0.0;  
private var segLength:float = 0.4;  // distance between objects
private var obj:GameObject[];
private var mousePos:Vector3;
private var gravity:int = 0; // 0=off, 1 =on

// init
function Start ()
{
	// init array sizes
	x = new float[objects];
	y = new float[objects];
	obj = new GameObject[objects];

	// instantiate objects
	for (var i : int = 0;i < objects; i++) 
	{
		obj[i] =  Instantiate (segmentObj, Vector3(i, i, 0), Quaternion.identity);
	}
}

// mainloop
function Update ()
{
	// get mouse position
	mousePos = Input.mousePosition;
    mousePos.z = 10.0;       // fixed distance from camera
	// convert mousepos into world pos
    var MouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

	// 1st segment is at mouse pos
	dragSegment(0, MouseWorldPos.x,MouseWorldPos.y);
	
	// draw rest of the segments
	for(var i:int=0; i<objects-1; i++)
	{
		dragSegment(i+1, x[i], y[i]);
	}
	
	// keyboard events
	if (Input.GetKey ("1") && segLength>0.1) segLength-=0.01; //adjust distance
	if (Input.GetKey ("2")) segLength+=0.01;
	if (Input.GetKeyDown ("g")) gravity=1-gravity; // gravity on/off
}  


function dragSegment(i:int,xin:float,yin:float) 
{
  var dx:float = xin - x[i];  
  var dy:float = yin - y[i];  
  var angle1:float = Mathf.Atan2(dy, dx);    
  x[i] = xin - Mathf.Cos(angle1) * segLength; 
  y[i] = yin - Mathf.Sin(angle1) * segLength;
  
  // simple gravity effect
  if (gravity==1)  y[i]+=0.1;
  
	// set object pos
	obj[i].transform.position = Vector3(x[i],y[i],10.0);
	
	// draw line 
	var lineRenderer : LineRenderer = obj[i].GetComponent(LineRenderer);
	lineRenderer.SetPosition(0,Vector3(xin,yin,10.0));
	lineRenderer.SetPosition(1,obj[i].transform.position);
}  
