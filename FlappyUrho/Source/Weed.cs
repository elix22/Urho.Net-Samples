using Urho;
using Urho.Physics;
using System;
using System.Collections.Generic;

namespace FlappyUrho
{
    public class Weed : Component
    {

        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);

            var cache = Application.ResourceCache;

            AnimatedModel animatedModel = Node.CreateComponent<AnimatedModel>();
            animatedModel.Model = cache.GetModel("Models/Weed.mdl");
            animatedModel.Material = cache.GetMaterial("Materials/VCol.xml").Clone();
            animatedModel.Material.SetShaderParameter("MatDiffColor", new Color(Randoms.Next(0.8f, 1.0f), Randoms.Next(0.8f, 1.0f), Randoms.Next(0.8f, 1.0f)));
            animatedModel.CastShadows = false;

            AnimationController animCtrl = Node.CreateComponent<AnimationController>();
            animCtrl.PlayExclusive("Models/Wave.ani", 1, true);
            animCtrl.SetAnimationTime(Randoms.Next(animCtrl.GetLength("Models/Wave.ani")));
            animCtrl.SetSpeed("Models/Wave.ani", Randoms.Next(0.42f, 0.666f));

        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (Global.gameState == GameState.GS_DEAD)
                return;

            Vector3 pos = Node.Position;
            pos += Vector3.Left * timeStep * Global.BAR_SPEED;

            if (pos.X < -Global.BAR_OUTSIDE_X && !Node.GetComponent<AnimatedModel>().InView)
            {
                pos.X += Global.NUM_BARRIERS * Global.BAR_INTERVAL + pos.Z;
                pos.Y = Global.WEED_RANDOM_Y;

                Node.Rotation = new Quaternion(0.0f, Randoms.Next(360.0f), 0.0f);
                Node.Scale = new Vector3(Randoms.Next(0.5f, 1.23f), Randoms.Next(0.8f, 2.3f), Randoms.Next(0.5f, 1.23f));
            }

            Node.Position = pos;

        }

    }


}