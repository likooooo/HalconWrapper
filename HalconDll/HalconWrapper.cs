using HalconDotNet;
using System.IO;

namespace HalconWrapper
{
    //halcon点云对象
    public class HalconPointcloud : ProcessPointcloudF
    {
        //点云对象
        private HTuple pointcloud;
        public HTuple Pointcloud
        {
            get
            {
                return pointcloud;
            }
        }

        /**构造函数
         * **/
        public HalconPointcloud(string X, string Y, string Z)
        {
            pointcloud = GenObjectModel3dFromPoints(ReadTuple(X),
                  ReadTuple(Y),
                    ReadTuple(Z));
        }
        public HalconPointcloud(float[] x, float[] y, float[] z)
        {
            HTuple htupleX = new HTuple(x);
            HTuple htupleY = new HTuple(y);
            HTuple htupleZ = new HTuple(z);
            pointcloud = GenObjectModel3dFromPoints(htupleX,
                   htupleY,
                     htupleZ);
        }

        
        /**释放点云
         * **/
        public void Dispose()
        {
            //清除点
            HOperatorSet.ClearObjectModel3d(this.pointcloud);
        }
        //相当于获取到了数据转换这一过程
        public static explicit operator 
            PointcloudWrapper.PointcloudF
            (HalconPointcloud halconPointcloud)
        {
            return new PointcloudWrapper.PointcloudF(
                  halconPointcloud.GetCoordX(),
                  halconPointcloud.GetCoordY(),
                  halconPointcloud.GetCoordZ());
        }

        public float[] GetCoordX()
        {
            return ProcessPointcloudF.GetCoordX(pointcloud);
        }
        public float[] GetCoordY()
        {
            return ProcessPointcloudF.GetCoordY(pointcloud);
        }
        public float[] GetCoordZ()
        {
            return ProcessPointcloudF.GetCoordZ(pointcloud);
        }
        public void SavePoints2Htuple(string path)
        {
            ProcessPointcloudF.SavePoints2Htuple(this.pointcloud, path);
        }
    }


    //halcon点云处理的静态方法
    public class ProcessPointcloudF
    {
        public static float[] GetCoordX(HTuple Pointclouds, ModelParams para = ModelParams.point_coord_x)
        {
            HTuple X = GetParamValue(Pointclouds, para);
            float[] arry = X.ToFArr();
            return arry;
        }
        public static float[] GetCoordY(HTuple Pointclouds, ModelParams para = ModelParams.point_coord_y)
        {
            HTuple Y = GetParamValue(Pointclouds, para);
            float[] arry = Y.ToFArr();
            return arry;
        }
        public static float[] GetCoordZ(HTuple Pointclouds, ModelParams para = ModelParams.point_coord_z)
        {
            HTuple Z = GetParamValue(Pointclouds, para);
            float[] arry = Z.ToFArr();
            return arry;
        }
        /**获取点云的属性，默认为获取点云个数
           * **/
        public static HTuple GetParamValue(HTuple Pointclouds, ModelParams para = ModelParams.num_points)
        {
            HTuple genPara = Enum2Htuple(para);
            HTuple result = new HTuple();
            HOperatorSet.GetObjectModel3dParams(Pointclouds, genPara, out result);
            return result;
        }
        /**一个枚举转Htuple的私有方法
         * **/
        public static HTuple Enum2Htuple(object obj)
        {
            return obj.ToString();
        }

        public static HTuple ReadTuple(string filePath)
        {
            HTuple tuple;
            HOperatorSet.ReadTuple(filePath, out tuple);
            return tuple;
        }
        public static HTuple GenObjectModel3dFromPoints(HTuple x, HTuple y, HTuple z)
        {
            HTuple Pointclouds = new HTuple();
            HOperatorSet.GenObjectModel3dFromPoints(x, y, z, out Pointclouds);
            return Pointclouds;
        }
        /**点云以Htuple的格式存储
        * **/
        public static void SavePoints2Htuple(HTuple pointcloud,string name)
        {
            //   int Id = 0;
            StreamWriter sx = new StreamWriter(File.Create(name + "_Htuple_x" + ".txt"));
            StreamWriter sy = new StreamWriter(File.Create(name + "_Htuple_y" + ".txt"));
            StreamWriter sz = new StreamWriter(File.Create(name + "_Htuple_z" + ".txt"));
            float[] x = GetCoordX(pointcloud);
            float[] y = GetCoordY(pointcloud);
            float[] z = GetCoordZ(pointcloud);
            sx.WriteLine(x.Length.ToString());
            sy.WriteLine(y.Length.ToString());
            sz.WriteLine(z.Length.ToString());

            for (int i = 0; i < x.Length; i++)
            {
                sx.WriteLine("2 " + (x[i]).ToString());
                sy.WriteLine("2 " + (y[i]).ToString());
                sz.WriteLine("2 " + (z[i]).ToString());
            }
            sx.Close();
            sy.Close();
            sz.Close();
        }
    }


    /**用于get_object_model_3d_params(),比较重要
    * **/
    public enum ModelParams
    {
        blue,
        bounding_box1,//外包络矩形
        center,//中心
        diameter_axis_aligned_bounding_box,
        extended_attribute_names,
        green,
        has_distance_computation_data,
        has_extended_attribute,
        has_lines,
        has_point_normals,
        has_points,//模型是否包含点
        has_polygons,//模型是否包含多边形
        has_primitive_data,
        has_primitive_rms,//是否有基元的二次剩余误差
        has_segmentation_data,//是否有分割数据
        has_shape_based_matching_3d_data,//是否有形状匹配数据
        has_surface_based_matching_data,//是否有表面匹配数据
        has_triangles,//是否有三角形
        has_xyz_mapping,//是否有XYZ的映射
        lines,
        mapping_col,
        mapping_row, neighbor_distance, num_extended_attribute,
        num_lines, num_neighbors, num_neighbors_fast,
        num_points,
        num_polygons,
        num_primitive_parameter_extension,
        num_triangles,
        point_coord_x, point_coord_y, point_coord_z,//点的坐标
        point_normal_x, point_normal_y, point_normal_z,//法向量
        polygons,
        primitive_parameter,
        primitive_parameter_extension,
        primitive_parameter_pose,
        primitive_pose,
        primitive_rms,
        primitive_type,
        red,
        reference_point,
        score,
        triangles
    }
}
