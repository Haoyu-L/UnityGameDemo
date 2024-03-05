using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    private const float rotationSlerpAmount = .68f;
    
    [Header("References")]
    public Transform patrollerRoot;
    public Transform patrollerModel;
    
    [Header("Stats")]
    public float moveSpeed = 2;
    
    private int currentPointIndex = 0;
    private Transform currentPoint;

    public Transform[] patrolPoints;
    private string patrolNamePattern = "PatrolPoint_"; 
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> points = GetUnsortedPatrolPoints();
        if (points.Count <= 0) return;

        patrolPoints = new Transform[points.Count];
        for (int i = 0; i < points.Count; i++) {
            Transform point = points[i];
            int index = Convert.ToInt32(
                point.gameObject.name.Substring(
                    patrolNamePattern.Length
                )
            );
            Debug.Log(String.Format("Index: {0}", index));
            Debug.Log(String.Format("Point: {0}", point.gameObject.name));
            patrolPoints[index] = point;
            point.SetParent(null);
            point.gameObject.hideFlags = HideFlags.HideInInspector;
        }
        SetCurrentPatrolPoint(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoint == null) {
            return;
        }
  
        // try to move to current point
        patrollerRoot.position = Vector3.MoveTowards(patrollerRoot.position, currentPoint.position, moveSpeed * Time.deltaTime);

        // already on the current point now, ready to set up for next move
        if (patrollerRoot.position == currentPoint.position) {
            
            // move to starting point if a loop is finished
            if (currentPointIndex >= patrolPoints.Length - 1) {
                SetCurrentPatrolPoint(0);
            } else {
                SetCurrentPatrolPoint(currentPointIndex + 1);
            }
        // stll moving, then keep moving
        } else {
            Quaternion lookRotation = Quaternion.LookRotation((currentPoint.position - patrollerRoot.position).normalized);
            patrollerModel.rotation = Quaternion.Slerp(patrollerModel.rotation, lookRotation, rotationSlerpAmount);
        }
    }

    private List<Transform> GetUnsortedPatrolPoints() {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        var points = new List<Transform>();

        for (int i = 0; i < children.Length; i++) {
            if (children[i].gameObject.name.StartsWith(patrolNamePattern)) {
                points.Add(children[i]);
            }

        }

        return points;
    }

    private void SetCurrentPatrolPoint(int index) {
        currentPointIndex = index;
        currentPoint = patrolPoints[index];
    }
}
