using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace EMap.Gis.Symbology
{
    public class Descriptor: CopyBase,IDescriptor
    {
        public void CopyProperties(object source)
        {
            OnCopyProperties(source);
        }
        public bool Matches(IMatchable other, out List<string> mismatchedProperties)
        {
            mismatchedProperties = new List<string>();
            return OnMatch(other, mismatchedProperties);
        }

        public void Randomize(Random generator)
        {
            OnRandomize(generator);
        }

        protected virtual void OnCopyProperties(object source)
        {
            Type original = GetType();
            Type copy = source.GetType();
            PropertyInfo[] originalProperties = DistinctNames(original.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            PropertyInfo[] copyProperties = DistinctNames(copy.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            foreach (PropertyInfo originalProperty in originalProperties)
            {
                if (originalProperty.CanWrite == false) continue;
                if (copyProperties.Contains(originalProperty.Name) == false) continue;

                PropertyInfo copyProperty = copyProperties.GetFirst(originalProperty.Name);
                if (copyProperty == null)
                {
                    continue;
                }

                object copyValue = copyProperty.GetValue(source, null);
                if (copyProperty.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length == 0)
                {
                    ICloneable cloneable = copyValue as ICloneable;
                    if (cloneable != null)
                    {
                        originalProperty.SetValue(this, cloneable.Clone(), null);
                        continue;
                    }
                }

                originalProperty.SetValue(this, copyValue, null);
            }

            FieldInfo[] originalFields = original.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo originalField in originalFields)
            {
                FieldInfo copyField = copyFields.GetFirst(originalField.Name);
                if (copyFields.Contains(originalField.Name) == false) continue;
                if (copyField == null)
                {
                    continue;
                }

                object copyValue = copyField.GetValue(source);
                if (copyField.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length == 0)
                {
                    ICloneable cloneable = copyValue as ICloneable;
                    if (cloneable != null)
                    {
                        originalField.SetValue(this, cloneable.Clone());
                        continue;
                    }
                }

                originalField.SetValue(this, copyValue);
            }
        }

        protected virtual bool OnMatch(IMatchable other, List<string> mismatchedProperties)
        {
            Type original = GetType();
            Type copy = other.GetType();

            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] copyProperties = copy.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo originalProperty in originalProperties)
            {
                if (!copyProperties.Contains(originalProperty.Name))
                {
                    mismatchedProperties.Add(originalProperty.Name);
                    continue;
                }

                PropertyInfo copyProperty = copyProperties.GetFirst(originalProperty.Name);
                if (copyProperty == null)
                {
                    mismatchedProperties.Add(originalProperty.Name);
                    continue;
                }

                object originalValue = originalProperty.GetValue(this, null);
                object copyValue = copyProperty.GetValue(other, null);
                if (!Match(originalValue, copyValue))
                {
                    mismatchedProperties.Add(originalProperty.Name);
                }
            }

            FieldInfo[] originalFields = original.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo originalField in originalFields)
            {
                if (!copyFields.Contains(originalField.Name))
                {
                    mismatchedProperties.Add(originalField.Name);
                    continue;
                }

                FieldInfo copyField = copyFields.GetFirst(originalField.Name);
                if (copyField == null)
                {
                    mismatchedProperties.Add(originalField.Name);
                    continue;
                }

                object originalValue = originalField.GetValue(this);
                object copyValue = copyField.GetValue(other);
                if (!Match(originalValue, copyValue))
                {
                    mismatchedProperties.Add(originalField.Name);
                }
            }

            return mismatchedProperties.Count <= 0;
        }

        protected virtual void OnRandomize(Random generator)
        {
            Type original = GetType();
            PropertyInfo[] originalProperties = original.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo originalProperty in originalProperties)
            {
                object prop = originalProperty.GetValue(this, null);
                IRandomizable rnd = prop as IRandomizable;
                rnd?.Randomize(generator);
            }
        }

        private static bool Match(object originalValue, object copyValue)
        {
            // If a custom IMatchable description exists use it to determine if there is a match
            IMatchable originalMatch = originalValue as IMatchable;
            IMatchable copyMatch = copyValue as IMatchable;
            if (originalMatch != null && copyMatch != null)
            {
                bool res = originalMatch.Matches(copyMatch, out List<string> ignoreMe);
                return res;
            }

            string origString = originalValue as string;
            if (origString != null)
            {
                string mString = copyValue as string;
                if (mString == null) return false;

                return origString.Equals(mString);
            }

            IEnumerable originalList = originalValue as IEnumerable;
            if (originalList != null)
            {
                IEnumerable copyList = copyValue as IEnumerable;
                if (copyList != null)
                {
                    IEnumerator e = copyList.GetEnumerator();
                    e.MoveNext();
                    foreach (object originalItem in originalList)
                    {
                        if (Match(originalItem, e.Current) == false)
                        {
                            return false;
                        }

                        e.MoveNext();
                    }
                }

                return true;
            }

            if (originalValue == null && copyValue == null) return true;

            return originalValue != null && originalValue.Equals(copyValue);
        }
    }
}