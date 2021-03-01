using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
	public GameObject localPlayer;
	public GameObject networkedPlayer;
	// Start is called before the first frame update
	void Start() {
		if (hasAuthority)
			localPlayer.SetActive(true);
        else
			networkedPlayer.SetActive(true);
	}
}