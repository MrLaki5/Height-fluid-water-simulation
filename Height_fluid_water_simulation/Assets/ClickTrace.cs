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

    // Check collision for boxes
    void OnCollisionStay(Collision collisionInfo)
    {
        float low_x = -1.0f;
        float high_x = -1.0f;
        float low_y = -1.0f;
        float high_y = -1.0f;
        print("in function");
        foreach (ContactPoint cp in collisionInfo.contacts)
        {
            RaycastHit hit;
            float rayLength = 0.1f;
            Ray ray = new Ray(cp.point + cp.normal * rayLength * 0.5f, -cp.normal);
            Color C = Color.white; // default color when the raycast fails for some reason ;)
            if (cp.thisCollider.Raycast(ray, out hit, rayLength))
            {
                Vector2 curr_hit = hit.textureCoord;
                if ((low_x < 0 || low_y < 0) || (low_x > curr_hit.x && low_y > curr_hit.y))
                {
                    low_x = curr_hit.x;
                    low_y = curr_hit.y;
                }
                if ((high_x < 0 || high_y < 0) || (high_x < curr_hit.x && high_y < curr_hit.y))
                {
                    high_x = curr_hit.x;
                    high_y = curr_hit.y;
                }
            }
        }
        //Collider other_collider = collisionInfo.gameObject.GetComponent<Collider>().;

        print("Low[x: " + low_x + ", y: " + low_y + "], high[x: " + high_x + ", y: " + high_y + "]");
    }
}
