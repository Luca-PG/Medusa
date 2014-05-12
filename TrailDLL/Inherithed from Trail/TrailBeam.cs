/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrailDLL
{
    public abstract class TrailBeam : Trail
    {
        public TrailBeam(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState)
            : base(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepeat, hSamplerState)
        {
        }

        /// <summary>
        /// Beam initialization. A beam has a triangle for head and a triangle for tail. The body
        /// between the head and the tail has the common structure of trail segments. Information 
        /// on texture sampling for a Beam can be found in the TrailTest project
        /// </summary>
        internal override void Init()
        {
			int iLenght = ((m_iCurrentLength - 2) * 2) + 2;
            if (m_vVertexData == null)
            {
                m_vVertexData = new VertexPositionColor4Texture[iLenght];
                m_iPrimitiveCount = iLenght - 2;
            }
            else
            {
                Array.Resize<VertexPositionColor4Texture>(ref m_vVertexData, iLenght);
                m_iPrimitiveCount = iLenght - 2;
            }

			int iIndex = 1;

            #region HEAD

            TrailSegment hSegment = new TrailSegment(m_fRadius);
			m_hSegmentStack.Push(hSegment);

			VertexPositionColor4Texture vVertex = new VertexPositionColor4Texture();
			vVertex.Color				= m_vStartColor;
			vVertex.Position			= hSegment.Position;
			vVertex.TextureCoordinate	= new Vector2(0, 0.5f);
            m_vVertexData[0]            = vVertex;

            #endregion

            #region BODY

            for (int i = 1; i < m_iCurrentLength - 1; i++)
			{
				hSegment = new TrailSegment(m_fRadius);

				m_hSegmentStack.Push(hSegment);

				vVertex = new VertexPositionColor4Texture();

				vVertex.Color               = m_vStartColor;
				vVertex.Position            = hSegment.TopPoint;
				vVertex.TextureCoordinate   = new Vector2(0, 0);
                m_vVertexData[iIndex]       = vVertex;

				vVertex.Position            = hSegment.BottomPoint;
				vVertex.TextureCoordinate   = new Vector2(0, 1);
                m_vVertexData[iIndex + 1]   = vVertex;
				iIndex += 2;
            }

            #endregion

            #region TAIL

            hSegment = new TrailSegment(m_fRadius);
			m_hSegmentStack.Push(hSegment);
			vVertex						= new VertexPositionColor4Texture();
			vVertex.Color				= m_vStartColor;
			vVertex.Position			= hSegment.Position;
			vVertex.TextureCoordinate	= new Vector2(0, 0.5f);
            m_vVertexData[iIndex]       = vVertex;

            #endregion
        }
    }
}