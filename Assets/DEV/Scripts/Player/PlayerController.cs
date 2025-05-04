using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform BlockParent { get { return blockParent; } }

    public static PlayerController instance;
    public static PlayerController Instance;

    public PlayerMovement Movement {  get { return instance.movement; } }
    public PlayerRandomPositioner Positioner { get { return instance.positioner; } }
    public PlayerBlockRadarV2 RadarV2 {  get { return instance.radarV2; } }
    public PlayerCollectibleRadar RadarV1 { get { return instance.collectibleRadar; } }
    public PlayerBlocker Blocker { get { return instance.blocker; } }
    public PlayerBlockRadar BlockRadar { get { return instance.blockRadar; } }
    public PlayerShootRadar ShootRadar { get { return instance.shootRadar; } }

    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerRandomPositioner positioner;
    [SerializeField] PlayerCollectibleRadar collectibleRadar;
    [SerializeField] PlayerBlockRadarV2 radarV2;
    [SerializeField] PlayerBlockRadar blockRadar;
    [SerializeField] PlayerBlocker blocker;
    [SerializeField] PlayerShootRadar shootRadar;

    [SerializeField] Transform blockParent;

    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }
}
