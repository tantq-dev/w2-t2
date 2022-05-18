using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class move_gameobject : MonoBehaviour
{

    public float MoveSpeed=5.0f;
    public float RotateSpeed = 2f;
    private int current = 0;
    public Waypoints waypoints = default;



    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = waypoints.ListOfPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPoint();
    }
    private void MoveToPoint()
    {
        if (Vector3.Distance(waypoints.ListOfPoints[current], transform.position) < 1)
        {
            current++;
            if (current >= waypoints.ListOfPoints.Count)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints.ListOfPoints[current], MoveSpeed * Time.deltaTime);
        var Rotation = Quaternion.LookRotation(waypoints.ListOfPoints[current] - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime*RotateSpeed);
    }
}
