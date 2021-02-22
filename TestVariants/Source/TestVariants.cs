using Urho;
using System.Linq;
using System.Collections.Generic;
using Urho.Gui;
using Urho.Resources;

namespace TestVariants
{
    public class TestVariants : Sample
    {

        /// Strings printed so far.
        List<string> chatHistory = new List<string>();
        /// Chat text element.
        Text chatHistoryText;
        /// Button container element.
        UIElement buttonContainer;
        /// Server address / chat message line editor element.
        LineEdit textEdit;
        /// Send button.


        Button nextButton;

        static int testIndex = 0;


        public TestVariants() : base(new ApplicationOptions(assetsFolder: "Data;CoreData")) { }


        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            switch (testIndex)
            {
                case 0:
                    TestDynamic();
                    break;
                case 1:
                    TestDynamic2();
                    break;
                case 2:
                    DynamicMap map = TestDynamicMap();
                    ShowDynamicMap(ref map);
                    break;
            }


        }

        protected void OnPostRenderUpdate(PostRenderUpdateEventArgs args)
        {
            chatHistory.Clear();
            chatHistoryText.Value = "";
        }

        protected override void Start()
        {
            base.Start();
            Input.SetMouseVisible(true, false);
            CreateUI();
            SubscribeToEvents();

        }
        void CreateUI()
        {
            IsLogoVisible = false; // We need the full rendering window

            var graphics = Graphics;
            UIElement root = UI.Root;
            var cache = ResourceCache;
            XmlFile uiStyle = cache.GetXmlFile("UI/DefaultStyle.xml");
            // Set style to the UI root so that elements will inherit it
            root.SetDefaultStyle(uiStyle);

            Font font = cache.GetFont("Fonts/Anonymous Pro.ttf");
            chatHistoryText = new Text();
            chatHistoryText.SetFont(font, 24);
            root.AddChild(chatHistoryText);

            buttonContainer = new UIElement();
            root.AddChild(buttonContainer);
            buttonContainer.SetFixedSize(graphics.Width, 60);
            buttonContainer.SetPosition(0, graphics.Height - 60);
            buttonContainer.LayoutMode = LayoutMode.Horizontal;

            textEdit = new LineEdit();
            textEdit.SetStyleAuto(null);
            textEdit.TextElement.SetFont(font, 24);
            buttonContainer.AddChild(textEdit);

            nextButton = CreateButton("Next", 140);

            UpdateButtons();

            // No viewports or scene is defined. However, the default zone's fog color controls the fill color
            Renderer.DefaultZone.FogColor = new Color(0.0f, 0.0f, 0.1f);
        }

        void SubscribeToEvents()
        {

            textEdit.TextFinished += (args => { ShowChatText(args.Text); textEdit.Text = string.Empty; });


            nextButton.Released += (args => HandleNextTest());

            Engine.PostRenderUpdate += OnPostRenderUpdate;

        }

        Button CreateButton(string text, int width)
        {
            var cache = ResourceCache;
            Font font = cache.GetFont("Fonts/Anonymous Pro.ttf");

            Button button = new Button();
            buttonContainer.AddChild(button);
            button.SetStyleAuto(null);
            button.SetFixedHeight(60);
            button.SetFixedWidth(width);

            var buttonText = new Text();
            button.AddChild(buttonText);
            buttonText.SetFont(font, 24);
            buttonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);

            buttonText.Value = text;

            return button;
        }

        void ShowChatText(string row)
        {
            chatHistory.Add(row);
            string outText = string.Join("\n", chatHistory) + "\n";
            chatHistoryText.Value = outText;
        }

        void UpdateButtons()
        {
            nextButton.Visible = true;
        }


        void HandleNextTest()
        {
            testIndex = (++testIndex%3);
        }


        void TestDynamic()
        {
            var bytes_buffer = new byte[400000];
            for (int index = 0; index < bytes_buffer.Length; index++)
            {
                bytes_buffer[index] = (byte)(index % 255);
            }

            Dynamic v = bytes_buffer;

            byte[] buf2 = v;
            int sum = 0;
            for (int index = 0; index < buf2.Length; index++)
            {
                sum += buf2[index];

            }
            ShowChatText(ToString(sum));

            Dynamic b = true;
            ShowChatText(ToString((bool)b));

            Dynamic i = 4567;
            ShowChatText(ToString((int)i));

            Dynamic f = 765.543f;
            ShowChatText(ToString((float)f));

            Dynamic d = 76432455.98754553;
            ShowChatText(ToString((double)d));

            Dynamic dv2 = new Vector2(45.32f, 12.34f);
            Vector2 v2 = dv2;
            ShowChatText(v2.ToString());


            Dynamic div2 = new IntVector2(1234, 9876);
            IntVector2 iv2 = div2;
            ShowChatText(iv2.ToString());


            Dynamic dv3 = new Vector3(654.32f, 876.34f, 34.21f);
            Vector3 v3 = dv3;
            ShowChatText(v3.ToString());

            Dynamic div3 = new IntVector3(1234, 9876, 456);
            IntVector3 iv3 = div3;
            ShowChatText(iv3.ToString());


            Dynamic dv4 = new Vector4(231.65f, 21.34f, 123.65f, 543.765f);
            Vector4 v4 = dv4;
            ShowChatText(v4.ToString());

            Dynamic dq = new Quaternion(new Vector3(34.45f, 765.65f, 789.445653f), 0.5f);
            Quaternion q = dq;
            ShowChatText(q.ToString());


            Dynamic dc = new Color(0.4f, 0.2f, 1.0f, 0.3f);
            Color c = dc;
            ShowChatText(c.ToString());

            Dynamic dr = new Rect(1.1f, 2.2f, 3.3f, 4.4f);
            Rect r = dr;
            ShowChatText(r.ToString());

            Dynamic dir = new IntRect(1, 2, 3, 4);
            IntRect ir = dir;
            ShowChatText(ir.ToString());


            Dynamic ds = "Hello World from elix22";
            string s = ds;
            ShowChatText(s);





        }

        void TestDynamic2()
        {
            float[] farray3 = Enumerable.Range(0, 9).Select(ii => 1.0f * ii).ToArray();
            Dynamic dm3 = new Matrix3(farray3);
            Matrix3 m3 = dm3;
            ShowChatText(m3.ToString());


            Vector4 row0 = new Vector4(1, 2, 3, 4);
            Vector4 row1 = new Vector4(5, 6, 7, 8);
            Vector4 row2 = new Vector4(9, 10, 11, 12);
            Vector4 row3 = new Vector4(13, 14, 15, 16);
            Dynamic dm4 = new Matrix4(row0, row1, row2, row3);
            Matrix4 m4 = dm4;
            ShowChatText(m4.ToString());

            ShowChatText(" ");
            Vector3 col0 = new Vector3(1, 2, 3);
            Vector3 col1 = new Vector3(5, 6, 7);
            Vector3 col2 = new Vector3(9, 10, 11);
            Vector3 col3 = new Vector3(12, 13, 14);
            Dynamic dm34 = new Matrix3x4(col0, col1, col2, col3);
            Matrix3x4 m34 = dm34;
            ShowChatText(m34.ToString());
        }

        DynamicMap TestDynamicMap()
        {
            DynamicMap container = new DynamicMap();
            container["q"] = new Quaternion(new Vector3(34.45f, 765.65f, 789.445653f), 0.5f);
            container["v"] = 456.78f;
            container["b"] = true;
            container["d"] = 456.78;
            container["s"] = "Hello World from elix22";
            container["v2"] = new Vector2(45.67f, 87.65f);
            container["v3"] = new Vector3(45.67f, 87.65f, 768.543f);
            container["v4"] = new Vector4(45.67f, 87.65f, 768.543f, 789.5432f);
            container["c"] = new Color(0.3f, 0.4f, 0.2f, 1.0f);


            var bytes_buffer = new byte[100];
            for (int i = 0; i < bytes_buffer.Length; i++)
            {
                bytes_buffer[i] = (byte)(i % 255);
            }
            container["buffer"] = bytes_buffer;


            bytes_buffer = new byte[400000];
            for (int i = 0; i < bytes_buffer.Length; i++)
            {
                bytes_buffer[i] = (byte)(i % 255);
            }
            container["buffer2"] = bytes_buffer;

            //container["buffer"] = new byte[] { 45, 32, 36, 76, 12, 32, 128, 230, 12, 245 };
            return container;
        }

        void ShowDynamicMap(ref DynamicMap container)
        {
            Quaternion q2 = container["q"];
            ShowChatText(q2.ToString());

            float y = container["v"];
            ShowChatText($"{ToString(y)}");

            bool b1 = container["b"];
            ShowChatText(ToString(b1));

            double d1 = container["d"];
            ShowChatText(ToString(d1));

            string s1 = container["s"];
            ShowChatText(s1);

            Vector2 v22 = container["v2"];
            ShowChatText(v22.ToString());

            Vector3 v33 = container["v3"];
            ShowChatText(v33.ToString());

            Vector4 v44 = container["v4"];
            ShowChatText(v44.ToString());

            Color cc = container["c"];
            ShowChatText(cc.ToString());

            byte[] buf2 = container["buffer2"];
            int sum = 0;
            for (int i = 0; i < buf2.Length; i++)
            {
                sum += buf2[i];

            }
            ShowChatText(ToString(sum));

            byte[] buf1 = container["buffer"];
            ShowChatText(ToReadableByteArray(buf1));


            //string str2  = ToReadableByteArray(buf2);
        }


        public string ToReadableByteArray(byte[] bytes)
        {
            string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                str += ToString(bytes[i]);
                if (i != 0 && i % 20 == 0)
                {
                    str += "\n";
                }
                else str += ",";

            }

            return str;
        }
        protected override string JoystickLayoutPatch => JoystickLayoutPatches.Hidden;

    }
}