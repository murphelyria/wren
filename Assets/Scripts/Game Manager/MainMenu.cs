using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject playPanel;
    public GameObject settingsPanel;
    public GameObject exitPanel;

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        playPanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    public void ShowPlayPanel()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(true);
        settingsPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(false);
        settingsPanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    public void ShowExitPanel()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
