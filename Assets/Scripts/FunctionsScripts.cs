
using UnityEngine;

namespace importedFunctions
{
    public class ClosestPointToLine
    {
        public static Vector3 getClosestPointToLine(Vector3 a, Vector3 b, Vector3 p)
        {
            float distance = Vector3.Distance(a, b);
            float t = (
                (p.x - a.x) * (b.x - a.x) +
                (p.y - a.y) * (b.y - a.y) +
                (p.z - a.z) * (b.z - a.z)
            ) / (distance * distance);
            return a + t * (b - a);
        } 
    }
    
}
