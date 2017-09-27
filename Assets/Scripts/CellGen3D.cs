using UnityEngine;
using MarchingCubesProject;
using System.Collections.Generic;

public class CellGen3D : Singleton<CellGen3D>
{

    #region Private Variables
    private int width;
    private int height;
    private int depth;
    private int randomFillPercent;
    private int smoothPasses;
    private int lowerLimit;
    private int upperLimit;
    private string seed;
    private int[,,] map;
    private bool createdMeshes;
    private GameObject group;
    #endregion

    #region Public accessors
    public int RandomFillPercent
    {
        get
        {
            return randomFillPercent;
        }

        set
        {
            randomFillPercent = value;
        }
    }

    public int[,,] Map
    {
        get
        {
            return map;
        }

        set
        {
            map = value;
        }
    }

    public GameObject Group
    {
        get
        {
            return group;
        }

        set
        {
            group = value;
        }
    }

    #endregion

    #region Initialization
    public void Init(int width, int height, int depth)
    {
        // Set the basic map parameters
        this.width = width;
        this.height = height;
        this.depth = depth;
        DestroyImmediate(group);
        createdMeshes = false;
        map = new int[width, height, depth];
        print("Generating");
    }    
    #endregion

    #region Public Methods
    public void RandomFill(bool useRandomSeed = false, string _seed = "")
    {
        // Destroy our previous mesh and show the Gizmos again
        DestroyImmediate(group);
        createdMeshes = false;

        // Select our seed for the Random Number Generator
        if (!useRandomSeed)
        {
            seed = _seed;
        }
        else
        {
            seed = System.DateTime.Now.Millisecond.ToString();
        }        
        Random.InitState(seed.GetHashCode());

        // Fill our map array using out parameters
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for(int z = 0; z < depth; z++)
                {
                    map[x, y, z] = (Random.Range(0.0f, 100.0f) < randomFillPercent) ? 1 : 0;
                    Edges(x, y, z);
                }
            }
        }
    }

    public void Smooth(int numPasses, int lLimit, int uLimit)
    {
        // Sets the parameters for our smoothing fucntions and loops thru it
        smoothPasses = numPasses;
        lowerLimit = lLimit;
        upperLimit = uLimit;
        for (int i = 0; i < smoothPasses; i++)
        {
            //print("Smooth Pass # " + (i + 1).ToString());
            SmoothPass();
        }
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Single iteration of the Smoothing function
    /// </summary>
    private void SmoothPass()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int walls = GetSourroundingWalls(x, y, z);
                    if (walls > upperLimit)
                    {
                        map[x, y, z] = 1;
                    }
                    else if (walls < lowerLimit)
                    {
                        map[x, y, z] = 0;
                    }
                    Edges(x, y, z);
                }
            }
        }
    }


    /// <summary>
    /// Get the number of surrounding walls for the selected cell
    /// </summary>
    /// <param name="gridX">X coordinate of the selected cell</param>
    /// <param name="gridY">Y coordinate of the selected cell</param>
    /// <param name="gridZ">Z coordinate of the selected cell</param>
    /// <returns></returns>
    private int GetSourroundingWalls(int gridX, int gridY, int gridZ)
    {
        int wallCount = 0;

        for (int nX = gridX - 1; nX <= gridX + 1; nX++)
        {
            for (int nY = gridY - 1; nY <= gridY + 1; nY++)
            {
                for (int nZ = gridZ - 1; nZ <= gridZ+1; nZ++)
                {
                    if (IsWithinBounds(nX, nY, nZ))
                    {
                        if (!(nX == gridX && nY == gridY & nZ == gridZ))
                        {
                            wallCount += map[nX, nY, nZ];
                        }
                    }
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// Checks if the selected cell is within bounds
    /// </summary>
    /// <param name="x">X coordinate of the selected cell</param>
    /// <param name="y">Y coordinate of the selected cell</param>
    /// <param name="z">Z coordinate of the selected cell</param>
    /// <returns></returns>
    private bool IsWithinBounds(int x, int y, int z)
    {
        return (x >= 0 && y >= 0 && x < width && y < height && z >= 0 && z < depth);
    }

    /// <summary>
    /// Makes the edges of the world
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void Edges(int x, int y, int z)
    {
        if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth -1)
        {
            map[x, y, z] = 1;
        }
    }

    #endregion

    #region Rendering
    private void OnDrawGizmos()
    {
        if (createdMeshes)
        {
            return;
        }
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Gizmos.color = (map[x, y, z] == 1) ? new Color(0.0f,0.0f,0.0f,0.05f) : new Color(1.0f, 1.0f, 1.0f, 0.2f);                        
                        Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, -depth/2 + z + 0.5f);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Create voxel meshes
    /// </summary>
    public void CreateMeshes()
    {
        if (map != null)
        {
            group = new GameObject("Group");
            group.transform.parent = this.transform;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if(map[x, y, z] == 1)
                        {
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.position = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, -depth / 2 + z + 0.5f); 
                            cube.transform.parent = group.transform;
                        }                        
                    }
                }
            }
            createdMeshes = true;
        }
    }

    /// <summary>
    /// Create mesh using marching cubes
    /// </summary>
    public void CreateMeshesMarching()
    {
        if (map != null)
        {
            group = new GameObject("Group");
            group.transform.parent = this.transform;
            //Marching march = new MarchingTertrahedron();
            Marching march = new MarchingCubes();
            march.Surface = 0.9f;



            float[] voxels = new float[width * height * depth];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        int idx = x + y * width + z * width * height;
                        voxels[idx] = map[x, y, z];
                    }
                }
            }


            List<Vector3> verts = new List<Vector3>();
            List<int> indices = new List<int>();


            //The mesh produced is not optimal. There is one vert for each index.
            //Would need to weld vertices for better quality mesh.
            march.Generate(voxels, width, height, depth, verts, indices);

            //A mesh in unity can only be made up of 65000 verts.
            //Need to split the verts between multiple meshes.

            int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
            int numMeshes = verts.Count / maxVertsPerMesh + 1;

            for (int i = 0; i < numMeshes; i++)
            {

                List<Vector3> splitVerts = new List<Vector3>();
                List<int> splitIndices = new List<int>();

                for (int j = 0; j < maxVertsPerMesh; j++)
                {
                    int idx = i * maxVertsPerMesh + j;

                    if (idx < verts.Count)
                    {
                        splitVerts.Add(verts[idx]);
                        splitIndices.Add(j);
                    }
                }

                if (splitVerts.Count == 0) continue;

                Mesh mesh = new Mesh();
                mesh.SetVertices(splitVerts);
                mesh.SetTriangles(splitIndices, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
                

                GameObject go = new GameObject("Mesh");
                go.transform.parent = group.transform;
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.GetComponent<Renderer>().material = new Material(Shader.Find("Diffuse"));
                go.GetComponent<MeshFilter>().mesh = mesh;
                go.transform.localPosition = new Vector3(-width / 2, -height / 2, -depth / 2);
            }


            createdMeshes = true;
        }
    }

    #endregion
}
