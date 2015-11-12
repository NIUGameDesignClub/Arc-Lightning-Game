
using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class PlayerControlScript : MonoBehaviour {
	Rigidbody2D RB;
	bool InputEnabled = true;
	public int PlayerNumber;
	public float Speed;
	public GameObject ThrusterSprite;
	Quaternion CurrentRot;
	Vector2 StickPosSnap;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;


	// Use this for initialization
	void Start () {	

		RB = GetComponent<Rigidbody2D> ();
		playerIndex = (PlayerIndex)PlayerNumber;

	}
	
	// Update is called once per frame
	void Update () {

		prevState = state;
		state = GamePad.GetState(playerIndex);


		if (InputEnabled) {
			// axis crontrols for horizontal movement
			if (Mathf.Abs(state.ThumbSticks.Left.X) > .5f || Mathf.Abs(state.ThumbSticks.Left.Y) > .5f){
				CurrentRot = Quaternion.Euler(new Vector3(0 ,0 ,Mathf.Atan2(-state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y)* Mathf.Rad2Deg));
				StickPosSnap.x = state.ThumbSticks.Left.X;
				StickPosSnap.y = state.ThumbSticks.Left.Y;
 			}
			transform.rotation = CurrentRot;
			// player thrust control
			if (state.Triggers.Right > .05f){
				RB.AddForce(new Vector2(StickPosSnap.x * Speed * state.Triggers.Right, StickPosSnap.y * Speed * state.Triggers.Right));
				if (ThrusterSprite.activeSelf == false){
					ThrusterSprite.SetActive(true);
				}
			}else {
				if (ThrusterSprite.activeSelf == true){
					ThrusterSprite.SetActive(false);
				}
			}
			// menu control
			if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released && Time.timeScale == 1) {

				//InputEnabled = false;
				//Time.timeScale = 0;

			}

		}
	}
	public void EnableControls(bool SetEnabled){
		InputEnabled = SetEnabled;
	}

	public int GetPlayerNum(){
		return PlayerNumber;
	}

}
