using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Objects;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Data
{
    public class NpcObjectData : BaseData
    {
        public NpcObjectData(Prefabs prefabs, IMapper mapper) : base(Constants.NPC_DATA_PATH, prefabs, mapper)
        {
        }

        public Crab CreateCrab()
        {
            var anims = GetValues<CrabAnim>();

            var dir = _rootDir + "crab/crab_";

            var crab = CreateAnimatorWithEnum<Crab>(32, 28, anims, dir, Constants.CREATURE_RENDER_LAYER);

            crab.PhrasesAskingForSandwich = new string[] { "ok, nice! now bring it to me!", "bring to me!", "hurry up!", "I'm starving here!" };

            crab.PhrasesReactToSandwichEating = new string[] {
                "wtf?!?!? you just ate my sandwich!!",
                "I'm warning you... stop eating my sandwiches",
                "I am dead serious, we need to get the hell outta here.",
                "HEY! STOP!!!"
            };

            crab.PhrasesPatienceLost = new string[] {
                "you're dead.",
                "you shouldn't have done that.",
                "die.",
                "run."
            };

            return crab;
        }

        public Rocket CreateRocket()
        {
            var anims = GetValues<RocketAnim>();

            var dir = Constants.MAP_DATA_PATH + "rocket/rocket_";

            var rocket = CreateAnimatorWithEnum<Rocket>(24, 41, anims, dir, Constants.CREATURE_RENDER_LAYER);

            rocket.BaseConfig = CreateBaseConfig(interactText: "Press [E] to escape!", talkAreaOffsetY: rocket.SpriteHeight / 4, talkAreaRadius: 15);

            return rocket;
        }

        public BreadBag CreateBreadBag()
        {
            var dir = Constants.MAP_DATA_PATH + "bread_bag";

            var bag = new BreadBag();

            var texture = Core.Content.LoadTexture(dir);

            var renderer = CreateSpriteRenderer(bag, texture, Constants.CREATURE_RENDER_LAYER);

            bag.SpriteWidth = (int)renderer.Width;
            bag.SpriteHeight = (int)renderer.Height;

            bag.BaseConfig = CreateBaseConfig(interactText: "Press [E] to make sandwich", talkAreaOffsetY: bag.SpriteHeight / 4, talkAreaRadius: 22);

            return bag;
        }

        public StarFish CreateStarFish()
        {
            var starFish = CreateDummyRenderer<StarFish>(12, 12, Color.Orange, Constants.CREATURE_RENDER_LAYER);

            return starFish;
        }

        public InteractableConfig InitializeCrab(uint islandId, Vector2 pos)
        {
            return _prefabs.CrabConfig.CloneNpc(islandId, pos);
        }

        public InteractableConfig CreateCrabConfig()
        {
            return new InteractableConfig()
            {
                HP = CrabValues.HP,
                Damage = CrabValues.DMG,
                Speed = CrabValues.SPEED,
                AtkSpeed = CrabValues.ATK_SPEED
            };
        }

        public InteractableConfig CreateBaseConfig(float talkAreaOffsetX = 0f, float talkAreaOffsetY = 0f, string interactText = "Press[E] to interact", float talkAreaRadius = 12f)
        {
            return new InteractableConfig()
            {
                TalkAreaOffsetX = talkAreaOffsetX,
                TalkAreaOffsetY = talkAreaOffsetY,
                TalkAreaRadius = talkAreaRadius,
                InteractText = interactText
            };
        }
    }
}