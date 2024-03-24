using UnityEngine;

namespace Constructors
{
    public sealed class PlayerContructor : IConstructible
    {
        public GameObject PrepareObject()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "Player";
            var p = go.transform.position;
            p.y = 1f;
            go.transform.position = p;
            PrepareRigidbody(go);
            PrepareEyes(go);
            return go;
        }

        private void PrepareEyes(GameObject go)
        {
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.black;

            var leftEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var rightEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            leftEye.transform.parent = rightEye.transform.parent = go.transform;

            leftEye.transform.localPosition = new(.2f, .5f, .5f);
            leftEye.transform.localScale = new(.25f, .25f, .25f);
            leftEye.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/eyes");

            rightEye.transform.localPosition = new(-.2f, .5f, .5f);
            rightEye.transform.localScale = new(.25f, .25f, .25f);
            rightEye.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/eyes");
        }

        private void PrepareRigidbody(GameObject go) => go.AddComponent<MovementController>();
    }
}