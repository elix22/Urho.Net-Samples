using Urho;
using Urho.Physics;
using System;
using System.Collections.Generic;


namespace FlappyUrho
{

    public class FlappyCam : Component
    {
        RenderPath effectRenderPath;
        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);
            var renderer = Application.Renderer;
            var cache = Application.ResourceCache;

            Node.Position = Global.CAMERA_DEFAULT_POS;

            Camera camera = Node.CreateComponent<Camera>();
            Viewport viewport = new Viewport(Application.Context, Scene, camera);
            renderer.SetViewport(0, viewport);

            effectRenderPath = viewport.RenderPath;
            effectRenderPath.Append(cache.GetXmlFile("PostProcess/BloomHDR.xml"));
            effectRenderPath.SetShaderParameter("BloomHDRThreshold", 0.8f);
            effectRenderPath.SetShaderParameter("BloomHDRMix", new Vector2(0.7f, 0.8f));
            effectRenderPath.SetEnabled("BloomHDR", true);

        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            effectRenderPath.SetShaderParameter("ElapsedTime", Application.Time.ElapsedTime * Scene.TimeScale);
        }

    }

}