using UnityEngine;
using System.Collections;

public class ShipDeathHandlerScript : MonoBehaviour {
	public LightningScript LS;
	Rigidbody2D RB;
	PlayerControlScript PCS;
	// Use this for initialization
	void Start () {
		PCS = GetComponent<PlayerControlScript> ();
		RB = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void KillShip(){
		StartCoroutine (KillFunc());
	}
	IEnumerator KillFunc(){
		PCS.EnableControls (false);
		RB.velocity = new Vector2 (0, 0);
		transform.position = new Vector3 (25, 25, 0);
		LS.setPlayerActive (PCS.GetPlayerNum (), false);
		LS.SetLineEnabled (false);

		yield return new WaitForSeconds(2f);
		LS.setPlayerActive (PCS.GetPlayerNum (), true);
		PCS.EnableControls (true);
		transform.position = LS.GetOtherPlayerPosition (PCS.GetPlayerNum());

		LS.SetLineEnabled (true);
	}
}
