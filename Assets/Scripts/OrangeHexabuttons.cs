using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeHexabuttons {
	private MonoBehaviour mono;
	private KMAudio Audio;
	private KMBombModule module;
	private int moduleId;
	private KMSelectable[] hexButtons;
	private MeshRenderer[] buttonMesh;
	private TextMesh[] buttonText;
	private Material[] ledColors;
	private MeshRenderer[] ledMesh;
	private Light[] ledLights;
	private Transform transform;
	
	private string[] voiceMessage;
	private string solution;
	private int numButtonPresses;
	private bool moduleSolved;
	private string[] positions = { "TL", "TR", "ML", "MR", "BL", "BR" };
	private int[] buttonIndex = { 0, 1, 2, 3, 4, 5 };
	private string scramble;
	public OrangeHexabuttons(MonoBehaviour m, KMAudio a, KMBombModule mod, int mi, KMSelectable[] hb, MeshRenderer[] bm, TextMesh[] bt, Material[] lc, MeshRenderer[] lm, Light[] ll, Transform t)
	{
		mono = m;
		Audio = a;
		module = mod;
		moduleId = mi;
		hexButtons = hb;
		buttonMesh = bm;
		buttonText = bt;
		ledColors = lc;
		ledMesh = lm;
		ledLights = ll;
		transform = t;
	}
	public void run()
	{
		numButtonPresses = 0;
		moduleSolved = false;
		Debug.LogFormat("[Colored Hexabuttons #{0}] Color Generated: Orange", moduleId);
		string[] wordList =
			{
					"ABACUS", "ACTION", "ADVICE", "AFFECT", "AGENCY", "ALMOND", "AMOUNT", "ANARCH", "APPEAR", "ARRIVE",
					"BALLAD", "BAKERY", "BEACON", "BINARY", "BLEACH", "BRONZE", "BOXING", "BREEZE", "BELIEF", "BITTER",
					"CACTUS", "CEREAL", "CHERRY", "CITRUS", "CLOSET", "COFFEE", "CRISIS", "CURSOR", "CONVEX", "CELLAR",
					"DANGER", "DEBRIS", "DINNER", "DOODLE", "DRIVER", "DUSTER", "DEFEAT", "DIRECT", "DOMINO", "DRAWER",
					"EASTER", "EDITOR", "EFFECT", "EGGNOG", "EMBLEM", "ENROLL", "EQUALS", "ERASER", "ESCAPE", "EXPERT",
					"FABRIC", "FELINE", "FILTER", "FLAVOR", "FOREST", "FREEZE", "FUTURE", "FACADE", "FOLLOW", "FINISH",
					"GALLON", "GEYSER", "GALAXY", "GLANCE", "GROWTH", "GUTTER", "GAMBLE", "GERBIL", "GINGER", "GIVING",
					"HAMMER", "HEIGHT", "HIDING", "HOLLOW", "HUNTER", "HYBRID", "HANDLE", "HELMET", "HAZARD", "HURDLE",
					"ICICLE", "IMPORT", "INSERT", "ITALIC", "IMPAIR", "INCOME", "IMPACT", "INSULT", "INSECT", "INTENT",
					"JESTER", "JINGLE", "JOGGER", "JUNGLE", "JERSEY", "JOCKEY", "JUGGLE", "JUMBLE", "JUNIOR", "JAILER",
					"KETTLE", "KIDNEY", "KNIGHT", "KENNEL", "KINGLY", "KITTEN", "KRAKEN", "KINDLY", "KERNEL", "KEEPER",
					"LAGOON", "LEADER", "LIMBER", "LOCKET", "LUXURY", "LYCHEE", "LADDER", "LEGACY", "LIQUID", "LOTION",
					"MAGNET", "MEADOW", "MIDDLE", "MOMENT", "MUSEUM", "MYSTIC", "MATRIX", "MELODY", "MIRROR", "MUFFIN",
					"NAPKIN", "NEEDLE", "NICKEL", "NOBODY", "NUTMEG", "NATION", "NECTAR", "NINETY", "NOTICE", "NARROW",
					"OBJECT", "OCELOT", "OFFICE", "OPTION", "ORANGE", "OUTPUT", "OXYGEN", "OYSTER", "OFFSET", "OUTFIT",
					"PALACE", "PEBBLE", "PICNIC", "PLAQUE", "POCKET", "PROFIT", "PUDDLE", "PENCIL", "PIGEON", "POETRY",
					"QUARTZ", "QUIVER", "QUARRY", "QUEASY", "RABBIT", "REFLEX", "RHYTHM", "RIBBON", "ROCKET", "RAFFLE",
					"RECIPE", "RUBBER", "RADIUS", "RECORD", "SAILOR", "SCHEME", "SEARCH", "SHADOW", "SIGNAL", "SLEIGH",
					"SMUDGE", "SNEEZE", "SOCIAL", "SQUEAK", "TAILOR", "TEACUP", "THIRST", "TICKET", "TOGGLE", "TRAVEL",
					"TUNNEL", "TWITCH", "TEMPLE", "THEORY", "UNISON", "UPWARD", "UTMOST", "UTOPIA", "UNIQUE", "UNREST",
					"UNSEEN", "UNWRAP", "UNVIEL", "UPHOLD", "VACUUM", "VECTOR", "VIEWER", "VORTEX", "VALLEY", "VERBAL",
					"VICTIM", "VOLUME", "VANISH", "VERMIN", "WAFFLE", "WEALTH", "WHEEZE", "WIDGET", "WOLVES", "WRENCH",
					"WALNUT", "WEIGHT", "WISDOM", "WONDER", "YEARLY", "YELLOW", "YONDER", "ZEALOT", "ZEBRAS", "ZODIAC"
			};
		string temp = wordList[UnityEngine.Random.Range(0, wordList.Length)].ToUpper();
		solution = temp.ToUpper();
		Debug.LogFormat("[Colored Hexabuttons #{0}] Generated Word: {1}", moduleId, solution);
		scramble = "";
		for (int aa = 0; aa < 6; aa++)
		{
			int pos = UnityEngine.Random.Range(0, temp.Length);
			scramble = scramble + "" + temp[pos];
			temp = temp.Remove(pos, 1);
		}
		Debug.LogFormat("[Colored Hexabuttons #{0}] Scrambled Word: {1}", moduleId, scramble);
		foreach (int i in buttonIndex)
			hexButtons[i].OnInteract = delegate { pressedOrange(i); return false; };
		hexButtons[6].OnInteract = delegate { pressedOrangeCenter(); return false; };
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		switch (UnityEngine.Random.Range(0, 7))
		{
			case 0://Atbash

				voiceMessage = new string[1];
				voiceMessage[0] = "ATBASH";
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					buttonText[aa].text = alpha[25 - alpha.IndexOf(scramble[aa])] + "";
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using ATBASH: {1}{2}{3}{4}{5}{6}", moduleId, buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			case 1://Caesar
				voiceMessage = new string[3];
				voiceMessage[0] = "CAESAR";
				int r1 = UnityEngine.Random.Range(1, 26);
				if (r1 < 10)
				{
					voiceMessage[1] = "0";
					voiceMessage[2] = r1 + "";
				}
				else
				{
					voiceMessage[1] = (r1 / 10) + "";
					voiceMessage[2] = (r1 % 10) + "";
				}
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					buttonText[aa].text = alpha[mod(alpha.IndexOf(scramble[aa]) - r1, 26)] + "";
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using CAESAR {1}{2}: {3}{4}{5}{6}{7}{8}", moduleId, voiceMessage[1], voiceMessage[2], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			case 2://Condi
				voiceMessage = new string[3];
				voiceMessage[0] = "CONDI";
				int r2 = UnityEngine.Random.Range(1, 26);
				if (r2 < 10)
				{
					voiceMessage[1] = "0";
					voiceMessage[2] = r2 + "";
				}
				else
				{
					voiceMessage[1] = (r2 / 10) + "";
					voiceMessage[2] = (r2 % 10) + "";
				}
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					buttonText[aa].text = alpha[mod(alpha.IndexOf(scramble[aa]) + r2, 26)] + "";
					r2 = alpha.IndexOf(scramble[aa]) + 1;
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using CONDI {1}{2}: {3}{4}{5}{6}{7}{8}", moduleId, voiceMessage[1], voiceMessage[2], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			case 3://Monoalphabetic
				voiceMessage = new string[7];
				voiceMessage[0] = "MONOALPHABETIC";
				temp = alpha.ToUpper();
				for (int aa = 1; aa < 7; aa++)
				{
					voiceMessage[aa] = temp[UnityEngine.Random.Range(0, temp.Length)] + "";
					temp = temp.Replace(voiceMessage[aa], "");
				}
				for (int aa = 6; aa >= 1; aa--)
					temp = voiceMessage[aa] + "" + temp;
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					buttonText[aa].text = temp[alpha.IndexOf(scramble[aa])] + "";
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using MONOALPHABETIC {1}{2}{3}{4}{5}{6}: {7}{8}{9}{10}{11}{12}", moduleId, voiceMessage[1], voiceMessage[2], voiceMessage[3], voiceMessage[4], voiceMessage[5], voiceMessage[6], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			case 4://Porta
				voiceMessage = new string[7];
				voiceMessage[0] = "PORTA";
				temp = alpha.ToUpper();
				for (int aa = 1; aa < 7; aa++)
				{
					voiceMessage[aa] = temp[UnityEngine.Random.Range(0, temp.Length)] + "";
					temp = temp.Replace(voiceMessage[aa], "");
				}
				string[] portaChart =
				{
					"NOPQRSTUVWXYZ",
					"OPQRSTUVWXYZN",
					"PQRSTUVWXYZNO",
					"QRSTUVWXYZNOP",
					"RSTUVWXYZNOPQ",
					"STUVWXYZNOPQR",
					"TUVWXYZNOPQRS",
					"UVWXYZNOPQRST",
					"VWXYZNOPQRSTU",
					"WXYZNOPQRSTUV",
					"XYZNOPQRSTUVW",
					"YZNOPQRSTUVWX",
					"ZNOPQRSTUVWXY"
				};
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					if (alpha.IndexOf(scramble[aa]) < 13)
						buttonText[aa].text = portaChart[alpha.IndexOf(voiceMessage[aa + 1]) / 2][alpha.IndexOf(scramble[aa])] + "";
					else
						buttonText[aa].text = alpha[portaChart[alpha.IndexOf(voiceMessage[aa + 1]) / 2].IndexOf(scramble[aa])] + "";
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using PORTA {1}{2}{3}{4}{5}{6}: {7}{8}{9}{10}{11}{12}", moduleId, voiceMessage[1], voiceMessage[2], voiceMessage[3], voiceMessage[4], voiceMessage[5], voiceMessage[6], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			case 5://Ragbaby
				voiceMessage = new string[3];
				voiceMessage[0] = "RAGBABY";
				int r3 = UnityEngine.Random.Range(0, 26);
				if (r3 < 10)
				{
					voiceMessage[1] = "0";
					voiceMessage[2] = r3 + "";
				}
				else
				{
					voiceMessage[1] = (r3 / 10) + "";
					voiceMessage[2] = (r3 % 10) + "";
				}
				for (int aa = 0; aa < 6; aa++)
				{
					buttonText[aa].color = Color.black;
					buttonText[aa].text = alpha[mod(alpha.IndexOf(scramble[aa]) + r3, 26)] + "";
					r3++;
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using RAGBABY {1}{2}: {3}{4}{5}{6}{7}{8}", moduleId, voiceMessage[1], voiceMessage[2], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
			default://Vigenere
				voiceMessage = new string[7];
				voiceMessage[0] = "VIGENERE";
				temp = alpha.ToUpper();
				string alpha2 = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				for (int aa = 1; aa < 7; aa++)
				{
					voiceMessage[aa] = temp[UnityEngine.Random.Range(0, temp.Length)] + "";
					temp = temp.Replace(voiceMessage[aa], "");
					int r4 = alpha2.IndexOf(scramble[aa - 1]) + alpha2.IndexOf(voiceMessage[aa]);
					while (r4 > 26)
						r4 -= 26;
					buttonText[aa - 1].color = Color.black;
					buttonText[aa - 1].text = alpha2[r4] + "";
				}
				Debug.LogFormat("[Colored Hexabuttons #{0}] Encryption using VIGENERE {1}{2}{3}{4}{5}{6}: {7}{8}{9}{10}{11}{12}", moduleId, voiceMessage[1], voiceMessage[2], voiceMessage[3], voiceMessage[4], voiceMessage[5], voiceMessage[6], buttonText[0].text, buttonText[1].text, buttonText[2].text, buttonText[3].text, buttonText[4].text, buttonText[5].text);
				break;
		}
	}
	void pressedOrange(int n)
	{
		if (!(moduleSolved))
		{
			Debug.LogFormat("[Colored Hexabuttons #{0}] User pressed {1}. Which is the decrypted letter {2}.", moduleId, positions[n], scramble[n]);
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			if (solution[numButtonPresses] == scramble[n])
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
				Debug.LogFormat("[Colored Hexabuttons #{0}] Strike! I was expecting {1}!", moduleId, solution[numButtonPresses]);
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
					hexButtons[i].OnInteract = delegate { pressedOrange(i); return false; };
			}
		}
	}
	void pressedOrangeCenter()
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
		hexButtons[6].OnInteract = delegate { pressedOrangeCenter(); return false; };
	}
	private int mod(int n, int m)
	{
		while (n < 0)
			n += 26;
		return (n % 26);
	}
}
