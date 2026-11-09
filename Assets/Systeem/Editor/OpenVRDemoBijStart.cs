using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Opent de scene VR-Demo als Unity start met een lege, nieuwe scene
/// (zoals bij de eerste keer openen na het downloaden van de ZIP).
/// Heb je zelf al een scene open, dan doet dit script niets.
/// </summary>
[InitializeOnLoad]
static class OpenVRDemoBijStart
{
    const string ScenePad = "Assets/VR-Basics/Scenes/VR-Demo.unity";
    const string AlGedaan = "VRBasics_OpenVRDemoBijStart";

    static OpenVRDemoBijStart()
    {
        // Alleen bij het opstarten van Unity, niet na elke keer compileren
        if (Application.isBatchMode || SessionState.GetBool(AlGedaan, false)) return;
        SessionState.SetBool(AlGedaan, true);
        EditorApplication.delayCall += OpenAlsLeeg;
    }

    static void OpenAlsLeeg()
    {
        var actief = EditorSceneManager.GetActiveScene();
        bool leegEnNieuw = string.IsNullOrEmpty(actief.path) && !actief.isDirty && EditorSceneManager.sceneCount == 1;
        if (!leegEnNieuw) return;

        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePad) == null)
        {
            Debug.LogWarning("VR-Basics: scene niet gevonden op " + ScenePad);
            return;
        }
        EditorSceneManager.OpenScene(ScenePad, OpenSceneMode.Single);
    }
}
