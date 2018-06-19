using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	private int maxHealth = 100;

	// Sync variable (send info to al clients)
	// When this var changes it will be pushed out to all clients
	[SyncVar]
	private int currentHealth;
	
	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults();
	}
	
	/* Test Death with K */
	void Update()
	{
		if(!isLocalPlayer)
			return;
		if(Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(9999);
		}	
	}

	// ClientRpc send this to all connected players
	[ClientRpc]
	public void RpcTakeDamage(int _ammount)
	{
		if(isDead)
			return;

		currentHealth -= _ammount;
		Debug.Log(transform.name + " now has " + currentHealth + " health.");
		if(currentHealth <= 0)
		{
			Die();
		}	
	}

	private void Die()
	{
		isDead = true;
		// Disable components
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}
		// This can´t be disabled on for loop and we make it here.
		Collider _col = GetComponent<Collider>();
		if(_col != null)
			_col.enabled = false;
 
		Debug.Log(transform.name + " is Dead!");

		// Call Respawn Method
		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
		Debug.Log("Respawning");
	}

    public void SetDefaults()
    {
        isDead = false;
		currentHealth = maxHealth;
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}
		Collider _col = GetComponent<Collider>();
		if(_col != null)
			_col.enabled = true;
    }
}
