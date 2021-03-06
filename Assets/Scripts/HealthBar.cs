﻿using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	float currentHealth = 1;
	float wantedHealth = 1;
	public float moveRate = .01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > wantedHealth)
		{
			dropHealth();
		}
		if (currentHealth <= 0)
		{
			Application.LoadLevel (GameManager.S.level);
		}
	}

	public void lowerHealth(float n)
	{
		wantedHealth -= n;
		if (wantedHealth < 0.0001f)
		{
			wantedHealth = 0;
		}
	}

	void dropHealth()
	{
		currentHealth -= moveRate;

		if (currentHealth < wantedHealth)
		{
			currentHealth = wantedHealth;
		}

		Vector3 changeScale = transform.localScale;
		changeScale.x = currentHealth;
		transform.localScale = changeScale;
	}
}
