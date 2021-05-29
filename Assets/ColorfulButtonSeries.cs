using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ColorfulButtonSeries : MonoBehaviour
{
	public class ColoredHexabuttonsSettings
	{
		public bool red = true;
		public bool orange = true;
		public bool yellow = true;
		public bool green = true;
		public bool blue = true;
		public bool purple = true;
		public bool white = true;
		public bool black = true;
	}
	private static int moduleIdCounter = 1;
	private int moduleId;

	private bool moduleSolved;
	public KMBombModule module;
	public KMAudio Audio;
	public KMBombInfo bomb;

	public Material[] buttonColors;
	public MeshRenderer[] buttonMesh;
	public KMSelectable[] hexButtons;
	public TextMesh[] buttonText;
	public MeshFilter[] buttonMF;
	public MeshFilter[] highlightMF;
	public Transform[] highlightTF;
	public Material[] ledColors;
	public MeshRenderer[] ledMesh;
	public Light[] ledLights;
	public Light[] flashLights;
	public AudioClip[] alphabet;
	public AudioClip[] numbers;
	public AudioClip[] ciphers;
	public MeshFilter[] shapes;
	public AudioClip[] notes;
	public AudioClip[] morseSounds;
	private string TPOrder;
	private int TPScore;
	private bool TPSwitch;
	private int colorIndex;
	/*
	 * __________________________
	 * | RULE SEED SUPPORT INFO |
	 * --------------------------
	 * For any one that wants to implement ruleseed support for this module. Here's what I think can be done to implement it:
	 *
	 *RED: Get all possible permutations of 123456 with 1 being in the first position. Substring so that each number (123456) gets an equal amount of being the first position. 
	 *
	 *ORANGE: A huge word list, pick only 230 out of the what ever amount we have in the word list. Make sure no word is an anagram of another word.
	 * 
	 *YELLOW: Randomize the priority list as well as the maze and the numbers.
	 * 
	 *GREEN: There are 462 different combinations of the notes with no duplicates. Randomize that with the number of permutations of 123456 to get positions of each note.
	 * 
	 *BLUE: Randomize which symbol relates to which movement. Randomize the values of the movements.
	 * 
	 *PURPLE: Randomize the characters that relate to which 2 bits.
	 * 
	 *WHITE: Randomize the instruction chart.
	 * 
	 *BLACK: Randomize the Button Position table and the Resulting Letters table
	 */
	void Awake()
	{
		//All of this needs to stay in the Awake Method.
		TPOrder = "0123456";
		TPSwitch = false;
		moduleId = moduleIdCounter++;
		moduleSolved = false;
		ModConfig<ColoredHexabuttonsSettings> modConfig = new ModConfig<ColoredHexabuttonsSettings>("ColoredHexabuttonsSettings");
		string colorChoices = FindColors(modConfig);
		colorIndex = colorChoices[UnityEngine.Random.Range(0, colorChoices.Length)] - '0';
		colorIndex = 0;
		switch (colorIndex)
		{
			case 0:
				TPScore = 4;
				RedHexabuttons red = new RedHexabuttons(this, Audio, module, moduleId, hexButtons, buttonMesh, ledColors, ledMesh, ledLights, transform);
				red.run();
				break;
			case 1:
				TPScore = 8;
				OrangeHexabuttons orange = new OrangeHexabuttons(this, Audio, module, moduleId, hexButtons, buttonMesh, buttonText, ledColors, ledMesh, ledLights, transform);
				orange.run();
				break;
			case 2:
				TPScore = 9;
				break;
			case 3:
				TPScore = 8;
				break;
			case 4:
				TPScore = 11;
				break;
			case 5:
				TPScore = 10;
				break;
			case 6:
				TPScore = 8;
				break;
			default:
				TPScore = 8;
				break;
		}
	}
	void Start()
	{
		float scalar = transform.lossyScale.x;
		for (int aa = 0; aa < 7; aa++)
		{
			buttonMesh[aa].material = buttonColors[colorIndex];
			ledLights[aa].enabled = false;
			ledLights[aa].range *= scalar;
			if (aa < 6)
			{
				flashLights[aa].enabled = false;
				flashLights[aa].range *= scalar;
			}
		}
	}
	string FindColors(ModConfig<ColoredHexabuttonsSettings> modConfig)
	{
		var settings = modConfig.Read();
		if (settings != null)
		{
			string colors = "";
			if (settings.red)
				colors = colors + "0";
			if (settings.orange)
				colors = colors + "1";
			if (settings.yellow)
				colors = colors + "2";
			if (settings.green)
				colors = colors + "3";
			if (settings.blue)
				colors = colors + "4";
			if (settings.purple)
				colors = colors + "5";
			if (settings.white)
				colors = colors + "6";
			if (settings.black)
				colors = colors + "7";
			if (colors.Length == 0)
				return "01234567";
			else
				return colors;
		}
		else return "01234567";
	}
}
