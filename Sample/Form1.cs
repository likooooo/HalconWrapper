using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconWrapper;

namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HalconWrapper.HalconPointcloud
                halconPointcloud
                = new HalconPointcloud("x.txt", "y.txt", "z.txt");
            //HalconPointcloud转PointcloudF的隐式转换
            PointcloudWrapper.PointcloudF
                    p = (PointcloudWrapper.PointcloudF)halconPointcloud;
            //HalconPointcloud数据处理方法调用如下
            halconPointcloud.GetCoordX();
         //   HalconWrapper.ProcessPointcloudF.GetCoordX(halconPointcloud);
        }
    }
}
