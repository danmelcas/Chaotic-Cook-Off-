using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrash : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Ingredient>() != null) {
            StartCoroutine(DeleteObject(other.gameObject));
        }
    }

    
    IEnumerator DeleteObject(GameObject obj) { 
        yield return new WaitForSeconds(2f);
        Destroy(obj); 
    }
}
