using System;
using System.Collections.Generic;
using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Manager;
using ComfyJamSummer.PoolObjects;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.Helpers
{
    public class UtilHelper
    {
        public static void SetResolution(Scene scene)
        {
            if (scene == null)
            {
                return;
            }

            if (Game1.SaveData == null)
            {
                SaveHelper.GetLocalSaveData();
            }

            if (Game1.SaveData == null)
            {
                return;
            }

            if (Core.Instance.IsFixedTimeStep != Game1.SaveData.IsVSyncEnabled)
            {
                Core.Instance.IsFixedTimeStep = Game1.SaveData.IsVSyncEnabled;
            }

            var timespan = TimeSpan.FromSeconds(1 / (float)Game1.SaveData.Fps);

            if (Core.Instance.TargetElapsedTime != timespan)
            {
                Core.Instance.TargetElapsedTime = timespan;
                Game1.SetTargetFPS(Game1.SaveData.Fps);
            }

            var resolutionFormatted = EnumHelper.GetEnumDescription(Game1.SaveData.ChosenResolution);

            var splitResolution = resolutionFormatted.Split("x");

            if (splitResolution != null && splitResolution.Length > 0)
            {
                var width = 0;
                var height = 0;

                if (int.TryParse(splitResolution[0], out width) && int.TryParse(splitResolution[1], out height))
                {
                    ConfigureGameResolution();

                    Core.Instance.IsFixedTimeStep = Game1.SaveData.IsVSyncEnabled;
                    Core.Instance.TargetElapsedTime = TimeSpan.FromSeconds(1 / (float)Game1.SaveData.Fps);

                    scene.SetDesignResolution(width, height, Game1.SaveData.IsFullScreen ? Scene.SceneResolutionPolicy.BestFit : Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
                    Screen.IsFullscreen = Game1.SaveData.IsFullScreen;

                    if (Screen.Width != width || Screen.Height != height)
                    {
                        Screen.SetSize(width, height);

                        Screen.ApplyChanges();
                    }
                }
            }
        }

        private static void ConfigureGameResolution()
        {
            if (Game1.SaveData == null)
            {
                SaveHelper.GetLocalSaveData();
            }

            if (Game1.SaveData == null)
            {
                return;
            }

            SetGameZoomByResolution();
        }

        public static void SetGameZoomByResolution()
        {
            switch (Game1.SaveData.ChosenResolution)
            {
                case GameResolution._1280x720:
                    Game1.GameZoom = 1.15f;
                    Game1.GameMaxZoom = 3f;
                    break;
                case GameResolution._1366x768:
                    Game1.GameZoom = 1.15f;
                    Game1.GameMaxZoom = 3f;
                    break;
                case GameResolution._1600x900:
                    Game1.GameZoom = 1.15f;
                    Game1.GameMaxZoom = 4f;
                    break;
                case GameResolution._1920x1080:
                    Game1.GameZoom = 1.15f;
                    Game1.GameMaxZoom = 4f;
                    break;
            }
        }

        public static float GetGameScale()
        {
            var larguraBase = Constants.GAME_WIDTH;
            var alturaBase = Constants.GAME_HEIGHT;

            var novaLargura = Screen.Width;
            var novaAltura = Screen.Height;

            var fatorEscalaLargura = (float)novaLargura / larguraBase;
            var fatorEscalaAltura = (float)novaAltura / alturaBase;

            return (fatorEscalaLargura + fatorEscalaAltura) / 2;
        }

        public static float GetGameScaleForUI()
        {
            var scale = GetGameScale();

            if (Core.Scene == null || Core.Scene?.Camera == null)
            {
                return scale;
            }

            return scale + Core.Scene.Camera.Zoom;
        }

        public static Prefabs Prefabs()
        {
            if (Core.Scene == null)
            {
                return null;
            }

            return Core.Scene.GetSceneComponent<Prefabs>();
        }

        public static GameManager GameManager()
        {
            if (Core.Scene == null)
            {
                return null;
            }

            return Core.Scene.GetSceneComponent<GameManager>();
        }

        public static CustomFont CustomFont()
        {
            if (Core.Scene == null)
            {
                return null;
            }

            return Core.Scene.GetSceneComponent<CustomFont>();
        }

        public static T GetEntity<T>(string name) where T : Entity, new()
        {
            if (Core.Scene == null)
            {
                return null;
            }

            var entity = Core.Scene.FindEntity(name);

            if (entity == null)
            {
                return null;
            }

            return entity as T;
        }

        public static Texture2D CreateRoundedRectangle(int width, int height, int cornerRadius, Color fillColor, Color borderColor, int borderWidth)
        {
            Texture2D texture = new Texture2D(Core.GraphicsDevice, width, height);

            Color[] colorData = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool inCorner = false;
                    bool inBorder = false;

                    if (x < cornerRadius && y < cornerRadius)
                    {
                        float distance = (x - cornerRadius) * (x - cornerRadius) + (y - cornerRadius) * (y - cornerRadius);
                        inCorner = distance > (cornerRadius * cornerRadius);
                        inBorder = distance <= (cornerRadius * cornerRadius) && distance >= (cornerRadius - borderWidth) * (cornerRadius - borderWidth);
                    }
                    else if (x >= width - cornerRadius && y < cornerRadius)
                    {
                        float distance = (x - (width - cornerRadius - 1)) * (x - (width - cornerRadius - 1)) + (y - cornerRadius) * (y - cornerRadius);
                        inCorner = distance > (cornerRadius * cornerRadius);
                        inBorder = distance <= (cornerRadius * cornerRadius) && distance >= (cornerRadius - borderWidth) * (cornerRadius - borderWidth);
                    }
                    else if (x < cornerRadius && y >= height - cornerRadius)
                    {
                        float distance = (x - cornerRadius) * (x - cornerRadius) + (y - (height - cornerRadius - 1)) * (y - (height - cornerRadius - 1));
                        inCorner = distance > (cornerRadius * cornerRadius);
                        inBorder = distance <= (cornerRadius * cornerRadius) && distance >= (cornerRadius - borderWidth) * (cornerRadius - borderWidth);
                    }
                    else if (x >= width - cornerRadius && y >= height - cornerRadius)
                    {
                        float distance = (x - (width - cornerRadius - 1)) * (x - (width - cornerRadius - 1)) + (y - (height - cornerRadius - 1)) * (y - (height - cornerRadius - 1));
                        inCorner = distance > (cornerRadius * cornerRadius);
                        inBorder = distance <= (cornerRadius * cornerRadius) && distance >= (cornerRadius - borderWidth) * (cornerRadius - borderWidth);
                    }
                    else
                    {
                        inBorder = (x < borderWidth || x >= width - borderWidth || y < borderWidth || y >= height - borderWidth);
                    }

                    if (inBorder)
                    {
                        colorData[x + y * width] = borderColor;
                    }
                    else if (!inCorner)
                    {
                        colorData[x + y * width] = fillColor;
                    }
                    else
                    {
                        colorData[x + y * width] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);

            return texture;
        }

        public static List<T> CopyListTo<T>(List<T> originalList)
        {
            if (originalList == null)
            {
                return new List<T>();
            }

            return new List<T>(originalList);
        }

        public static Texture2D DrawLineToTexture(int width, int height, float thickness, Color color)
        {
            var graphicsDevice = Core.GraphicsDevice;

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, width, height);
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            var spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin();
            spriteBatch.DrawLine(new Vector2(0, height / 2), new Vector2(width, height / 2), color, thickness);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            Texture2D resultTexture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            renderTarget.GetData(data);
            resultTexture.SetData(data);

            return resultTexture;
        }

        public static void DestroyEntities<T>() where T : Entity
        {
            if (Core.Scene == null)
            {
                return;
            }

            Core.Scene.EntitiesOfType<T>().ForEach(x => { x.Destroy(); });
        }

        public static Vector2 PerpendicularDirection(Vector2 source, Vector2 destination, float force = 32)
        {
            var perpendicularDirection = destination - source;
            perpendicularDirection.Normalize();

            perpendicularDirection = new Vector2(-perpendicularDirection.Y, perpendicularDirection.X);

            var surroundingPosition = destination + perpendicularDirection * force;

            var destinationDirection = surroundingPosition - source;
            destinationDirection.Normalize();

            return destinationDirection;
        }

        public static Texture2D CreatePixelCircle(GraphicsDevice graphicsDevice, int radius, Color color = default, float alpha = 0.09f)
        {
            var selectedColor = Constants.SPRITE_COLOR;

            if (color == default)
            {
                color = selectedColor;
            }

            int diameter = radius * 2;
            Color[] data = new Color[diameter * diameter];

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int dx = x - radius;
                    int dy = y - radius;

                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        data[x + y * diameter] = color * alpha;
                    }
                    else
                    {
                        data[x + y * diameter] = Color.Transparent;
                    }
                }
            }

            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            texture.SetData(data);

            return texture;
        }

        public static Texture2D CloneTexture(Texture2D source)
        {
            if (source == null)
            {
                return null;
            }

            Texture2D clonedTexture = new Texture2D(Core.GraphicsDevice, source.Width, source.Height);

            Color[] pixelData = new Color[source.Width * source.Height];
            source.GetData(pixelData);

            clonedTexture.SetData(pixelData);

            return clonedTexture;
        }
        public static bool HasSqueezeAnimationProperty(object obj)
        {
            return obj.GetType().GetProperty("SqueezeAnimation") != null;
        }

        public static SqueezeAnimation CreateSqueezeAnimation(Element element, float minOriginalValue = 0.91f)
        {
            var squeezeManager = SqueezeUIManager();

            if (squeezeManager == null)
            {
                return null;
            }

            var config = new SqueezeAnimationPoolConfig(element, minOriginalValue);

            return squeezeManager.Create(config);
        }
        public static SqueezeUIManager SqueezeUIManager()
        {
            if (Core.Scene == null)
            {
                return null;
            }

            return Core.Scene.GetSceneComponent<SqueezeUIManager>();
        }
    }
}