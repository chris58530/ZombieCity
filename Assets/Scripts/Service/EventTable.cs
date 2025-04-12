
public class GameEvent
{
    public const string ON_INIT_GAME = "GameEvent.ON_INIT_GAME";
    public const string ON_START_GAME = "GameEvent.ON_START_GAME";
    public const string ON_QUIT_GAME = "GameEvent.ON_QUIT_GAME";
}
public class PlayerDataEvent{
    public const string ON_SET_PLAYER_DATA = "PlayerData.ON_SET_PLAYER_DATA";
    public const string ON_UPDATE_PLAYER_DATA = "PlayerData.ON_UPDATE_PLAYER_DATA";
}
public class FloorEvent
{
    public const string ON_FLOOR_INIT = "FloorEvent.ON_FLOOR_INIT";
    public const string ON_FLOOR_INIT_COMPELET = "FloorEvent.ON_FLOOR_INIT_COMPELET";
    public const string ON_FLOOR_UPDATE = "FloorEvent.ON_FLOOR_UPDATE";
    public const string ON_FLOOR_DESTROY = "FloorEvent.ON_FLOOR_DESTROY";

    public const string ON_UPDATE_COLLIDER = "FloorEvent.ON_UPDATE_COLLIDER";
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
public class ClickHitEvent
{
    public const string ON_CLICK_ZOMBIE = "ClickHitEvent.ON_CLICK_ZOMBIE";
    public const string ON_CLICK_SURVIVOR = "ClickHitEvent.ON_CLICK_SURVIVOR";
    public const string ON_CLICK_UP = "ClickHitEvent.ON_CLICK_UP";
}
public class SurvivorEvent
{

    public const string ON_SURVIVOR_INIT = "SurvivorEvent.ON_SURVIVOR_INIT";
    public const string ON_SURVIVOR_MOVE = "SurvivorEvent.ON_SURVIVOR_MOVE";
    public const string ON_CLICK_SURVIVOR = "SurvivorEvent.ON_CLICK_SURVIVOR";
    public const string ON_CLICK_SURVIVOR_COMPLETE = "SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE";
}
public class PassiveHitEvent
{
    public const string ON_OPEN_PASSIVE_HIT = "PassiveHitEvent.ON_OPEN_PASSIVE_HIT";
    public const string ON_CLOSE_PASSIVE_HIT = "PassiveHitEvent.ON_CLOSE_PASSIVE_HIT";
}
public class ResourceInfoEvent
{
    public const string ON_ADD_MONEY = "ResourceInfoEvent.ON_ADD_MONEY";
    public const string ON_ADD_SATISFACTION = "ResourceInfoEvent.ON_ADD_SATISFACTION";
    public const string ON_UPDATE_RESOURCE = "ResourceInfoEvent.ON_UPDATE_RESOURCE";

}