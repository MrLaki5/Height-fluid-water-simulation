using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrace : MonoBehaviour
{

    public CustomRenderTexture my_texture;
    float low_x = -1.0f;
    float high_x = -1.0f;
    float low_y = -1.0f;
    float high_y = -1.0f;

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

        my_texture.material.SetFloat("_Box_low_x", low_x);
        my_texture.material.SetFloat("_Box_low_y", low_y);
        my_texture.material.SetFloat("_Box_high_x", high_x);
        my_texture.material.SetFloat("_Box_high_y", high_y);
        my_texture.Update();
    }

    // Check collision for boxes
    void OnCollisionStay(Collision collisionInfo)
    {
        low_x = -1.0f;
        high_x = -1.0f;
        low_y = -1.0f;
        high_y = -1.0f;
        foreach (ContactPoint cp in collisionInfo.contacts)
        {

            LayerMask mask = 0;
            for (int i = 0; i < 8; i++)
            {
                mask <<= 1;
                mask |= 1;
            }

            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cp.point)), out hit, 100f, mask))
                continue;

            Renderer renderer = hit.collider.GetComponent<Renderer>();
            Texture mainTexture = renderer.sharedMaterial.GetTexture("_Tex");
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (renderer == null || renderer.sharedMaterial == null || mainTexture == null || meshCollider == null)
                continue;


            //RaycastHit hit;
            //Ray ray = new Ray(cp.point - cp.normal, cp.normal);
            //if (Physics.Raycast(ray, out hit))
            //{ 
            Vector2 curr_hit = hit.textureCoord;
            print("in function: " + curr_hit.x + " -- " + curr_hit.y);
            if ((low_x < 0 || low_y < 0) || (low_x > curr_hit.x && low_y > curr_hit.y))
            {
                low_x = curr_hit.x;
                low_y = curr_hit.y;
            }
            if ((high_x < 0 || high_y < 0) || (high_x < curr_hit.x && high_y < curr_hit.y))
            {
                high_x = curr_hit.x;
                high_y = curr_hit.y;
             //   }
            }


        }
        print("Low[x: " + low_x + ", y: " + low_y + "], high[x: " + high_x + ", y: " + high_y + "]");
    }

    void OnCollisionExit(Collision collisionInfo) 
    {
        low_x = -1.0f;
        high_x = -1.0f;
        low_y = -1.0f;
        high_y = -1.0f;
    }
}
