/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrailDLL
{
    /// <summary>
    /// Custom VertexType for trail vertices.
    /// It is like VertexPositionColorTexture, but with Vector4 instead of Color.
    /// </summary>
    public struct VertexPositionColor4Texture : IVertexType
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TextureCoordinate;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
            new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    };


	public abstract class Trail : DrawableGameComponent, ITrail
	{
        /// <summary>
        /// Should be set by an engine
        /// </summary>
        public static Matrix  CameraProj_debug    = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 600, 1, 1000);
        public static Matrix  CameraView_debug    = Matrix.CreateLookAt(new Vector3(0, 0, -100), Vector3.Zero, Vector3.Up);
        public static Vector3 CameraEyePos_debug  = new Vector3(0,0,-100);
        public static Vector3 CameraEyeDir_debug  = Vector3.Forward;

        /// <summary>
        /// Minimum trail lenght, expressed in segments. A number lower than 4 will mess with most trails.
        /// </summary>
        const int MinLenght = 4;

		#region Private Fields

		protected bool								m_bColored;
        protected bool                              m_bFreeze;
        protected BlendState                        m_hBlending;
		protected Vector4				            m_vStartColor;
		protected Vector4				            m_vEndColor;
        protected Texture2D                         m_hTexture;
		protected float				                m_fRadius;

		protected bool								m_bShrinking; 
        protected int                               m_iPrimitiveCount;
        protected int								m_iCurrentLength;   //current lenght, expressed in TrailSegments
        protected VertexPositionColor4Texture[]     m_vVertexData;
        protected DepthStencilState                 m_hStencilState;
		internal  TrailStack<TrailSegment>		    m_hSegmentStack;
        internal  BasicEffect                       m_hEffect;

        List<Vector3> m_hPositions;
        List<Vector3> m_hDirections;

		#endregion

		#region Public Properties
		
		public bool					IsFrozen 
		{
			get
			{
				return m_bFreeze;
			}
		}
        public bool                 Shrinking
        {
            get
            {
                return m_bShrinking;
            }
            set
            {
                m_bShrinking = value;
                ResetSegments();
            }
        }
		public int                  Length
		{ 
			get 
			{
				return m_iCurrentLength;
			} 
			
			set 
			{
				m_iCurrentLength = value;
                if (m_iCurrentLength < MinLenght)
                    m_iCurrentLength = MinLenght;
				m_hSegmentStack.Resize(m_iCurrentLength);
				m_hSegmentStack.Reset();

				Init();
			}
		}
        public float                Radius
        {
            get
            {
                return m_fRadius;
            }
            set
            {
                m_fRadius = value;
            }
        }
        public Color                StartColor
        {
            get
            {
                return new Color(m_vStartColor);
            }
            set
            {
                m_vStartColor = value.ToVector4();
            }
        }
        public Color                EndColor
        {
            get
            {
                return new Color(m_vEndColor);
            }
            set
            {
                m_vEndColor = value.ToVector4();
            }
        }
        public BlendState           Blending
        {
            get
            {
                return m_hBlending;
            }
            set
            {
                m_hBlending = value;
            }
        }
        public float                TextureRepetition      { get; set; }
        public SamplerState         SamplerState           { get; set; }

		#endregion

		#region Constructor

        public Trail(Game hGame, int iLength, float fRadius, Color vStartColor, Color vEndColor, BlendState vBlend, bool bShrink, Texture2D hTexture, float fTextureRepeat, SamplerState hSamplerState) : base(hGame) 
		{
			m_iCurrentLength = iLength;

            if (m_iCurrentLength < MinLenght)
                m_iCurrentLength = MinLenght;

			StartColor		    = vStartColor;
			EndColor			= vEndColor;
			m_fRadius		    = fRadius;
			m_hTexture		    = hTexture;
			m_bShrinking	    = bShrink;
			m_bColored			= (vStartColor == vEndColor) ? false : true;
            m_hBlending         = vBlend;
			m_bFreeze			= false;
			m_hSegmentStack		= new TrailStack<TrailSegment>(m_iCurrentLength);
			m_hPositions		= new List<Vector3>();
			m_hDirections		= new List<Vector3>();
            m_hEffect           = new BasicEffect(hGame.GraphicsDevice);
            TextureRepetition   = fTextureRepeat;
            SamplerState        = hSamplerState;

            m_hStencilState = new DepthStencilState();
            m_hStencilState.DepthBufferEnable = false;
            m_hStencilState.DepthBufferWriteEnable = false;   

            SetupEffect();

			Init();
		}

		#endregion

		#region Public Methods

		public void Move(Vector3 vPosition, Vector3 vDirection)
        {
            Move(ref vPosition, ref vDirection);
        }

        public void Move(ref Vector3 vPosition, ref Vector3 vDirection)
        {
            m_hPositions.Add(vPosition);
            m_hDirections.Add(vDirection);
		}

        public void Reset()
        {
            Reset(Vector3.Zero);
        }

		public void Reset(Vector3 vNewPos)
		{
			Reset(ref vNewPos);
		}
		
		public void Reset(ref Vector3 vNewPos)
		{
			m_hPositions.Clear();
			m_hDirections.Clear();

			for (int i = 0; i < m_hSegmentStack.Count; i++)
			{
				m_hSegmentStack[i].Position = vNewPos;
                m_hSegmentStack[i].Radius = m_fRadius;
			}
		}

        public void Freeze()
        {
            m_bFreeze = !m_bFreeze;
        }
        
		#endregion

		#region Private Methods
        
        /// <summary>
        /// Every inherited trail must initialize its VertexData in its own way.
        /// </summary>
        internal abstract void Init();

        /// <summary>
        /// Every inherited concrete trail must have a different way to update itself.
        /// </summary>
        internal abstract void InnerUpdate();
       
        /// <summary>
        /// Moves the segments compounding the trail on the positions stacked by the
        /// Move methods
        /// </summary>
		protected void UpdateSegments()
		{
			if(m_hSegmentStack.Count == 0)
				return;

            //allows the trail to close on its head when it is not moving anymore
            if (m_hPositions.Count == 0)
            {
                Move(m_hSegmentStack.First.Position, m_hSegmentStack.First.RadiusDirection);
            }

            for (int i = 0; i < m_hPositions.Count; i++)
            {
				TrailSegment hSegment = m_hSegmentStack.Last;
				m_hSegmentStack.Pop();

                hSegment.Position           = m_hPositions[i];
                hSegment.RadiusDirection    = m_hDirections[i];

				m_hSegmentStack.Push(hSegment);
			}

			m_hPositions.Clear();
			m_hDirections.Clear();
		}

        /// <summary>
        /// Move all segments on the same position and maximize their radius
        /// </summary>
        private void ResetSegments()
        {
            for (int i = 0; i < m_hSegmentStack.Count; i++)
            {
                if(m_hPositions.Count > 0)
                    m_hSegmentStack[i].Position = m_hPositions[0];

                m_hSegmentStack[i].Radius = m_fRadius;
            }
        }

        /// <summary>
        /// Setup BasicEffect parameters
        /// </summary>
        private void SetupEffect()
        {
            if (m_hTexture != null)
            {
                m_hEffect.Texture = m_hTexture;
                m_hEffect.TextureEnabled = true;
            }

            m_hEffect.Projection    = CameraProj_debug;
            m_hEffect.View          = CameraView_debug; 

            m_hEffect.VertexColorEnabled = true;
        }

		#endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (!m_bFreeze)
                UpdateSegments();

            InnerUpdate();

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.DepthStencilState   = m_hStencilState; 
            Game.GraphicsDevice.BlendState          = m_hBlending;
            Game.GraphicsDevice.RasterizerState     = RasterizerState.CullNone;
            Game.GraphicsDevice.SamplerStates[0]    = SamplerState;
           
            m_hEffect.View          = CameraView_debug;
            m_hEffect.Projection    = CameraProj_debug;
            m_hEffect.CurrentTechnique.Passes[0].Apply();

            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor4Texture>
            (
                PrimitiveType.TriangleStrip,
                m_vVertexData,
                0,
                m_iPrimitiveCount
            );

            base.Draw(gameTime);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (m_hTexture != null)
                m_hTexture.Dispose();

            base.Dispose(disposing);
        }
    }
}