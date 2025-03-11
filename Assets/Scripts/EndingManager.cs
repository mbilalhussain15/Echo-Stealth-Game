using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    private int dataFragmentsCollected = 0;
    private int totalDataFragments = 3; 

    public void CollectDataFragment()
    {
        dataFragmentsCollected++;

        if (dataFragmentsCollected >= totalDataFragments)
        {
            UnlockSecretEnding();
        }
    }

    void UnlockSecretEnding()
    {
        SceneManager.LoadScene("SecretEnding");
    }

    public void NormalExit()
    {
        SceneManager.LoadScene("NormalEnding");
    }
}
