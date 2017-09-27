//C# Example
using UnityEditor;
using UnityEngine;

public class NIERCellAutomata : EditorWindow
{
    #region Variables
    bool mapSettings = false;
    bool rngSettings = false;
    bool smoothSettings = false;
    string mapName = "Cellular Map 1";
    string rngSeed = "ABC";
    bool useRandomSeed = false;
    int myWidth = 20;
    int myHeight = 20;
    int myDepth = 20;
    int myFillPercent = 55;
    int mySmoothPasses = 5;
    int myLowerLimit = 13;
    int myUpperLimit = 17;
    string generateButton = "Generate Map";
    string fillButton = "Fill Map";
    string smoothButton = "Smooth Map";
    string meshesButton = "Create Meshes";
    string saveButton = "Save Mesh";


    #endregion

    // Add menu item NIER to the Tools menu
    [MenuItem("Tools/Cellular Automata Terrain Tool 3D")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(NIERCellAutomata));

    }

    private void OnEnable()
    {
        titleContent = new GUIContent("CATT", "Cellular Automata Terrain Tool");
    }

    void OnGUI()
    {
        // Map settings foldout
        mapSettings = EditorGUILayout.Foldout(mapSettings, "Map Settings");
        if (mapSettings)
        {
            mapName = EditorGUILayout.TextField("Map Name", mapName);
            myWidth = EditorGUILayout.IntSlider("Map Width", myWidth, 1, 100);
            myHeight = EditorGUILayout.IntSlider("Map Height", myHeight, 1, 100);
            myDepth = EditorGUILayout.IntSlider("Map Depth", myDepth, 1, 100);
            myFillPercent = EditorGUILayout.IntSlider("Fill Percentage", myFillPercent, 0, 100);
        }

        // Random Number Generator settings foldout
        rngSettings = EditorGUILayout.Foldout(rngSettings, "RNG Settings");
        if (rngSettings)
        {
            useRandomSeed = EditorGUILayout.Toggle("Use random seed?", useRandomSeed);
            if (!useRandomSeed)
            {
                rngSeed = EditorGUILayout.TextField("Random Seed", rngSeed);
            }
            else
            {
                rngSeed = "";
            }
        }


        // Smoothing settings foldout
        smoothSettings = EditorGUILayout.Foldout(smoothSettings, "Smooth Settings");
        if (smoothSettings)
        {
            mySmoothPasses = EditorGUILayout.IntSlider("Smoothing Passes", mySmoothPasses, 1, 10);
            myLowerLimit = EditorGUILayout.IntSlider("Lower Limit", myLowerLimit, 1, 25);
            myUpperLimit = EditorGUILayout.IntSlider("Upper Limit", myUpperLimit, 1, 25);
        }

        if (GUILayout.Button(generateButton))
        {
            //Generate Map

            CellGen3D.Instance.Init(myWidth, myHeight, myDepth);

        }
        if (CellGen3D.Instance.Map != null)
        {
            if (GUILayout.Button(fillButton))
            {
                //Generate Map
                CellGen3D.Instance.RandomFillPercent = myFillPercent;
                CellGen3D.Instance.RandomFill(useRandomSeed, rngSeed);
                SceneView.RepaintAll();
            }

            if (GUILayout.Button(smoothButton))
            {
                // Smooth map
                CellGen3D.Instance.Smooth(mySmoothPasses, myLowerLimit, myUpperLimit);
                SceneView.RepaintAll();
            }

            if (GUILayout.Button(meshesButton))
            {
                // Smooth map
                CellGen3D.Instance.CreateMeshesMarching();

                // gen.CreateMeshes();
                SceneView.RepaintAll();
            }

            if (GUILayout.Button(saveButton))
            {
                // Smooth map
                string localPath = "Assets/" + mapName + ".prefab";
                PrefabUtility.CreatePrefab(localPath, CellGen3D.Instance.Group);
                DestroyImmediate(CellGen3D.Instance.Group);
                // gen.CreateMeshes();
                SceneView.RepaintAll();
            }
        }
    }
}
