using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK;

namespace AppWorms
{

    public partial class FormMain : Form
    {
        private GameBase appMain;
        private GLControl glControl;

        #region --- Constructor ---

        public FormMain()
        {
            InitializeComponent();
        }

        #endregion

        #region OnLoad

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            appMain = new GameBase();

            glControl.KeyDown += new KeyEventHandler(glControl_KeyDown);
            glControl.KeyUp += new KeyEventHandler(glControl_KeyUp);

            glControl.Resize+=new EventHandler(glControl_Resize);
            glControl.Paint +=new PaintEventHandler(glControl_Paint);
            glControl.MouseDown += new MouseEventHandler(glControl_MouseDown);
            glControl.MouseUp += new MouseEventHandler(glControl_MouseUp);
            glControl.MouseMove += new MouseEventHandler(glControl_MouseMove);
            glControl.MouseWheel += new MouseEventHandler(glControl_MouseWheel);


            Text="GPS";
            //Text =
            //    GL.GetString(StringName.Vendor) + " " +
            //    GL.GetString(StringName.Renderer) + " " +
            //    GL.GetString(StringName.Version);

            appMain.Initialize( glControl);


            Application.Idle += Application_Idle;

            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(glControl, EventArgs.Empty);

            UpdateUnity();
        }

        void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            appMain.MouseState.Z += e.Delta;
        }

        void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            appMain.MouseState.X = e.X;
            appMain.MouseState.Y = e.Y;
        }

        void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            appMain.MouseState.Y = e.X;
            appMain.MouseState.Y = e.Y;
            appMain.MouseState.buttons[0] = false;
        }

        void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            appMain.MouseState.X = e.X;
            appMain.MouseState.Y = e.Y;
            appMain.MouseState.buttons[0] = true;
        }

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                GrabScreenshot().Save("screenshot.png");
            }
        }

        #endregion

        #region OnClosing

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;

            base.OnClosing(e);
        }

        #endregion

        #region Application_Idle event

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                NoLoopRender();
            }
        }

        #endregion

        #region GLControl.Resize event handler

        void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

         
            appMain.OnResize(Width, Height);
           
        }

        #endregion

        #region GLControl.KeyDown event handler

        void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        #endregion

        #region GLControl.Paint event handler

        void glControl_Paint(object sender, PaintEventArgs e)
        {
            NoLoopRender();
        }

        #endregion

        #region private void Render()

        private void NoLoopRender()
        {
            appMain.Render();

            glControl.SwapBuffers();
        }

        #endregion



        #region private void GrabScreenshot()

        Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
            bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte,
                data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
        #endregion

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        public void UpdateUnity()
        {
            projectUserControl.UpdateUnity();
            assetsUserControl.UpdateUnity();

        }

        private void projectUserControl_OnActiveGameObjectChanged(object sender, EventArgs e)
        {
            flowLayoutPanel.Controls.Clear();
            UnityEngine.GameObject go = UnityEditor.Selection.activeGameObject;
            if (go != null)
            {
                foreach (UnityEngine.Component c in go.GetComponents())
                {
                    PropertyUserControl control = new PropertyUserControl(c);
                    control.Width = panelProperty.Width;
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                    flowLayoutPanel.Controls.Add(control);
                }
            }
        }
    }
}
