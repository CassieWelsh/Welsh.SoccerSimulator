using UnityEngine;

namespace Constructors
{
    public class BallConstructor : IConstructible
    {
        public GameObject PrepareObject()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ChangePosition(go);
            AddRigidbody(go);
            ApplyTexture(go);
            return go;
        }

        private void AddRigidbody(GameObject go)
        {
            go.AddComponent<Rigidbody>();
        }

        private void ChangePosition(GameObject go)
        {
            var transform = go.transform.position;
            transform.x = transform.y = 1;
            go.transform.position = transform;
        }

        private void ApplyTexture(GameObject go)
        {
            var texture = Resources.Load<Material>("Materials/ball");

            //var material = new Material(Shader.Find("Standart"));
            //material.mainTexture = texture;
            go.GetComponent<MeshRenderer>().material = texture;
        }
    }
}