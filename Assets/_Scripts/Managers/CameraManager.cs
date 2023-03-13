using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Managers
{
    public class CameraManager : MonoBehaviourService<CameraManager>
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float timeToMove;
        [SerializeField] private float moveDistance;
        [SerializeField] private bool isOrthographic;

        private float speed;
        private float distanceLeft;
        private float currentDistance;
        public bool isMoving;

        private void Update()
        {
            if (!isMoving) return;

            if (isOrthographic)
                mainCamera.orthographicSize += speed * Time.deltaTime;
            else
                mainCamera.transform.position += new Vector3(0, speed * Time.deltaTime, -speed * Time.deltaTime);
            currentDistance += speed * Time.deltaTime;

            if (!(currentDistance >= distanceLeft)) return;
            isMoving = false;
            currentDistance = 0;
        }

        public void StartMoving()
        {
            isMoving = true;
            distanceLeft += moveDistance;
            speed = distanceLeft / timeToMove;
        }

        protected override void OnCreateService()
        {
        }

        protected override void OnDestroyService()
        {
        }
    }
}