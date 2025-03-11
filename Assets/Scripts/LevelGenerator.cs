using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject securityNodePrefab;
    public GameObject securityBotPrefab;
    public int nodeCount = 3;
    public int baseBotCount = 3; 
    public int botCount;

    void Start()
    {
        botCount = Random.Range(2, 6);
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < nodeCount; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
            Instantiate(securityNodePrefab, randomPos, Quaternion.identity);
        }

        for (int i = 0; i < botCount; i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
            Instantiate(securityBotPrefab, randomPos, Quaternion.identity);
        }
    }
}
