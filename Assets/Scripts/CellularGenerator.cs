using UnityEngine;

public class CellularGenerator : Singleton<CellularGenerator> {

    #region Private Variables
    private int width;
    private int height;
    private int randomFillPercent;
    private int smoothPasses;
    private int lowerLimit;
    private int upperLimit;
    private string seed;
    private int[,] map;

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

    #endregion

    #region Initialization
    public void Init(int width, int height, int fill)
    {
        this.width = width;
        this.height = height;
        GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        print("Generating");
    }
    #endregion

    #region Public Methods
    public void RandomFill(bool useRandomSeed = false, string _seed = "")
    {
        print("Filling");
        if (!useRandomSeed)
        {
            seed = _seed;
        }
        else
        {
            seed = System.DateTime.Now.Millisecond.ToString();
        }

        print(seed.GetHashCode());
        Random.InitState(seed.GetHashCode());

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                map[x, y] = (Random.Range(0.0f, 100.0f) < randomFillPercent) ? 1 : 0;
                Edges(x, y);
            }
        }
    }

    public void Smooth(int numPasses, int lLimit, int uLimit)
    {
        smoothPasses = numPasses;
        lowerLimit = lLimit;
        upperLimit = uLimit;
        for(int i = 0; i<smoothPasses; i++)
        {
            print("Smooth Pass # " + (i + 1).ToString());
            SmoothPass();
        }
    }

    #endregion

    #region Utilities

    private void SmoothPass()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int walls = GetSourroundingWalls(x, y);
                if(walls > upperLimit)
                {
                    map[x, y] = 1;
                }
                else if( walls < lowerLimit)
                {
                    map[x, y] = 0;
                }
                Edges(x, y);
            }
        }
    }

    private int GetSourroundingWalls(int gridX, int gridY)
    {
        int wallCount = 0;

        for(int nX = gridX-1; nX <= gridX + 1; nX++)
        {
            for (int nY = gridY - 1; nY <= gridY + 1; nY++)
            {
                if( IsWithinBounds(nX,nY) )
                {
                    if (!(nX == gridX && nY == gridY))
                    {
                        wallCount += map[nX, nY];
                    }
                }                
            }
        }

        return wallCount;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < width && y < height);
    }

    private void Edges(int x, int y)
    {
        if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
        {
            map[x, y] = 1;
        }
    }

    #endregion

    #region Rendering
    private void OnDrawGizmos()
    {
        if(map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

#endregion
}
