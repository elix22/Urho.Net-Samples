using Urho;
using Urho.Physics;
using System;
using System.Collections.Generic;

namespace FlappyUrho
{
    public class Barrier : Component
    {
        static StaticModelGroup netGroup = null;
        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);

            var cache = Application.ResourceCache;

            Node.Rotation = (new Quaternion((Randoms.Next(2) > 0) ? 180.0f : 0.0f,
                                  (Randoms.Next(2) > 0) ? 180.0f + Randoms.Next(-5.0f, 5.0f) : 0.0f + Randoms.Next(-5.0f, 5.0f),
                                  (Randoms.Next(2) > 0) ? 180.0f + Randoms.Next(-5.0f, 5.0f) : 0.0f + Randoms.Next(-5.0f, 5.0f)));

            Node.CreateComponent<RigidBody>();
            CollisionShape shape = Node.CreateComponent<CollisionShape>();
            shape.ShapeType = (ShapeType.Box);
            shape.Size = new Vector3(1.0f, Global.BAR_GAP, 7.8f);

            Node netNode = Node.CreateChild("Net");

            if (netGroup == null)
            {
                netGroup = Node.Scene.CreateComponent<StaticModelGroup>();
                netGroup.Model = cache.GetModel("Models/Net.mdl");
                netGroup.CastShadows = true;
                netGroup.ApplyMaterialList();
            }

            netGroup.AddInstanceNode(netNode);

            foreach (float y in new List<float> { 15.0f, -15.0f })
            {
                netNode.CreateComponent<RigidBody>();
                shape = netNode.CreateComponent<CollisionShape>();
                shape.ShapeType = (ShapeType.Box);
                shape.Size = new Vector3(0.23f, 30.0f, 64.0f);
                shape.Position = new Vector3(0.0f, y + Math.Sign(y) * (Global.BAR_GAP / 2), 0.0f);
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
            if (Global.gameState != GameState.GS_PLAY)
                return;

            Vector3 pos = Node.Position;
            pos += Vector3.Left * timeStep * Global.BAR_SPEED;

            if (pos.X < -Global.BAR_OUTSIDE_X)
            {
                pos.X += Global.NUM_BARRIERS * Global.BAR_INTERVAL;
                pos.Y = Global.BAR_RANDOM_Y;

                Node.Rotation = (new Quaternion((Randoms.Next(2) > 0) ? 180.0f : 0.0f,
                                              (Randoms.Next(2) > 0) ? 180.0f + Randoms.Next(-5.0f, 5.0f) : 0.0f + Randoms.Next(-5.0f, 5.0f),
                                              (Randoms.Next(2) > 0) ? 180.0f + Randoms.Next(-5.0f, 5.0f) : 0.0f + Randoms.Next(-5.0f, 5.0f)));
            }

            Node.Position = pos;

        }

    }

}