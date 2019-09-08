using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OSGeo.OGR;
using OSGeo.OSR;

using System.Drawing;



namespace EMap.Gis.Symbology
{
    [Serializable]
    public abstract class BaseLayer : LegendItem, IBaseLayer
    {
        public IScheme Symbology { get; set; }
        public IFrame MapFrame { get; set; }
        public abstract Envelope Extents { get; }
        private Image _bufferImgage;
        public Image BufferImgage
        {
            get => _bufferImgage;
            set
            {
                if (_bufferImgage != value)
                {
                    if (_bufferImgage != null)
                    {
                        _bufferImgage.Dispose();
                    }
                    _bufferImgage = value;
                }
            }
        }
        public Envelope _bufferEnvelope;
        public Envelope BufferEnvelope
        {
            get => _bufferEnvelope;
            protected set
            {
                if (_bufferEnvelope != value)
                {
                    if (_bufferEnvelope != null)
                    {
                        _bufferEnvelope.Dispose();
                    }
                    _bufferEnvelope = value;
                }
            }
        }

        //public OSGeo.OSR.SpatialReference SpatialReference { get; protected set; }
        public ICategory DefaultCategory { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Symbology?.Dispose();
                Symbology = null;
                BufferImgage?.Dispose();
                BufferImgage = null;
            }
            _bufferEnvelope?.Dispose();
            _bufferEnvelope = null;
            base.Dispose(disposing);
        }
        private void GetResolution(Envelope envelope, int pixelWidth, int pixelHeight, out double xRes, out double yRes)
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
        private bool GetWhetherRequereResetBuffer(Rectangle rectangle, Envelope envelope)
        {
            bool requereResetBuffer = true;
            if (BufferImgage != null && BufferEnvelope != null)
            {
                GetResolution(BufferEnvelope, BufferImgage.Width, BufferImgage.Height, out double srcXRes, out double srcYRes);
                GetResolution(envelope, rectangle.Width, rectangle.Height, out double destXRes, out double destYRes);
                if (IsApproximate(srcXRes, destXRes) && IsApproximate(srcYRes, destYRes))
                {
                    if (BufferEnvelope.MinX <= envelope.MinX && BufferEnvelope.MinY <= envelope.MinY && BufferEnvelope.MaxX >= envelope.MaxX && BufferEnvelope.MaxY >= envelope.MaxY)
                    {
                        requereResetBuffer = false;
                    }
                }
            }
            return requereResetBuffer;
        }

        public abstract void ResetBuffer(Rectangle rectangle, Envelope envelope, bool selected, ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource);
        public void DrawReagion(Image image, Rectangle rectangle, Envelope envelope, bool selected, ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource)
        {
            if (image == null || envelope == null || cancellationTokenSource?.IsCancellationRequested == true)
            {
                return;
            }
            bool requereResetBuffer = GetWhetherRequereResetBuffer(rectangle, envelope);
            if (requereResetBuffer)
            {
                ResetBuffer(rectangle, envelope, selected,progressHandler, cancellationTokenSource);
            }
            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(BufferImgage, new Point(rectangle.X, rectangle.Y));
            }
        }
    }
}