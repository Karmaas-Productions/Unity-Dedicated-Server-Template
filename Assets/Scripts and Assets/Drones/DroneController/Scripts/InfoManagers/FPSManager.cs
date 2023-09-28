using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPSManager : MonoBehaviour {

	public bool _lockFps;
	public int _wantedFpsCount = 60;
	public Text _fpsText;

	IEnumerator Start()
	{
		if (_lockFps)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = _wantedFpsCount; //will be ignored if VSync is turned on
		}
		while (true)
		{			
			yield return new WaitForSeconds(1);
			_fpsText.text = (1.0f / Time.deltaTime).ToString("F0") + " FPS";

		}
	}
}
