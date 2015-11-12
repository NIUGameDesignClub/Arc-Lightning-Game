using UnityEngine;
using System.Collections;

public class LightningScript : MonoBehaviour {
	LineRenderer LR;
	public bool IsRedTeam, p1Enabled= true, p2Enabled = true;
	public Transform p1, p2;
	bool LineEnabled = true;
	int TeamLayer, DebrisLayer;
	float DistanceApart, LineLength = 6;
	Vector3 diff;

	// Use this for initialization
	void Start () {
		LR = GetComponent<LineRenderer> ();
		DebrisLayer = 1 << 10;
		if (IsRedTeam){
			TeamLayer = 1 << 9;
		}else {
			TeamLayer = 1 << 8;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (LineEnabled) {
			LR.SetPosition (0, p1.position);
			LR.SetPosition (1, p2.position);
		}
	}
	void Update(){
		diff = (p2.position - p1.position);
		DistanceApart = Mathf.Sqrt(diff.x*diff.x + diff.y*diff.y);
		if (LineEnabled) {
			KillLineCast( diff, DistanceApart);
		}else if ( p1Enabled ==  true && p2Enabled == true){
			if (DistanceApart < LineLength){
				SetLineEnabled(true);
			}
		}

	}

	// function takes in new state then checks if state is viable and adjusts line state
	public void SetLineEnabled(bool NewState){
		RaycastHit2D hit = Physics2D.Raycast (p1.position, diff, DistanceApart, DebrisLayer);
		if (NewState == true && p1Enabled ==  true && p2Enabled == true && DistanceApart < LineLength && hit.collider == null){
			LineEnabled = true;
		} else {
			LineEnabled = false;
			LR.SetPosition (0, new Vector3(0,0,0));
			LR.SetPosition (1, new Vector3(0,0,0));
		}
	}

	// function to handle ship death
	void KillObject(GameObject ThingToKill){
		ThingToKill.GetComponent<ShipDeathHandlerScript> ().KillShip();

	}

	// takes in a player number and sets their state to active
	public void setPlayerActive(int PlayerNum, bool NewState){
		if (PlayerNum == 0 || PlayerNum == 2){
			p1Enabled = NewState;
		}else {
			p2Enabled = NewState;
		}
	}

	// function does nessary check and then casts ray to destroy other team ships
	void KillLineCast(Vector3 diff, float DistanceApart){

		RaycastHit2D hit = Physics2D.Raycast (p1.position, diff, DistanceApart, DebrisLayer);
		if (hit.collider == null) {
			if (DistanceApart > LineLength) {
				SetLineEnabled (false);
			} else {
			
				LR.SetColors (new Color (1, 1, 1f, (1 - (DistanceApart / LineLength)) + .15f), new Color (1, 1, 1f, (1 - (DistanceApart / LineLength)) + .25f));
				hit = Physics2D.Raycast (p1.position, diff, DistanceApart, TeamLayer);
				Debug.DrawRay (p1.position, diff);
				if (hit.collider != null) {
					Debug.Log (hit.collider.tag);
				
					if (IsRedTeam && hit.collider.tag == "BlueTeam") {
						KillObject (hit.collider.gameObject);
					} else if (!IsRedTeam && hit.collider.tag == "RedTeam") {
						KillObject (hit.collider.gameObject);
					}
				}
			}
		} else {
			SetLineEnabled(false);
		}
	}

	// function takes in a player number and returns other teammates ship position
	public Vector3 GetOtherPlayerPosition(int PlayerNum){
		if (PlayerNum == 0 || PlayerNum == 2) {
			if ((p2.position.x > -9 && p2.position.x < 9) && (p2.position.y < 4.6f && p2.position.y > -4.6f)){
				return p2.position;
			}else {
				if (IsRedTeam){
					return new Vector3 (8f, 0, 0);
				}else {
					return new Vector3 (-8f, 0, 0);
				}
			}
		} else {
			if ((p1.position.x > -9 && p1.position.x < 9) && (p1.position.y < 4.6f && p1.position.y > -4.6f)){
				return p1.position;
			}else {
				if (IsRedTeam){
					return new Vector3 (8f, 0, 0);
				}else {
					return new Vector3 (-8f, 0, 0);
				}
			}
		}
	}

}
