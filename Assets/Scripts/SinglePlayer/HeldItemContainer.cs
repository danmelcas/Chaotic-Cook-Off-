using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeldItemContainer : MonoBehaviour
{
    [SerializeField] private float _distance; 
    void Update()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * _distance;
        transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);
    }
}
