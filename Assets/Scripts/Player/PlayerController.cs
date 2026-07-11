using UnityEngine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Player
{
    public class PlayerController : MonoBehaviour, IPersistable
    {
        [HideInInspector] public Vector3 respawnPos;

        void OnEnable()
        {
            PlayerEvents.OnPlayerDeath += Die;
            PlayerEvents.OnCheckPointReached += SetRespawnPos;
        }

        void OnDisable()
        {
            PlayerEvents.OnPlayerDeath -= Die;
            PlayerEvents.OnCheckPointReached -= SetRespawnPos;
        }

        private void Die()
        {
            Respawn();
        }

        public void Respawn()
        {
            transform.position = respawnPos;
        }

        private void SetRespawnPos(Vector3 pos) => respawnPos = pos;

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(jsonData);
            
            transform.position = saveData.playerPosition;
            respawnPos = saveData.playerRespawnPosition;
        }

        public string SaveData()
        {
            PlayerSaveData data = new()
            {
                playerPosition = transform.position,
                playerRespawnPosition = respawnPos,
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}