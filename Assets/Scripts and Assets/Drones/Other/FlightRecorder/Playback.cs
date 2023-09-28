﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlightRecorderPlugin;

public class Playback : FlightRecorderPlayback
{

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		StartPlayback(() => {
			Debug.Log("I am done!");
			//do your code here when playbacking is done...
		});
	}

	public override void Update()
	{
		base.Update();
	}
}
