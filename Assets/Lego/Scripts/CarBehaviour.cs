﻿using UnityEngine;

class RoadPoint
{
  public Vector3[] point;
  public int length;

  public RoadPoint(int num)
  {
    this.length = num;
    point = new Vector3[num];
  }
}

public class CarBehaviour : MonoBehaviour
{
  private float timeElapsed;
  private RoadPoint[] roadPointArray = new RoadPoint[2];
  int roadPointNum;
  void Start()
  {
    this.transform.position = new Vector3(-1f, 0f, 15f);
    roadPointArray[0] = new RoadPoint(2);
    roadPointArray[0].point[0] = new Vector3(1f, 0f, 15f);
    roadPointArray[0].point[1] = new Vector3(1f, 0f, 5f);
    roadPointArray[1] = new RoadPoint(3);
    roadPointArray[1].point[0] = new Vector3(1f, 0f, 5f);
    roadPointArray[1].point[1] = new Vector3(1f, 0f, -1f);
    roadPointArray[1].point[2] = new Vector3(-5f, 0f, -1f);

    timeElapsed = 0f;
    roadPointNum = 0;
  }
  void Update()
  {
    timeElapsed += Time.deltaTime;
    float t = timeElapsed / 5f;

    MoveCar(roadPointNum, t);

    Debug.Log(t);
    if (timeElapsed > 5f)
    {
      timeElapsed = 0;
      roadPointNum++;
    }
  }

  void MoveCar(int num, float t)
  {
    if (roadPointArray[num].length == 2)
    {
      MoveStraight(num, t);
    }
    else if (roadPointArray[num].length == 3)
    {
      MoveCurve(num, t);
    }
  }

  Vector2 Bezier2D(Vector2 p1, Vector2 p2, float t)
  {
    return (1 - t) * p1 + t * p2;
  }

  void MoveStraight(int num, float t)
  {
    Vector2 p1 = new Vector2(roadPointArray[num].point[0].x, roadPointArray[num].point[0].z);
    Vector2 p2 = new Vector2(roadPointArray[num].point[1].x, roadPointArray[num].point[1].z);
    Vector2 pos = Bezier2D(p1, p2, t);
    Vector3 newPos = new Vector3(pos.x, 0f, pos.y);

    this.transform.position = newPos;
  }

  void MoveCurve(int num, float t)
  {
    Vector2 p1 = new Vector2(roadPointArray[num].point[0].x, roadPointArray[num].point[0].z);
    Vector2 p2 = new Vector2(roadPointArray[num].point[1].x, roadPointArray[num].point[1].z);
    Vector2 p3 = new Vector2(roadPointArray[num].point[2].x, roadPointArray[num].point[2].z);
    Vector2 pos = Bezier2D(Bezier2D(p1, p2, t), Bezier2D(p2, p3, t), t);
    Vector3 newPos = new Vector3(pos.x, 0f, pos.y);
    Vector2 direction = 2 * ((t - 1) * p1 + (1 - 2 * t) * p2 + p3 * t);
    Vector3 direction3d = new Vector3(direction.x, 0f, direction.y);

    this.transform.position = newPos;
    this.transform.rotation = Quaternion.LookRotation(direction3d);
  }
}