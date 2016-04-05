using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShotPath : MonoBehaviour {

	public LineRenderer 	line;
	Vector3 				start, end;
	public LayerMask		ricochetLayer;
	private int				numSegments = 2;
	public List<Vector3>	vertices;

	
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		SegmentCount(2);
		vertices = new List<Vector3>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	public void SetStartPos(Vector3 start_line)
	{
		start = start_line;
		line.SetPosition(0, start);
	}
	public void SetEndPos(Vector3 end_line)
	{
		end = end_line;
		end.z = start.z;
		line.SetPosition(1, end);
	}
	public void HideLine()
	{
		SegmentCount(1);
	}

	public void Ricochet(Vector3 footPosition, Vector3 right, float length)
	{
		vertices.Clear();
		SegmentCount(2); //reset number of segments

		Vector3 segStart = footPosition;
		Vector3 segEnd = segStart + (right * length);

		line.SetPosition(0, segStart);
		line.SetPosition(1, segEnd);

		vertices.Add(segStart); vertices.Add(segEnd);

		Vector2 segStart2D = new Vector2(segStart.x, segStart.y);
		Vector2 segEnd2D = new Vector2(segEnd.x, segEnd.y);

		float lengthRemaining = length;

		Ray2D ray = new Ray2D(segStart2D, segEnd2D - segStart2D);
		RaycastHit2D hit = Physics2D.Raycast(segStart2D, segEnd2D - segStart2D, Vector2.Distance(segEnd2D, segStart2D), ricochetLayer);

		int infiniteLoopBlocker = 0; //sometimes the line goes into an infinite loop. This will stop it
		while (hit.collider != null && infiniteLoopBlocker < 10)
		{
			Vector2 extensionCheck = new Vector2(hit.point.x, hit.point.y);
			if (Vector2.Distance(extensionCheck, segStart2D) < .15f)
			{
				SegmentCount(numSegments - 1);
				return;
			}

			++infiniteLoopBlocker;
			//there will be a line ricochet, so there needs to be another line segment
			SegmentCount(numSegments + 1);

			//find where the ricochet hit
			Vector3 hitPos = hit.point;
			vertices.RemoveAt(numSegments -2);
			vertices.Add(hitPos);

			//set previous line segment to end there
			line.SetPosition(numSegments - 2, hitPos);

			lengthRemaining -= Vector3.Distance(segStart, hitPos);

			Vector2 ricDir2D = Vector2.Reflect(ray.direction, hit.normal);
			Vector3 ricDir3D = new Vector3(ricDir2D.x, ricDir2D.y, hitPos.z);

			segStart = hitPos;
			segEnd = hitPos + (ricDir3D * lengthRemaining);

			line.SetPosition(numSegments - 1, segEnd);

			segStart2D = new Vector2(segStart.x, segStart.y);
			segEnd2D = new Vector2(segEnd.x, segEnd.y);
			vertices.Add(segEnd);

			ray = new Ray2D(segStart2D, segEnd2D - segStart2D);
			hit = Physics2D.Raycast(segStart2D + ((segEnd2D - segStart2D) * 0.05f), segEnd2D - segStart2D, Vector2.Distance(segEnd2D, segStart2D), ricochetLayer);
		}
	}

	void SegmentCount(int num)
	{
		numSegments = num;
		line.SetVertexCount(num);
	}
}
