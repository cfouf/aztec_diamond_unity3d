using UnityEngine;

namespace _Scripts.Utilities
{
    public class SceneManager : MonoBehaviourService<SceneManager>
    {
        public void ToMenuScene() => SceneChanger.ChangeScene(0);

        public void To2dScene() => SceneChanger.ChangeScene(1);

        public void To2d3dScene() => SceneChanger.ChangeScene(2);

        public void To3dScene() => SceneChanger.ChangeScene(3);

        public void ExitGame() => Application.Quit();

        protected override void OnCreateService()
        {
        }

        protected override void OnDestroyService()
        {
        }
    }
}