using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] KeyCode spawnKey;
	[SerializeField] private GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(spawnKey))
        {
            if (Prefab != null) Instantiate(Prefab, this.transform);
        }
	}
}
