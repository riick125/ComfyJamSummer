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

    public class Constants
    {
        public const int GAME_WIDTH = 320;
        public const int GAME_HEIGHT = 180;

        public const float MIN_GAME_ZOOM = 1.15f;

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
        public static float HP = 100;

        public static float DMG = 38;

        public static float SPEED = 120;

        public static float ATK_SPEED = 0.75f;
    }
}