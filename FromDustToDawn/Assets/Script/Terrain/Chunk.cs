using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private GenerationOptions options;

    public void GenerateTerrain(GenerationOptions options)
    {
        this.options = options;

        Vector3[,] heightMap = new Vector3[options.gridSize, options.gridSize];

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Mesh mesh = new Mesh();

        //Define height map
        for (int i = 0; i < options.gridSize; i++)
        {
            for (int j = 0; j < options.gridSize; j++)
            {
                float step = options.meshSize / (options.gridSize - 1);
                Vector3 vertex = new Vector3(i * step, GetHeight(i, j), j * step);
                heightMap[i, j] = vertex;
            }
        }


        //Define triangles and add vertices to the list
        for (int i = 0; i < options.gridSize - 1; i++)
        {
            for (int j = 0; j < options.gridSize - 1; j++)
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


        GetComponent<MeshFilter>().mesh = mesh;
    }


    //Define height with Perlin noise
    float GetHeight(int x, int z)
    {
        float height = 0;

        float amplitude = 1;
        float frequence = options.perlinScale;

        float step = options.meshSize / (options.gridSize - 1);

        Vector3 worldPosition = new Vector3(x * step + transform.position.x + options.seed, 0, z * step + transform.position.z + options.seed);

        for (int i = 0; i < options.octaveCount; i++)
        {
            height += Mathf.PerlinNoise(worldPosition.x * frequence, worldPosition.z * frequence) * amplitude;

            frequence /= options.lacunarity;
            amplitude *= options.persistance;
        }

        //Vector2 centerPoint = new Vector2((options.meshSize * options.chunkWidth) / 2, (options.meshSize * options.chunkWidth) / 2);
        //Debug.Log(options.meshSize);
        //Debug.Log(options.chunkWidth);

        //float absX = Mathf.Abs(centerPoint.x + transform.position.x);
        //float absZ = Mathf.Abs(centerPoint.y + z);
        
        //Debug.Log(absX);
        //if (absX > centerPoint.x + 10)
        //{
        //    return height * options.heightMultiplier ;
        //}

        return height * options.heightMultiplier;
    }

}
