using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointRadius = 0.4f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(getWayPointPosition(i), waypointRadius);
                Gizmos.DrawLine(getWayPointPosition(i),getWayPointPosition(j));
            }
        }
        public int GetNextIndex(int point)
        {
            if(point+1== transform.childCount)
            {
                return 0;
            }
            return point + 1;
        }
        public Vector3 getWayPointPosition(int point)
        {
            return transform.GetChild(point).position;
        }
        
    }
}
