using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleting : MonoBehaviour
{
    
    //deleting the clone when he hit an other colider
    void OnCollisionEnter(Collision other)
        {
            Destroy(this.gameObject);
        }


}
