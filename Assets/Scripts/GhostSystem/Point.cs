using System;
using UnityEngine;

namespace GhostSystem
{
    [Serializable]
    public class Point
    {
        public float x;
        public float y;
        public float z;
        public float time;

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
            time = 0;
        }
        
        public Point(float xCord, float yCord, float zCord, float timeMoment)
        {
            x = xCord;
            y = yCord;
            z = zCord;
            time = timeMoment;
        }
        
        public Vector3 Position() => new Vector3(x, y, z);
    }
}