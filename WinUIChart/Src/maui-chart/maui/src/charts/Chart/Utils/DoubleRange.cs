using System;
using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public struct DoubleRange
    {
        #region Members
        /// <summary>
        /// Initializes C_empty
        /// </summary>
        private static readonly DoubleRange C_empty = new DoubleRange(double.NaN, double.NaN);
        private bool isEmpty;

        /// <summary>
        /// Initializes endValue
        /// </summary>
        private double startValue;

        /// <summary>
        /// Initializes endValue
        /// </summary>
        private double endValue;

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange Empty
        {
            get
            {
                return C_empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Start
        {
            get
            {
                return startValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double End
        {
            get
            {
                return endValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Delta
        {
            get
            {
                return endValue - startValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Median
        {
            get
            {
                return (startValue + endValue) / 2d;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public DoubleRange(double start, double end)
        {
            if (!double.IsNaN(start) && !double.IsNaN(end))
            {
                this.isEmpty = false;
            }
            else
            {
                this.isEmpty = true;
            }

            if (start > end)
            {
                startValue = end;
                endValue = start;
            }
            else
            {
                startValue = start;
                endValue = end;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange operator +(DoubleRange leftRange, DoubleRange rightRange)
        {
            return Union(leftRange, rightRange);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange operator +(DoubleRange range, double value)
        {
            return Union(range, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >(DoubleRange range, double value)
        {
            return range.startValue > value;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator >(DoubleRange range, DoubleRange value)
        {
            return range.startValue > value.startValue && range.endValue > value.endValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <(DoubleRange range, DoubleRange value)
        {
            return range.startValue < value.startValue && range.endValue < value.endValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator <(DoubleRange range, double value)
        {
            return range.endValue < value;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(DoubleRange leftRange, DoubleRange rightRange)
        {
            return leftRange.Equals(rightRange);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(DoubleRange leftRange, DoubleRange rightRange)
        {
            return !leftRange.Equals(rightRange);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange Union(params double[] values)
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            foreach (double val in values)
            {
                if (double.IsNaN(val))
                {
                    min = val;
                }
                else if (min > val)
                {
                    min = val;
                }

                if (max < val)
                {
                    max = val;
                }
            }

            return new DoubleRange(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange Union(DoubleRange leftRange, DoubleRange rightRange)
        {
            if (leftRange.IsEmpty)
            {
                return rightRange;
            }
            else if (rightRange.IsEmpty)
            {
                return leftRange;
            }

            return new DoubleRange(Math.Min(leftRange.startValue, rightRange.startValue), Math.Max(leftRange.endValue, rightRange.endValue));
        }

        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange Union(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return new DoubleRange(value, value);
            }

            return new DoubleRange(Math.Min(range.startValue, value), Math.Max(range.endValue, value));
        }

        /// <summary>
        /// 
        /// </summary>
        public static DoubleRange Scale(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return range;
            }

            return new DoubleRange(range.startValue - (value * range.Delta), range.endValue + (value * range.Delta));
        }

        /// <summary>
        ///
        /// </summary>
        public static DoubleRange Offset(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return range;
            }

            return new DoubleRange(range.startValue + value, range.endValue + value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool Exclude(DoubleRange range, DoubleRange excluder, out DoubleRange leftRange, out DoubleRange rightRange)
        {
            leftRange = DoubleRange.Empty;
            rightRange = DoubleRange.Empty;

            if (!(range.IsEmpty || excluder.IsEmpty))
            {
                if (excluder.endValue < range.startValue)
                {
                    if (excluder.endValue > range.startValue)
                    {
                        leftRange = new DoubleRange(excluder.startValue, range.startValue);
                    }
                    else
                    {
                        leftRange = excluder;
                    }
                }

                if (excluder.endValue > range.endValue)
                {
                    if (excluder.startValue < range.endValue)
                    {
                        rightRange = new DoubleRange(range.endValue, excluder.endValue);
                    }
                    else
                    {
                        rightRange = excluder;
                    }
                }
            }

            return !(leftRange.IsEmpty && rightRange.IsEmpty);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Intersects(DoubleRange range)
        {
            if (this.IsEmpty || this.IsEmpty)
            {
                return false;
            }

            return this.Inside(range.startValue) || this.Inside(range.endValue) || range.Inside(this.startValue) || range.Inside(this.endValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Intersects(double start, double end)
        {
            return this.Intersects(new DoubleRange(start, end));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Inside(double value)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            return (value <= endValue) && (value >= startValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Inside(DoubleRange range)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            return startValue <= range.startValue && endValue >= range.endValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is DoubleRange)
            {
                DoubleRange range = (DoubleRange)obj;
                return (startValue == range.startValue) && (endValue == range.endValue);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int GetHashCode()
        {
            return startValue.GetHashCode() ^ endValue.GetHashCode();
        }
        #endregion
    }
}
