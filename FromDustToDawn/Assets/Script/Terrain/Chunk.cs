using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private GenerationOptions options;
    private float previousHeight = 0;
    public void GenerateTerrain(GenerationOptions options)
    {
        this.options = options;

        Vector3[,] heightMap = new Vector3[options.chunkResolution, options.chunkResolution];

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Mesh mesh = new Mesh();

        //Define height map
        for (int i = 0; i < options.chunkResolution; i++)
        {
            for (int j = 0; j < options.chunkResolution; j++)
            {
                float step = options.chunkSize / (options.chunkResolution - 1);
                Vector3 vertex = new Vector3(i * step, GetHeight(i, j), j * step);
                heightMap[i, j] = vertex;
            }
        }


        //Define triangles and add vertices to the list
        for (int i = 0; i < options.chunkResolution - 1; i++)
        {
            for (int j = 0; j < options.chunkResolution - 1; j++)
            {
                Vector3 v0 = heightMap[i, j];
                Vector3 v1 = heightMap[i, j + 1];
                Vector3 v2 = heightMap[i + 1, j + 1];
                Vector3 v3 = heightMap[i + 1, j];

                int currentIndex = vertices.Count;

                vertices.Add(v0);
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);

                triangles.Add(currentIndex);
                triangles.Add(currentIndex + 1);
                triangles.Add(currentIndex + 2);

                triangles.Add(currentIndex);
                triangles.Add(currentIndex + 2);
                triangles.Add(currentIndex + 3);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshCollider>().sharedMesh = mesh;


        GetComponent<MeshFilter>().mesh = mesh;
    }


    //Define height with Perlin noise
    float GetHeight(int x, int z)
    {
        float height = 0;

        float amplitude = 1;
        float frequence = options.perlinScale;

        float step = options.chunkSize / (options.chunkResolution - 1);

        Vector3 worldPosition = new Vector3(x * step + transform.position.x + options.seed, 0, z * step + transform.position.z + options.seed);

        for (int i = 0; i < options.octaveCount; i++)
        {
            height += Mathf.PerlinNoise(worldPosition.x * frequence, worldPosition.z * frequence) * amplitude;

            frequence /= options.lacunarity;
            amplitude *= options.persistance;
        }
        return height * options.heightMultiplier;
    }

}
