using System;
using System.Collections.Generic;
using Metro.Movement;
using Metro.Trains;
using UnityEngine;

namespace Metro.Audio
{

    public sealed class AssemblyAudioSource : AssemblyComponent
    {

        private static readonly Comparison<Vector3> DistanceComparison = (a, b) => Vector3.Distance(a, MovementController.LastPosition).CompareTo(Vector3.Distance(b, MovementController.LastPosition));

        private readonly List<CarriageAudioBounds> _bounds = new();
        private readonly List<Vector3> _closestPoints = new();

        private Transform _t;

        private void Awake() => _t = transform;

        private void LateUpdate()
        {
            foreach (var car in Parent.Cars)
            {
                if (!car.IsPlayerMounted)
                    continue;
                _t.position = MovementController.LastPosition;
                return;
            }

            for (var i = 0; i < _bounds.Count; i++)
                _closestPoints[i] = _bounds[i].Collider.ClosestPoint(MovementController.LastPosition);
            _closestPoints.Sort(DistanceComparison);
            _t.position = _closestPoints[0];
        }

        protected override void OnInitialized()
        {
            foreach (var car in Parent.Cars)
                _bounds.Add(car.RequireComponent<CarriageAudioBounds>());
            for (var i = 0; i < _bounds.Count; i++)
                _closestPoints.Add(Vector3.zero);
        }

    }

}
