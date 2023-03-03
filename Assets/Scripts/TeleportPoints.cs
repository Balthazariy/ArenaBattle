using System.Collections.Generic;
using UnityEngine;

namespace Balthazariy.ArenaBattle
{
    public class TeleportPoints : MonoBehaviour
    {
        private List<Transform> _points;

        private void Awake()
        {
            _points = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
                _points.Add(transform.GetChild(i));
        }

        public Vector3 GetRandomPoint() => _points[UnityEngine.Random.Range(0, _points.Count)].localPosition;
    }
}