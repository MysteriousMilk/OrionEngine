#region File Description
//-----------------------------------------------------------------------------
// BloomSettings.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

namespace Orion.Core.Effect
{
    /// <summary>
    /// Class holds all the settings used to tweak the bloom effect.
    /// </summary>
    public class BloomSettings
    {
        /// <summary>
        /// Name of a preset bloom setting, for display to the user.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Controls how bright a pixel needs to be before it will bloom.
        /// Zero makes everything bloom equally, while higher values select
        /// only brighter colors. Somewhere between 0.25 and 0.5 is good.
        /// </summary>
        public float BloomThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Controls how much blurring is applied to the bloom image.
        /// The typical range is from 1 up to 10 or so.
        /// </summary>
        public float BlurAmount
        {
            get;
            set;
        }


        /// <summary>
        ///Controls the amount of the bloom image that
        /// will be mixed into the final scene. Range 0 to 1.
        /// </summary>
        public float BloomIntensity
        {
            get;
            set;
        }

        /// <summary>
        /// Controls the amount of the base image that
        /// will be mixed into the final scene. Range 0 to 1.
        /// </summary>
        public float BaseIntensity
        {
            get;
            set;
        }


        /// <summary>
        /// Independently control the color saturation of the bloom image.
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </summary>
        public float BloomSaturation
        {
            get;
            set;
        }

        /// <summary>
        /// Independently control the color saturation of the base image.
        /// Zero is totally desaturated, 1.0 leaves saturation
        /// unchanged, while higher values increase the saturation level.
        /// </summary>
        public float BaseSaturation
        {
            get;
            set;
        }


        /// <summary>
        /// Constructs a new bloom settings descriptor.
        /// </summary>
        public BloomSettings(string name, float bloomThreshold, float blurAmount,
                             float bloomIntensity, float baseIntensity,
                             float bloomSaturation, float baseSaturation)
        {
            Name = name;
            BloomThreshold = bloomThreshold;
            BlurAmount = blurAmount;
            BloomIntensity = bloomIntensity;
            BaseIntensity = baseIntensity;
            BloomSaturation = bloomSaturation;
            BaseSaturation = baseSaturation;
        }
        

        /// <summary>
        /// Table of preset bloom settings, used by the sample program.
        /// </summary>
        public static BloomSettings[] PresetSettings =
        {
            //                Name           Thresh  Blur Bloom  Base  BloomSat BaseSat
            new BloomSettings("Default",     0.25f,  4,   1.25f, 1,    1,       1),
            new BloomSettings("Soft",        0,      3,   1,     1,    1,       1),
            new BloomSettings("Desaturated", 0.5f,   8,   2,     1,    0,       1),
            new BloomSettings("Saturated",   0.25f,  4,   2,     1,    2,       0),
            new BloomSettings("Blurry",      0,      2,   1,     0.1f, 1,       1),
            new BloomSettings("Subtle",      0.5f,   2,   1,     1,    1,       1),
        };
    }
}
