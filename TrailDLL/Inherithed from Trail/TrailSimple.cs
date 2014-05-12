/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TrailDLL
{
    public abstract class TrailSimple : Trail
    {
        public TrailSimple(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState)
            : base(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepeat, hSamplerState)
        {
        }

        /// <summary>
        /// Initialization for a simple trail. A simple trail has a common structure of trail
        /// segments and ignores texture information.
        /// </summary>
        internal override void Init()
        {
            if (m_vVertexData == null)
            {
                m_vVertexData = new VertexPositionColor4Texture[m_iCurrentLength * 2];
                m_iPrimitiveCount = (m_iCurrentLength - 1) * 2;
            }
            else
            {
                Array.Resize<VertexPositionColor4Texture>(ref m_vVertexData, m_iCurrentLength * 2);
                m_iPrimitiveCount = (m_iCurrentLength - 1) * 2;
            }

            int iIndex = 0;

            for (int i = 0; i < m_iCurrentLength; i++)
            {
                TrailSegment hSegment = new TrailSegment(m_fRadius);

                m_hSegmentStack.Push(hSegment);

                VertexPositionColor4Texture vVertex = new VertexPositionColor4Texture();

                vVertex.Color               = m_vStartColor;
                vVertex.Position            = hSegment.TopPoint;
                m_vVertexData[iIndex]       = vVertex;

                vVertex.Position            = hSegment.BottomPoint;
                m_vVertexData[iIndex + 1]   = vVertex;

                iIndex += 2;
            }

        }
    }
}