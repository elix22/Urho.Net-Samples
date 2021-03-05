using Urho;
using Urho.Physics;
using System;
using System.Collections.Generic;

namespace FlappyUrho
{
    public class Crown : Component
    {
        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);

            var cache = Application.ResourceCache;

            Node.Position = Vector3.Right * 19.0f;

            Node.Rotation = new Quaternion(23.5f, Vector3.Forward);

            StaticModel crownModel = Node.CreateComponent<StaticModel>();
            crownModel.Model = cache.GetModel("Models/Crown.mdl");
            crownModel.CastShadows = true;
            crownModel.ApplyMaterialList();

            Vector3 v = Vector3.Normalize(Vector3.Left);

        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (Global.Score > Global.Highscore)
            {

                Node.Position = Vector3.Lerp(Node.Position, Global.CAMERA_DEFAULT_POS, Math.Clamp(2.0f * timeStep, 0.0f, 1.0f));
                Node.Rotation = Quaternion.Slerp(Node.Rotation, new Quaternion(90.0f, Vector3.Right), Math.Clamp(3.0f * timeStep, 0.0f, 1.0f));
                Node.Rotate(new Quaternion(235.0f * timeStep, Vector3.Up), TransformSpace.Local);

            }
            else
            {

                Node.Rotate(new Quaternion(timeStep * 23.0f, Vector3.Up), TransformSpace.World);
                Node.Rotate(new Quaternion(timeStep * 23.0f, Vector3.Up), TransformSpace.Local);

                float x = 2.3f + ((Global.Highscore == 0) ? 1.0f : 0.0f) + 25.0f * (Global.Highscore - Global.Score) / (float)Math.Max(Global.Highscore, 1);
                float y = Node.Scene.GetChild("Urho").Position.Y - Node.Position.Y;
                Vector3 targetPos = new Vector3(x, y, Node.Position.Z);

                Node.Position = new Vector3(0.01f * (targetPos + 99.0f * Node.Position));
            }

        }

        public void Reset()
        {
            Node.Position = Vector3.Right * 19.0f;
            Node.Rotation = new Quaternion(23.5f, Vector3.Forward);
        }

    }

}