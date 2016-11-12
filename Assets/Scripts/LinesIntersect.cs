using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LinesIntersect : MonoBehaviour {

	private LineRenderer line;
	private bool isMousePressed;
	public List<Vector3> pointsList;
	private Vector3 mousePos;

	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake() {
		line = GetComponent<LineRenderer>();
		line.SetVertexCount (0);  
		isMousePressed = false;
		pointsList = new List<Vector3> ();
	}
	
	void Update ()
	{
		Touch touch = default(Touch);

		if (Input.touchCount != 0) {
			for (int i = 0; i < Input.touchCount; i++) {

				touch = Input.GetTouch (i);
				switch (touch.phase) {
				case TouchPhase.Began:
					RestartLine ();
					break;
				case TouchPhase.Ended:
					isMousePressed = false;
					break;
				}
				if (isMousePressed) {
					Draw (touch.position);
				}
			}
		}

		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown (0)) {
				RestartLine();
			}
			if (Input.GetMouseButtonUp (0)) {
				isMousePressed = false;
			}
			if (isMousePressed) {
				Draw (Input.mousePosition);
			}
		}
	}

	private void RestartLine(){
		isMousePressed = true;
		line.SetVertexCount (0);
		pointsList.Clear();
		line.SetColors (Color.white, Color.white);
	}

	private void Draw(Vector3 position){
		mousePos = Camera.main.ScreenToWorldPoint (position);
		mousePos.z = 0;
		//TODO: TO OPTIMIZE
		if (!pointsList.Contains (mousePos)) {
			pointsList.Add (mousePos);
			line.SetVertexCount (pointsList.Count);
			line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
			int indexVector = 0;
			if (isLineCollide (out indexVector)) {
				isMousePressed = false;
				line.SetColors (Color.red, Color.red);

				//Making Polygon for colliders
				Vector2[] vectorArray = new Vector2[pointsList.Count-1];
				for (int i = 0; i < pointsList.Count-1; i++){
					if (i > indexVector) {
						vectorArray [i] = (Vector2)pointsList [i];
					}
				}
				PolygonCollider2D polygonCol = gameObject.AddComponent<PolygonCollider2D>();
				polygonCol.points = vectorArray;
				polygonCol.isTrigger = true;

				//TODO: Check who's in the collider

				Destroy (polygonCol);
			}
		}
	}

	//    -----------------------------------    
	// Following method checks is currentLine(line drawn by last two points) collided with line
	//    -----------------------------------    
	private bool isLineCollide (out int index)
	{
		index = 0;
		if (pointsList.Count < 3) {
			return false;
		}
		int TotalLines = pointsList.Count - 1;
		myLine[] lines = new myLine[TotalLines];
		if (TotalLines > 1) {
			for (int i=0; i<TotalLines; i++) {
				lines [i].StartPoint = (Vector3)pointsList [i];
				lines [i].EndPoint = (Vector3)pointsList [i + 1];
			}
		}
		for (int i=0; i<TotalLines-1; i++) {
			myLine currentLine;
			currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];
			currentLine.EndPoint = (Vector3)pointsList [pointsList.Count - 1];
			if (isLinesIntersect (lines [i], currentLine)) {
				index = i;
				return true;
			}
		}
		return false;
	}
	//    -----------------------------------    
	//    Following method checks whether given two points are same or not
	//    -----------------------------------    
	private bool checkPoints (Vector3 pointA, Vector3 pointB)
	{
		return (pointA.x == pointB.x && pointA.y == pointB.y);
	}
	//    -----------------------------------    
	//    Following method checks whether given two line intersect or not
	//    -----------------------------------    
	private bool isLinesIntersect (myLine L1, myLine L2)
	{
		if (checkPoints (L1.StartPoint, L2.StartPoint) ||
			checkPoints (L1.StartPoint, L2.EndPoint) ||
			checkPoints (L1.EndPoint, L2.StartPoint) ||
			checkPoints (L1.EndPoint, L2.EndPoint))
			return false;

		return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
			(Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
			(Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
			(Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y))
		);
	}
}
