using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace ComfyJamSummer.Enums
{
    public enum GameTextProperty
    {
        name,
        description
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GameTextArchiveName
    {
    }

    public enum UIGameTextType
    {
    }

    public enum GeneralGameTextName
    {
    }

    public enum PlayerGameTextName
    {
    }

    public enum UIGameTextName
    {
        label_yes,
        label_no,
        btn_start,
        btn_options,
        btn_credits,
        btn_quit,
        btn_diff_normal,
        btn_confirm,
        label_title_dialog_quit_menu,
        label_description_dialog_quit_menu,
        label_title_dialog_restart,
        label_diff_normal_description,
        label_diff_normal_penalties,
        title,
        btn_back,
        label_description,
        title_game,
        title_video,
        title_audio,
        title_controls,
        label_language,
        label_scanline_effect,
        label_scanline_effect_on,
        label_scanline_effect_off,
        label_resolution,
        label_full_screen,
        label_full_screen_state_on,
        label_full_screen_state_off,
        label_music_volume,
        label_fx_volume,
        label_input,
        label_keyboard,
        label_gamepad,
        label_sensitivity,
        label_defaults,
        label_reset_default,
        label_move_up,
        label_move_left,
        label_move_down,
        label_move_right,
        label_interact,
        label_key_binded,
        label_press_any_key,
        label_left_thumbstick_up,
        label_left_thumbstick_down,
        label_left_thumbstick_right,
        label_left_thumbstick_left,
        label_left_shift,
        label_right_shift,
        label_left_windows,
        label_right_windows,
        label_left_alt,
        label_right_alt,
        label_left_control,
        label_right_control,
        label_space,
        label_left_mouse_button,
        label_right_mouse_button,
        label_left_trigger,
        label_right_trigger,
        label_left_bumper,
        label_right_bumper,
        label_left_stick,
        label_right_stick,
        label_feeding_monsters,
        label_resume,
        label_restart,
        label_options,
        label_quit_menu,
        label_quit_game,
        label_more_info,
        label_play_again,
        label_enemies_killed,
        label_damage_dealt,
        label_damage_received,
        label_debuffs_received,
        label_the_end,
        label_clear_time,
        label_press_to_restart,
        label_press_get_back_menu,
        label_matches_played,
        label_matches_won,
        label_lost_matches,
        label_bosses_killed,
        label_dmg_dealt,
        label_dmg_received,
        label_highest_level,
        label_total_time_played
    }
}