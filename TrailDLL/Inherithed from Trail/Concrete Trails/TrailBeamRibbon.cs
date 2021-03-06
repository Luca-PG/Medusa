﻿/*
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
	public class TrailBeamRibbon : TrailBeam
	{
        float m_fStep;

        public TrailBeamRibbon(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState)
            : base(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepeat, hSamplerState)
        {
            m_fStep = (0.5f / (iLength - 3));
        }

        /// <summary>
        /// Calculate shrinking, color and BEAM texture coordinates, updates VertexData with 
        /// 3D positions given by the segments.
        /// </summary>
        internal override void InnerUpdate()
        {
			int iIndex = 1;
			TrailSegment hCurrentSegment;
			TrailSegment hPreviousSegment;

			//Trail Head
			m_vVertexData[0].Position = m_hSegmentStack[0].Position;
			m_vVertexData[0].TextureCoordinate.X = 0;
			m_vVertexData[0].TextureCoordinate.Y = 0;
            m_vVertexData[0].Color = m_vStartColor;

			int i;
			for (i = 1; i < m_hSegmentStack.Count - 1; i++)
			{
				hCurrentSegment = m_hSegmentStack[i];
				hPreviousSegment = m_hSegmentStack[i - 1];

				float fRatio = 1 - (1 / ((float)m_iCurrentLength) * (i + 1));

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

				float fStep = ((i - 1) * m_fStep);

				m_vVertexData[iIndex].TextureCoordinate.X = 0;
				m_vVertexData[iIndex].TextureCoordinate.Y = 0.5f + fStep;
				m_vVertexData[iIndex + 1].TextureCoordinate.X = 1;
				m_vVertexData[iIndex + 1].TextureCoordinate.Y = fStep;

				iIndex += 2;
			}

			//Trail Tail
			m_vVertexData[iIndex].Position = m_hSegmentStack[i].Position;
			m_vVertexData[iIndex].TextureCoordinate.X = 1;
			m_vVertexData[iIndex].TextureCoordinate.Y = 1;
            m_vVertexData[iIndex].Color = m_vEndColor;
		}
	}
}