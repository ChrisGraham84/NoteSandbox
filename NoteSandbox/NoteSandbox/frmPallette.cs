using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NoteSandbox
{
    public partial class frmPallette : Form
    {
       
        // Thanks Joe Hick for the basic form drawing setup
        
        bool paint = false;
        Pen p = new Pen(Color.Black, 2);
        Pen p2 = new Pen(Color.Transparent, 1);
        List<List<Point>> lstDrawings = new List<List<Point>>();
        List<Point> lstPoints;
        List<List<Point>> lstListOfPoints = new List<List<Point>>();
        int count = 0;
        public frmPallette()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            lstListOfPoints.Clear();
          this.Invalidate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Image img = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, this.Width, this.Height));

            Draw(g);

            try
            {
                img.Save(String.Format(System.Configuration.ConfigurationManager.AppSettings["SavePath"] + "Notes_{0}_{1}.png", DateTime.Now.ToString("yyyy_MM_dd"),count));
                MessageBox.Show("Saved!");
                count++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmPallette_Load(object sender, EventArgs e)
        {
           
        }

        private void frmPallette_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Draw(g);
        }


        private void frmPallette_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            lstPoints = new List<Point>();
            lstListOfPoints.Add(lstPoints);
        }

        private void frmPallette_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
        }

        private void frmPallette_MouseMove(object sender, MouseEventArgs e)
        {
            //bool continuous = false;
            if (paint)
            {
               
                Point p = new Point(e.X, e.Y);
                lstPoints.Add(p);
                this.Invalidate();
            }
            else
            {
                //lstPoints.Add(Point.Empty);
            }
        }
        private void Draw(Graphics g)
        {
            
            foreach (var lst in lstListOfPoints)
            {
                Point prevPt = Point.Empty;
                foreach (Point pt in lst)
                {
                    if (prevPt != Point.Empty)
                    {
                        g.DrawLine(p, prevPt.X, prevPt.Y, pt.X, pt.Y);

                    }
                    else
                    {
                        g.DrawLine(p2, prevPt.X, prevPt.Y, pt.X, pt.Y);
                    }
                    prevPt = pt;
                }
                
            }
        }
    }
}
