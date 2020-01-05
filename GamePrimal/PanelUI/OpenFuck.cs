using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFuck : MonoBehaviour {

	public void Disable() {
		gameObject.SetActive (false);
	}

	public void Enable() {
		gameObject.SetActive (true);
	}

	public void Switch() {
		if (gameObject.activeSelf)
			gameObject.SetActive (false);
		else
			gameObject.SetActive (true);
	}
}	
