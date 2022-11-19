using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
  public void OnPlayClick()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
  }

  public void OnQuitClick()
  {
    Application.Quit();
  }
}
