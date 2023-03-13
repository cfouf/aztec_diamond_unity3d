using UnityEngine;

namespace _Scripts.Utilities
{
    public static class SceneChanger
    {
        public static void ChangeScene(int id)
        {
            if (id >= 0 && id <= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
                UnityEngine.SceneManagement.SceneManager.LoadScene(id);
            else
                Debug.LogError("Id scene does not exist. Max of them is - " + UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings +
                               " but was - " + id);
        }
    }
}