namespace EMap.Gis.Projections
{
    public interface IEsriString
    {
        string ToEsriString();
        void ParseEsriString(string esriString);
    }
}