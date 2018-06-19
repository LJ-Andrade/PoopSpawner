using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

	[SerializeField]
	GameObject playerUnitPrefab;
	
	void Start () 
	{
		// Check if server has authority
		// isLocalPlayer
		// hasAuthority
		// This values return the same
		if(isLocalPlayer == false)
		{
			// This belongs to another plater
			return;
		} 
		// Instantiate() Only creaters an object on the LOCAL COMPUTER
		// Even if it has a NetwordkIdentity is still will NOT exist on
		// the network (and therefor not on any other client) 
		// UNLESS NetworkServer.Spawn() is called on this object.

		// Command (politely) the sever to SPAWN our unit
		CmdSpawnMyUnit();
	}
	
	void Update () {
		
	}

	/////// Commands ////////
	// Commands are special functions that ONLY get executed on the server.

	[Command]
	void CmdSpawnMyUnit()
	{
        // We are guaranteed to be on the server right now.
        GameObject go = Instantiate(playerUnitPrefab);

        //go.GetComponent<NetworkIdentity>().AssignClientAuthority( connectionToClient );

        // Now that the object exists on the server, propagate it to all
        // the clients (and also wire up the NetworkIdentity)
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}
}
