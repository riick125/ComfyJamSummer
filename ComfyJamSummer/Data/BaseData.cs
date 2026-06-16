using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.TextureData;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.JsonsData;
using ComfyJamSummer.JsonsData.AsepriteData;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Results;
using ComfyJamSummer.UI.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ComfyJamSummer.Data
{
    public class BaseData
    {
        protected readonly string _rootDir;
        protected readonly Prefabs _prefabs;
        protected readonly IMapper _mapper;

        public BaseData(string rootDir, Prefabs prefabs, IMapper mapper)
        {
            _rootDir = rootDir;
            _prefabs = prefabs;
            _mapper = mapper;
        }

        public SpriteRenderer CreateSpriteRenderer(Entity entity, Texture2D texture, int renderLayer = 0)
        {
            if (entity == null) return null;

            return entity.AddComponent(new SpriteRenderer(texture) { RenderLayer = renderLayer });
        }

        public SpriteRenderer CreateSingleColorSpriteRenderer(Animated entity, Color color, int renderLayer = 0)
        {
            if (entity == null) return null;

            return entity.AddComponent(new SpriteRenderer(Graphics.CreateSingleColorTexture(entity.SpriteWidth, entity.SpriteHeight, color)) { RenderLayer = renderLayer });
        }


        public SpriteAnimator CreateAnimatorWithEnum(Animated entity, List<Enum> animationsNames, string directory, int renderLayer = 0)
        {
            if (entity == null) return null;

            var animator = entity.AddComponent<SpriteAnimator>();
            animator.RenderLayer = renderLayer;

            foreach (var animation in animationsNames)
            {
                if (!FileExists(directory, animation))
                    continue;

                var sprites = SpriteHelper.LoadSpritesFromAtlas(directory, animation.ToString(), entity.SpriteWidth, entity.SpriteHeight);
                animator.AddAnimation(animation.ToString(), sprites);
            }

            return animator;
        }
        public T CreateAnimatorWithEnum<T>(int width, int height, List<Enum> animationsNames, string directory, int renderLayer = 0) where T : Animated, new()
        {
            var entity = new T() { SpriteWidth = width, SpriteHeight = height };

            var animator = entity.AddComponent<SpriteAnimator>();
            animator.RenderLayer = renderLayer;

            foreach (var animation in animationsNames)
            {
                if (!FileExists(directory, animation))
                    continue;

                var sprites = SpriteHelper.LoadSpritesFromAtlas(directory, animation.ToString(), entity.SpriteWidth, entity.SpriteHeight);
                animator.AddAnimation(animation.ToString(), sprites);
            }

            return entity;
        }

        public SpriteAnimator CreateAnimatorOneAnimation(Entity entity, int width, int height, string directory, string animationName, int renderLayer = 0)
        {
            if (entity == null) return null;

            var animator = entity.AddComponent<SpriteAnimator>();
            animator.RenderLayer = renderLayer;

            var sprites = SpriteHelper.LoadSpritesFromSheet(directory, width, height);
            animator.AddAnimation(animationName, sprites);

            return animator;
        }

        public SpriteAnimator CreateAnimatorOneAnimation(Animated entity, string directory, string animationName, int renderLayer = 0)
        {
            if (entity == null) return null;

            var animator = entity.AddComponent<SpriteAnimator>();
            animator.RenderLayer = renderLayer;

            var sprites = SpriteHelper.LoadSpritesFromSheet(directory, entity.SpriteWidth, entity.SpriteHeight);
            animator.AddAnimation(animationName, sprites);

            return animator;
        }

        public SpriteAnimatorUI CreateUIAnimatorWithEnum(Animated entity, List<Enum> animationsNames, string directory)
        {
            if (entity == null) return null;

            var animator = entity.AddComponent<SpriteAnimatorUI>();

            foreach (var animation in animationsNames)
            {
                if (!FileExists(directory, animation))
                    continue;

                var sprites = SpriteHelper.LoadSpritesFromAtlas(directory, animation.ToString(), entity.SpriteWidth, entity.SpriteHeight);
                animator.AddAnimation(animation.ToString(), sprites);
            }

            return animator;
        }

        public SpriteAnimatorUI CreateUIAnimatorTextureData(Animated entity, List<Enum> spritesName, bool includeStatePrefix = false)
        {
            if (entity == null) return null;

            var rootFolder = Constants.UI_DATA_PATH;

            var animator = entity.AddComponent<SpriteAnimatorUI>();

            foreach (var enumName in spritesName)
            {
                var textureData = _prefabs.GetUITextureData(enumName);

                if (textureData == null)
                    continue;

                var fileDirectory = $"{rootFolder}{textureData.ScreenName}/{textureData.Name}";

                if (!FileExistsXnb(fileDirectory))
                    continue;

                if (string.IsNullOrEmpty(textureData.Name))
                    continue;

                var splitSpriteName = textureData.Name.Split('_');

                if (!splitSpriteName.Any())
                    continue;

                var animName = TextHelper.CapitalizeFirstLetter(splitSpriteName.Last());

                if (includeStatePrefix && splitSpriteName.Length > 1)
                {
                    var statePrefix = TextHelper.CapitalizeFirstLetter(splitSpriteName[splitSpriteName.Length - 2]);

                    animName = animName.Insert(0, $"{statePrefix}_");
                }

                var sprites = SpriteHelper.LoadSpritesFromAtlas(fileDirectory, textureData.SpriteWidth, textureData.SpriteHeight);
                animator.AddAnimation(animName, sprites);
            }

            return animator;
        }

        protected bool FileExists(string directory, Enum animation)
        {
            var realDir = Path.Combine(Core.Content.RootDirectory, $"{directory}{animation.ToString().ToLower()}.xnb").Replace("/", "\\");

            return File.Exists(realDir);
        }

        protected bool FileExistsXnb(string directory, string extension = ".xnb")
        {
            var realDir = Path.Combine(Core.Content.RootDirectory, $"{directory}{extension}").Replace("/", "\\");

            return File.Exists(realDir);
        }

        protected bool FileExists(string directory)
        {
            var realDir = Path.Combine(directory.Contains("Content", StringComparison.InvariantCultureIgnoreCase) ? "" : Core.Content.RootDirectory, $"{directory}").Replace("/", "\\");

            return File.Exists(realDir);
        }

        protected List<Enum> GetValues<T>() where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();
            return Array.ConvertAll(values, x => (Enum)x).ToList();
        }

        /// <summary>
        /// Ignores the last name of the enums like state of animations (Ex: idle, walk etc)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<Enum> GetValuesByNameIgnoringStates<T>(Enum nameEnum, int underScoreEndIndex) where T : struct, Enum
        {
            if (nameEnum == null)
            {
                return new List<Enum>();
            }

            var split = nameEnum.ToString().Split("_");

            var name = string.Empty;
            var index = 0;

            while (index < underScoreEndIndex)
            {
                name += split[index];

                if (index < underScoreEndIndex - 1)
                {
                    name += "_";
                }

                index++;
            }

            T[] values = Enum.GetValues<T>().Where(x => x.ToString().Contains(name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            return Array.ConvertAll(values, x => (Enum)x).ToList();
        }

        protected virtual List<CustomTextureData> CreateAllJsonData(JsonDataResult<SpriteJsonData> jsonData)
        {
            var result = new List<CustomTextureData>();

            if (jsonData.Validate())
            {
                var list = jsonData.GetAllData();

                foreach (var item in list)
                {
                    var dir = $"{_rootDir}{item.ScreenName}/{item.Name}";

                    result.Add(Create(item, dir));
                }
            }

            return result;
        }

        protected CustomTextureData Create(SpriteJsonData data, string dir)
        {
            var textureData = Pool<CustomTextureData>.Obtain();

            if (FileExistsXnb(dir))
            {
                var texture = Core.Content.LoadTexture(dir);

                textureData.Initialize(data, texture);
            }

            return textureData;
        }

        protected void CreateDummyAnimatorUI(SpriteAnimatorUI animator, Texture2D texture, Enum animation, int width, int height)
        {
            var sprites = Sprite.SpritesFromAtlas(texture, width, height).ToArray();
            animator.AddAnimation(animation.ToString(), sprites);
        }


        protected T CreateDummyRenderer<T>(int width, int height, Color color, int renderLayer) where T : Animated, new()
        {
            var animated = new T()
            {
                SpriteWidth = width,
                SpriteHeight = height
            };

            CreateSingleColorSpriteRenderer(animated, color, renderLayer);

            return animated;
        }

        public SpriteFrameData LoadAsepriteJson(string dir)
        {
            try
            {
                var jsonString = File.ReadAllText(dir);

                var spriteData = JsonConvert.DeserializeObject<SpriteFrameData>(jsonString);

                if (spriteData != null && spriteData.Frames != null)
                {
                    return spriteData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
    }
}