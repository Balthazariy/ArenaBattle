using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy
{
    public class TeleportPoints : MonoBehaviour
    {
        private List<Transform> _points;

        private void Awake()
        {
            _points = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                _points.Add(transform.GetChild(i));
            }
        }

        public Vector3 GetRandomPoint()
        {
            int index = UnityEngine.Random.Range(0, _points.Count);
            Debug.Log(index + " " + _points[index].name);
            Debug.Log(_points[index].position + " " + _points[index].localPosition);
            return _points[index].localPosition;
        }
    }
}