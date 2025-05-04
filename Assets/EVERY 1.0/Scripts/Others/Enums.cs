namespace EVERY
{

    public enum MoveType { Fix, Lerp, MoveToWards }
    public enum UpdateType { Late, Fixed, Update, Manuel }
    public enum TrackerType { Pos, Rotate, Scale }
    public enum AnimCurveType { Ease,Curve,CurveID}
    public enum SizeAnimType {ToTarget, Shake, ThreeAxis }
    public enum ShaderEffectType { MaterialColor, ShaderProperty, Higlight }
    public enum ShaderEffectValueType { Color,Float }
    public enum CamOffsetAnimType { Position, Rotate }
    public enum OffsetType { Self,Add}
    public enum ObjectType { Player,Enemy,Environment,Ground,Camera}
    public enum FXSpawnType { PlayFX_1, PlayFX_2, PlayFX_3, PlayFXWithTracker_1, PlayFXWithTracker_2 }
    public enum RadarType { Layer, ObjectType, Both }
    public enum GameState { Ready, Go, Pause, Finish }
    public enum TransformAxis { X,Y,Z}
    public enum BlockState { InPlayer,Free,Static,Anim,Shoot}
    public enum BlockSpawnAnimType { Fall,Scale,Siklon}
    public enum BlockType { Normal, Dangerous}
    public enum GamePhase { Collect,Fight}
    public enum CollectibleType { Null,Magnet }
    public enum CollectibleState { Ready, Hide, ToPlayer, Collected }
    public enum EnemyMovePhase { InWater, InAir, InGround, Kill }
}
