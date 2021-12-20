using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace amco
{
    [DefaultEvent("_TextChanged")]
    public partial class TonyTextbox : UserControl
    {
        //fields 
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private Color borderFocusColor = Color.Blue;
        private bool isFocused  = false;
        private int borderRadius = 0;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private bool isPasswordChar = false;

        //constructor
        public TonyTextbox()
        {
            InitializeComponent();
        }

        //Events
        public event EventHandler _TextChanged;

        //properties
        public Color BorderColor {
            get => borderColor;
            set {borderColor = value;
            this.Invalidate();}
        }
        public int BorderSize { 
            get => borderSize;
            set
            {
                borderSize = value;
                this.Invalidate();

            }
        }
        public bool UnderlinedStyle { 
            get => underlinedStyle;
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        
        }

        [Category("Tony Textbox")]
        public bool PasswordChar
        {
            get { return isPasswordChar; }
            set {
                isPasswordChar = value;
                textBox1.UseSystemPasswordChar = value;
            }
        }

        [Category("Tony Textbox")]
        public bool Multiline
        {
            get { return textBox1.UseSystemPasswordChar; }
            set { textBox1.Multiline = value; }
        }
        [Category("Tony Textbox")]
        public override Color BackColor { get => base.BackColor;
            set { base.BackColor = value;
                textBox1.BackColor = value;
            }
        }
        [Category("Tony Textbox")]
        public override Color ForeColor { get => base.ForeColor;
            set { base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }
        [Category("Tony Textbox")]
        public override Font Font { get => base.Font;
            set { base.Font = value;
                textBox1.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }
        [Category("Tony Textbox")]
        public string Texts {
            get {
                if (isPasswordChar) return "";
               else return textBox1.Text;
            }
            set {
                textBox1.Text = value;
                setPlaceholder();
            }
        }
        [Category("Tony Textbox")]
        public Color BorderFocusColor { get => borderFocusColor; set => borderFocusColor = value; }
        [Category("Tony Textbox")]
        public bool IsFocused { get => isFocused; set => isFocused = value; }
        [Category("Tony Textbox")]
        public int BorderRadius { get => borderRadius;
            set {
                if(value >= 0)
                {
                   borderRadius = value;
                    this.Invalidate(); //redraw control
                }
                 
            
            } 
        }
        [Category("Tony Textbox")]
        public Color PlaceholderColor { get => placeholderColor;
            set
            {
                placeholderColor = value;
                if (isPasswordChar)
                    textBox1.ForeColor = value;
            }
        }
        [Category("Tony Textbox")]
        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                textBox1.Text = "";
                setPlaceholder();
            }
        }

        private void setPlaceholder()
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text) && placeholderText != "")
            {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeholderColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = false;
            }
        }

        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "")
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }

        //override methods 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);   
            Graphics graph = e.Graphics;

            //rounded border
            if(borderRadius > 1) //rounded textbox
            { 
                //fields
                var rectangleBorderSmooth = this.ClientRectangle;
                var rectangleBorder = Rectangle.Inflate(rectangleBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectangleBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectangleBorder, borderRadius - borderSize))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor,smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    //Drawing
                    this.Region = new Region(pathBorderSmooth); //Set the rounded region of userControl
                    if (borderRadius > 15) SetTextBoxRoundedRegion(); //set the rounded region
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                    if (isFocused)
                    {
                        if (underlinedStyle) //linestyle
                        {
                            //Draw boder smoothing 
                            graph.DrawPath(penBorderSmooth,pathBorderSmooth);
                            //draw border
                            graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);

                    }
                    else  //normal style
                          //Draw boder smoothing 
                            graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }
                    else
                    {
                        penBorder.Color = borderFocusColor;

                        if (underlinedStyle) //linestyle
                            graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                        else  //normal style
                            graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    }

                }
                

            }
            else
            {
                        //draw border 
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                this.Region = new Region(this.ClientRectangle);
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                if (isFocused)
                {
                    if (underlinedStyle) //linestyle
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else  //normal style
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
                else
                {
                    penBorder.Color = borderFocusColor;

                    if (underlinedStyle) //linestyle
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else  //normal style
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
            }
            }


        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt;
            if (Multiline)
            {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
                textBox1.Region = new Region(pathTxt);
            }
            else
            {
                pathTxt = GetFigurePath(textBox1.ClientRectangle, borderSize * 2);
                textBox1.Region = new Region(pathTxt);
            }
            pathTxt.Dispose();
        }



        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if(this.DesignMode)
                UpdateControlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        public void UpdateControlHeight()
        {
            if(textBox1.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            setPlaceholder();
        }
    }
}
