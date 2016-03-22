using UnityEngine;
using System.Collections;

public enum CamState {
	NORMAL,
	NINJA,
	TOMMY
}

public class CombatCamera : MonoBehaviour {

	Camera camera;

	CamState _cam;
	public CamState cam 
	{
		get {return _cam;}
		set
		{
			if (_cam == value) return;
			_cam = value;
			switch(_cam) {
			case CamState.NORMAL:
				break;
			case CamState.NINJA:
				break;
			case CamState.TOMMY:
				break;
			default:
				break;
			}
		}
	}

	public float originalZoom = 10;
	public float finalZoom = 6;
	public float zoomSpeed;

	public Vector3 	originalPosition;
	public Vector3 	targetPosition;
	public float 	moveSpeed;

	private int	currentNinjaNumber;
	private Ninja currentNinja;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
		originalPosition = transform.position;
		currentNinjaNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void LookAtNinja()
	{
		if (camera.orthographicSize != finalZoom)
		{
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, finalZoom, zoomSpeed * Time.deltaTime);
		}
		if (transform.position != targetPosition)
		{
			transform.position = Vector3.Lerp(transform.position, targetPosition, zoomSpeed * Time.deltaTime); //purposefully using zoomSpeed here
		}
	}

//	private void FollowNinja()
//	{
//		Vector3 newPos = CombatController.S.ninjaList[currentNinjaNumber].transform.position;
//		newPos.z = transform.position.z;
//		transform.position = newPos;
//	}
//
//	private void NextNinja()
//	{
//		++currentNinjaNumber;
//		if (currentNinjaNumber >= CombatController.S.ninjaList.Count)
//		{
//			cam = CamState.NORMAL;
//			return;
//		}
//		targetPosition = CombatController.S.ninjaList[currentNinjaNumber].transform.position;
//		targetPosition.z = transform.position.z;
//		currentNinja = CombatController.S.ninjaList[currentNinjaNumber].transform.GetComponent<Ninja>();
//	}
}
