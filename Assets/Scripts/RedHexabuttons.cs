using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RedHexabuttons
{
	private MonoBehaviour mono;
	private KMAudio Audio;
	private KMBombModule module;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Light[] ledLights;
	private Transform transform;
	private string[] voiceMessage;
	private int[] solution;
	private int numButtonPresses;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	public RedHexabuttons(MonoBehaviour m, KMAudio aud, KMBombModule mod, int MI, KMSelectable[] HB, MeshRenderer[] BM, Material[] LC, MeshRenderer[] LM, Light[] LL, Transform T)
	{
		mono = m;
		Audio = aud;
		module = mod;
		moduleId = MI;
		hexButtons = HB;
		buttonMesh = BM;
		ledColors = LC;
		ledMesh = LM;
		ledLights = LL;
		transform = T;
	}
	public void run()
	{
		moduleSolved = false;
		Debug.LogFormat("[Colored Hexabuttons #{0}] Color Generated: Red", moduleId);
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedRed(buttonIndex[i]); return false; };
		hexButtons[6].OnInteract = delegate { pressedRedCenter(); return false; };
		int[][] chart =
		{
			new int[6]{2, 4, 5, 1, 0, 3},
			new int[6]{1, 5, 3, 0, 2, 4},
			new int[6]{5, 4, 3, 1, 2, 0},
			new int[6]{1, 0, 2, 3, 4, 5},
			new int[6]{3, 0, 1, 2, 5, 4},
			new int[6]{5, 0, 1, 2, 4, 3},
			new int[6]{4, 2, 3, 5, 0, 1},
			new int[6]{0, 4, 5, 1, 3, 2},
			new int[6]{0, 5, 3, 2, 1, 4},
			new int[6]{4, 3, 5, 1, 2, 0},
			new int[6]{2, 0, 3, 4, 5, 1},
			new int[6]{2, 5, 4, 3, 1, 0},
			new int[6]{3, 1, 0, 4, 2, 5},
			new int[6]{1, 4, 5, 2, 3, 0},
			new int[6]{3, 2, 4, 5, 0, 1},
			new int[6]{5, 2, 4, 3, 0, 1},
			new int[6]{5, 3, 2, 0, 1, 4},
			new int[6]{5, 1, 0, 4, 3, 2},
			new int[6]{4, 0, 1, 2, 3, 5},
			new int[6]{1, 3, 4, 5, 0, 2},
			new int[6]{3, 5, 2, 1, 4, 0},
			new int[6]{0, 1, 2, 3, 4, 5},
			new int[6]{0, 2, 1, 4, 5, 3},
			new int[6]{2, 1, 0, 5, 3, 4},
			new int[6]{4, 1, 0, 3, 5, 2},
			new int[6]{0, 3, 4, 5, 2, 1}
		};
		int pos1 = UnityEngine.Random.Range(0, chart.Length);
		int pos2 = UnityEngine.Random.Range(0, 6);
		voiceMessage = new string[2] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[pos1] + "", (pos2 + 1) + "" };
		Debug.LogFormat("[Colored Hexabuttons #{0}] Generated Message: {1}{2}", moduleId, voiceMessage[0], voiceMessage[1]);
		solution = new int[6];
		string output = "";
		for (int aa = 0; aa < 6; aa++)
		{
			solution[aa] = chart[pos1][(pos2 + aa) % 6];
			output = output + "" + positions[solution[aa]] + " ";
		}
		Debug.LogFormat("[Colored Hexabuttons #{0}] Solution: {1}", moduleId, output);
		numButtonPresses = 0;
	}
	void pressedRed(int n)
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Debug.LogFormat("[Colored Hexabuttons #{0}] User pressed {1}", moduleId, positions[n]);
			if (solution[numButtonPresses] == n)
			{
				Vector3 pos = buttonMesh[n].transform.localPosition;
				pos.y = 0.0126f;
				buttonMesh[n].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
				hexButtons[n].OnInteract = null;
				ledMesh[n].material = ledColors[1];
				ledLights[n].enabled = true;
				if (numButtonPresses == 5)
				{
					moduleSolved = true;
					module.HandlePass();
				}
				else
					numButtonPresses++;
			}
			else
			{
				numButtonPresses = 0;
				Debug.LogFormat("[Colored Hexabuttons #{0}] Strike! I was expecting {1}!", moduleId, positions[solution[numButtonPresses]]);
				module.HandleStrike();
				for (int aa = 0; aa < 6; aa++)
				{
					Vector3 pos = buttonMesh[aa].transform.localPosition;
					pos.y = 0.0169f;
					buttonMesh[aa].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
					ledMesh[aa].material = ledColors[0];
					ledLights[aa].enabled = false;
				}
				foreach (int i in buttonIndex)
					hexButtons[i].OnInteract = delegate { pressedRed(buttonIndex[i]); return false; };
			}
		}
	}
	void pressedRedCenter()
	{
		if (!(moduleSolved))
		{
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			Vector3 pos = buttonMesh[6].transform.localPosition;
			pos.y = 0.0126f;
			buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
			mono.StartCoroutine(playAudio());
		}
	}
	IEnumerator playAudio()
	{
		hexButtons[6].OnInteract = null;
		yield return new WaitForSeconds(0.5f);
		for (int aa = 0; aa < voiceMessage.Length; aa++)
		{
			Audio.PlaySoundAtTransform(voiceMessage[aa], transform);
			yield return new WaitForSeconds(1.5f);
		}
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
		Vector3 pos = buttonMesh[6].transform.localPosition;
		pos.y = 0.0169f;
		buttonMesh[6].transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
		hexButtons[6].OnInteract = delegate { pressedRedCenter(); return false; };
	}
}
