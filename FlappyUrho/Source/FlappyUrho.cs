using Urho;
using Urho.Audio;
using Urho.Resources;
using Urho.Physics;
using Urho.Gui;
using System;

namespace FlappyUrho
{
    public class FlappyUrho : Application
    {
        Scene scene;

        [Preserve]
        public FlappyUrho() : base(new ApplicationOptions(assetsFolder: "Data/FlappyUrho;Data;CoreData")) { }

        protected override void Start()
        {
            base.Start();

            var cache = ResourceCache;

            CreateScene();
            CreateUI();

            Time.FrameStarted += HandleBeginFrame;

            SoundSource musicSource = scene.GetOrCreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            Sound music = cache.GetSound("Music/Urho - Disciples of Urho_LOOP.ogg");
            music.Looped = true;
            musicSource.Play(music);

            Audio.SetMasterGain(SoundType.Music.ToString(), 0.33f);


        }

        void CreateScene()
        {
            scene = new Scene();


            scene.CreateComponent<Octree>();
            scene.CreateComponent<PhysicsWorld>();


            // Create a scene node for the camera, which we will move around
            // The camera will use default settings (1000 far clip distance, 45 degrees FOV, set aspect ratio automatically)
            Node cameraNode = scene.CreateChild("camera");
            cameraNode.CreateComponent<FlappyCam>();


            Zone zone = cameraNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-100.0f * Vector3.One, 100.0f * Vector3.One));
            zone.FogStart = 34.0f;
            zone.FogEnd = 62.0f;
            zone.FogHeight = -19.0f;
            zone.HeightFog = true;
            zone.FogHeightScale = 0.1f;
            zone.FogColor = new Color(0.05f, 0.23f, 0.23f);
            zone.AmbientColor = new Color(0.05f, 0.13f, 0.13f);


            var lightNode = scene.CreateChild("DirectionalLight");
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.CastShadows = true;
            light.ShadowIntensity = 0.23f;
            light.Brightness = 1.23f;
            light.Color = new Color(0.8f, 1.0f, 1.0f);
            lightNode.SetDirection(new Vector3(-0.5f, -1.0f, 1.0f));




            var envNode = scene.CreateChild("Environment");

            var skybox = envNode.CreateComponent<Skybox>();
            skybox.Model = ResourceCache.GetModel("Models/Box.mdl");
            skybox.SetMaterial(ResourceCache.GetMaterial("Materials/Env.xml"));
            skybox.SetZone(zone);
            envNode.CreateComponent<Environment>();


            CreateUrho();
            CreateNets();
            CreateWeeds();
            CreateCrown();
        }

        void CreateUrho()
        {
            Node urhoNode = scene.CreateChild("Urho");
            urhoNode.CreateComponent<Fish>();
        }

        void CreateNets()
        {
            for (int i = 0; i < Global.NUM_BARRIERS; ++i)
            {
                Node barrierNode = scene.CreateChild("Barrier");
                barrierNode.CreateComponent<Barrier>();
                barrierNode.Position = new Vector3(Global.BAR_OUTSIDE_X * 1.23f + i * Global.BAR_INTERVAL, Global.BAR_RANDOM_Y, 0.0f);
            }
        }

        void CreateWeeds()
        {
            for (int r = 0; r < 3; ++r)
            {
                for (int i = 0; i < Global.NUM_WEEDS; ++i)
                {
                    Node weedNode = scene.CreateChild("Weed");
                    weedNode.CreateComponent<Weed>();
                    weedNode.Position = new Vector3(i * Global.BAR_INTERVAL * Randoms.Next(0.1f, 0.23f) - 23.0f,
                                                  Global.WEED_RANDOM_Y,
                                                  Randoms.Next(-27.0f + r * 34.0f, -13.0f + r * 42.0f));

                    weedNode.Rotation = new Quaternion(0.0f, Randoms.Next(360.0f), 0.0f);
                    weedNode.Scale = new Vector3(Randoms.Next(0.5f, 1.23f), Randoms.Next(0.8f, 2.3f), Randoms.Next(0.5f, 1.23f));
                }
            }
        }

        void CreateCrown()
        {
            Node crownNode = scene.CreateChild("Crown");
            crownNode.CreateComponent<Crown>();
        }

        void CreateUI()
        {
            Node scoreNode = scene.CreateChild("Score");
            Node highscoreNode = scene.CreateChild("Highscore");
            Global.SetScores3D(scoreNode.CreateComponent<Score3D>(),
                                highscoreNode.CreateComponent<Score3D>());
        }


        void HandleBeginFrame(FrameStartedEventArgs arg)
        {
            if (Global.gameState == Global.neededGameState)
                return;

            var cache = ResourceCache;

            if (Global.neededGameState == GameState.GS_DEAD)
            {

                Node urhoNode = scene.GetChild("Urho");
                SoundSource soundSource = urhoNode.GetOrCreateComponent<SoundSource>();
                soundSource.Play(cache.GetSound("Samples/Hit.ogg"));

            }
            else if (Global.neededGameState == GameState.GS_INTRO)
            {

                Node urhoNode = scene.GetChild("Urho");
                Fish fish = urhoNode.GetComponent<Fish>();
                fish.Reset();

                Node crownNode = scene.GetChild("Crown");
                Crown crown = crownNode.GetComponent<Crown>();
                crown.Reset();

                if (Global.Score > Global.Highscore)
                    Global.Highscore = Global.Score;
                Global.Score = 0;
                Global.sinceLastReset = 0.0f;

                Node[] barriers = scene.GetChildrenWithComponent<Barrier>();
                foreach (Node b in barriers)
                {

                    Vector3 pos = b.Position;
                    pos.Y = Global.BAR_RANDOM_Y;

                    if (pos.X < Global.BAR_OUTSIDE_X)
                        pos.X += Global.NUM_BARRIERS * Global.BAR_INTERVAL;

                    b.Position = pos;
                }


                Node[] weeds = scene.GetChildrenWithComponent<Weed>();
                foreach (Node w in weeds)
                {

                    w.Remove();
                }
                CreateWeeds();
            }

            Global.gameState = Global.neededGameState;

            UpdateUIVisibility();
        }

        void UpdateUIVisibility()
        {


        }
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            var input = Input;

            if (Global.gameState == GameState.GS_PLAY)
            {

                Global.sinceLastReset += timeStep;
            }

            scene.TimeScale = (float)Math.Pow(1.0f + Math.Clamp(Global.sinceLastReset * 0.0023f, 0.0f, 1.0f), 2.3f);
            SoundSource musicSource = scene.GetComponent<SoundSource>();
            musicSource.Frequency = (float)(0.5f * (musicSource.Frequency + 44000.0f * Math.Sqrt(scene.TimeScale)));

            if (input.GetMouseButtonPress(MouseButton.Left) || (input.NumTouches > 0))
            {

                if (Global.gameState == GameState.GS_INTRO)
                    Global.neededGameState = GameState.GS_PLAY;
                else if (Global.gameState == GameState.GS_DEAD)
                    Global.neededGameState = GameState.GS_INTRO;
            }


            if (input.GetKeyPress(Key.Escape))
            {
                Exit();
            }

			Global.OnUpdate();

        }
    }
}