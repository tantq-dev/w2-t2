using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoints))]
public class pointlistEditor : Editor
{
    Vector3 temp;
    Waypoints list;

    Vector3 newPoint;
    Vector3 newWaypointPosition = default;
    int insertIndex = default;


    private void OnEnable()
    {
        list = (Waypoints)target;
        SceneView.duringSceneGui += DuringSceneView;
    }
    private void DuringSceneView(SceneView scene)
    {
        serializedObject.Update();
        for (int i = 0; i < list.ListOfPoints.Count; i++)
        {
            if (list != null)
            {
                list.ListOfPoints[i] = Handles.PositionHandle(list.ListOfPoints[i], Quaternion.identity);
                list.ListOfPoints[i] = Handles.FreeMoveHandle(list.ListOfPoints[i], Quaternion.identity, 3f, Vector3.zero, Handles.SphereHandleCap);
            }
        }

        if (Event.current.keyCode == (KeyCode.D) && Event.current.type == EventType.KeyDown)
        {
            DeletePoint();
            Repaint();
            Event.current.Use();
        }
        if (Event.current.keyCode == (KeyCode.A) && Event.current.type == EventType.KeyDown)
        {
            AddWaypointBetweenPoints();
            Repaint();
            Event.current.Use();
        }
    }

    private void DeletePoint()
    {

        int deleteIndex = default;
        for (int i = 0; i < list.ListOfPoints.Count; i++)
        {
            Vector3 closet = HandleUtility.ClosestPointToPolyLine(list.ListOfPoints.ToArray());
            GetPropertyValues(i);
            float distancetoPoint = Vector3.Distance(closet, temp);
            Debug.Log("distancetoPoint" + distancetoPoint);
            Debug.Log("Closet" + closet);
            if (distancetoPoint < 1f)
            {
                deleteIndex = i;
                Debug.Log("Index" + i);
            }
        }
        if (deleteIndex >= 0)
        {
            list.ListOfPoints.Remove(list.ListOfPoints[deleteIndex]);
        }
    }
    private void AddWaypoint(int insertIndex, int modifiedIndex, Vector3 propertyVector3Value)
    {
        SetNewWaypointPosition(Vector3.zero);

        if (Physics.Raycast(GetRayFromMouse(), out RaycastHit hitInfo))
        {
            SetNewWaypointPosition(hitInfo.point);
        }
        else if (MakePlane(propertyVector3Value).Raycast(GetRayFromMouse(), out float hitDistance))
        {
            SetNewWaypointPosition(GetRayFromMouse().origin + (GetRayFromMouse().direction / 2) + (GetRayFromMouse().direction * hitDistance));
        }
        InsertElement(insertIndex + 1, newWaypointPosition);
        GetNewPropertyValues(modifiedIndex);
        newPoint = newWaypointPosition;
    }
    private void GetNewPropertyValues(int index)
    {
        newPoint = list.ListOfPoints[index];
    }
    private void InsertElement(int insertIndex, Vector3 pos)
    {
        list.ListOfPoints.Insert(insertIndex, pos);
    }
    private Ray GetRayFromMouse()
    {
        return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
    }
    private Plane MakePlane(Vector3 vector3Value)
    {
        return new Plane(GetRayFromMouse().direction, vector3Value);
    }
    private void SetNewWaypointPosition(Vector3 newPosition)
    {
        newWaypointPosition = newPosition;
    }
    private void FindNearestWaypoint(ref float distanceComparisonValue, ref int firstWaypointIndex)
    {
        for (int i = 0; i < list.ListOfPoints.Count; i++)
        {
            GetPropertyValues(i);

            int nextIndex = i + 1;

            nextIndex %= list.ListOfPoints.Count;

            Vector3 nextProperty = list.ListOfPoints[nextIndex];

            float distanceFromMouseToLine = HandleUtility.DistanceToLine(temp, nextProperty);

            if (distanceComparisonValue > distanceFromMouseToLine)
            {
                distanceComparisonValue = distanceFromMouseToLine;
                firstWaypointIndex = i;
            }
        }
    }
    private void AddWaypointBetweenPoints()
    {
        SetInsertIndex(0);

        float distanceComparisonValue = 515;

        FindNearestWaypoint(ref distanceComparisonValue, ref insertIndex);

        AddWaypoint(insertIndex, insertIndex + 1, temp);
    }
    private void SetInsertIndex(int listIndex)
    {
        insertIndex = listIndex;
    }
    private void GetPropertyValues(int i)
    {
        temp = list.ListOfPoints[i];
    }
    private void CreateNewWaypoint()
    {
        if (list.ListOfPoints.Count == 0 || list.ListOfPoints == null)
        {
            AddFirstWaypoint();
        }
        else
        {
            AddWaypointBetweenPoints();
        }
    }
    private void AddFirstWaypoint()
    {
        SetInsertIndex(0);

        SetNewWaypointPosition(Vector3.zero);

        if (Physics.Raycast(GetRayFromMouse(), out RaycastHit hitInfo))
        {
            SetNewWaypointPosition(hitInfo.point);
        }
        else
        {
            Debug.LogWarning("No terrain found. Waypoint was created at position 0,0,0.");
        }
        InsertElement(insertIndex, newWaypointPosition);
        GetNewPropertyValues(insertIndex);
        newPoint = newWaypointPosition;
    }

public static float DistanceToLine(Ray ray, Vector3 point)
{
    return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
}
}
