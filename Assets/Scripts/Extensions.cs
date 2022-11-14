
using UnityEngine.SceneManagement;

public static class Extensions
{
    
    public static void Load(this Scene s)
    {
        SceneManager.LoadScene(s.buildIndex);
    }
}