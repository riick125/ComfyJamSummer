using Microsoft.Xna.Framework;

namespace ComfyJamSummer
{
    public class UINames
    {
        public const string SAVING = "SavingGameUI";
        public const string DEBUG = "DebugUI";
        public const string COMPANY_LOGO = "CompanyLogoUI";
    }

    public class TiledLayerNames
    {
        public const string FLOOR = "floor";
        public const string WALLS = "walls";
    }

    public class EntityNames
    {
        public const string PLAYER = "Player";
        public const string ISLAND = "Island";
        public const string STONE = "Stone";
        public const string BATTLE = "Battle";
        public const string COLLECTIBLE_SPAWNER = "CollectibleSpawner";
    }

    public static class StaticStuff
    {
        static int _uiLayer = -1;

        public static int UILayer
        {
            get
            {
                return _uiLayer--;
            }
        }
    }

    public class WaveDefaultValues
    {
        public const int ALIVE_ENEMIES_QTY_LIMIT = 15;

        public const float ALIVE_ENEMIES_GROW_PERCENT = 1f;

        public const float ALIVE_ENEMIES_MODIFIER_VALUE = 0.15f;

        public const float START_COOLDOWN = 5f;

        public const float DURATION = 20f;

        public const float MAX_DURATION = 60f;
    }

    public class Constants
    {
        public const int GAME_WIDTH = 320;
        public const int GAME_HEIGHT = 180;

        public const float MIN_GAME_ZOOM = 0.75f;

        public const float GameZoom = 2;

        public const float GameMaxZoom = 3;

        public static int SCREEN_SIZE_MULTIPLIER = 4;

        public static readonly Microsoft.Xna.Framework.Color BG_COLOR_LOGO = new Microsoft.Xna.Framework.Color(4, 16, 2);

        public static Color RED_COLOR = new Color(237, 24, 24);

        public static Color WHITE_COLOR = new Color(255, 255, 255);

        public static Color SPRITE_COLOR = new Color(98, 237, 24);

        public static Color BG_COLOR = new Color(3, 18, 2);

        public const string SAVE_PATH = "data/stats.json";

        public const string JSON_DATA_PATH = "jsons/data/";

        public const string JSON_DATA_REAL_PATH = "jsons\\data\\";

        public const string NPC_DATA_PATH = "sprites/gameplay/npc/";

        public const string PLAYER_DATA_PATH = "sprites/gameplay/player/";

        public const string COLLECTIBLE_DATA_PATH = "sprites/gameplay/collectible/";

        public const string ENEMY_DATA_PATH = "sprites/gameplay/enemy/";

        public const string MAP_DATA_PATH = "Content/tmx/";

        public const string UI_DATA_PATH = "sprites/gameplay/ui/";

        public const string ART_DATA_PATH = "art/";

        public const int CREATURE_RENDER_LAYER = 3;

        public const int MAP_RENDER_LAYER = 10;

        public const float SPRITE_MAX_SIZE = 208;
        public const int CONTROLLER_SENSITIVITY_MIN_VALUE = 100;

        public const int CONTROLLER_SENSITIVITY_DEFAULT_VALUE = 225;

        public const int CONTROLLER_SENSITIVITY_MAX_VALUE = 500;

        public static readonly float SATURATION_FACTOR_NORMAL = 1.75f;

        public const float SLOW_SCENE_FADE_IN = 0.15f;

        public const float DELAY_BEFORE_FADE_IN_DURATION = 0.15f;

        public const float FAST_SCENE_FADE_IN = 0.05f;
    }

    public static class PlayerValues
    {
        public static float HP = 350;

        public static float SPEED = 120;
    }

    public static class CrabValues
    {
        public const float DMG = 87.5f;

        public const float SPEED = 160;

        public const float ATK_SPEED = 1f;
    }

    public static class EnemyValues
    {
        public const float HP = 75;

        public const float DMG = 38;

        public const float SPEED = 87;

        public const float BULLET_SPEED = 115;

        public const float ATK_SPEED = 3.25f;

        public const float IDLE_TIME_STATE = 0.5f;

        public const float MOVE_TIME_STATE = 10f;

        public const float PATROL_TIME_STATE = 1.1f;

        public const float PATROL_COOLDOWN = 1f;

        public const float ATK_TIME_STATE = 4f;
    }

    public static class BuffEnemyModifierValues
    {
        public const float HP = 0.025f;

        public const float DMG = 0.04f;

        public const float SPEED = 0.0025f;

        public const float ATK_SPEED = 0.001f;
    }
}