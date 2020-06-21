using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float spehereRadius = .3f;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (var i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), spehereRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int currentIndex)
        {
            if (currentIndex + 1 == transform.childCount)
            {
                return 0;
            }
            return currentIndex + 1;
        }
    }
}