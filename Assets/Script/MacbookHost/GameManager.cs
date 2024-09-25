using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform monsterSpawnPoint;

    [SerializeField] private GameObject fireCharacterPrefab;
    [SerializeField] private Transform fireCharacterSpawnPoint;

    [SerializeField] private GameObject earthCharacterPrefab;
    [SerializeField] private Transform earthCharacterSpawnPoint;

    [SerializeField] private GameObject waterCharacterPrefab;
    [SerializeField] private Transform waterCharacterSpawnPoint;

    [SerializeField] private GameObject windCharacterPrefab;
    [SerializeField] private Transform windCharacterSpawnPoint;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId, int characterIndex)
    {
        Debug.Log($"SpawnPlayerServerRpc called for client {clientId} with character index {characterIndex}");
        switch (characterIndex)
        {
            case 0:
                SpawnFireCharacter(clientId);
                break;
            case 1:
                SpawnEarthCharacter(clientId);
                break;
            case 2:
                SpawnWaterCharacter(clientId);
                break;
            case 3:
                SpawnWindCharacter(clientId);
                break;
            default:
                Debug.LogError($"Invalid character index: {characterIndex}");
                break;
        }
    }


    private void SpawnFireCharacter(ulong clientId)
    {
        SpawnCharacter(clientId, fireCharacterPrefab, fireCharacterSpawnPoint, "Fire");
    }

    private void SpawnEarthCharacter(ulong clientId)
    {
        SpawnCharacter(clientId, earthCharacterPrefab, earthCharacterSpawnPoint, "Earth");
    }

    private void SpawnWaterCharacter(ulong clientId)
    {
        SpawnCharacter(clientId, waterCharacterPrefab, waterCharacterSpawnPoint, "Water");
    }

    private void SpawnWindCharacter(ulong clientId)
    {
        SpawnCharacter(clientId, windCharacterPrefab, windCharacterSpawnPoint, "Wind");
    }

    private void SpawnCharacter(ulong clientId, GameObject prefab, Transform spawnPoint, string characterType)
    {
        Debug.Log($"Attempting to spawn {characterType} character for client {clientId}");
        if (prefab != null && spawnPoint != null)
        {
            GameObject playerInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            NetworkObject playerNetObj = playerInstance.GetComponent<NetworkObject>();
            
            if (playerNetObj != null)
            {
                playerNetObj.SpawnAsPlayerObject(clientId);
                playerNetObj.ChangeOwnership(clientId);
                Debug.Log($"Successfully spawned {characterType} character for client {clientId}");
            }
            else
            {
                Debug.LogError($"NetworkObject missing on {characterType} character prefab");
            }
        }
        else
        {
            Debug.LogError($"{characterType} character prefab or spawn point is null. Prefab: {prefab}, SpawnPoint: {spawnPoint}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMonsterServerRpc()
    {
        if (monsterPrefab != null && monsterSpawnPoint != null)
        {
            GameObject monsterInstance = Instantiate(monsterPrefab, monsterSpawnPoint.position, monsterSpawnPoint.rotation);
            NetworkObject monsterNetObj = monsterInstance.GetComponent<NetworkObject>();
            
            if (monsterNetObj != null)
            {
                monsterNetObj.Spawn();
                Debug.Log("Monster spawned");
            }
            else
            {
                Debug.LogError("NetworkObject missing on monster prefab");
            }
        }
        else
        {
            Debug.LogError("Monster prefab or spawn point is null");
        }
    }

    public void HandleTriggeredAttack(string element, ulong clientId)
    {
        Debug.Log($"Handling triggered attack for element: {element} from client {clientId}");
        // Implement your game logic to handle the triggered attack
        // For example, apply damage to the monster or update game state
    }

}
