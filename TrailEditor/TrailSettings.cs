/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrailDLL;

namespace TrailEditor
{
    public delegate void TrailSettingsEventHandler();

    //used to interpret BlendMicrosoft.Xna.Framework.Graphics.BlendState State values
    public enum BlendStateProxy
    {
        AlphaBlend = 0,
        Additive,
        NonPremultiplied,
        Opaque
    }

    /// <summary>
    /// Singleton holding data about the trail and the editor environment
    /// </summary>
    public class TrailSettings
    {
        public static TrailSettings Instance = new TrailSettings();

        public event TrailSettingsEventHandler OnSettingsChanged;
        public event TrailSettingsEventHandler OnTextureChanged;
        public event TrailSettingsEventHandler OnBackgroundChanged;
        public event TrailSettingsEventHandler OnTrailChanged;

        private TrailSettings()
        {
            Lenght              = 100;
            Shrinking           = true;
            Billboard           = false;
            IsBGActive          = false;
            Radius              = 15;
            HeadColor           = Color.Yellow;
            TailColor           = Color.Transparent;
            ClearColor          = Color.CornflowerBlue;
            Blend               = BlendStateProxy.AlphaBlend;
            Texturing           = TrailTexturing.STRETCHED;
            TextureRepetition   = 1;
            ModelVisible        = true;
            ModelRotationSpeed  = 2.5f;
        }

        public void Changed()
        {
            if (OnSettingsChanged != null)
                OnSettingsChanged();
        }

        #region Size and billboard Properties

        private int m_iLenght;
        [Category("Size")]
        public int Lenght 
        {
            get { return m_iLenght; }
            set
            {
                m_iLenght = value;
                if (OnTrailChanged != null)
                    OnTrailChanged();
            }
        
        }

        [Category("Size")]
        public float Radius { get; set; }

        [Category("Size")]
        public bool Shrinking { get; set; }

        private bool m_bBillboard;
        [Category("Size")]
        public bool Billboard
        {
            get { return m_bBillboard; }
            set
            {
                m_bBillboard = value;
                if (OnTrailChanged != null)
                    OnTrailChanged();
            }

        }

        #endregion

        #region Color and Blending Properties

        [DisplayName("Head Color"), Category("Color")]
        public Color HeadColor { get; set; }

        [DisplayName("Tail Color"), Category("Color")]
        public Color TailColor { get; set; }

        [DisplayName("Blending"), Category("Color")]
        public BlendStateProxy Blend { get; set; }

        [Browsable(false)]
        public BlendState XNABlend
        {
            get
            {
                switch (TrailSettings.Instance.Blend)
                {
                    case BlendStateProxy.Additive: return BlendState.Additive;
                    case BlendStateProxy.AlphaBlend: return BlendState.AlphaBlend;
                    case BlendStateProxy.NonPremultiplied: return BlendState.NonPremultiplied;
                    case BlendStateProxy.Opaque: return BlendState.Opaque;
                    default: return BlendState.AlphaBlend;
                }
            }
        }

        #endregion

        #region Trail Texture Properties

        /// <summary>
        /// Trail texture name
        /// </summary>
        private string hTexture;
        [Editor(typeof(UITypeEditorTexture), typeof(UITypeEditor)), DisplayName("Image"), Category("Texture")]
        public string TextureName
        {
            get { return hTexture; }
            set
            {
                hTexture = value;
                if (OnTextureChanged != null)
                    OnTextureChanged();
            }
        }

        /// <summary>
        /// Texture mode (beam, stretched)
        /// </summary>
        private TrailTexturing m_vTexturing;
        [DisplayName("Texture mode"), Category("Texture")]
        public TrailTexturing Texturing
        {
            get { return m_vTexturing; }
            set
            {
                m_vTexturing = value;
                if (OnTrailChanged != null)
                    OnTrailChanged();
            }

        }

        /// <summary>
        /// Texture repetition along the trail
        /// </summary>
        [DisplayName("Texture repetition"), Category("Texture")]
        public float TextureRepetition { get; set; }

        #endregion

        #region Background Properties

        /// <summary>
        /// Background image name
        /// </summary>
        private string hBackground;
        [Editor(typeof(UITypeEditorTexture), typeof(UITypeEditor)), DisplayName("Image"), Category("Background")]
        public string Background 
        {
            get { return hBackground; }
            set
            {
                hBackground = value;
                if (OnBackgroundChanged != null)
                    OnBackgroundChanged();
            }
        }

        /// <summary>
        /// Enable or disable background image
        /// </summary>
        [DisplayName("Show image"), Category("Background")]
        public bool IsBGActive  { get; set; }

        /// <summary>
        /// Backbuffer clear color
        /// </summary>
        [DisplayName("Background color"), Category("Background")]
        public Color ClearColor  { get; set; }

        #endregion

        #region 3D Model Properties

        /// <summary>
        /// 3D Model visible
        /// </summary>
        [DisplayName("Visible"), Category("3D Model")]
        public bool ModelVisible { get; set; }

        /// <summary>
        /// 3D Model rotation speed
        /// </summary>
        [DisplayName("Rotation Speed"), Category("3D Model")]
        public float ModelRotationSpeed { get; set; }

        #endregion


        #region Import - Export

        public TrailSettingsDataContent ToDataContent()
        {
            TrailSettingsDataContent data = new TrailSettingsDataContent();
            
            data.Lenght               = Lenght              ;
            data.Shrinking            = Shrinking           ;
            data.Billboard            = Billboard           ;
            data.IsBGActive           = IsBGActive          ;
            data.Radius               = Radius              ;
            data.HeadColor            = HeadColor           ;
            data.TailColor            = TailColor           ;
            data.ClearColor           = ClearColor          ;
            data.Blend                = (int)Blend          ;
            data.Texturing            = (int)Texturing      ;
            data.TextureRepetition    = TextureRepetition   ;
            data.ModelVisible         = ModelVisible        ;
            data.ModelRotationSpeed   = ModelRotationSpeed  ;
            data.Background           = Background          ;
            data.TextureName          = TextureName         ;
            
            return data;
        }

        public void FromDataContent(TrailSettingsDataContent data)
        {            
            Lenght               = data.Lenght                      ;
            Shrinking            = data.Shrinking                   ;
            Billboard            = data.Billboard                   ;
            IsBGActive           = data.IsBGActive                  ;
            Radius               = data.Radius                      ;
            HeadColor            = data.HeadColor                   ;
            TailColor            = data.TailColor                   ;
            ClearColor           = data.ClearColor                  ;
            Blend                = (BlendStateProxy)data.Blend      ;
            Texturing            = (TrailTexturing)data.Texturing   ;
            TextureRepetition    = data.TextureRepetition           ;
            ModelVisible         = data.ModelVisible                ;
            ModelRotationSpeed   = data.ModelRotationSpeed          ;
            Background           = data.Background                  ;
            TextureName          = data.TextureName                 ;
        }

        #endregion
    }

    /// <summary>
    /// Helper class holding only serializable data, used to import and export XML files
    /// </summary>
    public class TrailSettingsDataContent
    { 
        public int      Lenght              ;
        public bool     Shrinking           ;
        public bool     Billboard           ;
        public bool     IsBGActive          ;
        public float    Radius              ;
        public Color    HeadColor           ;
        public Color    TailColor           ;
        public Color    ClearColor          ;
        public int      Blend               ;
        public int      Texturing           ;
        public float    TextureRepetition   ;
        public bool     ModelVisible        ;
        public float    ModelRotationSpeed  ;
        public string   TextureName         ;
        public string   Background          ; 
    }
}