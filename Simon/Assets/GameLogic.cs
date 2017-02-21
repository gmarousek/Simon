using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameLogic : MonoBehaviour {

	public Button YellowButton; //Button 0
	public Button BlueButton; 	//Button 1
	public Button RedButton; 	//Button 2
	public Button GreenButton; 	//Button 3
	public Button play;
	public Text LoseText;		
	public float GameSpeed; 	//The smaller the number, the faster the game

	AudioSource Csharp; //Yellow sound
	AudioSource Ehigh;	//Blue sound
	AudioSource A;		//Red sound
	AudioSource Elow;	//Green sound

	private Button[] buttons;			//A better way to keep track of the button objects
	private List<Button> CPUmoves;		//A list of the moves Simon has made

	Button buttonPress = null;

	void Start () {
		LoseText.enabled = false;
		ResetColors();
		ButtonsEnabled (false);

		YellowButton.onClick.AddListener (() => ButtonClick (0));
		BlueButton.onClick.AddListener (() => ButtonClick (1));
		RedButton.onClick.AddListener (() => ButtonClick (2));
		GreenButton.onClick.AddListener (() => ButtonClick (3));

		play.onClick.AddListener (() => StartCoroutine (Play ()));

		Csharp = YellowButton.GetComponent<AudioSource> ();
		Ehigh = BlueButton.GetComponent<AudioSource> ();
		A = RedButton.GetComponent<AudioSource> ();
		Elow = GreenButton.GetComponent<AudioSource> ();

		buttons = new Button[4] {YellowButton, BlueButton, RedButton, GreenButton};

		CPUmoves = new List<Button> ();
	}
	
	IEnumerator Play () {
		play.interactable = false;
		bool lose = false;
		LoseText.enabled = false;
		ResetColors ();
		CPUmoves.Clear ();

		int ButtonIndex;
		while (!lose) {
			ButtonsEnabled (false);
			ButtonIndex = Random.Range (0, 4); 			//Pick a random button to press
			CPUmoves.Add (buttons [ButtonIndex]);		//Add it to the list of moves Simon has made

			foreach (Button move in CPUmoves) { 		//Simon shows you what moves to make
				move.onClick.Invoke ();
				yield return new WaitForSeconds (GameSpeed + 0.1f);
				ResetColors ();
				yield return new WaitForSeconds (0.1f);
			}
			buttonPress = null;

			for (int i = 0; i < CPUmoves.Count; i++) { 	//Now it's the player's turn to move
				Button nextButton = CPUmoves [i];
				ButtonsEnabled (true);
				yield return new WaitUntil (() => buttonPress != null);
				ButtonsEnabled (false);
				if (nextButton != buttonPress) {
					lose = true;
					break;
				}
				buttonPress = null;
				ResetColors ();
				yield return new WaitForSeconds (GameSpeed + 0.1f); 	//Wait for the audio clip to stop playing
				ButtonsEnabled (true);
			}
		}
		LoseText.enabled = true;
		play.interactable = true;
	}

	void ButtonClick(int button) {
		switch (button) {
		case 0:
			Csharp.Play ();
			Csharp.SetScheduledEndTime (AudioSettings.dspTime + GameSpeed);
			YellowButton.image.color = Color.white;
			buttonPress = YellowButton;
			break;
		case 1:
			Ehigh.Play ();
			Ehigh.SetScheduledEndTime (AudioSettings.dspTime + GameSpeed);
			BlueButton.image.color = Color.white;
			buttonPress = BlueButton;
			break;
		case 2:
			A.Play ();
			A.SetScheduledEndTime (AudioSettings.dspTime + GameSpeed);
			RedButton.image.color = Color.white;
			buttonPress = RedButton;
			break;
		case 3:
			Elow.Play ();
			Elow.SetScheduledEndTime (AudioSettings.dspTime + GameSpeed);
			GreenButton.image.color = Color.white;
			buttonPress = GreenButton;
			break;
		default:
			Debug.LogError (button + " is not a valid button code.");
			break;
		}
	}

	void ResetColors () {
		YellowButton.image.color = Color.yellow;
		BlueButton.image.color = Color.blue;
		RedButton.image.color = Color.red;
		GreenButton.image.color = Color.green;
	}

	void ButtonsEnabled (bool enable) {
		YellowButton.interactable = enable;
		BlueButton.interactable = enable;
		RedButton.interactable = enable;
		GreenButton.interactable = enable;
	}
}
