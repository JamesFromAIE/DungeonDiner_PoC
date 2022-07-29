//Based on https://catlikecoding.com/unity/tutorials/procedural-grid/

using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public int xSize;
    public int ySize;

    public float power = 0.1f;
    public float scale = 0.2f;

    void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        Mesh mesh = new Mesh
        {
            name = "New Generated Terrain"
        };

        Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Color[] colors = new Color[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        Vector2 offset = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f));

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float height = Mathf.PerlinNoise(x * scale + offset.x, y * scale + offset.y) * power;
                height -= Mathf.Pow(Vector3.Distance(new Vector3(x, 0, 0), new Vector3(x, 0, y - ySize / 2.0f)), 2) * 0.0005f;

                if (x == 0 || x >= xSize * 0.75f || y == 0 || y >= ySize)
                    height = -0.1f;

                vertices[i] = new Vector3(x * (1.0f / xSize) - 0.5f, height, y * (1.0f / ySize) - 0.5f);
                colors[i] = Color.Lerp(new Color32(255, 0, 0, 255), new Color32(0, 0, 255, 255), vertices[i].y / power);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}

