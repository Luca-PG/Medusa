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
using Microsoft.Xna.Framework.Input;
using TrailDLL;

namespace TrailTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager   graphics;
        SpriteBatch             spriteBatch;
        ITrail[]                hTrails;
        Texture2D               hRibbonTexture;
        Texture2D               hBeamTexture;
        Texture2D               hBeamExpTexture;
        Texture2D[]             hBackgroundTextures;
        SpriteFont              hSpriteFont;
        float                   fElapsedTime = 0;
        float                   fSpeed = 3;
        float                   fViewRotationY = 0;
        float                   fViewRotationX = 0;
        bool                    bShrinking = false;

        int iBGN = 0;
        int iCurrentBG
        {
            get { return iBGN; }
            set
            {
                iBGN = value;
                if (iBGN > 3)
                    iBGN = 0;
            }
        }

        int iTCS;
        int iTrailColorStart
        {
            get { return iTCS; }
            set
            {
                iTCS = value;
                if (iTCS > 2)
                    iTCS = 0;
            }
        }
        string StartColorName
        {
            get
            {
                switch (iTCS)
                {
                    case 0: return "Yellow";
                    case 1: return "White";
                    case 2: return "Transparent";
                    default: return "Transparent";
                }
            }
        }

        int iTCE;
        int iTrailColorEnd
        {
            get { return iTCE; }
            set
            {
                iTCE = value;
                if (iTCE > 2)
                    iTCE = 0;
            }
        }
        string EndColorName
        {
            get 
            {
                switch (iTCE)
                {
                    case 0: return "Transparent";
                    case 1: return "Yellow";
                    case 2: return "White";
                    default: return "Transparent";
                }
            }
        }


        int iBLEND;
        int iBlending
        {
            get { return iBLEND; }
            set
            {
                iBLEND = value;
                if (iBLEND > 3)
                    iBLEND = 0;
            }
        }
        string BlendName
        {
            get
            {
                switch (iBLEND)
                {
                    case 0: return "AlphaBlend";
                    case 1: return "Additive";
                    case 2: return "Opaque";
                    case 3: return "NonPremult.";
                    default: return "Alpha";
                }
            }
        }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth  = 800;

            Trail.CameraProj_debug = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 480, 1, 1000);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hBackgroundTextures = new Texture2D[3];

            hSpriteFont = Content.Load<SpriteFont>("font");
            hRibbonTexture = Content.Load<Texture2D>("ribbon");
            hBeamTexture = Content.Load<Texture2D>("beam");
            hBeamExpTexture = Content.Load<Texture2D>("beamX");
            hBackgroundTextures[0] = Content.Load<Texture2D>("background0");
            hBackgroundTextures[1] = Content.Load<Texture2D>("background1");
            hBackgroundTextures[2] = Content.Load<Texture2D>("background2");

            hTrails = new ITrail[6];
            hTrails[0] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, false);
            hTrails[1] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, true);
            hTrails[2] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, hRibbonTexture, 1, SamplerState.LinearClamp);
            hTrails[3] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED, hRibbonTexture, 1, SamplerState.LinearClamp);
            hTrails[4] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, false, TrailTexturing.BEAM, hBeamTexture, 1, SamplerState.LinearClamp);
            hTrails[5] = TrailFactory.GetNewTrail(this, 50, 2, false, Color.Yellow, Color.Transparent, BlendState.AlphaBlend, true, TrailTexturing.BEAM, hBeamTexture, 1, SamplerState.LinearClamp);

            for (int i = 0; i < hTrails.Length; i++)
            {
                Components.Add(hTrails[i]); 
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            fElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (fElapsedTime > 0.25f)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    bShrinking = !bShrinking;
                    for (int i = 0; i < hTrails.Length; i++)
                    {
                        hTrails[i].Shrinking = (bShrinking) ? true : false;
                    }
                    fElapsedTime = 0;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    for (int i = 0; i < hTrails.Length; i++)
                    {
                        switch (iTrailColorStart)
                        {
                            case 0: { hTrails[i].StartColor = Color.White; } break;
                            case 1: { hTrails[i].StartColor = Color.Transparent; } break;
                            case 2: { hTrails[i].StartColor = Color.Yellow; } break;
                            default: { hTrails[i].StartColor = Color.Yellow; } break;
                        }
                    }
                    fElapsedTime = 0;
                    iTrailColorStart++;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    for (int i = 0; i < hTrails.Length; i++)
                    {
                        switch (iTrailColorEnd)
                        {
                            case 0: { hTrails[i].EndColor = Color.Yellow;} break;
                            case 1: { hTrails[i].EndColor = Color.White;} break;
                            case 2: { hTrails[i].EndColor = Color.Transparent; } break;
                            default: { hTrails[i].EndColor = Color.Transparent; } break;
                        }
                    }

                    fElapsedTime = 0;
                    iTrailColorEnd++;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D4))
                {
                    for (int i = 0; i < hTrails.Length; i++)
                    {
                        switch (iBlending)
                        {
                            case 0: { hTrails[i].Blending = BlendState.Additive; } break;
                            case 1: { hTrails[i].Blending = BlendState.Opaque; } break;
                            case 2: { hTrails[i].Blending = BlendState.NonPremultiplied; } break;
                            case 3: { hTrails[i].Blending = BlendState.AlphaBlend; } break;
                            default: { hTrails[i].Blending = BlendState.AlphaBlend; } break;
                        }
                    }

                    fElapsedTime = 0;
                    iBlending++;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    fSpeed += 0.5f;
                    fElapsedTime = 0;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                {
                    fSpeed -= 0.5f;
                    fElapsedTime = 0;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
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
                    Trail.CameraEyePos_debug    = new Vector3(0, 0, 100);
                    Trail.CameraEyePos_debug    = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                    Trail.CameraEyePos_debug    = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                    Trail.CameraView_debug      = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                    Trail.CameraEyeDir_debug    = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    fViewRotationX += MathHelper.Pi / 72;
                    Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                    Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                    Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                    Trail.CameraView_debug   = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                    Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    fViewRotationX -= MathHelper.Pi / 72;
                    Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
                    Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationX(fViewRotationX));
                    Trail.CameraEyePos_debug = Vector3.Transform(Trail.CameraEyePos_debug, Matrix.CreateRotationY(fViewRotationY));
                    Trail.CameraView_debug   = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
                    Trail.CameraEyeDir_debug = Vector3.Normalize(Vector3.Zero - Trail.CameraEyePos_debug);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    iCurrentBG++;
                    fElapsedTime = 0;
                }


                
            }

            Vector3 vPos = new Vector3(
                (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * fSpeed),
                (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * fSpeed),
                0);


            for (int i = 0; i < hTrails.Length; i++)
            {
                hTrails[i].Move((i+1)*6*vPos, Vector3.Left);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if(iCurrentBG < hBackgroundTextures.Length)
            spriteBatch.Draw(hBackgroundTextures[iCurrentBG], Vector2.Zero, Color.White);

            spriteBatch.DrawString(hSpriteFont, 
                "Starting from the innermost trail:\n"+
                "1) Color\n" +
                "2) Color (Billboard)\n" +
                "3) Color + Texture\n" +
                "4) Color + Texture (Billboard)\n" +
                "5) Color + Beam\n" +
                "6) Color + Beam (Billboard)\n" +
                "\n\n\n\n\n\n\n\n\n\n\n\n\n\n D1 Shrinking: " + bShrinking +
                "\n D2 Head Color: " + StartColorName +
                "\n D3 Tail Color: " + EndColorName+
                "\n D4 Blending: " + BlendName+
                "\n +- Speed: " + fSpeed+
                "\n\n Left/Right rotate camera"+
                "\n Space change background",
                Vector2.One * 5, Color.White);

            string  text        = "Texture(3 and 4): ";
            Vector2 vMeasure    = hSpriteFont.MeasureString(text);
            Vector2 vPos        = new Vector2(graphics.PreferredBackBufferWidth - 128, 0);

            spriteBatch.DrawString(hSpriteFont, text, vPos, Color.White);
            //vPos.X += hRibbonTexture.Width / 4;
            vPos.Y += vMeasure.Y;
            spriteBatch.Draw(hRibbonTexture, vPos, Color.White);



            text        = "Beam(5 and 6): ";
            vMeasure    = hSpriteFont.MeasureString(text);
            vPos        = new Vector2(graphics.PreferredBackBufferWidth - 128, vPos.Y + hRibbonTexture.Height + 64);
            
            spriteBatch.DrawString(hSpriteFont, text, vPos, Color.White);
            vPos.X += hBeamTexture.Width/2;
            vPos.Y += vMeasure.Y;
            spriteBatch.Draw(hBeamTexture, vPos, Color.White);
            vPos.X -= 32;
            vPos.Y += hBeamTexture.Height;
            spriteBatch.Draw(hBeamExpTexture, vPos, Color.White);

            spriteBatch.End();

              

            base.Draw(gameTime);
        }
    }
}
