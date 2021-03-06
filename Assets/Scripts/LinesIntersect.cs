﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LinesIntersect : MonoBehaviour {

    [SerializeField]
    private float selectionCooldown;

    [SerializeField]
    private float extrapolationDistance;

	private LineRenderer line;
	private bool isMousePressed;
	private List<Vector3> pointsList;
	private Vector3 mousePos;

	private int boundaryLayer;
    private float cooldown = 0f;

	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake() {
		line = GetComponent<LineRenderer>();
        line.numPositions = 0;  
        line.sortingLayerName = "Line";
		isMousePressed = false;
		pointsList = new List<Vector3> ();
		boundaryLayer = LayerMask.NameToLayer ("Boundaries");
	}
	
	void Update ()
	{
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0f)
            {
                RestartLine();
            }
        }
        else
        {
            Touch touch = default(Touch);

            if (Input.touchCount != 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {

                    touch = Input.GetTouch(i);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            isMousePressed = true;
                            RestartLine();
                            break;
                        case TouchPhase.Ended:
                            isMousePressed = false;
                            Draw(touch.position);
                            break;
                    }
                    if (isMousePressed)
                    {
                        Draw(touch.position);
                    }
                }
            }

            if (Input.touchCount == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isMousePressed = true;
                    RestartLine();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isMousePressed = false;
                    Draw(Input.mousePosition);
                }
                if (isMousePressed)
                {
                    Draw(Input.mousePosition);
                }
            }
        }
	}

	private void OnEnable(){
		isMousePressed = false;
		RestartLine ();
	}

	private void RestartLine(){
        line.numPositions = 0;
		pointsList.Clear();
	}

	private void Draw(Vector3 position){
		mousePos = Camera.main.ScreenToWorldPoint (position);
		mousePos.z = 0;

        if (!isMousePressed)
        {
            if (pointsList.Count >= 2)
            {
                var lastPoint = pointsList[pointsList.Count - 1];
                var secondToLastPoint = pointsList[pointsList.Count - 2];

                mousePos = lastPoint + (lastPoint - secondToLastPoint).normalized * extrapolationDistance;

                pointsList.Add(mousePos);
                line.numPositions = pointsList.Count;
                line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
                int indexVector = 0;
                if (isLineCollide(out indexVector))
                {
                    Select(indexVector);
                }
                else
                {
                    RestartLine();
                    return;
                }
            }
            else
            {
                RestartLine();
                return;
            }
        }
        else if (!pointsList.Contains(mousePos))
        {
            pointsList.Add(mousePos);
            line.numPositions = pointsList.Count;
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            int indexVector = 0;
            if (isLineCollide(out indexVector))
            {
                isMousePressed = false;
                Select(indexVector);
            }
        }
	}

    private void Select(int indexVector)
    {
        //Making Polygon for colliders
        pointsList.RemoveRange(0, indexVector + 1);
        pointsList[pointsList.Count-1] = pointsList [0];

        line.numPositions = pointsList.Count;
        line.SetPositions(pointsList.ToArray());

        int nucleoCount = BoardController.instance.nucleoCount;
        List<Nucleo> selectedNucleos = new List<Nucleo>();

        for (int i = 0; i < nucleoCount; i++)
        {
            Nucleo nucleo = BoardController.instance.GetNucleo(i);
            if (IsPointInside(nucleo.transform.position, pointsList))
            {
                selectedNucleos.Add(nucleo);
            }
        }

        if (BoardController.instance.ProcessMatch(selectedNucleos))
        {
            cooldown = selectionCooldown;
        }
        else
        {
            RestartLine();
        }
    }

    private bool IsPointInside(Vector2 position, List<Vector3> points)
    {
        int intersections = 0;
        myLine checkLine = new myLine();
        checkLine.StartPoint = position;
        checkLine.EndPoint = position + Vector2.right * 100f;

        int pointsCount = points.Count;

        myLine borderLine = new myLine();

        if (pointsCount > 1)
        {
            for (int i = 1; i < pointsCount; i++)
            {
                borderLine.StartPoint = points[i - 1];
                borderLine.EndPoint = points[i];
                if (isLinesIntersect(checkLine, borderLine))
                {
                    intersections++;
                }
            }
        }

        return intersections % 2 != 0;
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
