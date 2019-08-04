using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void OnCreditsBackButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("CharacterAndEnemy");
    }

    public void OnCreditsButtonClicked()
    {
        SceneManager.LoadScene("Credits");
    }
}
