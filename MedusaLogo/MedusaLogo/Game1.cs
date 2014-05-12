/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */


/////////////////////////////////////////////////////////////////////
//  This code is a mess and it is just used to render the medusa logo
/////////////////////////////////////////////////////////////////////


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrailDLL;

namespace MedusaLogo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ITrail[] Trails;
        float fElapsedTime = 0;
        Vector4 BGColor;
        bool Frozen = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            BGColor = Color.Black.ToVector4();

            Texture2D texture = Content.Load<Texture2D>("trail");

            Trails = new ITrail[34]
            {
                TrailFactory.GetNewTrail(this, 50, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true,  TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),   //curl l
                TrailFactory.GetNewTrail(this, 50, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 50, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true,  TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),   //curl r
                TrailFactory.GetNewTrail(this, 50, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
             
                TrailFactory.GetNewTrail(this, 40, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp), //eight
                TrailFactory.GetNewTrail(this, 60, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true,  TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),  //snakes
                TrailFactory.GetNewTrail(this, 60, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true,  TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 80, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 80, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 80, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 80, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                
                TrailFactory.GetNewTrail(this, 60, 1,      false,  Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.75f,  false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.5f,   false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),

                TrailFactory.GetNewTrail(this, 60, 1,       false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.75f,   false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.5f,    false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),

                TrailFactory.GetNewTrail(this, 60, 1,       false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.75f,   false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.5f,    false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                
                TrailFactory.GetNewTrail(this, 60, 1,     true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED,  texture, 1, SamplerState.LinearClamp),  //outer snakes
                TrailFactory.GetNewTrail(this, 60, 1,     true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED,  texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 1,     true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 1,     true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.75f, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
                TrailFactory.GetNewTrail(this, 60, 0.75f, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),

                
                TrailFactory.GetNewTrail(this, 56, 1f, false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, true, TrailTexturing.STRETCHED,  texture, 1, SamplerState.LinearClamp),  //mouth
                TrailFactory.GetNewTrail(this, 50, 2f, false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp), //eyes
                TrailFactory.GetNewTrail(this, 50, 2f, false, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
             
                TrailFactory.GetNewTrail(this, 60, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp), //small curls
                TrailFactory.GetNewTrail(this, 60, 1, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp),
             
                TrailFactory.GetNewTrail(this, 40, 2, true, Color.DarkOliveGreen, Color.Black, BlendState.AlphaBlend, false, TrailTexturing.STRETCHED, texture, 1, SamplerState.LinearClamp), //eight
            };




            for (int i = 0; i < Trails.Length; i++)
            {   
                Components.Add(Trails[i]);
            }

            Trail.CameraEyePos_debug = new Vector3(0, 0, 100);
            Trail.CameraView_debug = Matrix.CreateLookAt(Trail.CameraEyePos_debug, Vector3.Zero, Vector3.Up);
            Trail.CameraProj_debug = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 600f / 600, 1, 1000);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            fElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && fElapsedTime > 0.25f) 
            {
                Frozen = !Frozen;
                fElapsedTime = 0;
            }

            // TODO: Add your update logic here

            //BGColor.X = (10 - (float)(gameTime.TotalGameTime.TotalSeconds)) / 10;
            //BGColor.Y = (10 - (float)(gameTime.TotalGameTime.TotalSeconds)) / 10;
            //BGColor.Z = (10 - (float)(gameTime.TotalGameTime.TotalSeconds)) / 10;
            
            if (!Frozen)
            {

                Trails[29].StartColor = Color.DarkOliveGreen;
                Trails[30].StartColor = Color.DarkOliveGreen;

                Vector3 vPos = new Vector3(
                    20 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 3) * 12,
                    20 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) * 12,
                    0);
                Trails[0].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[3].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    20 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 4) * 10,
                    20 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) * 10,
                    0);
                Trails[1].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[4].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    18 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 8,
                    18 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 8,
                    0);
                Trails[2].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[5].Move(vPos, Vector3.Left);

                //eight curve

                vPos = new Vector3(
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 16,
                    18 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 16,
                    0);
                Trails[6].Move(vPos, Vector3.Left);


                //////////// inner snakes

                vPos = new Vector3(
                    -21 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * 3 + 10) * 5,
                    -4 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) * 10,
                    0);
                Trails[7].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[8].Move(vPos, Vector3.Left);



                vPos.X = -vPos.X - 4;
                vPos.Y -= 4;
                Trails[9].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[10].Move(vPos, Vector3.Left);



                vPos = new Vector3(
                     -21 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * 3 + 10) * 5,
                     -4 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * -2) * 10,
                     0);
                Trails[11].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[12].Move(vPos, Vector3.Left);




                ////////////upper curl

                vPos = new Vector3(
                    34 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) * 6,
                    5 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 4) * 6,
                    0);
                Trails[13].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[16].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    33 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) * 4,
                    6 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 3) * 4,
                    0);
                Trails[14].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[17].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    32 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 2,
                    7 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 2,
                    0);
                Trails[15].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[18].Move(vPos, Vector3.Left);






                vPos = new Vector3(
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) * 6,
                    30 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 4) * 6,
                    0);
                Trails[19].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) * 4,
                    28 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 3) * 4,
                    0);
                Trails[20].Move(vPos, Vector3.Left);

                vPos = new Vector3(
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 2,
                    27 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 2,
                    0);
                Trails[21].Move(vPos, Vector3.Left);



                //////////// outer snakes

                vPos = new Vector3(
                    -33 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * 3 + 10) * 2.5f,
                    -8 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * -2) * 5,
                    0);
                Trails[22].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[23].Move(vPos, Vector3.Left);



                vPos.X = -vPos.X - 4;
                vPos.Y -= 4;
                Trails[24].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[25].Move(vPos, Vector3.Left);



                vPos = new Vector3(
                     -33 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * 3 + 10) * 2.5f,
                     -8 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) * 5,
                     0);
                Trails[26].Move(vPos, Vector3.Left);

                vPos.X = -vPos.X;
                Trails[27].Move(vPos, Vector3.Left);


                //mouth
                vPos = new Vector3(
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 7) * 2,
                    -12 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 7) * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 7) * 2,
                    0);
                Trails[28].Move(vPos, Vector3.Left);

                float a = 17;
                //eyes
                //vPos = new Vector3(
                //    7 + (3 * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * a)),
                //    3 + (-3 * (((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * a) * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * a)) * (2 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * a))) / (3 + ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * a) * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * a)))),
                //    0);

                vPos = new Vector3(
                    8 + (2 * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * a)),
                    3 + (1 * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * a)),
                    0);

                vPos = Vector3.Transform(vPos, Matrix.CreateRotationZ(MathHelper.Pi/12));
                Vector3 vDir = Vector3.Transform(Vector3.Left, Matrix.CreateRotationZ(MathHelper.Pi / 12));

                Trails[29].Move(vPos, vDir);

                vPos = new Vector3(
                    -8 + (-2 * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * a)),
                    3 + (1 * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * a)),
                    0);

                vPos = Vector3.Transform(vPos, Matrix.CreateRotationZ(MathHelper.Pi / -12));
                vDir = Vector3.Transform(Vector3.Left, Matrix.CreateRotationZ(MathHelper.Pi / -12));

                Trails[30].Move(vPos, vDir);


               
                ////small curls 
                vPos = new Vector3(
                    21 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5.5f) * 4,
                    21 + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5.5f) * 4,
                    0);
                Trails[31].Move(vPos, Vector3.Left);
                vPos.X = -vPos.X;
                Trails[32].Move(vPos, Vector3.Left);

                //eight curve

                vPos = new Vector3(
                    -(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 16,
                    18 + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5) * 16,
                    0);
                Trails[33].Move(vPos, Vector3.Left);




            }

            else
            {
                Trails[29].StartColor = Color.Red;
                Trails[30].StartColor = Color.Red;
                //Vector3 vPos = new Vector3(0, 10000, 0);
                for (int i = 0; i < Trails.Length; i++)
                {
                    //Trails[i].Move(vPos, Vector3.Left);
                    Trails[i].Freeze();
                }
            }

           // x = a sin(nt + c)
            //y = b sin(t) 
            //fElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds*5;
            //float fA = 16;
            //float fC = 14;
            //float fSum = fA + 6;
            //float fDiv = (fA / fC + 1) * fElapsedTime;

            //vPos.X = ((fSum) * (float)(Math.Cos(fElapsedTime)) - fC * (float)Math.Cos(fDiv));
            //vPos.Z = -10;
            //vPos.Y = ((fSum) * (float)(Math.Sin(fElapsedTime)) - fC * (float)Math.Sin(fDiv));


            //Trails[7].Move(vPos, Vector3.Left);
            //Trails[8].Move(-vPos, Vector3.Left);











            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(BGColor));

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
