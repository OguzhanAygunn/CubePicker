using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [Title("Wave Anim")]
    [SerializeField] bool activeWaveAnim = true;
    [SerializeField] MeshRenderer waterMesh;
    [SerializeField] Vector2 waveAnimSpeed;
    private Material material;

    private void Awake()
    {
        material = waterMesh.material;
    }

    private void Update()
    {
        material.mainTextureOffset += waveAnimSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            //Block
            Block block = other.GetComponent<Block>();

            if (block.State is not BlockState.Free)
                return;

            block.gameObject.SetActive(false);

            //FX Spawn
            Vector3 fxPos = block.transform.position;
            block.transform.position += Vector3.up;
            FXManager.PlayFX(id: "Block Water", pos: fxPos, desTime: 3).Forget();
        }

        if (other.CompareTag("Player"))
        {
            Transform playerTrs = PlayerController.instance.transform;
            Vector3 effectSpawnPos = playerTrs.position;

            FXManager.PlayFX("Player Coll Water", effectSpawnPos, 3f).Forget();
            GameManager.instance.SetCompleteLevel(active: false, delay: 1.25f).Forget();
        }
    }
}
