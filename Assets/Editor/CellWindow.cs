//C# Example
using UnityEditor;
using UnityEngine;

public class CellWindow : EditorWindow
{
    #region Variables
    bool mapSettings = false;
    bool rngSettings = false;
    bool smoothSettings = false;
    string mapName = "Cellular Map 1";
    string rngSeed = "ABC";
    bool useRandomSeed = false;
    int myWidth = 100;
    int myHeight = 100;
    int myFillPercent = 100;
    int mySmoothPasses = 5;
    int myLowerLimit = 4;
    int myUpperLimit = 5;
    string generateButton = "Generate Map";
    string fillButton = "Fill Map";
    string smoothButton = "Smooth Map";
    CellularGenerator gen;


#endregion

    // Add menu item NIER to the Tools menu
    [MenuItem("Tools/Cellular Automata Terrain Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CellWindow));
        
    }

    private void OnEnable()
    {
        titleContent = new GUIContent("CATT", "Cellular Automata Terrain Tool");
    }

    void OnGUI()
    {
        // Map settings foldout
        mapSettings = EditorGUILayout.Foldout(mapSettings,"Map Settings");
        if (mapSettings)
        {
            mapName = EditorGUILayout.TextField("Map Name", mapName);
            myWidth = EditorGUILayout.IntSlider("Map Width", myWidth, 1, 100);
            myHeight = EditorGUILayout.IntSlider("Map Height", myHeight, 1, 100);
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
            myLowerLimit = EditorGUILayout.IntSlider("Lower Limit", myLowerLimit, 1, 5);
            myUpperLimit = EditorGUILayout.IntSlider("Upper Limit", myUpperLimit, 3, 7);
        }

        if (GUILayout.Button(generateButton))
        {
            //Generate Map
            
            gen.Init(myWidth, myHeight, myFillPercent);

        }
        if (gen)
        {
            if (GUILayout.Button(fillButton))
            {
                //Generate Map
                gen.RandomFillPercent = myFillPercent;
                gen.RandomFill(useRandomSeed, rngSeed);
                SceneView.RepaintAll();
            }

            if (GUILayout.Button(smoothButton))
            {
                // Smooth map
                gen.Smooth(mySmoothPasses, myLowerLimit, myUpperLimit);
                SceneView.RepaintAll();
            }
        }
    }
}