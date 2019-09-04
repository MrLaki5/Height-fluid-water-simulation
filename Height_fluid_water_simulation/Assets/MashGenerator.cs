using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashGenerator : MonoBehaviour
{

    Mesh waterMash; 
    Vector3[] vertices;
    public int mashSize = 10;
    public int[] triangles;
    public float triangle_size = 1f;

    // Start is called before the first frame update
    void Start()
    {
        int vertex_number = (mashSize + 1) * (mashSize + 1);
        vertices = new Vector3[vertex_number];

        float vertex_d1 = 0;
        float vertex_d2 = 0;
        float vertex_d3 = 0;

        for (int i = 0; i < (mashSize + 1); i++) 
        { 
            for ( int j = 0; j < (mashSize + 1); j++)
            {
                vertices[i * (mashSize + 1) + j] = new Vector3(vertex_d2, vertex_d3, vertex_d1);
                vertex_d2 += triangle_size;
            }
            vertex_d1 += triangle_size;
            vertex_d2 = 0;
        }

        triangles = new int[mashSize * mashSize * 6];

        int triangles_num = 0;
        for (int i = 0; i < mashSize; i++)
        {
            for (int j = 0; j < mashSize; j++)
            {
                triangles[triangles_num] = i * (mashSize + 1) + j; //down left
                triangles_num++;
                triangles[triangles_num] = (i + 1) * (mashSize + 1) + j; //up left
                triangles_num++;
                triangles[triangles_num] = i * (mashSize + 1) + (j + 1); //down right
                triangles_num++;

                triangles[triangles_num] = i * (mashSize + 1) + (j + 1); //down right
                triangles_num++;
                triangles[triangles_num] = (i + 1) * (mashSize + 1) + j; //up left
                triangles_num++;
                triangles[triangles_num] = (i + 1) * (mashSize + 1) + (j + 1); //up right
                triangles_num++;
            }
        }
        print(triangles_num);
        for (int i= 0; i < triangles_num; i++)
        {
            print(triangles[i]);
            print(vertices[triangles[i]].ToString());
        }



        waterMash = new Mesh();

        waterMash.Clear();
        waterMash.vertices = vertices;
        waterMash.triangles = triangles;
        waterMash.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = waterMash;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5.0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
    }
}
