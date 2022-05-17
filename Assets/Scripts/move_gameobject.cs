using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class move_gameobject : MonoBehaviour
{

    public float MoveSpeed=5.0f;
    public float RotateSpeed = 2f;
    public List<Vector3> ListOfPoints = new List<Vector3>();
    private int current = 0;


    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = ListOfPoints[0];
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        for (int i = 0; i < ListOfPoints.Count; i++)
        {
            Vector3 position = ListOfPoints[i];
            Gizmos.DrawWireSphere(position, 0.2f);
            if (i == 0)
            {
                Gizmos.DrawLine(position, ListOfPoints[ListOfPoints.Count-1]);
            }
            if (i > 0)
            {
                Vector3 previousPoint = ListOfPoints[i - 1];
                Gizmos.DrawLine(position, previousPoint);
                
            }
        }
    }
    void Update()
    {
        MoveToPoint();
    }
    private void MoveToPoint()
    {
        if (Vector3.Distance(ListOfPoints[current], transform.position) < 1)
        {
            current++;
            if (current >= ListOfPoints.Count)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, ListOfPoints[current], MoveSpeed * Time.deltaTime);
        var Rotation = Quaternion.LookRotation(ListOfPoints[current] - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime*RotateSpeed);
    }
}
