/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrailDLL
{
    public enum TrailTexturing { STRETCHED = 0, BEAM };

	public class TrailFactory
	{

		/// <summary>
		/// Request the most common Trail: ribbon with stretched texture
		/// </summary>
        public static Trail GetNewTrail(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, Texture2D hTexture)
		{
            return GetNewTrail(hGame, iLength, fRadius, true, vStartColor, vEndColor, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, hTexture, 1, SamplerState.LinearClamp);
		}

		/// <summary>
		/// Request a colored Ribbon Trail
		/// </summary>
        public static Trail GetNewTrail(Game hGame, int iLength, float fRadius, bool bShrink, Color vStartColor, Color vEndColor, BlendState vBlend, bool bBillboard)
		{
            return GetNewTrail(hGame, iLength, fRadius, bShrink, vStartColor, vEndColor, vBlend, bBillboard, TrailTexturing.STRETCHED, null, 1, SamplerState.LinearClamp);
		}

		/// <summary>
		/// Request any kind of Trail
		/// </summary>
        public static Trail GetNewTrail(Game hGame, int iLength, float fRadius, bool bShrink, Color vStartColor, Color vEndColor, BlendState vBlend, bool vBillboardingType, TrailTexturing vTexturingMode, Texture2D hTexture, float fTextureRepetition, SamplerState hSamplerState)
		{
			switch (vBillboardingType)
			{
				case false:
					{
						if(hTexture == null)
						{
                            return new TrailSimpleRibbon(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
						}

						switch (vTexturingMode)
						{
							case TrailTexturing.BEAM:
								{
                                    return new TrailBeamRibbon(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}

							case TrailTexturing.STRETCHED:
								{
                                    return new TrailStretchedRibbon(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}

							default :
								{
                                    return new TrailStretchedRibbon(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}
						}
					}

				case true:
					{
						if (hTexture == null)
						{
                            return new TrailSimpleBillboard(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
						}

						switch (vTexturingMode)
						{
							case TrailTexturing.BEAM:
								{
                                    return new TrailBeamBillboard(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}

							case TrailTexturing.STRETCHED:
								{
                                    return new TrailStretchedBillboard(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}

							default:
								{
                                    return new TrailStretchedBillboard(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
								}
						}
					}

				default:
					{
                        return new TrailSimpleRibbon(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepetition, hSamplerState);
					}
			}
		}
	}
}