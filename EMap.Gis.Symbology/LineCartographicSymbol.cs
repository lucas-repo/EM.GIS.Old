using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace EMap.Gis.Symbology
{
    [Serializable]
    public class LineCartographicSymbol : LineSimpleSymbol, ILineCartographicSymbol
    {
        #region Fields

        private float[] _compondArray;
        private bool[] _compoundButtons;
        private bool[] _dashButtons;
        private DashCap _dashCap;
        private float[] _dashPattern;
        private IList<ILineDecoration> _decorations;
        private LineCap _endCap;
        private LineJoinType _joinType;
        private float _offset;
        private LineCap _startCap;

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets an array of floating point values ranging from 0 to 1 that
        /// indicate the start and end point for where the line should draw.
        /// </summary>
        [XmlIgnore]
        public float[] CompoundArray
        {
            get
            {
                return _compondArray;
            }

            set
            {
                _compondArray = value;
            }
        }

        /// <summary>
        /// Gets or sets the compound buttons. This is a cached version of the vertical pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] CompoundButtons
        {
            get
            {
                return _compoundButtons;
            }

            set
            {
                _compoundButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets the dash buttons. This is a cached version of the horizontal pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] DashButtons
        {
            get
            {
                return _dashButtons;
            }

            set
            {
                _dashButtons = value;
            }
        }

        /// <summary>
        /// gets or sets the DashCap for both the start and end caps of the dashes
        /// </summary>
        public DashCap DashCap
        {
            get
            {
                return _dashCap;
            }

            set
            {
                _dashCap = value;
            }
        }

        /// <summary>
        /// Gets or sets the DashPattern as an array of floating point values from 0 to 1
        /// </summary>
        [XmlIgnore]
        public float[] DashPattern
        {
            get
            {
                return _dashPattern;
            }

            set
            {
                _dashPattern = value;
            }
        }

        /// <summary>
        /// Gets or sets the line decoration that describes symbols that should
        /// be drawn along the line as decoration.
        /// </summary>
        public IList<ILineDecoration> Decorations
        {
            get
            {
                return _decorations;
            }

            set
            {
                _decorations = value;
            }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        public LineCap EndCap
        {
            get
            {
                return _endCap;
            }

            set
            {
                _endCap = value;
            }
        }

        /// <summary>
        /// Gets or sets the OGC line characteristic that controls how connected segments
        /// are drawn where they come together.
        /// </summary>
        public LineJoinType JoinType
        {
            get
            {
                return _joinType;
            }

            set
            {
                _joinType = value;
            }
        }

        /// <summary>
        /// Gets or sets the floating poing offset (in pixels) for the line to be drawn to the left of
        /// the original line. (Internally, this will modify the width and compound array for the
        /// actual pen being used, as Pens do not support an offset property).
        /// </summary>
        public float Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        public LineCap StartCap
        {
            get
            {
                return _startCap;
            }

            set
            {
                _startCap = value;
            }
        }

        #endregion
        public LineCartographicSymbol() : this(LineSymbolType.Cartographic)
        {
            Configure();
        }
        protected LineCartographicSymbol(LineSymbolType lineSymbolType) : base(lineSymbolType)
        {
            Configure();
        }
        private void Configure()
        {
            _joinType = LineJoinType.Round;
            _startCap = LineCap.Round;
            _endCap = LineCap.Round;
            _decorations = new List<ILineDecoration>();
        }
        public override Pen ToPen(float scaleWidth)
        {
            Pen myPen = base.ToPen(scaleWidth);
            myPen.EndCap = _endCap;
            myPen.StartCap = _startCap;
            if (_compondArray != null) myPen.CompoundArray = _compondArray;
            if (_offset != 0F)
            {
                float[] pattern = { 0, 1 };
                float w = (float)Width;
                if (w == 0) w = 1;
                w = (float)(scaleWidth * w);
                float w2 = (Math.Abs(_offset) + (w / 2)) * 2;
                if (_compondArray != null)
                {
                    pattern = new float[_compondArray.Length];
                    for (int i = 0; i < _compondArray.Length; i++)
                    {
                        pattern[i] = _compondArray[i];
                    }
                }

                for (int i = 0; i < pattern.Length; i++)
                {
                    if (_offset > 0)
                    {
                        pattern[i] = (w / w2) * pattern[i];
                    }
                    else
                    {
                        pattern[i] = 1 - (w / w2) + ((w / w2) * pattern[i]);
                    }
                }

                myPen.CompoundArray = pattern;
                myPen.Width = w2;
            }

            if (_dashPattern != null)
            {
                myPen.DashPattern = _dashPattern;
            }
            else
            {
                if (myPen.DashStyle == DashStyle.Custom)
                {
                    myPen.DashStyle = DashStyle.Solid;
                }
            }

            switch (_joinType)
            {
                case LineJoinType.Bevel:
                    myPen.LineJoin = LineJoin.Bevel;
                    break;
                case LineJoinType.Mitre:
                    myPen.LineJoin = LineJoin.Miter;
                    break;
                case LineJoinType.Round:
                    myPen.LineJoin = LineJoin.Round;
                    break;
            }

            return myPen;
        }
    }
}
