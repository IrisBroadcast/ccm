using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CCM.Tests.Helpers
{
    public class TestdataGenerator
    {
        private static Random rnd = new Random();

        /// <summary>
        /// Skapar ett objekt av typ T och sätter dess enkla datatyper till slumpade värden
        /// </summary>
        public static T CreateRandomObject<T>()
        {
            return SetRandomPropertyValues(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Uppdaterar värden på egenskaper till slumpade värden.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="excludedProperties">Dessa egenskaper uppdateras inte</param>
        /// <returns></returns>
        public static T SetRandomPropertyValues<T>(T o, params Expression<Func<T, object>>[] excludedProperties)
        {
            string[] propNames = excludedProperties.Select(GetPropertyName).ToArray();
            return SetRandomPropertyValues(o, propNames);
        }

        /// <summary>
        /// Sätter enkla datatyper till slumpade värden på objektet
        /// </summary>
        private static T SetRandomPropertyValues<T>(T o, string[] exceptions = null)
        {
            List<PropertyInfo> propertyInfos = o.GetType().GetProperties().ToList();
            if (exceptions != null)
            {
                propertyInfos = propertyInfos.Where(x => !exceptions.Contains(x.Name)).ToList();
            }
            foreach (var propertyInfo in propertyInfos)
            {
                object value = null;
                switch (propertyInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        value = string.Format("{0}_{1}", propertyInfo.Name, Guid.NewGuid().ToString().Substring(0, 4));
                        break;
                    case "int32":
                    case "long":
                        value = rnd.Next(1, 10000);
                        break;
                    case "bool":
                        value = Convert.ToBoolean(rnd.Next(0, 1));
                        break;
                    case "datetime":
                        var dt = DateTime.Now.Subtract(new TimeSpan(rnd.Next(400), rnd.Next(24), rnd.Next(60), rnd.Next(60)));
                        value = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second); // Milliseconds always 0
                        break;
                }

                if (value != null)
                {
                    propertyInfo.SetValue(o, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }

            return o;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> prop)
        {
            MemberExpression expr;

            if (prop.Body is MemberExpression)
                // .Net interpreted this code trivially like t => t.Id
                expr = (MemberExpression)prop.Body;
            else
                // .Net wrapped this code in Convert to reduce errors, meaning it's t => Convert(t.Id) - get at the
                // t.Id inside
                expr = (MemberExpression)((UnaryExpression)prop.Body).Operand;

            string name = expr.Member.Name;

            return name;
        }


    }
}
