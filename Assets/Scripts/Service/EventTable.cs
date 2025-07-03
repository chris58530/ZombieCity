
using CW.Common;

public class GameEvent
{
    public const string ON_GAME_STATE_START = "GameEvent.ON_GAME_STATE_START";
    public const string ON_GAME_STATE_END = "GameEvent.ON_GAME_STATE_END";
    public const string ON_BATTLE_STATE_START = "GameEvent.ON_BATTLE_STATE_START";
    public const string ON_BATTLE_STATE_END = "GameEvent.ON_BATTLE_STATE_END";
}
public class JsonDataEvent
{
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

    public const string ON_ADD_PRODUCT = "FloorEvent.ON_ADD_PRODUCT";
    public const string ON_ADD_LEVEL = "FloorEvent.ON_ADD_LEVEL";
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
    public const string ON_USE_FEATURE_CAMERA = "CameraEvent.ON_USE_FEATURE_CAMERA";
    public const string ON_USE_FEATURE_CAMERA_COMPLETE = "CameraEvent.ON_USE_FEATURE_CAMERA_COMPLETE";
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

    public const string ON_SURVIVOR_LEAVE_FACILITY = "SurvivorEvent.ON_SURVIVOR_LEAVE_FACILITY";
    public const string ON_SURVIVOR_INIT = "SurvivorEvent.ON_SURVIVOR_INIT";
    public const string ON_SURVIVOR_MOVE = "SurvivorEvent.ON_SURVIVOR_MOVE";
    public const string ON_CLICK_SURVIVOR = "SurvivorEvent.ON_CLICK_SURVIVOR";
    public const string ON_CLICK_SURVIVOR_COMPLETE = "SurvivorEvent.ON_CLICK_SURVIVOR_COMPLETE";

    public const string ON_ADD_SURVIVOR_LEVEL = "SurvivorEvent.ON_ADD_SURVIVOR_LEVEL";
    public const string ON_SET_SURVIVOR_STAYINGFLOOR = "SurvivorEvent.ON_SET_SURVIVOR_STAYINGFLOOR";
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
    public const string ON_ADD_ZOMBIECORE = "ResourceInfoEvent.ON_ADD_ZOMBIECORE";
    public const string ON_UPDATE_RESOURCE = "ResourceInfoEvent.ON_UPDATE_RESOURCE";
}
public class TrasitionBackGroundEvent
{
    public const string ON_TRASITION_BACKGROUND = "TrasitionBackGroundEvent.ON_TRASITION_BACKGROUND";
    public const string ON_TRASITION_COMPLETE = "TrasitionBackGroundEvent.ON_TRASITION_COMPLETE";
}
public class DrawCardEvent
{
    public const string ON_DRAW_CARD_SHOW = "DrawCardEvent.ON_DRAW_CARD_SHOW";
    public const string ON_DRAW_CARD_CLOSE = "DrawCardEvent.ON_DRAW_CARD_CLOSE";
}
public class DropItemEvent
{
    public const string REQUEST_DROP_FLOOR_ITEM = "DropItemEvent.REQUEST_DROP_FLOOR_ITEM";
    public const string REQUEST_DROP_RESOURCE_ITEM = "DropItemEvent.REQUEST_DROP_RESOURCE_ITEM";
}
public class CampCarEvent
{
    public const string ON_SELECT_LEVEL_SHOW = "CampCarEvent.ON_SELECT_LEVEL_SHOW";
    public const string ON_SELECT_LEVEL_HIDE = "CampCarEvent.ON_SELECT_LEVEL_HIDE";
    public const string ON_BATTLE_CAR_SHOW = "CampCarEvent.ON_BATTLE_CAR_SHOW";
}
public class SelectLevelEvent
{
    public const string ON_SELECT_LEVEL_CLICKED = "SelectLevelEvent.ON_SELECT_LEVEL_CLICKED";
}