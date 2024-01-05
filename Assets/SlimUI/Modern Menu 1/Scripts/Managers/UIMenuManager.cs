using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	public class UIMenuManager : MonoBehaviour {
		private Animator CameraObject;

		// campaign button sub menu
        [Header("MENUS")]
        [Tooltip("The Menu for when the MAIN menu buttons")]
        public GameObject mainMenu;
        [Tooltip("THe first list of buttons")]
        public GameObject firstMenu;
        [Tooltip("The Menu for when the PLAY button is clicked")]
        public GameObject playMenu;
        [Tooltip("The Menu for when the EXIT button is clicked")]
        public GameObject exitMenu;

		[Header("SFX")]
        [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
        public AudioSource hoverSound;

		void Start(){
			CameraObject = transform.GetComponent<Animator>();
			exitMenu.SetActive(false);
			firstMenu.SetActive(true);
			mainMenu.SetActive(true);
		}

		public void ReturnMenu(){
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}

		public void Position2(){
			CameraObject.SetFloat("Animate",1);
		}

		public void Position1(){
			CameraObject.SetFloat("Animate",0);
		}

		public void PlayHover(){
			hoverSound.Play();
		}

		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			exitMenu.SetActive(true);
		}

		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}
}