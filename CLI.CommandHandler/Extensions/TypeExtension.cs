using System;
using System.Linq;

namespace CLI.CommandHandler.Extensions
{
    internal static class TypeExtension
    {
        /// <summary>
        /// Returns true if the current type is assignable from `type` given a generic type
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsGenericallyAssignableTo(this Type currentType, Type type, Type genericType)
        {
            return IsGenericallyAssignableTo(currentType, type, new[] {genericType});
        }

        /// <summary>
        /// Returns true if the current type is assignable from `type` given a set of generic types 
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="type"></param>
        /// <param name="genericTypes"></param>
        /// <returns></returns>
        public static bool IsGenericallyAssignableTo(this Type currentType, Type type, Type[] genericTypes)
        {
            return currentType.FindInterfaces((t, _) =>
                FilterByGenerics(t, type, genericTypes), null).Any();
        }

        private static bool FilterByGenerics(Type type, Type interfaceType, Type[] genericTypes)
        {
            var genericParametersForCurrentType = type.GenericTypeArguments;
            if (genericParametersForCurrentType.Length != genericTypes.Length)
                return false;

            interfaceType = interfaceType.MakeGenericType(genericTypes);

            return type == interfaceType;
        }
    }
}