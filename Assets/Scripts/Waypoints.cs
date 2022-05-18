using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<Vector3> ListOfPoints = new List<Vector3>();
    private void OnDrawGizmos()
    {
        for (int i = 0; i < ListOfPoints.Count; i++)
        {
            Vector3 position = ListOfPoints[i];
            Gizmos.DrawWireSphere(position, 0.2f);
            if (i == 0)
            {
                Gizmos.DrawLine(position, ListOfPoints[ListOfPoints.Count - 1]);
            }
            if (i > 0)
            {
                Vector3 previousPoint = ListOfPoints[i - 1];
                Gizmos.DrawLine(position, previousPoint);
            }
        }
    }
}
