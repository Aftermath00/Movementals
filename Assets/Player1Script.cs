using UnityEngine;
using Unity.Netcode;

public class Player1Script : NetworkBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

   private void Update()
    {
        if (IsOwner)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireBulletServerRpc();
            }

            //kebutuhan test

        }
    }

    [ServerRpc]
    private void FireBulletServerRpc()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.right * bulletSpeed;
        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        if (bulletNetworkObject != null)
        {
            bulletNetworkObject.Spawn();
        }
    }
}
