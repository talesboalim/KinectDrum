using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace KinDrum
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Variáveis usadas para o kinect
        KinectSensor kinect; //Detecta o sensor
        Skeleton[] skeletons; //Armazena todos os esqueletos detectados
        Skeleton p1; //Variável onde armazena os JOINTS do jogadorJogador

        //cenário
        SpriteFont font;
        int status=0;
        clsObjeto myMenuStick;

        //objeto
        clsObjeto myBaquetaDireita;
        clsObjeto myBaquetaEsquerda;

        clsObjeto myBateria;
        clsObjeto myBateriaPrato1;
        clsObjeto myBateriaPrato2;
        clsObjeto myBateriaPrato3;
        clsObjeto myBateriaCaixa1;
        clsObjeto myBateriaCaixa2;
        clsObjeto myBateriaCaixa3;
        clsObjeto myBateriaCaixa4;
        //sons

        SoundEffect myBateriaSomCaixa1, myBateriaSomCaixa2, myBateriaSomCaixa3, myBateriaSomCaixa4;
        SoundEffect myBateriaSomPrato1, myBateriaSomPrato2, myBateriaSomPrato3;

        //esqueleto
        bool bateuBateriaPrato1, bateuBateriaPrato2, bateuBateriaPrato3;
        bool bateuBateriaCaixa1, bateuBateriaCaixa2, bateuBateriaCaixa3, bateuBateriaCaixa4;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

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
            // TODO: Add your initialization logic here
            try //Inicializa o kinect
            {
                kinect = KinectSensor.KinectSensors[0]; //Busca o kinect conectado        
                kinect.SkeletonStream.Enable(); //Inicializa o esqueleto        
                kinect.Start(); //Inicia a comunicação com o kinect
                //Evento acionado quando mapeia os esqueletos
                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
            }
            catch { }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.4
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Carrega a font para escrita 
            font = Content.Load<SpriteFont>("gameFont");

            myMenuStick = new clsObjeto(this, Content.Load<Texture2D>("MenuStick"), new Vector2(335f, 300f), new Vector2(104f, 162f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //carrega os objetos da tela
            myBaquetaDireita = new clsObjeto(this, Content.Load<Texture2D>("baqueta"), new Vector2(50f, 50f), new Vector2(15f, 15f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBaquetaEsquerda = new clsObjeto(this, Content.Load<Texture2D>("baqueta"), new Vector2(50f, 500f), new Vector2(15f, 15f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            myBateria = new clsObjeto(this, Content.Load<Texture2D>("bateria1"), new Vector2(120f, 0f), new Vector2(800f, 650f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            myBateriaPrato1 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(150f, 140f), new Vector2(190f, 80f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBateriaPrato2 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(300f, 20f), new Vector2(180f, 70f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBateriaPrato3 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(720f, 50f), new Vector2(190f, 90f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            myBateriaCaixa1 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(320f, 270f), new Vector2(130f, 70f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBateriaCaixa2 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(390f, 140f), new Vector2(130f, 90f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBateriaCaixa3 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(560f, 140f), new Vector2(130f, 90f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            myBateriaCaixa4 = new clsObjeto(this, Content.Load<Texture2D>("prato"), new Vector2(650f, 250f), new Vector2(130f, 70f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            

            //sons
            this.myBateriaSomCaixa1 = this.Content.Load<SoundEffect>("bateriaCaixa1");
            this.myBateriaSomCaixa2 = this.Content.Load<SoundEffect>("bateriaCaixa2");
            this.myBateriaSomCaixa3 = this.Content.Load<SoundEffect>("bateriaCaixa3");
            this.myBateriaSomCaixa4 = this.Content.Load<SoundEffect>("bateriaCaixa4");
            this.myBateriaSomPrato1 = this.Content.Load<SoundEffect>("bateriaPrato1");
            this.myBateriaSomPrato2 = this.Content.Load<SoundEffect>("bateriaPrato2");
            this.myBateriaSomPrato3 = this.Content.Load<SoundEffect>("bateriaPrato3"); 

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (kinect != null)
            {
                kinect.Stop();
            }
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

            // TODO: Add your update logic here

            if (status == 0)
            {
                if (p1 != null)
                {
                    //Verifica se a posição foi feita e inicia o jogo
                    if (p1.Joints[JointType.HandRight].Position.Y > p1.Joints[JointType.Head].Position.Y)
                    {
                        status = 1;
                    }
                }
                
            }
            else if (status == 1)
            {

                //muda a posição da baqueta
                //myBaquetaDireita.position = new Vector2((float)(p1.Joints[JointType.HandRight].Position.X) * (float)1.5, p1.Joints[JointType.HandRight].Position.Y * (float)1.5);
                //myBaquetaEsquerda.position = new Vector2((float)(p1.Joints[JointType.HandLeft].Position.X) * (float)1.5, p1.Joints[JointType.HandLeft].Position.X * (float)1.5);

                Joint scaledJointHR = p1.Joints[JointType.HandRight].Scale(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                Vector2 positionHR = new Vector2(scaledJointHR.Position.X, scaledJointHR.Position.Y);
                myBaquetaDireita.position = positionHR;

                Joint scaledJointHL = p1.Joints[JointType.HandLeft].Scale(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                Vector2 positionHL = new Vector2(scaledJointHL.Position.X, scaledJointHL.Position.Y);
                myBaquetaEsquerda.position = positionHL;

                //spriteBatch.Draw(texJoint1, position, null, Color.Red, 0f, Vector2.Zero, 0.5f / scaledJoint.Position.Z, SpriteEffects.None, 0);


                //verifica se a baqueta bateu em um prato ou caixa            
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaPrato1) || myBaquetaDireita.ObjetoColisao(myBateriaPrato1))
                {
                    if (bateuBateriaPrato1 == false)
                    {
                        //tocar
                        myBateriaSomPrato1.Play();
                        bateuBateriaPrato1 = true;
                    }
                }
                else
                {
                    bateuBateriaPrato1 = false;
                }
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaPrato2) || myBaquetaDireita.ObjetoColisao(myBateriaPrato2))
                {
                    if (bateuBateriaPrato2 == false)
                    {
                        //tocar
                        myBateriaSomPrato2.Play();
                        bateuBateriaPrato2 = true;
                    }
                }
                else
                {
                    bateuBateriaPrato2 = false;
                }
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaPrato3) || myBaquetaDireita.ObjetoColisao(myBateriaPrato3))
                {
                    if (bateuBateriaPrato3 == false)
                    {
                        //tocar
                        myBateriaSomPrato3.Play();
                        bateuBateriaPrato3 = true;
                    }
                }
                else
                {
                    bateuBateriaPrato3 = false;
                }

                if (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa1) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa1))
                {
                    if (bateuBateriaCaixa1 == false)
                    {
                        //tocar
                        myBateriaSomCaixa1.Play();
                        bateuBateriaCaixa1 = true;
                    }
                }
                else
                {
                    bateuBateriaCaixa1 = false;
                }
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa2) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa2))
                {
                    if (bateuBateriaCaixa2 == false)
                    {
                        //tocar
                        myBateriaSomCaixa2.Play();
                        bateuBateriaCaixa2 = true;
                    }
                }
                else
                {
                    bateuBateriaCaixa2 = false;
                }
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa3) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa3))
                {
                    if (bateuBateriaCaixa3 == false)
                    {
                        //tocar
                        myBateriaSomCaixa3.Play();
                        bateuBateriaCaixa3 = true;
                    }
                }
                else
                {
                    bateuBateriaCaixa3 = false;
                }
                if (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa4) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa4))
                {
                    if (bateuBateriaCaixa4 == false)
                    {
                        //tocar
                        myBateriaSomCaixa4.Play();
                        bateuBateriaCaixa4 = true;
                    }
                }
                else
                {
                    bateuBateriaCaixa4 = false;
                }

                /*
                bateuBateriaPrato2 = (myBaquetaEsquerda.ObjetoColisao(myBateriaPrato2) || myBaquetaDireita.ObjetoColisao(myBateriaPrato2));
                bateuBateriaPrato3 = (myBaquetaEsquerda.ObjetoColisao(myBateriaPrato3) || myBaquetaDireita.ObjetoColisao(myBateriaPrato3));

                bateuBateriaCaixa1 = (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa1) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa1));
                bateuBateriaCaixa2 = (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa2) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa2));
                bateuBateriaCaixa3 = (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa3) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa3));
                bateuBateriaCaixa4 = (myBaquetaEsquerda.ObjetoColisao(myBateriaCaixa4) || myBaquetaDireita.ObjetoColisao(myBateriaCaixa4));
                */
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
            graphics.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (status == 0)//menu
            {
                spriteBatch.DrawString(font, "KinDrum", new Vector2(350, 50), Color.Yellow);

                if (p1 != null) 
                {
                    spriteBatch.DrawString(font, "Ola jogador", new Vector2(350, 100), Color.Red);
                }
                

                spriteBatch.DrawString(font, "Para iniciar faca a pose demonstrada abaixo e aguarde ate iniciar.", new Vector2(150, 200), Color.Turquoise);
                spriteBatch.DrawString(font, "Caso voce nao seja detectado, saia da frente do Kinect e volte a fazer a pose.", new Vector2(150, 220), Color.Turquoise);

                spriteBatch.DrawString(font, "-Objetivo: tocar bateria", new Vector2(100, 600), Color.White);
                spriteBatch.DrawString(font, "-Como jogar: use as 2 maos como baquetas", new Vector2(100, 630), Color.White);

                myMenuStick.Draw(spriteBatch);

                

            }
            else 
            {

                myBateriaPrato1.Draw(spriteBatch);
                myBateriaPrato2.Draw(spriteBatch);
                myBateriaPrato3.Draw(spriteBatch);

                myBateriaCaixa1.Draw(spriteBatch);
                myBateriaCaixa2.Draw(spriteBatch);
                myBateriaCaixa3.Draw(spriteBatch);
                myBateriaCaixa4.Draw(spriteBatch);
                myBateria.Draw(spriteBatch);

                //spriteBatch.DrawString(font, "P1:" + bateuBateriaPrato1 + " P2:" + bateuBateriaPrato2 + " P3:" + bateuBateriaPrato3 + " B1:" + bateuBateriaCaixa1 + " B2:" + bateuBateriaCaixa2 + " B3:" + bateuBateriaCaixa3 + " B4:" + bateuBateriaCaixa4, new Vector2(50, 700), Color.Black);
                /*
                if (bateuBateriaPrato1 == true)
                {
                    spriteBatch.DrawString(font, "bateu", new Vector2(50, 680), Color.Red);
                }
                */
                myBaquetaDireita.Draw(spriteBatch);
                myBaquetaEsquerda.Draw(spriteBatch);

                //spriteBatch.DrawString(font, "LX:" + (int)posLeftHandX + "  LY:" + (int)posLeftHandY + "  LZ:" + (int)posLeftHandZ + "  RX:" + (int)posRightHandX + "  RY:" + (int)posRightHandY + "  RZ:" + (int)posRightHandZ, new Vector2(50, 730), Color.White);

                spriteBatch.DrawString(font, "KinDrum", new Vector2(450, 670), Color.White);
                spriteBatch.DrawString(font, "Bata as bolinhas amarelas nos pratos ou nas caixas", new Vector2(50, 730), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if (skeletons == null)
                    {
                        skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                    p1 = skeletons.Where(s => s.TrackingState ==
                    SkeletonTrackingState.Tracked).FirstOrDefault();
                }
            }
        }


    }

    internal static class ExtensionMethods
    {
        public static Joint Scale(this Joint joint, int width, int height)
        {
            SkeletonPoint skeletonPoint = new SkeletonPoint()
            {
                X = Scale(joint.Position.X, width),
                Y = Scale(-joint.Position.Y, height),
                Z = joint.Position.Z
            };

            Joint scaledJoint = new Joint()
            {
                TrackingState = joint.TrackingState,
                Position = skeletonPoint
            };

            return scaledJoint;
        }

        public static float Scale(float value, int max)
        {
            return (max >> 1) + (value * (max >> 1));
        }
    }
}
