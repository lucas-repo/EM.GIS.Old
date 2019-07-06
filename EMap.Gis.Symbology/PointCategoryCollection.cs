using System;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public class PointCategoryCollection : List<IPointCategory>
    {
        #region Fields

        private IPointScheme _scheme;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the parent scheme.
        /// </summary>
        public IPointScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        #endregion

        #region Methods
        

        /// <summary>
        /// Cycles through all the categories and resets the parent item.
        /// </summary>
        private void UpdateItemParentPointers()
        {
            foreach (IPointCategory item in this)
            {
                if (_scheme == null)
                {
                    item.Parent = null;
                }
                else
                {
                    item.Parent = _scheme;
                }
            }
        }

        #endregion
    }
}