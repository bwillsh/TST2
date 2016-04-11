using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public static MovementController S;

    private Vector2 mousePos;

	public bool disableControls = false;
	bool rightHeld = false;
	bool leftHeld = false;


    //Used to figure out the hit box of the buttons
    //Created a square with size of 3x3 and covered up each of the buttons. the position of the square is the vectors below
    float squareSize = 1.5f;
    Vector2 buttonPos = new Vector2( -.14f, -3 );
    Vector2 leftPos = new Vector2(-4.37f, -3);
    Vector2 rightPos = new Vector2(4.06f, -3);

    float mapEdgeRight = -44.5f;
    float mapEdgeLeft = 5.75f;

    public GameObject background;

    //speed must be changed in the unity editor, at least for right now.
    public float speed = 5;

	void Awake () {
		S = this;
	}

    // Use this for initialization
    void Start () {
        background.transform.position = GameManager.S.backPos;
		disableControls = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!disableControls) {
			if (leftHeld) {
				Vector2 bpos = background.transform.position;
				bpos.x += speed * Time.deltaTime;
				if (bpos.x > mapEdgeLeft)
					bpos.x = mapEdgeLeft;
				background.transform.position = bpos;

				//updates the gameManager to current position
				GameManager.S.backPos = bpos;

				if (PlayerController.S.facingRight) PlayerController.S.Flip();
				if (!PlayerController.S.anim.GetBool("Moving")) PlayerController.S.Move(true);
			} else if (rightHeld) {
				Vector2 bpos = background.transform.position;
				bpos.x -= speed * Time.deltaTime;
				if (bpos.x < mapEdgeRight)
				{
					bpos.x = mapEdgeRight;
				}

				background.transform.position = bpos;
				GameManager.S.backPos = bpos;

				if (!PlayerController.S.facingRight) PlayerController.S.Flip();
				if (!PlayerController.S.anim.GetBool("Moving")) PlayerController.S.Move(true);

			}
		}

		if (Input.GetMouseButtonUp(0) || disableControls)
		{
			PlayerController.S.Move(false);
		}
    }

    //updates the background object's position
    //the player character is static, so GoLeft() moves the background right
    public void StartLeft()
    {
		leftHeld = true;
    }

	public void StopLeft() {
		leftHeld = false;
	}

    public void StartRight()
    {
		rightHeld = true;
    }

	public void StopRight()
	{
		rightHeld = false;
	}
}
