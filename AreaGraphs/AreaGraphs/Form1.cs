using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AreaGraphs
{
    public partial class Form1 : Form
    {                     
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadingUI();
        }

        GraphEdit graphEdit;
        Color boardColor = Color.FromArgb(17, 81, 138);//指定绘制图的背景色  
        Thread toUpdate;                               //刷新线程
        private void LoadingUI()
        {
            graphEdit = new GraphEdit(640,350 , boardColor);
            graphEdit.HorizontalMargin = 50;                                   //横水平边距
            graphEdit.VerticalMargin = 80;                                     //竖垂直边距
            graphEdit.AreasColor = Color.FromArgb(100, 0, 0, 0);         //画图区域颜色
            graphEdit.GraphColor = Color.FromArgb(255, 110, 176);        //曲线面积颜色
            graphEdit.AxisColor = Color.FromArgb(255, 255, 255);         //坐标轴颜色
            graphEdit.ScaleColor = Color.FromArgb(20, 255, 255, 255);          //刻度线颜色

            graphEdit.XScaleCount = 24;          //X轴刻度线数量
            graphEdit.YScaleCount = 10;          //Y轴刻度线数量



            toUpdate = new Thread(new ThreadStart(Run));
            toUpdate.Start();
        }

        private void Run()
        {
            while (true)
            {
                Image image = graphEdit.GetCurrentGraph(this.GetBaseData(), XRange, YRange, true);  //如果是面积曲线图将最后一个参数设为true
                Graphics g = this.CreateGraphics();  //指定使用那个控件来接受曲线图
 
                g.DrawImage(image, 0, 0);
                g.Dispose();
                Thread.Sleep(500);                 //每2秒钟刷新一次  
            }
        }

        float XRange = 1440;   //X轴最大范围（0-1440）
        float YRange = 500;    //Y轴最大范围（0-500）

        /// <summary>
        /// 得到（数据库）数据
        /// </summary>
        /// <returns></returns>
        private List<Point> GetBaseData()
        {
            Random r = new Random();
            List<Point> result = new List<Point>();  //数据
            for (int i = 0; i < XRange-200; i+=30)
            {
                Point p;
                if(i<100)
                p = new Point(i, r.Next(180, 200));
                else
                    p = new Point(i, r.Next(200, 220));
                result.Add(p);
            }
            return result;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                toUpdate.Abort();
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
        }



    }
}
