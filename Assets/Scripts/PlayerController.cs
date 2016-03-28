using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool	startMoving = true;
    public GameObject cur = null;
	public Animator anim;
	public bool facingRight = true;
	public bool touching;

    public static PlayerController S;
    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool ("Moving", startMoving);
	}
	
	// Update is called once per frame
	void Update () {
			
	}

    //If the player collides with an interactable object, saves it in cur
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Interact")
        {
            cur = coll.gameObject;
        }
    }

    //If you are no longer in the range of an object, remove it from cur
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Interact")
        {
            cur = null;
        }
    }

    //lets Cur know that the button was pushed
    //Dosen't do anything if cur == null, i.e. you are not near an object
    public void ButtonPressed()
    {
        if (cur == null)
            return;
        cur.SendMessage("Button");
    }

	public void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void Move(bool moving)
	{
		anim.SetBool("Moving", moving);
	}

}
