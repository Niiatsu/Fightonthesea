using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Material waterMaterial;
    Vector2 uvOffset = new Vector2();
    Vector2 uvOffset2 = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        waterMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        uvOffset.x += 0.0005f * Mathf.Sin(Time.time) ;
        uvOffset.y += 0.0005f;
        uvOffset2.x += 0.0002f * Mathf.Sin(Time.time);
        uvOffset2.y -= 0.0002f * Mathf.Sin(Time.time);

        waterMaterial.SetTextureOffset("_MainTex", uvOffset);
        waterMaterial.SetTextureOffset("_DetailAlbedoMap", uvOffset2);
    }
}
