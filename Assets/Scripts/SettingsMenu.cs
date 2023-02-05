using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioSource sounds;
    [SerializeField]
    private Slider[] sliders;

    //Resolution
    [SerializeField]
    private List<ResItem> resolutions = new List<ResItem>();
    [SerializeField]
    private TMP_Text resolutionLabel;
    private int selectedResolution;
    private bool currentFullScreen = true;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (!PlayerPrefs.HasKey("volumeGame"))
        {
            PlayerPrefs.SetFloat("volumeGame", .5f);
            PlayerPrefs.SetFloat("volumeMusic", .5f);
            PlayerPrefs.SetFloat("sensitivity", 1f);
        }
        else
        {
            sliders[0].value = PlayerPrefs.GetFloat("volumeGame");
            sliders[1].value = PlayerPrefs.GetFloat("volumeMusic");
            sliders[2].value = PlayerPrefs.GetFloat("sensitivity") / 5;
        }
        audioMixer.SetFloat("volumeGame", Mathf.Log10(PlayerPrefs.GetFloat("volumeGame")) * 20);
        audioMixer.SetFloat("volumeMusic", Mathf.Log10(PlayerPrefs.GetFloat("volumeMusic")) * 20);

        //Checks if the resolution exists, otherwise it makes a new one. Classic video. https://www.youtube.com/watch?v=yeaELkoxD9w
        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
            }
        }
        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;
            UpdateResLabel();
        }
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("volumeGame", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("volumeGame", sliderValue);
    }

    public void SetVolumeMusic(float sliderValue)
    {
        audioMixer.SetFloat("volumeMusic", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("volumeMusic", sliderValue);
    }

    public void SetSensitivity(Slider slide)
    {
        PlayerPrefs.SetFloat("sensitivity", slide.value * 5);
    }

    public void ToggleFullScreen()
    {
        Debug.Log("Toggling full screen.");
        Screen.fullScreen = !Screen.fullScreen;
        currentFullScreen = !currentFullScreen;
    }

    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void ToMenu()
    {
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void UnloadSettings()
    {
        SceneManager.UnloadSceneAsync("TySettings");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("TySettings");
        Cannon cann = FindObjectOfType<Cannon>();
        cann.MouseSensitivity = PlayerPrefs.GetFloat("sensitivity");
    }

    public void GameScene()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MenuNav(AudioClip audioClip)
    {
        sounds.PlayOneShot(audioClip);
    }

    //Resolution
    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, currentFullScreen);
    }

    [System.Serializable]
    public class ResItem
    {
        public int horizontal, vertical;
    }
}