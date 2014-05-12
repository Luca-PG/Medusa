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
	public sealed class TrailStretchedRibbon : TrailStretched
	{
        public TrailStretchedRibbon(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState)
            : base(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepeat, hSamplerState)
        {
        }

        /// <summary>
        /// Calculate shrinking, color and texture coordinates, updates VertexData with 
        /// 3D positions given by the segments.
        /// </summary>
        internal override void InnerUpdate()
        {
			TrailSegment hCurrentSegment;
			TrailSegment hPreviousSegment;
			int iIndex = 0;

			for (int i = 0; i < m_iCurrentLength; i++)
			{
				float fRatio = 1 - (1 / ((float)m_iCurrentLength) * (i + 1));

				hCurrentSegment = m_hSegmentStack[i];
				hPreviousSegment = m_hSegmentStack[i - 1];

				if (m_bShrinking)
				{
					hCurrentSegment.Radius = fRatio * m_fRadius;
				}

				if (m_bColored)
				{
                    Vector4.Lerp(ref m_vStartColor, ref m_vEndColor, 1 - fRatio, out m_vVertexData[iIndex].Color);

					m_vVertexData[iIndex + 1].Color = m_vVertexData[iIndex].Color;
				}

				m_vVertexData[iIndex].Position.X = hCurrentSegment.Position.X +
													hCurrentSegment.RadiusDirection.X * hCurrentSegment.Radius;
				m_vVertexData[iIndex].Position.Y = hCurrentSegment.Position.Y +
													hCurrentSegment.RadiusDirection.Y * hCurrentSegment.Radius;
				m_vVertexData[iIndex].Position.Z = hCurrentSegment.Position.Z +
													hCurrentSegment.RadiusDirection.Z * hCurrentSegment.Radius;

				m_vVertexData[iIndex + 1].Position.X = hCurrentSegment.Position.X -
													hCurrentSegment.RadiusDirection.X * hCurrentSegment.Radius;
				m_vVertexData[iIndex + 1].Position.Y = hCurrentSegment.Position.Y -
													hCurrentSegment.RadiusDirection.Y * hCurrentSegment.Radius;
				m_vVertexData[iIndex + 1].Position.Z = hCurrentSegment.Position.Z -
													hCurrentSegment.RadiusDirection.Z * hCurrentSegment.Radius;

                //float fStep = i * (1f / m_hSegmentStack.Count);
                float fStep = (i+1)* (TextureRepetition / m_hSegmentStack.Count);

				m_vVertexData[iIndex].TextureCoordinate.X = fStep;
				m_vVertexData[iIndex].TextureCoordinate.Y = 0;
				m_vVertexData[iIndex + 1].TextureCoordinate.X = fStep;
				m_vVertexData[iIndex + 1].TextureCoordinate.Y = 1;

				iIndex += 2;
			}

		}
	}
}