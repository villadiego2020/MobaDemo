using UnityEditor;
using UnityEngine;

public class Headless : MonoBehaviour
{
    public static void BeforeBuild()
    {
        EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Server;
    }
}
