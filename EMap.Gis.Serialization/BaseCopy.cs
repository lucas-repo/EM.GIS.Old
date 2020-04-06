using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EMap.Gis.Serialization
{
    /// <summary>
    /// 可拷贝基类
    /// </summary>
    [Serializable]
    public abstract class BaseCopy : ICloneable
    {
        #region Methods

        /// <summary>
        /// Creates a duplicate of this descriptor using MemberwiseClone
        /// </summary>
        /// <returns>A clone of this object as a duplicate</returns>
        public object Clone()
        {
            object copy = MemberwiseClone();
            OnCopy(copy);
            return copy;
        }

        #endregion

        /// <summary>
        /// PropertyInfo returns overridden members as separate entries. We would rather work with each members
        /// only one time.
        /// </summary>
        /// <param name="allProperties">All the properties, including duplicates created by overridden members</param>
        /// <returns>An Array of PropertyInfo members</returns>
        public static PropertyInfo[] DistinctNames(IEnumerable<PropertyInfo> allProperties)
        {
            List<string> names = new List<string>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (PropertyInfo property in allProperties)
            {
                if (names.Contains(property.Name)) continue;
                result.Add(property);
                names.Add(property.Name);
            }

            return result.ToArray();
        }
        /// <summary>
        /// 将源对象的属性和特性拷贝给目标对象
        /// </summary>
        /// <param name="src">源对象</param>
        /// <param name="copy">目标对象</param>
        public static void CopyTo(object src, object copy)
        {
            if (src == null || copy == null)
            {
                return;
            }

            // This checks any property on copy, and if it is cloneable, it
            // creates a clone instead
            var srcType = src.GetType();
            Type copyType = copy.GetType();

            PropertyInfo[] copyProperties = DistinctNames(copyType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            PropertyInfo[] srcProperties = DistinctNames(srcType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            foreach (PropertyInfo p in copyProperties)
            {
                if (p.CanWrite == false) continue;
                if (!srcProperties.Any(x => x.Name == p.Name)) continue;
                PropertyInfo srcProperty = srcProperties.First(x => x.Name == p.Name);
                object srcValue = srcProperty.GetValue(src, null);
                if (srcProperty.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                {
                    // This property is marked as shallow, so skip cloning it
                    continue;
                }
                if (srcValue is ICloneable cloneable)
                {
                    p.SetValue(copy, cloneable.Clone(), null);
                }
            }

            FieldInfo[] copyFields = copyType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] srcFields = srcType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo f in copyFields)
            {
                if (!srcFields.Any(x => x.Name == f.Name)) continue;
                FieldInfo myField = srcFields.First(x => x.Name == f.Name);
                object myValue = myField.GetValue(copy);

                if (myField.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0)
                {
                    // This field is marked as shallow, so skip cloning it
                    continue;
                }

                ICloneable cloneable = myValue as ICloneable;
                if (cloneable == null) continue;
                f.SetValue(copy, cloneable.Clone());
            }
        }
        /// <summary>
        /// This occurs during the Copy method and is overridable by sub-classes
        /// </summary>
        /// <param name="copy">The duplicate descriptor</param>
        protected virtual void OnCopy(object copy)
        {
            CopyTo(this, copy);
        }
        /// <summary>
        /// 拷贝属性
        /// </summary>
        /// <param name="source"></param>
        public void CopyProperties(object source)
        {
            CopyTo(source, this);
        }
    }
}
