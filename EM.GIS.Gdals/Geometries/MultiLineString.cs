using EM.GIS.Geometries;

namespace EM.GIS.Gdals
{
    public class MultiLineString:GeometryCollection, IMultiLineString
    {
        public MultiLineString(OSGeo.OGR.Geometry geometry) : base(geometry)
        { }
        public override object Clone()
        {
            return new MultiLineString(OgrGeometry.Clone());
        }
    }
}
