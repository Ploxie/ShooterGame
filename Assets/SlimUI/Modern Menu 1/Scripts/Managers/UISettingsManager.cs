using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class UISettingsManager : MonoBehaviour {

		// sliders
		public GameObject musicSlider;

		public void  Start (){
			// check slider values
			musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
			
		}

		public void MusicSlider (){
			//PlayerPrefs.SetFloat("MusicVolume", sliderValue);
			PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
		}
	}
}