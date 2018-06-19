using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;
	[SerializeField]
	string remoteLayerName = "RemotePlayer";
	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;
	[SerializeField]
	GameObject playerUIPrefab;
	private GameObject playerUIInstance;

	Camera sceneCamera;
	
	void Start() {
			
		if(!isLocalPlayer)
		{	
			// Is player is no localplayer disable camera, movement, audio listener, shoot script and assing "RemotePlayer" layer name.
			DisableComponents();
			AssingRemoteLayer();
		}
		else
		{
			// We are local player, disable scene camera.
			sceneCamera = Camera.main;
			if(sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(false);
			}
			// Disable player graphics for local player (Like helmets, glasses or something that blocks the first person view)
			if(playerGraphics != null)
			{
				SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
			}

			// Create Player UI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
		}
		GetComponent<Player>().Setup();
	}

	// Disable graphic components to local player
	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;
		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively (child.gameObject, newLayer);
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();
		GameManager.RegisterPlayer(_netID, _player);
	}
	
	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}

	void AssingRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void OnDisable()
	{
		Destroy(playerUIInstance);
		// Re enable scene camera
		if(sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive(true);
		}	
		// Unregister when is destroy
		GameManager.UnRegisterPlayer(transform.name);
	}

}
