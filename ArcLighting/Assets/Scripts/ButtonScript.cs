using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public int level;
	// Use this for initialization
	void Start () {
		level++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ButtonPress(){
		Application.LoadLevel (level);
	}
}
