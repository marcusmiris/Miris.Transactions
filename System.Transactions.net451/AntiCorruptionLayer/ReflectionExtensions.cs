using System;
using System.Linq;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace System.Transactions
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static object GetPrivateFieldValue(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Type t = obj.GetType();
            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            while (fieldInfo == null && propertyInfo == null && t != null)
            {
                fieldInfo = t.GetField(propName, Public | NonPublic | Instance);
                if (fieldInfo == null)
                {
                    propertyInfo = t.GetProperty(propName, Public | NonPublic | Instance);
                }

                t = t.BaseType;
            }
            if (fieldInfo == null && propertyInfo == null)
                throw new ArgumentOutOfRangeException(nameof(propName), $"Field {propName} was not found in Type {obj.GetType().FullName}");

            if (fieldInfo != null)
                return fieldInfo.GetValue(obj);

            return propertyInfo.GetValue(obj, null);
        }

        public static TField GetPrivateField<TField>(
            this object @source,
            string fieldName)
        {
            if (!fieldName.Contains("."))
                return (TField)@source.GetPrivateFieldValue(fieldName);
            else
                return (TField)
                    fieldName.Split('.')
                        .Aggregate(@source, (current, prop) => current.GetPrivateFieldValue(prop));
        }

    }
}
