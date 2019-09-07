using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 

        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer renderer = hit.collider.GetComponent<Renderer>();
        Texture mainTexture = renderer.sharedMaterial.GetTexture("_Tex");
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (renderer == null || renderer.sharedMaterial == null || mainTexture == null || meshCollider == null)
            return;

        Vector2 pixelUV = hit.textureCoord;
        print((int)(pixelUV.x * mainTexture.width) + "--" + (int)(pixelUV.y * mainTexture.height));
    }
}
