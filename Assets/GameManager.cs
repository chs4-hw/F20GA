using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    bool playerDeath = false;
    public float restartTimer = 3f;

    public void EndGame()
    {
        if (playerDeath == false)
        {
            playerDeath = true;
            Debug.Log("Game Over");
            Invoke("GameRestart", restartTimer);
            //GameRestart();
        }

    }

    void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
