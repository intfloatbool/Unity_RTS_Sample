using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UAT_Generated
{
	static class SceneSwitcherEditor
	{
		[MenuItem("UAT_SceneSwitcher/Preloader")]
		private static void Load_Bootstrap()
		{
			OpenScene("Assets/Scenes/Preloader.unity");
		}

		[MenuItem("UAT_SceneSwitcher/BattleScene")]
		private static void Load_GameLoading()
		{
			OpenScene("Assets/Scenes/BattleScene.unity");
		}
		
		private static void OpenScene(string path)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene(path);			
		}
	}
}