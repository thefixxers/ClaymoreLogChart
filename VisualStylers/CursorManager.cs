using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

//
//
// Based on: http://www.codeproject.com/Articles/33789/Custom-Mouse-Cursors-VB-NET
//
//


namespace ClaymoreLogChart.VisualStylers
{
    public class CursorManager
    {
        private readonly StringFormat sf = new StringFormat();

        private readonly Cursor CursorNone, CursorMove, CursorCopy;

        private static CursorManager _instance;
        public static CursorManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CursorManager();
                return _instance;
            }
        }

        private CursorManager()
        {
            //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\..\\..\\..\\Graphics\\";
            string path = Globals.CursorsFolder;
            CursorNone = new Cursor(System.IO.Path.Combine(path, "No.cur"));
            CursorMove = new Cursor(System.IO.Path.Combine(path, "Move.cur"));
            CursorCopy = new Cursor(System.IO.Path.Combine(path, "Copy.cur"));
        }




        [DllImport("user32.dll", EntryPoint = "CreateIconIndirect")]
        private static extern IntPtr CreateIconIndirect(IntPtr iconInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyIcon(IntPtr handle);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr obj);

        private IntPtr curPtr;

        #region -- Cursor Properties --

        private Cursor _gCursor = Cursors.Default;
        public Cursor Current
        {
            get { return _gCursor; }
            set { _gCursor = value; }
        }

        private Bitmap _gCursorImage;
        public Bitmap CursorImage
        {
            get { return _gCursorImage; }
            set { _gCursorImage = value; }
        }

        public enum Effect
        {
            No,
            Move,
            Copy
        }
        private Effect _gEffect = Effect.No;
        public Effect EffectMode
        {
            get { return _gEffect; }
            set
            {
                if (_gEffect != value)
                {
                    _gEffect = value;
                    MakeCursor();
                }
            }
        }

        public enum Scrolling
        {
            No,
            ScrollUp,
            ScrollDown,
            ScrollLeft,
            ScrollRight
        }
        private Scrolling _gScrolling = Scrolling.No;
        public Scrolling ScrollingMode
        {
            get { return _gScrolling; }
            set { _gScrolling = value; }
        }

        public enum DrawType
        {
            Text,
            Picture,
            Both
        }
        private DrawType _gType = DrawType.Text;
        public DrawType DrawMode
        {
            get { return _gType; }
            set { _gType = value; }
        }

        private bool _gBlackBitBack;
        public bool BlackBit
        {
            get { return _gBlackBitBack; }
            set { _gBlackBitBack = value; }
        }

        private bool _gBoxShadow = true;
        public bool DrawBoxShadow
        {
            get { return _gBoxShadow; }
            set { _gBoxShadow = value; }
        }

        private Point _gHotSpotPt = new Point(0, 0);
        private ContentAlignment _gHotSpot = ContentAlignment.MiddleCenter;
        public ContentAlignment HotSpot
        {
            get { return _gHotSpot; }
            set { _gHotSpot = value; }
        }

        #endregion

        #region -- Image Properties --

        private Bitmap _gImage;
        public Bitmap Image
        {
            get { return _gImage; }
            set { if (value != null) _gImage = value; }
        }

        private Size _gImageBox = new Size(75, 56);
        public Size ImageBoxSize
        {
            get { return _gImageBox; }
            set { _gImageBox = value; }
        }

        private bool _gShowImageBox;
        public bool DrawImageBox
        {
            get { return _gShowImageBox; }
            set { _gShowImageBox = value; }
        }

        private Color _gImageBoxColor = Color.White;
        public Color ImageBoxColor
        {
            get { return _gImageBoxColor; }
            set { _gImageBoxColor = value; }
        }

        private Color _gImageBorderColor = Color.Black;
        public Color ImageBorderColor
        {
            get { return _gImageBorderColor; }
            set { _gImageBorderColor = value; }
        }

        private int _gImageTransp = 255;
        private int _gITransp;
        public int ImageTransparency
        {
            get { return _gITransp; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                _gITransp = value;
                _gImageTransp = (int)(0.01f * (100 - value) * 255);
            }
        }

        private int _gImageBoxTransp = 255;
        private int _gIBTransp = 80;
        public int ImageBackgroundTransparency
        {
            get { return _gIBTransp; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                _gIBTransp = value;
                _gImageBoxTransp = (int)(0.01f * (100 - value) * 255);
            }
        }

        #endregion

        #region -- Text Properties --

        private Size _gTextBoxArea = new Size(100, 30);
        private Size _gTextBox = new Size(100, 30);
        public Size TextBoxSize
        {
            get { return _gTextBox; }
            set { _gTextBox = value; }
        }

        private int _gTextTransp = 255;
        private int _gTTransp;
        public int TextTransparency
        {
            get { return _gTTransp; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                _gTTransp = value;
                _gTextTransp = (int)(0.01f * (100 - value) * 255);
            }
        }

        private int _gTextBoxTransp = 255;
        private int _gTBTransp = 80;
        public int TextBackgroundTransparency
        {
            get { return _gTBTransp; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                _gTBTransp = value;
                _gTextBoxTransp = (int)(0.01f * (100 - value) * 255);
            }
        }

        private bool _gShowTextBox;
        public bool DrawTextBox
        {
            get { return _gShowTextBox; }
            set { _gShowTextBox = value; }
        }

        private bool _gTextMultiline;
        public bool TextMultiline
        {
            get { return _gTextMultiline; }
            set { _gTextMultiline = value; }
        }

        public enum TextAutoFit
        {
            None,
            Width,
            Height,
            All
        }
        private TextAutoFit _gTextAutoFit = TextAutoFit.None;
        public TextAutoFit AutoFit
        {
            get { return _gTextAutoFit; }
            set { _gTextAutoFit = value; }
        }

        private string _gText = "";
        public string Text
        {
            get { return _gText; }
            set { _gText = value; }
        }

        private Color _gTextColor = Color.Blue;
        public Color TextColor
        {
            get { return _gTextColor; }
            set { _gTextColor = value; }
        }

        private Color _gTextShadowColor = Color.Black;
        public Color TextShadowColor
        {
            get { return _gTextShadowColor; }
            set { _gTextShadowColor = value; }
        }

        private Color _gTextBoxColor = Color.Blue;
        public Color TextBoxColor
        {
            get { return _gTextBoxColor; }
            set { _gTextBoxColor = value; }
        }

        private Color _gTextBorderColor = Color.Red;
        public Color TextBorderColor
        {
            get { return _gTextBorderColor; }
            set { _gTextBorderColor = value; }
        }

        private ContentAlignment _gTextAlignment = ContentAlignment.TopCenter;
        public ContentAlignment TextAlignment
        {
            get { return _gTextAlignment; }
            set
            {
                _gTextAlignment = value;
                switch (value)
                {
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                }
                switch (value)
                {
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.TopRight:
                        sf.Alignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.TopCenter:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.TopLeft:
                        sf.Alignment = StringAlignment.Near;
                        break;
                }
            }
        }

        public enum FadeMode
        {
            Solid,
            Linear,
            Path
        }
        private FadeMode _gTextFade = FadeMode.Solid;
        public FadeMode TextFadeMode
        {
            get { return _gTextFade; }
            set { _gTextFade = value; }
        }

        private FadeMode _gTextBoxFade = FadeMode.Solid;
        public FadeMode TextBoxFadeMode
        {
            get { return _gTextBoxFade; }
            set { _gTextBoxFade = value; }
        }

        private Font _gFont = new Font("Arial", 10, FontStyle.Bold);
        public Font Font
        {
            get { return _gFont; }
            set { _gFont = value; }
        }

        #endregion

        public Cursor CreateCursor(Bitmap bmp)
        {
            if (_gCursorImage != null)
                _gCursorImage.Dispose();

            if (curPtr != IntPtr.Zero)
                DestroyIcon(curPtr);

            IconInfo tmp = new IconInfo();
            tmp.xHotSpot = _gHotSpotPt.X;
            tmp.yHotSpot = _gHotSpotPt.Y;
            tmp.fIcon = false;

            if (_gBlackBitBack)
            {
                tmp.hbmMask = bmp.GetHbitmap(Color.FromArgb(0, 0, 0, 0));
                tmp.hbmColor = bmp.GetHbitmap(Color.FromArgb(0, 0, 0, 0));
            }
            else
            {
                tmp.hbmMask = bmp.GetHbitmap();
                tmp.hbmColor = bmp.GetHbitmap();
            }

            IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(tmp));
            Marshal.StructureToPtr(tmp, pnt, true);
            curPtr = CreateIconIndirect(pnt);

            _gCursorImage = Icon.FromHandle(curPtr).ToBitmap();

            if (pnt != IntPtr.Zero)
                DestroyIcon(pnt);
            pnt = IntPtr.Zero;

            if (tmp.hbmMask != IntPtr.Zero)
                DeleteObject(tmp.hbmMask);
            if (tmp.hbmColor != IntPtr.Zero)
                DeleteObject(tmp.hbmColor);

            return new Cursor(curPtr);
        }

        #region -- Building Cursor --
        public void MakeTextCursor(string text, Font font)
        {
            this.Text = text;
            this.Font = font;
            Size s = TextRenderer.MeasureText(this.Text, this.Font);
            this.DrawBoxShadow = false;
            this.DrawMode = DrawType.Text;
            this.AutoFit = TextAutoFit.Height;
            s.Width += 30;
            this.TextBoxSize = s;
            this.TextMultiline = true;
            this.HotSpot = ContentAlignment.TopLeft;
            this.DrawTextBox = true;
            this.TextBoxFadeMode = FadeMode.Solid;
            //this.Image = (Bitmap) imageList1.Images[1];
            this.TextAlignment = ContentAlignment.MiddleCenter;
            this.TextColor = Color.White;
            this.TextBoxColor = Color.FromArgb(0, 0, 122, 204);
            this.TextBorderColor = Color.Transparent;
            this.TextBackgroundTransparency = 60;
            this.TextFadeMode = FadeMode.Solid;
            this.MakeCursor();
        }

        public void MakeCursor(bool addEffect = true)
        {
            //Set the TextBox Size
            SetTextBox();

            //Set the size of the gCursor
            int cWidth = 0;
            int cHeight = 0;
            switch (_gType)
            {
                case DrawType.Picture:
                    cWidth = (int)(_gImageBox.Width + 5);
                    cHeight = (int)(_gImageBox.Height + 5);
                    break;
                case DrawType.Text:
                    cWidth = (int)(_gTextBoxArea.Width + 6);
                    cHeight = (int)(_gTextBoxArea.Height + 6);
                    break;
                case DrawType.Both:
                    cWidth = (int)(_gImageBox.Width + _gTextBoxArea.Width + 16);
                    cHeight = (int)(Math.Max(_gImageBox.Height + 6, _gTextBoxArea.Height + 6));
                    break;
            }

            //Set the Position of the gCursor HotSpot
            SetHotSpot(cWidth, cHeight);

            //Draw the gCursor
            Bitmap bm = new Bitmap(cWidth + 32, cHeight + 32, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bm))
            {
                switch (_gType)
                {
                    case DrawType.Picture:
                        //Draw the Image Box Shadow 
                        if (_gBoxShadow && _gShowImageBox)
                            AddShadow(g, new Point(0, 0), new Size(cWidth, cHeight), false);
                        //Draw the Picture
                        DrawImageFunc(g, (int)(_gImageBox.Width), (int)(_gImageBox.Height));
                        break;
                    case DrawType.Text:
                        //Draw the Text Box
                        if (_gShowTextBox)
                        {
                            //Draw the Text Box Shadow 
                            if (_gBoxShadow)
                                AddShadow(g, new Point(0, 0), _gTextBoxArea);
                            //Draw the Text Box
                            DrawTextBoxFunc(g);
                        }
                        //Draw the Text String
                        DrawTextFunc(g);
                        break;
                    case DrawType.Both:
                        //Draw the Image Box Shadow 
                        if (_gBoxShadow && _gShowImageBox)
                            AddShadow(g, new Point(0, 0), new Size((int)(_gImageBox.Width + 5), (int)(_gImageBox.Height + 4)), false);
                        //Draw the Picture
                        DrawImageFunc(g, (int)(_gImageBox.Width), (int)(_gImageBox.Height));
                        //Draw the Text Box
                        if (_gShowTextBox)
                        {
                            //Draw the Text Box Shadow 
                            if (_gBoxShadow)
                                AddShadow(g, new Point(_gImageBox.Width + 10, 0), _gTextBoxArea);
                            //Draw the Text Box
                            DrawTextBoxFunc(g, (int)(_gImageBox.Width + 10), 0);
                        }
                        //Draw the Text String
                        DrawTextFunc(g, (int)(_gImageBox.Width + 10), 0);
                        break;
                }

                //Draw the whole thing to the gCursor
                g.DrawImage(bm, 0, 0);

                //Add the image of the Effect Cursor to the gCursor Image
                if (addEffect)
                {
                    Cursor EffectCursor = Cursors.Default;
                    switch (ScrollingMode)
                    {
                        case Scrolling.No:
                            switch (_gEffect)
                            {
                                case Effect.No:
                                    EffectCursor = CursorNone; break;
                                case Effect.Move:
                                    EffectCursor = CursorMove; break;
                                case Effect.Copy:
                                    EffectCursor = CursorCopy; break;
                            }
                            break;
                        case Scrolling.ScrollDown:
                            EffectCursor = Cursors.PanSouth; break;
                        case Scrolling.ScrollUp:
                            EffectCursor = Cursors.PanNorth; break;
                        case Scrolling.ScrollLeft:
                            EffectCursor = Cursors.PanWest; break;
                        case Scrolling.ScrollRight:
                            EffectCursor = Cursors.PanEast; break;
                    }
                    EffectCursor.Draw(g, new Rectangle(_gHotSpotPt.X, _gHotSpotPt.Y, EffectCursor.Size.Width, EffectCursor.Size.Height));
                }
                //Create the new Cursor
                Current = CreateCursor(bm);
            }
            bm.Dispose();
        }

        private void SetTextBox()
        {
            Bitmap bm = new Bitmap(1000, 1000);
            sf.Trimming = StringTrimming.EllipsisCharacter;
            if (!_gTextMultiline)
                sf.FormatFlags = StringFormatFlags.NoWrap;
            else
                sf.FormatFlags = 0;

            using (Graphics g = Graphics.FromImage(bm))
            {
                int gHeight;
                if (_gTextMultiline)
                    if (_gTextAutoFit == TextAutoFit.Height)
                        gHeight = (int)(g.MeasureString(_gText, Font, (int)(_gTextBox.Width)).Height);
                    else
                        gHeight = (int)(g.MeasureString(_gText, Font).Height);
                else
                    gHeight = Font.Height;

                switch (_gTextAutoFit)
                {
                    case TextAutoFit.Height:
                        _gTextBoxArea = new Size(_gTextBox.Width, gHeight); break;
                    case TextAutoFit.Width:
                        _gTextBoxArea = new Size((int)(g.MeasureString(_gText, Font).Width + 1), _gTextBox.Height); break;
                    case TextAutoFit.All:
                        _gTextBoxArea = new Size((int)(g.MeasureString(_gText, Font).Width + 1), gHeight); break;
                    case TextAutoFit.None:
                        _gTextBoxArea = new Size(_gTextBox.Width, _gTextBox.Height); break;
                }

                //if (_gTextShadow){
                //    _gTextBoxArea.Width = (int)(_gTextBoxArea.Width + (_gTextShadower.Offset.X * _gTextShadower.Blur))
                //    _gTextBoxArea.Height = (int)(_gTextBoxArea.Height + (_gTextShadower.Offset.Y * _gTextShadower.Blur))
                //}
            }
            bm.Dispose();
        }

        public Point GetHotSpot()
        {
            return _gHotSpotPt;
        }

        public void SetHotSpot(int cWidth, int cHeight)
        {
            switch (_gHotSpot)
            {
                case ContentAlignment.BottomCenter:
                    _gHotSpotPt = new Point((int)(cWidth / 2), cHeight); break;
                case ContentAlignment.BottomLeft:
                    _gHotSpotPt = new Point(0, cHeight); break;
                case ContentAlignment.BottomRight:
                    _gHotSpotPt = new Point(cWidth, cHeight); break;
                case ContentAlignment.MiddleCenter:
                    _gHotSpotPt = new Point((int)(cWidth / 2), (int)(cHeight / 2)); break;
                case ContentAlignment.MiddleLeft:
                    _gHotSpotPt = new Point(0, (int)(cHeight / 2)); break;
                case ContentAlignment.MiddleRight:
                    _gHotSpotPt = new Point(cWidth, (int)(cHeight / 2)); break;
                case ContentAlignment.TopCenter:
                    _gHotSpotPt = new Point((int)(cWidth / 2), 0); break;
                case ContentAlignment.TopLeft:
                    _gHotSpotPt = new Point(0, 0); break;
                case ContentAlignment.TopRight:
                    _gHotSpotPt = new Point(cWidth, 0); break;
            }
        }

        #endregion

        #region -- Drawing --

        private Bitmap ImageTransp()
        {
            Bitmap bm = new Bitmap(_gImage.Width, Image.Height);
            using (ImageAttributes ia = new ImageAttributes())
            {
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix33 = _gImageTransp / 255.0f;
                ia.SetColorMatrix(cm);
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.DrawImage(_gImage, new Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
                }
            }
            return bm;
        }

        private void DrawImageFunc(Graphics g, int cWidth, int cHeight)
        {
            if (_gShowImageBox)
                g.FillRectangle(new SolidBrush(Color.FromArgb(_gImageBoxTransp, ImageBoxColor)), 0, 0, cWidth + 4, cHeight + 4);

            if (_gImage != null)
                g.DrawImage(ImageTransp(), 2, 2, cWidth, cHeight);

            if (_gShowImageBox)
                g.DrawRectangle(new Pen(_gImageBorderColor), 0, 0, cWidth + 4, cHeight + 4);
        }

        private void DrawTextBoxFunc(Graphics g, int ptX = 0, int ptY = 0)
        {
            //using (Pen pn = new Pen(_gTextBorderColor))
            //{
            //    pn.Alignment = PenAlignment.Inset;
            //    g.FillRectangle(new SolidBrush(Color.FromArgb((int)(_gTextBoxTransp),TextBoxColor)),
            //                    new Rectangle(ptX, ptY, (int)(_gTextBoxArea.Width + 6), (int)(_gTextBoxArea.Height + 6)));
            //    g.DrawRectangle(new Pen(_gTextBorderColor),
            //                    new Rectangle(ptX, ptY, (int)(_gTextBoxArea.Width + 6), (int)(_gTextBoxArea.Height + 6)));
            //}
            Brush br;
            Color color = Color.FromArgb((int)_gTextBoxTransp, _gTextBoxColor);
            Rectangle drawAreaRect = new Rectangle(ptX, ptY, (int)(_gTextBoxArea.Width + 6), (int)(_gTextBoxArea.Height + 6));
            if (_gTextBoxFade == FadeMode.Linear)
            {
                br = new LinearGradientBrush(drawAreaRect, color, Color.Transparent, LinearGradientMode.Horizontal);
            }
            else if (_gTextBoxFade == FadeMode.Path)
            {
                GraphicsPath gp = new GraphicsPath();

                drawAreaRect.Inflate(8, 8);
                //gp.AddEllipse(drawAreaRect);
                gp.AddRectangle(drawAreaRect);

                br = new PathGradientBrush(gp);
                (br as PathGradientBrush).CenterColor = color;
                (br as PathGradientBrush).SurroundColors = new Color[] { Color.Transparent };
                gp.Dispose();
            }
            else
            {
                br = new SolidBrush(color);
            }

            g.FillRectangle(br, drawAreaRect);
            g.DrawRectangle(new Pen(_gTextBorderColor), drawAreaRect);
        }

        private void DrawTextFunc(Graphics g, int ptX = 0, int ptY = 0)
        {
            //Setup Text Brushes
            Brush br = new SolidBrush(Color.FromArgb((int)(_gTextTransp), TextColor));
            //Brush brs = new SolidBrush(Color.FromArgb(_gTextShadower.ShadowTransp,gTextShadowColor));
            if (_gTextFade == FadeMode.Linear)
            {
                br = new LinearGradientBrush(
                                new Rectangle(ptX + 3, ptY + 3, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)),
                               TextColor, Color.Transparent, LinearGradientMode.Horizontal);
                //brs = new LinearGradientBrush(
                //                new Rectangle(0, 0, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)),
                //                Color.FromArgb(_gTextShadower.ShadowTransp,gTextShadowColor),
                //                Color.Transparent, LinearGradientMode.Horizontal);
            }
            else if (_gTextFade == FadeMode.Path)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(new Rectangle(ptX + 3, ptY + 3, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)));
                br = new PathGradientBrush(gp);
                (br as PathGradientBrush).CenterColor = TextColor;
                (br as PathGradientBrush).SurroundColors = new Color[] { Color.Transparent };

                gp.Reset();
                gp.AddRectangle(new Rectangle(0, 0, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)));
                //brs = new PathGradientBrush(gp);
                //(brs as PathGradientBrush).CenterColor = Color.FromArgb(_gTextShadower.ShadowTransp,gTextShadowColor);
                //(brs as PathGradientBrush).SurroundColors = new Color() {Color.Transparent};
                gp.Dispose();
            }

            //if (_gTextShadow) 
            //{
            //    //If shadow is requested setup and use the TextShadower
            //        throw new Exception("Not Implemented!");
            //    //WithgTextShadower
            //    //    .Font =gFont
            //    //    .TextBrush = br
            //    //    .ShadowBrush = brs
            //    //    .Alignment =gTextAlignment
            //    //    .ShadowTheText(g, New Rectangle(ptX + 3, ptY + 3, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)),gText)
            //    //End With
            //        }
            //else
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(_gText, Font, br, new Rectangle(ptX + 3, ptY + 3, (int)(_gTextBoxArea.Width), (int)(_gTextBoxArea.Height + 3)), sf);
            }

            br.Dispose();
            //brs.Dispose();
        }

        private void AddShadow(Graphics g, Point ShadowPt, Size BoxToShadow, bool UseTextBuffer = true)
        {
            LinearGradientBrush br;
            GraphicsPath gp = new GraphicsPath();
            Point[] pts;
            Size shadowsz;
            if (UseTextBuffer)
                shadowsz = Size.Add(BoxToShadow, new Size(7, 7));
            else
                shadowsz = BoxToShadow;

            Color ShadowColor = Color.Black;
            pts = new Point[] {
                    new Point(ShadowPt.X + shadowsz.Width, ShadowPt.Y + 5),
                    new Point(ShadowPt.X + shadowsz.Width + 5, ShadowPt.Y + 5),
                    new Point(ShadowPt.X + shadowsz.Width + 5, ShadowPt.Y + shadowsz.Height + 5),
                    new Point(ShadowPt.X + shadowsz.Width, ShadowPt.Y + shadowsz.Height)};
            gp.AddLines(pts);
            br = new LinearGradientBrush(new RectangleF((ShadowPt.X + shadowsz.Width - 1), (ShadowPt.Y + 5),
                        6, (shadowsz.Height)), ShadowColor, Color.Transparent, LinearGradientMode.Horizontal);
            g.FillPath(br, gp);
            gp.Reset();
            pts = new Point[] {
                    new Point(ShadowPt.X + 5, ShadowPt.Y + shadowsz.Height),
                    new Point(ShadowPt.X + shadowsz.Width, ShadowPt.Y + shadowsz.Height),
                    new Point(ShadowPt.X + shadowsz.Width + 5, ShadowPt.Y + shadowsz.Height + 5),
                    new Point(ShadowPt.X + 5, ShadowPt.Y + shadowsz.Height + 5)};
            gp.AddLines(pts);
            br = new LinearGradientBrush(new RectangleF((ShadowPt.X + 5), (ShadowPt.Y + shadowsz.Height + 5),
                        (shadowsz.Width), 6), ShadowColor, Color.Transparent, LinearGradientMode.Vertical);
            g.FillPath(br, gp);
            br.Dispose();
            gp.Dispose();
        }

        #endregion

        private struct IconInfo
        {
            public bool fIcon;
            public Int32 xHotSpot;
            public Int32 yHotSpot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }
    }
}
