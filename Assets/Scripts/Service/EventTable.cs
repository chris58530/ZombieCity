
public class GameEvent
{
    public const string ON_INIT_GAME = "GameEvent.ON_INIT_GAME";
    public const string ON_QUIT_GAME = "GameEvent.ON_QUIT_GAME";
}
public class FloorEvent
{
    public const string ON_FLOOR_INIT = "FloorEvent.ON_FLOOR_INIT";
    public const string ON_FLOOR_UPDATE = "FloorEvent.ON_FLOOR_UPDATE";
    public const string ON_FLOOR_DESTROY = "FloorEvent.ON_FLOOR_DESTROY";
}
public class DebugEvent
{
    public const string ON_DEBUG_EVENT = "DebugEvent.ON_DEBUG_EVENT";
    public const string ON_ZOMBIE_SPAWN = "DebugEvent.ON_ZOMBIE_SPAWN";
}
public class CameraEvent
{
    public const string MOVE_TO_GAME_VIEW = "CameraEvent.MOVE_TO_GAME_VIEW";
    public const string INIT_CAMERA_SWIPE = "CameraEvent.INIT_CAMERA_SWIPE";
    public const string OPEN_CAMERA_SWIPE = "CameraEvent.OPEN_CAMERA_SWIPE";
    public const string CLOSE_CAMERA_SWIPE = "CameraEvent.CLOSE_CAMERA_SWIPE";
}
public class ZombieSpawnerEvent
{
    public const string ON_ZOMBIE_INIT = "ZombieSpawner.ON_ZOMBIE_INIT";
    public const string ON_ZOMBIE_SPAWN = "ZombieSpawner.ON_ZOMBIE_SPAWN";
    public const string ON_ZOMBIE_HIT = "ZombieSpawner.ON_ZOMBIE_HIT";

}
public class ClickHitEvent{
    public const string ON_CLICK_ZOMBIE = "ClickHitEvent.ON_CLICK_ZOMBIE";
}