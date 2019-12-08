using System;
using Assets.GamePrimal.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GamePrimal.Controllers
{
    [Serializable]
    public struct SceneManagerStruct
    {
        public SceneField MainBattleScene;
        public SceneField MainMenuScene;
    }

    public class SceneShift : MonoBehaviour
    {
        public SceneManagerStruct SceneManagerStruct;

        public void LoadBattleScene()
        {
            SceneManager.LoadScene(SceneManagerStruct.MainBattleScene);
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}