using System;
using System.Collections.Generic;

namespace GhostSystem
{
    [Serializable]
    public class Way
    {
        public List<Point> points = new List<Point>();
        
        public void Add(float x, float y, float z, float time)
        {
            points.Add(new Point(x,y,z,time));
        }
    }
}