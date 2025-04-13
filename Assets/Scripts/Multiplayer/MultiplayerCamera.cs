using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode; 
using Unity.Netcode.Components; 

public class MultiplayerCamera : NetworkBehaviour
{
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { 
            GetComponent<Camera>().enabled = false; 
            GetComponent<AudioListener>().enabled = false; 
        }
    }

    void Update() { 
        
    }
}
