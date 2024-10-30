using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Instantiate a rigidbody then set the velocity

public class GunFire : MonoBehaviour
{

    // attributes
    // Assign a Rigidbody component in the inspector to instantiate

    public Rigidbody bullet1;
    public Rigidbody bullet2;
    public float power;
    private int bullet;
    List<int> rando = new List<int>();



    // methods

        private void Start()
    {
            rando.Clear();
            for (int i = 0; i < 2; i++)
            {
                rando.Add(Random.Range(0, 2));
            }

        }
        void Update()
        {
            
            bullet = rando[0];

            // Ctrl was pressed, launch a projectile
            if (Input.GetButtonDown("Fire1"))
            {
            // chose what bullet to shoot 
                
                if (bullet == 0)
                {
                    // Instantiate the projectile at the position and rotation of this transform
                    Rigidbody clone;
                    clone = Instantiate(bullet1, transform.position, transform.rotation);

                    // Give the cloned object an initial velocity along the current
                    // object's Z axis
                    clone.velocity = transform.TransformDirection(Vector3.forward * power);
                    rando.RemoveAt(0);
                    rando.Add(Random.Range(0, 2));

                }
                else if (bullet == 1)
                {
                    // Instantiate the projectile at the position and rotation of this transform
                    Rigidbody clone;
                    clone = Instantiate(bullet2, transform.position, transform.rotation);

                    // Give the cloned object an initial velocity along the current
                    // object's Z axis
                    clone.velocity = transform.TransformDirection(Vector3.forward * power);
                    rando.RemoveAt(0);
                    rando.Add(Random.Range(0, 2));
                }



            }
        }
    
}
