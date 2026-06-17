using ComfyJamSummer.Components.Cutscenes;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using Nez;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Crab : InteractableObject
    {
        public string[] PhrasesAskingForSandwich { get; set; }

        public string[] PhrasesReactToSandwichEating { get; set; }

        public string[] PhrasesPatienceLost { get; set; }

        public CrabAnim ActualState { get; set; }

        float _satiation = 50, _satiationLossPerWave, _maxSatiation = 100;
        float _hungryValue = 25;

        float _patience = 50, _patienceLossPerSlipUp, _maxPatience = 100;

        public bool IsHungry { get { return _satiation <= _hungryValue; } }

        List<uint> _sandwichIds;

        public Crab CloneCrab(InteractableConfig config)
        {
            var clone = base.CloneInteractable(config) as Crab;
            clone.Name = EntityNames.CRAB;
            clone._sandwichIds = new List<uint>();

            clone.PhrasesAskingForSandwich = PhrasesAskingForSandwich.CopyArray(clone.PhrasesAskingForSandwich);

            clone.PhrasesReactToSandwichEating = PhrasesReactToSandwichEating.CopyArray(clone.PhrasesReactToSandwichEating);

            clone.PhrasesPatienceLost = PhrasesPatienceLost.CopyArray(clone.PhrasesPatienceLost);

            clone.ActualState = CrabAnim.Talk;

            AnimHelper.Play(clone.Animator, CrabAnim.Idle);

            return clone;
        }

        public void PassWave()
        {
            _satiation -= _satiationLossPerWave;
            _satiation = Mathf.Clamp(_satiation, 0, _maxSatiation);
        }

        public void ModifyPatience(bool reduce = true)
        {
            _patience += reduce ? -_patienceLossPerSlipUp : _patienceLossPerSlipUp / 2;
            _patience = Mathf.Clamp(_patience, 0, _maxPatience);
        }

        public void LookAtSandwich(uint sandwichId)
        {
            var result = GrabSandwichId(sandwichId);

            if (!result)
            {
                return;
            }

            var firstTime = false;

            if (_sandwichIds.Count == 1)
            {
                var saveData = SaveHelper.GetLocalSaveData();

                if (saveData != null && !saveData.HasSeenCrabLookingAtSandwichCutsceneForFirstTime)
                {
                    firstTime = true;
                    saveData.HasSeenCrabLookingAtSandwichCutsceneForFirstTime = true;

                    SaveHelper.SaveGame(saveData);

                    this.Scene.AddSceneComponent(new ZoomAtTargetCutscene(this, 3));
                }
            }

            if (PhrasesAskingForSandwich != null && PhrasesAskingForSandwich.Any())
            {
                Core.Schedule(0.75f, t =>
                {
                    var phrase = firstTime ? PhrasesAskingForSandwich.FirstOrDefault() : PhrasesAskingForSandwich[Nez.Random.Range(0, PhrasesAskingForSandwich.Length)];

                    TextHelper.Talk(this, phrase);

                    t.Stop();
                });
            }
        }

        private bool GrabSandwichId(uint sandwichId)
        {
            if (_sandwichIds == null)
            {
                return false;
            }

            if (_sandwichIds.Contains(sandwichId))
            {
                return false;
            }

            _sandwichIds.Add(sandwichId);
            return true;
        }

        public void ReactToPlayerEatingSandwich(Player player, uint sandwichId)
        {
            if (player == null)
            {
                return;
            }

            var result = GrabSandwichId(sandwichId);

            if (!result)
            {
                return;
            }

            ModifyPatience();

            var firstTime = false;

            if (player.DevouredSandwiches == 1)
            {
                var saveData = SaveHelper.GetLocalSaveData();

                if (saveData != null && !saveData.HasSeenAngryCrabCutsceneForFirstTime)
                {
                    firstTime = true;
                    saveData.HasSeenCrabLookingAtSandwichCutsceneForFirstTime = true;

                    SaveHelper.SaveGame(saveData);
                }
            }

            var phrases = PhrasesReactToSandwichEating;

            if (phrases != null && phrases.Any())
            {
                Core.Schedule(0.75f, t =>
                {
                    var phrase = firstTime ? phrases.FirstOrDefault() : phrases[Nez.Random.Range(0, phrases.Length)];

                    TextHelper.Talk(this, phrase);

                    t.Stop();
                });
            }
        }
    }
}