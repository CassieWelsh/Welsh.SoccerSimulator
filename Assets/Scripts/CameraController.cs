using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class CameraController
    {
        private static CameraState _cameraState = CameraState.Player;

        private static Dictionary<CameraState, GameObject> _cameraPositions { get; set; }

        public static void SetPositions(Dictionary<CameraState, GameObject> cp) => _cameraPositions = cp;

        public static Transform NextCameraPosition()
        {
            switch (_cameraState)
            {
                case CameraState.Player:
                    Camera.main.orthographic = true;
                    Camera.main.orthographicSize = 25;
                    _cameraState = CameraState.Top;
                    return _cameraPositions[CameraState.Top].transform;
                case CameraState.Top:
                    Camera.main.orthographic = false;
                    _cameraState = CameraState.Alongside;
                    return _cameraPositions[CameraState.Alongside].transform;
                case CameraState.Alongside:
                    Camera.main.orthographic = false;
                    _cameraState = CameraState.Player;
                    return _cameraPositions[CameraState.Player].transform;
            }

            return null;
        }

        public enum CameraState : byte
        {
            Player,
            Top,
            Alongside
        }

        public static void NextCameraPosition(out Transform cameraTarget)
        {
            throw new System.NotImplementedException();
        }
    }
}