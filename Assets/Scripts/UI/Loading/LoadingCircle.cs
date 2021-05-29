using UnityEngine;

namespace Assets.Scripts.UI.Loading
{
    public class LoadingCircle : MonoBehaviour
    {
        public GameObject Character;
        public GameObject Weapon;
        public float AngularVelocity;

        void Update()
        {
            Weapon.transform.RotateAround(Character.transform.position, Vector3.forward, AngularVelocity * Time.deltaTime);
        }
    }
}
