namespace ComfyJamSummer.JsonsData.AsepriteData
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SpriteFrameData
    {
        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        public int Width
        {
            get
            {
                if (Meta == null)
                {
                    return 0;
                }

                return Meta.Size.Width;
            }
        }
        public int Height
        {
            get
            {
                if (Meta == null)
                {
                    return 0;
                }

                return Meta.Size.Height;
            }
        }
    }

    public class Frame
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("frame")]
        public Rectangle FrameRect { get; set; }

        [JsonProperty("rotated")]
        public bool Rotated { get; set; }

        [JsonProperty("trimmed")]
        public bool Trimmed { get; set; }

        [JsonProperty("spriteSourceSize")]
        public Rectangle SpriteSourceSize { get; set; }

        [JsonProperty("sourceSize")]
        public Size SourceSize { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }

    public class Rectangle
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class Size
    {
        [JsonProperty("w")]
        public int Width { get; set; }

        [JsonProperty("h")]
        public int Height { get; set; }
    }

    public class Meta
    {
        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }
    }
}