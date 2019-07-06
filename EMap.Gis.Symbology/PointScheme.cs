using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    //todo 待修改
    public class PointScheme:FeatureScheme, IPointScheme
    {
        public override int NumCategories => throw new NotImplementedException();
        #region Properties

        /// <summary>
        /// Gets or sets the list of scheme categories belonging to this scheme.
        /// </summary>
        PointCategoryCollection Categories { get; set; }

        public override void AddCategory(ICategory category)
        {
            throw new NotImplementedException();
        }

        public override void ClearCategories()
        {
            throw new NotImplementedException();
        }

        public override ICategory CreateNewCategory(Rgba32 fillColor, double size)
        {
            throw new NotImplementedException();
        }

        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IFeatureCategory> GetCategories()
        {
            throw new NotImplementedException();
        }

        public override ICategory GetCategory(int index)
        {
            throw new NotImplementedException();
        }

        public override void InsertCategory(int index, ICategory category)
        {
            throw new NotImplementedException();
        }

        public override void RemoveCategory(ICategory category)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
