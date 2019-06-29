using OSGeo.OGR;

namespace IMap.Gis.Symbology
{
    public abstract class FeatureLayer : BaseLayer, IFeatureLayer
    {
        public DataSource DataSource { get; set; }
        private Layer _layer;
        public Layer Layer
        {
            get
            {
                if (_layer == null)
                {
                    _layer = DataSource?.GetLayerByIndex(0);
                }
                return _layer;
            }
        }
        public new IFeatureScheme Symbology { get => base.Symbology as IFeatureScheme; set => base.Symbology = value; }
        public FeatureLayer(DataSource dataSource)
        {
            DataSource = dataSource;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Layer?.Dispose();
                DataSource?.Dispose();
                DataSource = null;
            }
            base.Dispose(disposing);
        }
    }
}