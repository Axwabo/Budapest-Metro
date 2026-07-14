using System;
using System.Collections.Generic;
using Metro.Movement;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    public sealed class AssemblyAudioSource : AssemblyComponent
    {

        private static readonly Comparison<(Vector3, float)> DistanceComparison = (a, b) => a.Item2.CompareTo(b.Item2);

        private readonly List<(Vector3, float)> _closestPoints = new();

        private Transform _t;

        private void Awake() => _t = transform;

        private void LateUpdate()
        {
            var last = MovementController.LastPosition;
            if (IsPlayerMounted)
            {
                _t.position = last;
                return;
            }

            for (var i = 0; i < _closestPoints.Count; i++)
            {
                var closest = Parent.Cars[i].Bounds.ClosestPoint(last);
                _closestPoints[i] = (closest, Vector3.Distance(closest, last));
            }

            _closestPoints.Sort(DistanceComparison);
            _t.position = _closestPoints[0].Item1;
        }

        protected override void OnInitialized()
        {
            var count = Parent.Cars.Length;
            for (var i = 0; i < count; i++)
                _closestPoints.Add((Vector3.zero, 0));
        }

    }

}
