using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrace : MonoBehaviour
{

    public CustomRenderTexture my_texture;

    // Start is called before the first frame update
    void Start()
    {
        if (!my_texture)
        {
            print("Error: ClickTrace: no custom texture assigned!");
            return;
        }
        my_texture.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!my_texture)
        {
            print("Error: ClickTrace: no custom texture assigned!");
            return;
        }

        float clickX = -1.0f;
        float clickY = -1.0f;

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                Texture mainTexture = renderer.sharedMaterial.GetTexture("_Tex");
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (!(renderer == null || renderer.sharedMaterial == null || mainTexture == null || meshCollider == null))
                {
                    Vector2 pixelUV = hit.textureCoord;
                    print((int)(pixelUV.x * mainTexture.width) + "--" + (int)(pixelUV.y * mainTexture.height));
                    clickX = pixelUV.x;
                    clickY = pixelUV.y;
                }
            }
        }
        my_texture.material.SetFloat("_Click_x", clickX);
        my_texture.material.SetFloat("_Click_y", clickY);
        my_texture.Update();
    }
}
