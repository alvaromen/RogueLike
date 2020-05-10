using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyExplosionPrefab());
    }

    private IEnumerator DestroyExplosionPrefab(){
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
