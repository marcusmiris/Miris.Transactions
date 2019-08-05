using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Reflection.BindingFlags;

namespace Miris.Transactions
{
    [DebuggerStepThrough]
    public static class ReflectionExtensions
    {

        #region ' GetPrivateMemberValue '

        public static TField GetPrivateMemberValue<TField>(
            this object @source,
            string fieldName)
        {
            return (TField)@source.GetPrivateMemberValue(fieldName);
        }

        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static object GetPrivateMemberValue(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(propName)) throw new ArgumentNullException(nameof(propName));
            //
            var navigationProperties = propName.Split('.');
            var candidate = obj;
            //
            foreach (var navProp in navigationProperties)
            {
                var t = candidate.GetType();
                FieldInfo fieldInfo = null;
                PropertyInfo propertyInfo = null;

                while (fieldInfo == null && propertyInfo == null && t != null)
                {
                    fieldInfo = t.GetField(navProp, Public | NonPublic | Instance);
                    if (fieldInfo == null)
                    {
                        propertyInfo = t.GetProperty(navProp, Public | NonPublic | Instance);
                    }

                    t = t.BaseType;
                }
                if (fieldInfo == null && propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propName), $"Field {navProp} was not found in Type {candidate.GetType().FullName}");

                candidate = fieldInfo != null
                    ? fieldInfo.GetValue(candidate)
                    : propertyInfo.GetValue(candidate, null);
            }

            return candidate;
        }

        #endregion

        #region ' SetPrivateMemberValue '

        public static void SetPrivateMemberValue(this object obj, string propName, object newValue)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(propName)) throw new ArgumentNullException(nameof(propName));


            var x = Regex.Match(propName, @"((?<NavProperty>.*)\.)?(?<MemberName>.*)");

            // navega até o objeto cujo atributo será modificado.
            var navProperty = x.Groups["NavProperty"];
            if (navProperty.Success) obj = GetPrivateMemberValue(obj, navProperty.Value);

            // altera o atributo privado;
            var navProp = x.Groups["MemberName"].Value;

            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            var t = obj.GetType();

            while (fieldInfo == null && propertyInfo == null && t != null)
            {
                fieldInfo = t.GetField(navProp, Public | NonPublic | Instance);
                if (fieldInfo == null)
                {
                    propertyInfo = t.GetProperty(navProp, Public | NonPublic | Instance);
                }

                t = t.BaseType;
            }
            if (fieldInfo == null && propertyInfo == null)
                throw new ArgumentOutOfRangeException(nameof(propName), $@"Field {navProp} was not found in Type {obj.GetType().FullName}");

            if (fieldInfo != null)
                fieldInfo.SetValue(obj, newValue);
            else
                propertyInfo.SetValue(obj, newValue);
        }

        public static void SetPrivateMemberValue<TNewValue>(
            this object obj, string propName, TNewValue newValue)
            => SetPrivateMemberValue(obj, propName, (object)newValue);

        #endregion

        #region ' GetPrivateStaticMemberValue '

        public static object GetPrivateStaticMemberValue(
            this Type type,
            string memberName)
        {
            var member = type.GetProperty(
                memberName,
                Static | Public | NonPublic);

            return member.GetValue(null);
        }

        #endregion

        #region ' InvokePrivateAction '

        public static void InvokePrivateAction(
            this object candidate,
            string methodName,
            object[] arguments)
        {
            if (candidate == null) throw new NullReferenceException();
            //
            var methodInfo = candidate.GetType().GetMethod(methodName, Instance | NonPublic);
            if (methodInfo == null)
            {
                throw new Exception($"Método não encontrado: { candidate.GetType().FullName}.{  methodName }({ string.Join(", ", arguments?.Select(_ => _.GetType().FullName)) }).");
            }

            methodInfo.Invoke(candidate, arguments);
        }
        #endregion

        #region ' FieldInfo.GetValue<T> '

        public static T GetValue<T>(this FieldInfo fieldInfo, object obj = null)
        {
            if (fieldInfo == null) throw new NullReferenceException();

            // verifica se os tipos são compatíveis
            if (!typeof(T).IsAssignableFrom(fieldInfo.FieldType))
            {
                throw new InvalidCastException($"O valor retornado pelo field `{ fieldInfo.Name }` (`{ fieldInfo.FieldType.FullName }`) é incompatível com o tipo esperado (`{ typeof(T).FullName }`).");
            }

            var result = fieldInfo.GetValue(obj);
            return (T)result;
        }

        #endregion
    }
}
