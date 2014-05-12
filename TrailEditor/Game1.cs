/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrailDLL;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace TrailEditor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager   hGraphics;
        SpriteBatch             hSpriteBatch;
        ContentBuilder          hContentBuilder;

        TrailEditorForm hTrailPanel; 

        ITrail          hTrail;
        Texture2D       hTrailTexture;
        float           fViewRotationY = 0;
        float           fViewRotationX = 0;

        Model           hSword;
        Texture2D       hBackground;
        Matrix          vModelWorld;

        public Game1()
        {
            hGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            hGraphics.PreferredBackBufferHeight = 480;
            hGraphics.PreferredBackBufferWidth  = 800;
            base.Initialize();

            //init content builder
            hContentBuilder = new ContentBuilder();
            Content.RootDirectory = hContentBuilder.OutputDirectory;

            //init forms
            Form gameWindowForm = (Form)Form.FromHandle(this.Window.Handle);
            gameWindowForm.Shown += new EventHandler(gameWindowForm_Shown);

	        hTrailPanel = new TrailEditorForm();
	        hTrailPanel.HandleDestroyed += new EventHandler(myForm_HandleDestroyed);
            hTrailPanel.Resize += new EventHandler(myForm_Resize);
            hTrailPanel.XNAPanel.Resize += new EventHandler(XNAPanel_Resize);
	        hTrailPanel.Show();

            TrailSettings.Instance.OnSettingsChanged += UpdateTrailSettings;
            TrailSettings.Instance.OnBackgroundChanged += UpdateBackground;
            TrailSettings.Instance.OnTextureChanged += UpdateTrailTexture;
            TrailSettings.Instance.OnTrailChanged += UpdateWithNewTrail;

            //load sword model
            DirectoryInfo dirinfo = Directory.GetParent(Directory.GetCurrentDirectory());
            string dir = dirinfo.Parent.FullName;
            LoadModel(dir + "\\" + "sword.fbx", out hSword);
        }

        public void ChangeResolution(int width, int height)
        {
            int resolutionWidth = width;
            int resolutionHeight = height;

            if (resolutionWidth <= 0 || resolutionWidth <= 0)
            {
                resolutionWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                resolutionHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            hGraphics.PreferredBackBufferWidth = resolutionWidth;
            hGraphics.PreferredBackBufferHeight = resolutionHeight;
            hGraphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            hSpriteBatch = new SpriteBatch(GraphicsDevice);

            //creates the first trail
            UpdateWithNewTrail();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        Vector3 vPrevPos = Vector3.Zero;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            GetCameraInput();

            //Vector3 vPos = new Vector3(
            //(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * fSpeed),
            //(float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * fSpeed),
            //0);

            vModelWorld = Matrix.CreateRotationZ((float)gameTime.TotalGameTime.TotalSeconds * TrailSettings.Instance.ModelRotationSpeed);
            Vector3 vPos = Vector3.Transform(Vector3.Up, vModelWorld);
            hTrail.Move(22 * vPos, Vector3.Normalize(Vector3.Cross((vPos - vPrevPos), Vector3.Forward)));
            vPrevPos = vPos;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(TrailSettings.Instance.ClearColor);

            //Draws the background
            if (TrailSettings.Instance.IsBGActive)
            {
                DrawBackground();
            }

            //Draws the sword model
            if (TrailSettings.Instance.ModelVisible)
            {
                DrawModel();
            }

            base.Draw(gameTime);

            //presents the rendered frame to the form
            GraphicsDevice.Present(null,  null, hTrailPanel.PanelHandle);
        }

        #region Draw helpers

        void DrawBackground()
        {
            if (hBackground != null)
            {
                //the background image is stretched to fit the whole backbuffer
                hSpriteBatch.Begin();
                hSpriteBatch.Draw(hBackground,
                    new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight),
                    null,
                    Color.White);
                hSpriteBatch.End();
            }
        }

        void DrawModel()
        {
            // Set suitable renderstates for drawing a 3D model.
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in hSword.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = vModelWorld;
                    effect.View = Trail.CameraView_debug;
                    effect.Projection = Trail.CameraProj_debug;
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = new Vector3(0.5f);
                }

                mesh.Draw();
            }
        }

        #endregion

        #region Load helpers
        /// <summary>
        /// Build and load content at runtime
        /// </summary>
        void LoadTexture(string fileName, out Texture2D hTexture)
        {
            string buildName = Path.GetFileName(fileName);
            // Tell the ContentBuilder what to build.
            hContentBuilder.Clear();

            ProjectItem item = hContentBuilder.Add(fileName, buildName, null, "TextureProcessor");
            item.SetMetadataValue("ProcessorParameters_ResizeToPowerOfTwo", "True");

            // Build this new texture data.
            string buildError = hContentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                hTexture = Content.Load<Texture2D>(buildName);
            }
            else
            {
                // If the build failed, display an error message.
                hTexture = null;
                MessageBox.Show(buildError, "Error");
            }
        }

        void LoadModel(string fileName, out Model Model)
        {
            string buildName = Path.GetFileName(fileName);
            // Tell the ContentBuilder what to build.
            hContentBuilder.Clear();

            ProjectItem item = hContentBuilder.Add(fileName, buildName, null, "ModelProcessor");
            item.SetMetadataValue("ProcessorParameters_Scale", "2.5");
            item.SetMetadataValue("ProcessorParameters_RotationZ", "90");

            // Build this new model data.
            string buildError = hContentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                Model = Content.Load<Model>(buildName);
            }
            else
            {
                // If the build failed, display an error message.
                Model = null;
                MessageBox.Show(buildError, "Error");
            }
        }

        #endregion

        #region Forms Events
        void XNAPanel_Resize(object sender, EventArgs e)
        {
            ChangeResolution(((Panel)sender).Width, ((Panel)sender).Height);
        }

        void myForm_Resize(object sender, EventArgs e)
        {
            ChangeResolution(((TrailEditorForm)sender).PanelWidth, ((TrailEditorForm)sender).PanelHeight);
        }

        void myForm_HandleDestroyed(object sender, EventArgs e)
        {
            this.Exit();
        }

        void gameWindowForm_Shown(object sender, EventArgs e)
        {
            ((Form)sender).Hide();
        }
        #endregion

        #region Settings Events
        void UpdateBackground()
        {
            hBackground = null;
            if (!string.IsNullOrEmpty(TrailSettings.Instance.Background))
            {
                LoadTexture(TrailSettings.Instance.Background, out hBackground);
            }
        }

        void UpdateTrailTexture()
        {
            hTrailTexture = null;
            if (TrailSettings.Instance.TextureName != null)
            {
                LoadTexture(TrailSettings.Instance.TextureName, out hTrailTexture);
            }
            UpdateWithNewTrail();
        }

        void UpdateTrailSettings()
        {
            hTrail.Radius = TrailSettings.Instance.Radius;
            hTrail.Shrinking = TrailSettings.Instance.Shrinking;
            hTrail.StartColor = TrailSettings.Instance.HeadColor;
            hTrail.EndColor = TrailSettings.Instance.TailColor;
            hTrail.Radius = TrailSettings.Instance.Radius;
            hTrail.Blending = TrailSettings.Instance.XNABlend;
            hTrail.TextureRepetition = TrailSettings.Instance.TextureRepetition;
        }

        void UpdateWithNewTrail()
        {
            Components.Remove(hTrail);
            hTrail = null;

            hTrail = TrailFactory.GetNewTrail(this,
                TrailSettings.Instance.Lenght,
                TrailSettings.Instance.Radius,
                TrailSettings.Instance.Shrinking,
                TrailSettings.Instance.HeadColor,
                TrailSettings.Instance.TailColor,
                TrailSettings.Instance.XNABlend,
                TrailSettings.Instance.Billboard,
                TrailSettings.Instance.Texturing,
                hTrailTexture,
                TrailSettings.Instance.TextureRepetition,
                SamplerState.LinearWrap);

            Components.Add(hTrail);
        }

        #endregion

        #region Camera Input

        void GetCameraInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                fViewRotationY += MathHelper.Pi / 72;
                Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                Trail.CameraView_debug = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                fViewRotationY -= MathHelper.Pi / 72;
                Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                Trail.CameraView_debug = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                fViewRotationX += MathHelper.Pi / 72;
                Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                Trail.CameraView_debug = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                fViewRotationX -= MathHelper.Pi / 72;
                Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                Trail.CameraView_debug = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
            }
        }


        
        #endregion
    }
}