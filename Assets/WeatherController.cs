using UnityEngine;

namespace DefaultNamespace
{
    public class WeatherController
    {
        private static WeatherStates _state = WeatherStates.Usual;
        private static readonly Quaternion UsualLight = new(50, -30, 0, 0);
        private static readonly Quaternion RainLight = new(1, 2, -4.5f, 0);

        public static void RefreshWeather(GameObject rain, GameObject light)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                UpdateWeatherInternal(rain, light);
            }
        }

        private static void UpdateWeatherInternal(GameObject rain, GameObject light)
        {
            switch (_state)
            {
                case WeatherStates.Rain:
                    rain.SetActive(false);
                    light.transform.rotation = RainLight;
                    Resources.Load<Material>("Materials/grass2").color = Color.white;
                    Camera.main.clearFlags = CameraClearFlags.Skybox;
                    _state = WeatherStates.Usual;
                    return;
                case WeatherStates.Usual:
                    rain.SetActive(true);
                    light.transform.rotation = UsualLight;
                    Camera.main.clearFlags = CameraClearFlags.SolidColor;
                    Resources.Load<Material>("Materials/grass2").color = Color.grey;
                    _state = WeatherStates.Rain;
                    return;
            }
        }
    }

    public enum WeatherStates : byte
    {
        Usual,
        Rain
    }
}