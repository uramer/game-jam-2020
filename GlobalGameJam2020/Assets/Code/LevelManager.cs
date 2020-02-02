using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    [SerializeField] public int currentLevel = 0;
    [SerializeField] public string[] levels;
    public void Win() {
        Debug.Log("WIN");
        currentLevel++;
        if(currentLevel < levels.Length) {
            //Addressables.LoadAssetAsync<Scene>(levels[currentLevel]).Completed += OnLoadDone;
            Addressables.LoadScene(levels[currentLevel], LoadSceneMode.Single);
        }
    }

    public void Lose() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
