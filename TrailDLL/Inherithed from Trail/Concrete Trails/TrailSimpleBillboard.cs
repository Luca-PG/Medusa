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
	public sealed class TrailSimpleBillboard : TrailSimple
	{
        public TrailSimpleBillboard(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState)
            : base(hGame, iLength, fRadius, vStartColor, vEndColor, vBlend, bShrink, hTexture, fTextureRepeat, hSamplerState)
        {
        }

        /// <summary>
        /// Calculate shrinking and color, updates VertexData with 3D billboarded positions 
        /// given by the segments.
        /// </summary>
        internal override void InnerUpdate()
        {
			TrailSegment hCurrentSegment;
			TrailSegment hPreviousSegment;
			int iIndex = 0;


			#region Trail Head

			hCurrentSegment = m_hSegmentStack[0];
			hPreviousSegment = m_hSegmentStack[1];

			hCurrentSegment.Radius = m_fRadius;

			Vector3 vAxis = Vector3.Normalize(hPreviousSegment.Position - hCurrentSegment.Position);

			Matrix vBillboard = Matrix.CreateConstrainedBillboard
				   (hCurrentSegment.Position,
				   CameraEyePos_debug,
				   vAxis,
				   CameraEyeDir_debug,
				   null);

			//float fWidth = hCurrentSegment.Radius * 2;
			Vector3 vRight = new Vector3(
              vBillboard.M11 * hCurrentSegment.Radius,
              vBillboard.M12 * hCurrentSegment.Radius,
              vBillboard.M13 * hCurrentSegment.Radius
			);

			m_vVertexData[iIndex].Position.X = vBillboard.M41 - vRight.X;
			m_vVertexData[iIndex].Position.Y = vBillboard.M42 - vRight.Y;
			m_vVertexData[iIndex].Position.Z = vBillboard.M43 - vRight.Z;
			m_vVertexData[iIndex + 1].Position.X = vBillboard.M41 + vRight.X;
			m_vVertexData[iIndex + 1].Position.Y = vBillboard.M42 + vRight.Y;
			m_vVertexData[iIndex + 1].Position.Z = vBillboard.M43 + vRight.Z;

            m_vVertexData[iIndex].Color = m_vStartColor;
            m_vVertexData[iIndex + 1].Color = m_vStartColor;

			iIndex += 2;

			#endregion

			for (int i = 1; i < m_hSegmentStack.Count; i++)
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

				vAxis = Vector3.Normalize(hCurrentSegment.Position - hPreviousSegment.Position);

				vBillboard = Matrix.CreateConstrainedBillboard
					   (hCurrentSegment.Position,
					   CameraEyePos_debug,
					   vAxis,
					   CameraEyeDir_debug,
					   null);

				//fWidth = hCurrentSegment.Radius * 1;
				vRight = new Vector3(
                  vBillboard.M11 * hCurrentSegment.Radius,
                  vBillboard.M12 * hCurrentSegment.Radius,
                  vBillboard.M13 * hCurrentSegment.Radius
				);

				m_vVertexData[iIndex].Position.X = vBillboard.M41 - vRight.X;
				m_vVertexData[iIndex].Position.Y = vBillboard.M42 - vRight.Y;
				m_vVertexData[iIndex].Position.Z = vBillboard.M43 - vRight.Z;

				m_vVertexData[iIndex + 1].Position.X = vBillboard.M41 + vRight.X;
				m_vVertexData[iIndex + 1].Position.Y = vBillboard.M42 + vRight.Y;
				m_vVertexData[iIndex + 1].Position.Z = vBillboard.M43 + vRight.Z;

				iIndex += 2;
			}
		}
	}
}