using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

    public Button startButton;
    public AudioSource clickSource;

	void Awake () {
        startButton.onClick.AddListener(delegate { StartGame(); });
	}

    private void StartGame() {
        AudioManager.instance.StartBackgroundMusic();
        clickSource.Play();
    }
}
