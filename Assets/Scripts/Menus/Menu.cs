using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    public class Menu : MonoBehaviour
    {
    const string Scene1 = "MainMenu";
    const string Scene2 = "FightPhoton";
    const string Scene3 = "MainMenu";
    const string Scene4 = "MainMenu";
    [SerializeField] Animator anim;
    public void Load(int level)
        {
            SceneManager.LoadScene(level);
        }
        public void Quit()
        {
            Application.Quit();
        }
}

