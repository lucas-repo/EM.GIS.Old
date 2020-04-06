using EMap.Gis.Data;
using OSGeo.OGR;
using System;
using System.Drawing;
using System.Text;
using System.Threading;


namespace EMap.Gis.Symbology
{
    [Serializable]
    public abstract class BaseLayer : IBaseLayer
    {
        public bool Name { get; set; }
        public bool AliasName { get; set; }
        public virtual Extent Extent { get;}
        public bool IsVisible { get; set; } = true;

        public IScheme Symbology { get; set; }

        //public OSGeo.OSR.SpatialReference SpatialReference { get; protected set; }
        public ICategory DefaultCategory { get; set; }
        public bool UseDynamicVisibility { get; set; }
        public double MaxScaleFactor { get; set; }
        public double MinScaleFactor { get; set; }

        private void GetResolution(Extent envelope, int pixelWidth, int pixelHeight, out double xRes, out double yRes)
        {
            double worldWidth = envelope.MaxX - envelope.MinX;
            double worldHeight = envelope.MaxY - envelope.MinY;
            xRes = worldWidth / pixelWidth;
            yRes = worldHeight / pixelHeight;
        }
        /// <summary>
        /// 判断两个值是否为近似值
        /// </summary>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <returns></returns>
        private static bool IsApproximate(double value0, double value1)
        {
            string str0 = value0.ToString("G15");
            StringBuilder sb = new StringBuilder();
            sb.Append("0.");
            int precision = str0.Split('.')[1].Length;
            for (int i = 0; i < precision; i++)
            {
                if (i == precision - 1)
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }
            double threshold = Convert.ToDouble(sb.ToString());
            bool ret = Math.Abs(value0 - value1) <= threshold;
            return ret;
        }

        public void Draw(Graphics graphics, Rectangle rectangle, Extent extent, bool selected = false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null)
        {
            if (graphics == null || rectangle.Width * rectangle.Height == 0 || extent == null || extent.Width * extent.Height == 0 || cancellationTokenSource?.IsCancellationRequested == true)
            {
                return;
            }
            OnDraw(graphics, rectangle, extent, selected, progressHandler, cancellationTokenSource);
            progressHandler?.Invoke(0, string.Empty);
        }
        protected abstract void OnDraw(Graphics graphics, Rectangle rectangle, Extent extent, bool selected = false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null);
        public bool GetVisible(Extent extent, Rectangle rectangle)
        {
            bool visible = false;
            if (!IsVisible)
            {
                return visible;
            }

            if (UseDynamicVisibility)
            {
                //todo compare the scalefactor 
            }

            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    Symbology = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BaseLayer()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }


        #endregion
    }
}