using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private Vector3 rotation = Vector3.zero;
	[SerializeField] private KeyCode resetKey;
    
    void Update()
    {
		if (Input.GetKeyDown(resetKey))
        {
            this.gameObject.transform.position = position;
            this.gameObject.transform.rotation = Quaternion.Euler(rotation);
        }
	}
}
