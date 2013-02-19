using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public bool IsDead { get; private set; }
	
    public float MoveSpeed = 1;
	public float MoveSpeedBonus = 0.2f;
    public float RotateSpeed = 1;
    public GameObject playerDiedText;
    public PlayerID playerID;
    OTAnimatingSprite spriteControl;
	
	GameObject[] Players;
	
	Hookable _myHookable;
	HookShot _myHookShot;

    void Start()
    {
        spriteControl = GetComponentInChildren<OTAnimatingSprite>();
        if (spriteControl != null)
            spriteControl.PlayLoop("DownIdle");
		
		Players = GameObject.FindGameObjectsWithTag("Player");
		
		_myHookable = GetComponent<Hookable>();
		_myHookShot = GetComponent<HookShot>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        float horizontal = 0;
        float vertical = 0;
        switch (playerID)
        {
            case PlayerID.Player1:
                horizontal = Input.GetAxis("Horizontal1");
                vertical = Input.GetAxis("Vertical1");
                break;
            case PlayerID.Player2:
                horizontal = Input.GetAxis("Horizontal2");
                vertical = Input.GetAxis("Vertical2");
                break;
            case PlayerID.Player3:
                horizontal = Input.GetAxis("Horizontal3");
                vertical = Input.GetAxis("Vertical3");
                break;
            case PlayerID.Player4:
                horizontal = Input.GetAxis("Horizontal4");
                vertical = Input.GetAxis("Vertical4");
                break;
        }


        //SPRITE CONTROL
        Debug.DrawLine(Vector3.zero, Vector3.right);
        Vector3 axisVector = new Vector3(horizontal, vertical, 0);
        //print(Vector3.Dot(Vector3.right, axisVector));
        //print(spriteControl.animationFrameset);
        /*if (spriteControl.frameName.CompareTo("DownIdle").Equals(1))
        {
            print("WTF?!");
        }*/

        if (vertical == 0 && horizontal == 0)
        {
            if (spriteControl.animationFrameset.CompareTo("Down").Equals(0)) spriteControl.PlayLoop("DownIdle");
            if (spriteControl.animationFrameset.CompareTo("Up").Equals(0)) spriteControl.PlayLoop("UpIdle");
            if (spriteControl.animationFrameset.CompareTo("Left").Equals(0)) spriteControl.PlayLoop("LeftIdle");
            if (spriteControl.animationFrameset.CompareTo("Right").Equals(0)) spriteControl.PlayLoop("RightIdle");
        }
        else if (vertical > 0)
        { // going down

            if (Vector3.Dot(Vector3.right, axisVector) < -0.201)//down-left
            {
                if (!(spriteControl.animationFrameset.CompareTo("Left").Equals(0))) spriteControl.PlayLoop("Left");
            }
            else if (Vector3.Dot(Vector3.right, axisVector) > 0.2)//down-right
            {
                if (!(spriteControl.animationFrameset.CompareTo("Right").Equals(0))) spriteControl.PlayLoop("Right");
            }
            else//down
            {
                if (!(spriteControl.animationFrameset.CompareTo("Down").Equals(0))) spriteControl.PlayLoop("Down");
            }
        }
        else if (vertical < 0)// going up
        {
            if (Vector3.Dot(Vector3.right, axisVector) < -0.2)//up-left
            {
                if (!(spriteControl.animationFrameset.CompareTo("Left").Equals(0))) spriteControl.PlayLoop("Left");
            }
            else if (Vector3.Dot(Vector3.right, axisVector) > 0.2)//up-right
            {
                if (!(spriteControl.animationFrameset.CompareTo("Right").Equals(0))) spriteControl.PlayLoop("Right");
            }
            else//up
            {
                if (!(spriteControl.animationFrameset.CompareTo("Up").Equals(0))) spriteControl.PlayLoop("Up");
            }
        }
        else if (horizontal < 0)//left
        {
            if (!(spriteControl.animationFrameset.CompareTo("Left").Equals(0))) spriteControl.PlayLoop("Left");
        }
        else
        {
            if (!(spriteControl.animationFrameset.CompareTo("Right").Equals(0))) spriteControl.PlayLoop("Right");
        }



        //print(horizontal);

        rigidbody.velocity = Vector3.zero;
        //		transform.position = transform.position + new Vector3(horizontal, 0, -vertical) * Time.deltaTime * MoveSpeed;
		float speed = MoveSpeed;
		if (_myHookable.currentHooks.Count > 0 || (_myHookShot.HookOut && _myHookShot.HookAttached && _myHookShot.GetTarget().IAmPlayer))
			speed += MoveSpeedBonus;
		
        rigidbody.AddForce(new Vector3(horizontal, 0, -vertical) * speed);
    }

    public void Died()
    {
		IsDead = true;
		
        //		Debug.LogError("Player died");
        GetComponent<HookShot>().PlayerDied();
        GetComponent<Hookable>().Died();
		
		bool gameOver = true;
		int alive = 0;
		foreach (GameObject go in Players)
		{
			if (go != null && !go.GetComponent<PlayerControl>().IsDead)
			{
				gameOver = false;
				alive++;
			}
		}
		
		if (alive == 1)
		{
			EnemyBehaviour.bonusSpeed = 4;	
		}
		
		if (gameOver)
		{
			CountdownToStart cts = FindObjectOfType(typeof(CountdownToStart)) as CountdownToStart;
			cts.EndGame();
		}

        Instantiate(playerDiedText, new Vector3(-8.5f, 10, 0), new Quaternion(0, 0, 0, 0));

		Destroy(gameObject);
    }
}

public enum PlayerID { Player1, Player2, Player3, Player4, Null }
