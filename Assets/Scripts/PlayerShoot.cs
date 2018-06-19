using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask mask;

	// Use this for initialization
	void Start () 
	{
		if(cam == null)
		{
			Debug.LogError("No camera referenced!");
			this.enabled = false;
		}
	}
	
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	// Check locally if it hit something
	[Client]
    void Shoot()
    {
        RaycastHit _hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
		{
			//We hit something
			Debug.Log("We hit " + _hit.collider.name);
			if(_hit.collider.tag == PLAYER_TAG)
			{
				// If we hit something send who receive the shot and some damage
				CmdPlayerShot(_hit.collider.name, weapon.damage);
			}
		}
    }
	
	// Send to server shot info
	[Command]
	void CmdPlayerShot(string _playerID, int _damage)
	{	
		Debug.Log(_playerID + "has been shot");
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage); 
	}
}
