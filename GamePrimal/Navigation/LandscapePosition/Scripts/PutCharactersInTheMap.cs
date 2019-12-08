using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.GamePrimal.Navigation.LandscapePosition.Scripts
{
    public class PutCharactersInTheMap : MonoBehaviour
    {
        public MonoAmplifierRpg[] Characters;
        public Vector2[] Positions;
        public bool Inverse = false;
        private Transform _lastTransform;

        // Start is called before the first frame update
        void Start()
        {
            Vector3 leftBottom = transform.GetChild(0).position;

            if (Inverse)
                _lastTransform = Instantiate(Characters[0],
                    new Vector3(Positions[0].x * -1 + leftBottom.x, leftBottom.y, Positions[0].y * -1 + leftBottom.z),
                    Quaternion.identity).transform;
            else
                _lastTransform = Instantiate(Characters[0],
                    new Vector3(Positions[0].x + leftBottom.x, leftBottom.y, Positions[0].y + leftBottom.z),
                    Quaternion.identity).transform;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 leftBottom = transform.GetChild(0).position;


            if (Inverse)
                _lastTransform.position = new Vector3(Positions[0].x * -1 + leftBottom.x, leftBottom.y,
                Positions[0].y * -1 + leftBottom.z);
            else
                _lastTransform.position = new Vector3(Positions[0].x + leftBottom.x, leftBottom.y,
                    Positions[0].y + leftBottom.z);
        }
    }
}
