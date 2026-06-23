using AutoMapper;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Nez;
using System;
using System.Collections.Generic;

namespace ComfyJamSummer.Data
{
    public class SoundData : BaseData
    {
        public SoundData(Prefabs prefabs, IMapper mapper) : base(Constants.SFX_DATA_PATH, prefabs, mapper)
        {
        }

        public List<SoundPrefab> CreateAllSFXs()
        {
            try
            {
                var result = new List<SoundPrefab>();

                var all = Enum.GetValues<SoundFxName>();

                foreach (var sfx in all)
                {
                    var dir = _rootDir + sfx.ToString().ToLower();

                    if (!FileExistsXnb(dir))
                    {
                        continue;
                    }

                    var sound = Core.Content.LoadSoundEffect(dir);

                    result.Add(new SoundPrefab() { SoundEffect = sound, SoundEffectInstance = sound.CreateInstance(), Name = sfx.ToString() });
                }

                return result;
            }
            catch (Exception)
            {
                return new List<SoundPrefab>();
            }
        }
    }
}