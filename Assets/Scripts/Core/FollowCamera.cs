using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float cameraSpeed = 5f;

        private void Awake()
        {
            transform.position = target.position;
        }

        private void LateUpdate()
        {
            var smooth = 1f - Mathf.Pow(0.5f, Time.deltaTime * cameraSpeed);
            transform.position = Vector3.Lerp(transform.position, target.position, smooth);
        }
    }
}
