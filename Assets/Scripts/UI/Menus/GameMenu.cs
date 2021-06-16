using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI.Loading;

namespace Assets.Scripts.UI.Menus
{
    public class GameMenu : MonoBehaviour
    {
        public void Exit()
        {
            LoadingData.NextScene = "MainMenu";
            SceneManager.LoadScene("Loading");
        }
    }
}
