using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQL_Classes
{
    [Serializable]
    public class User
    {
        public int id;
        public string name;
        public List<Level> levels = new List<Level>();
    }

    [Serializable]
    public class Level
    {
        public int num;
        public float time;
    }
    
    [Serializable]
    public class Point
    {
        public Vector3 Position => new Vector3(x, y, z);
        public float x;
        public float y;
        public float z;
        public float time;

        public Point(float xCord, float yCord, float zCord, float timeMoment)
        {
            x = xCord;
            y = yCord;
            z = zCord;
            time = timeMoment;
        }
    }

    [Serializable]
    public class Points
    {
        public List<Point> points = new List<Point>();

        public void Add(float x, float y, float z, float time)
        {
            points.Add(new Point(x,y,z,time));
        }
    }

    [Serializable]
    public class Ways
    {
        public List<Points> ways = new List<Points>();
    }
}