using ComfyJamSummer.AI.Enemies;
using ComfyJamSummer.Components.Cutscenes;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Objects;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.UI;
using Nez;
using Nez.AI.FSM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Crab : InteractableObject
    {
        string _canInteractText = "Press [E] to feed the crab", _cannotInteractText = "Go make some sandwich for the crab!!!";

        public string[] PhrasesAskingForSandwich { get; set; }

        public string[] PhrasesReactToSandwichEating { get; set; }

        public string[] PhrasesPatienceLost { get; set; }

        public CrabAnim ActualState { get; set; }

        float _satiation = 65, _satiationValuePerChange, _satiationLossValuePerChange, _maxSatiation = 100;
        float _hungryValue = 33;
        float _satiationMediumPercent = 0.3125f;
        float _satiationLowPercent = 0.064f;

        float _patience = 50, _patienceLossPerSlipUp, _maxPatience = 100;

        public bool LostPatience { get { return _patience <= 0; } }

        public bool IsHungry { get { return _satiation < _hungryValue; } }

        public CrabController CrabController => this.GetComponent<CrabController>();

        public StateMachine<Crab> Machine
        {
            get
            {
                if (CrabController == null)
                {
                    return null;
                }

                return CrabController.Machine;
            }
        }

        public float ActualSatiation { get => _satiation; }

        public float ActualPatience { get => _patience; }

        public float MaxSatiation { get => _maxSatiation; }

        public float MaxPatience { get => _maxPatience; }

        List<uint> _sandwichIds;

        public Crab CloneCrab(InteractableConfig config)
        {
            var clone = base.CloneInteractable(config) as Crab;
            clone._satiation = _satiation;
            clone._maxSatiation = _maxSatiation;
            clone._satiationLowPercent = _satiationLowPercent;
            clone._satiationMediumPercent = _satiationMediumPercent;
            clone._satiationValuePerChange = _maxSatiation * clone._satiationMediumPercent;
            clone._satiationLossValuePerChange = _maxSatiation * clone._satiationLowPercent;
            clone._hungryValue = _hungryValue;
            clone._hungryValue = _hungryValue;

            clone._patience = _patience;
            clone._maxPatience = _maxPatience;
            clone._patienceLossPerSlipUp = _maxPatience / 3;

            clone._canInteractText = _canInteractText;
            clone._cannotInteractText = _cannotInteractText;
            clone.Name = EntityNames.CRAB;
            clone._sandwichIds = new List<uint>();

            clone.PhrasesAskingForSandwich = PhrasesAskingForSandwich.CopyArray(clone.PhrasesAskingForSandwich);

            clone.PhrasesReactToSandwichEating = PhrasesReactToSandwichEating.CopyArray(clone.PhrasesReactToSandwichEating);

            clone.PhrasesPatienceLost = PhrasesPatienceLost.CopyArray(clone.PhrasesPatienceLost);

            clone.ActualState = CrabAnim.Talk;

            AnimHelper.Play(clone.Animator, CrabAnim.Idle);

            if (clone.Animator != null)
            {
                clone.Animator.OnAnimationCompletedEvent += Crab_OnAnimationCompletedEvent;
            }

            clone.AddComponent(new CrabController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            clone.AddComponent(new FakeShadowComponent(9, SpriteHeight / 2, true));

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            CrabUI.Emitter?.Emit(EventDatas.UIEvent.UpdateSatiationBar, new EventDatas.UIEventData() { Target = this });
            CrabUI.Emitter?.Emit(EventDatas.UIEvent.UpdatePatienceBar, new EventDatas.UIEventData() { Target = this });
        }

        private void Crab_OnAnimationCompletedEvent(string obj)
        {
            var value = CrabAnim.Idle;

            if (Enum.TryParse<CrabAnim>(obj, out value))
            {
                switch (value)
                {
                    case CrabAnim.Eat:
                        AnimHelper.Play(Animator, CrabAnim.Idle);
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (!Validate())
            {
                return;
            }

            var player = UtilHelper.Player();

            if (player == null)
            {
                return;
            }

            MeasureInteractText(player.Sandwich != null ? _canInteractText : _cannotInteractText);
        }

        public void ModifySatiation(bool reduce = true)
        {
            var value = reduce ? _satiationLossValuePerChange : _satiationValuePerChange;

            if (reduce)
            {
                if (Nez.Random.Chance(0.35f))
                {
                    var lowValue = _maxSatiation * _satiationLowPercent;

                    var extraLossValue = Nez.Random.Range(lowValue * 0.25f, lowValue) * (Nez.Random.MinusOneToOne());

                    value += extraLossValue;
                }
            }
            else if (Nez.Random.Chance(0.2f))
            {
                value *= 1.2f;
            }

            _satiation += reduce ? -value : value;
            _satiation = Mathf.Clamp(_satiation, 0, _maxSatiation);

            CrabUI.Emitter?.Emit(EventDatas.UIEvent.UpdateSatiationBar, new EventDatas.UIEventData() { Target = this });
        }

        public void ModifyPatience(bool reduce = true)
        {
            _patience += reduce ? -_patienceLossPerSlipUp : _patienceLossPerSlipUp / 4;
            _patience = Mathf.Clamp(_patience, 0, _maxPatience);

            CrabUI.Emitter?.Emit(EventDatas.UIEvent.UpdatePatienceBar, new EventDatas.UIEventData() { Target = this });
        }

        public void EatSandwich()
        {
            ModifySatiation(false);

            ModifyPatience(false);

            Machine?.ChangeState<CrabIdleState>();

            var breadBag = UtilHelper.GetEntity<BreadBag>();

            if (breadBag != null)
            {
                breadBag.IsInteracting = false;
            }

            Animator.Speed = 1f;

            AnimHelper.Play(Animator, CrabAnim.Eat, Nez.Sprites.SpriteAnimator.LoopMode.ClampForever);
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
                    this.Scene.AddSceneComponent(new ZoomAtTargetCutscene(this, 3));
                }
            }

            if (_patience <= 0)
            {
                this.Scene.AddSceneComponent(new ZoomAtTargetCutscene(this, 3));
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