using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEditorDebugHelpers
{
    public class UsefulLinks : MonoBehaviour
    {
        private static readonly string hook_name = "[HOOK_FOLDER].prefab";
        
        [MenuItem("UsefulLinks/Prefabs folder")]
        private static void OpenPrefabs()
        {
            var path = "Assets/Prefabs/" + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Preloader folder")]
        private static void OpenPreloader()
        {
            var path = "Assets/Prefabs/Preloader/"  + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Characters Ragdolls folder")]
        private static void OpenCharacters()
        {
            var path = "Assets/Prefabs/Ragdolls/" + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Scenes folder")]
        private static void OpenScenes()
        {
            var path = "Assets/Scenes/"  + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Scripts folder")]
        private static void OpenScripts()
        {
            var path = "Assets/Scripts/"  + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Characters skins folder")]
        private static void OpenSkins()
        {
            var path = "Assets/Sprites/Characters/"  + hook_name;
            TryPingObjectbyPath(path);
        }

        [MenuItem("UsefulLinks/Settings folder")]
        private static void OpenSettingsFolder()
        {
            var path ="Assets/Settings/" + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Items data folder")]
        private static void OpenItemsDataFolder()
        {
            var path ="Assets/Settings/Data/Items/" + hook_name;
            TryPingObjectbyPath(path);
        }

        private static void TryPingObjectbyPath(string path)
        {
            Object assetInPath = GetAssetByPath(path);
            int id = -1;
            if (assetInPath != null)
            {
                id = assetInPath.GetInstanceID();
            }
            if (id <= -1)
            {
                Debug.LogError($"There is no file at path {path}!");
                return;
            }
            EditorGUIUtility.PingObject(id);
        }

        private static Object GetAssetByPath(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Object>(path);
        }
    } 
}

