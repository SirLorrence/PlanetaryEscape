using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class OnPlayerAwake : NetworkBehaviour
{
    public InputManager input;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        input.enabled = true;
    }
}
