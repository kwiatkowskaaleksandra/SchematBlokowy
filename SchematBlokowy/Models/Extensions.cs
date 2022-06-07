
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SchematBlokowy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Routing;

namespace SchematBlokowy.Utils
{
    public static class Extensions
    {
        public static string ToJson(this object ob, string dateTimeFormat = null)
        {
            if (ob == null)
                return null;
            if (dateTimeFormat == null)
            {
                dateTimeFormat = DateTimeFormats.DateFormat;
            }
            var isoConvert = new IsoDateTimeConverter();
            isoConvert.DateTimeFormat = dateTimeFormat;
            string result = JsonConvert.SerializeObject(ob, isoConvert);
            return result;
        }
        public static bool EqualsOr(this object baseObject, params object[] objects)
        {
            if (baseObject == null)
                throw new NullReferenceException("Base object cannot be null");
            if (objects == null)
                return false;
            foreach (var ob in objects)
            {
                if (baseObject.Equals(ob))
                    return true;
            }
            return false;
        }
        public static bool IsNullOrEmpty(this string st)
        {
            return string.IsNullOrEmpty(st);
        }

        public static string ToStringSafe(this string st)
        {
            if (string.IsNullOrEmpty(st))
                return string.Empty;
            return st;
        }

        public static string ToStringSafe(this object st)
        {
            if (st == null)
                return string.Empty;
            if (string.IsNullOrEmpty(st.ToString()))
                return string.Empty;
            return st.ToString();
        }
        private static string PropertyName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public static byte[] ToByteArray(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string RemoveAdditionalCharsFromBankAccountNumber(this string number)
        {
            if (!string.IsNullOrEmpty(number))
            {
                number = string.Join("", number.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); //Usuwanie bia³ych znaków
                number = string.Join("", number.Split('-'));    //usuwanie kresek
            }
            return number;
        }

        public static string ReplaceDiacritics(this string source)
        {
            string sourceInFormD = source.Normalize(NormalizationForm.FormD);

            var outputSB = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    outputSB.Append(c);
            }
            string output = outputSB.ToString().Normalize(NormalizationForm.FormC);

            output = Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(output));

            return (output);
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
    public static class DateTimeHelpers
    {
        public static DateTime TodayUtc
        {
            get
            {
                return DateTime.Now.Date;
            }
        }

        public static DateTime GetThitryDaysAgo()
        {
            return DateTime.Today.AddDays(-30);
        }
    }
    public static class EnumHelpers
    {
        public static string GetDisplayName(this Enum value)
        {

            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                var attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                    typeof(DisplayAttribute), false);

                if (attributes.Length > 0)
                    return attributes[0].GetName();
                else
                    return value.ToString();
            }
            return null;
        }
        public static List<SelectModelBinder<int>> GetSelectBinderList<T>() where T : struct
        {
            List<EnumModelBinder> enumModelBinderList = GetEnumBinderList<T>();
            return enumModelBinderList.Select(x => new SelectModelBinder<int>
            {
                Value = x.Value,
                Text = x.Text
            }).ToList();
        }
        public static List<EnumModelBinder> GetEnumBinderList<T>() where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            IEnumerable<T> list = Enum.GetValues(enumType).Cast<T>();
            List<EnumModelBinder> result = GetEnumBinderList<T>(list.ToArray());
            return result.OrderBy(x => x.Text).ToList();
        }

        public static string GetEnumBinderListJson<T>() where T : struct
        {
            List<EnumModelBinder> enumList = GetEnumBinderList<T>();
            string result = JsonConvert.SerializeObject(enumList);
            return result;
        }
        public static string GetEnumBinderListJson<T>(params T[] enums) where T : struct
        {
            List<EnumModelBinder> enumList = GetEnumBinderList<T>(enums);
            string result = JsonConvert.SerializeObject(enumList);
            return result;
        }

        public static List<EnumModelBinder> GetEnumBinderList<T>(params T[] enums) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            List<EnumModelBinder> result = new List<EnumModelBinder>();
            if (enums == null)
                return result;
            foreach (T item in enums)
            {
                EnumModelBinder modelItem = new EnumModelBinder();
                modelItem.Value = Convert.ToInt32(item);
                modelItem.Text = (item as Enum).GetDisplayName();
                result.Add(modelItem);
            }
            return result.ToList();
        }

        public static EnumModelBinder GetEnumModelBinder(this Enum value)
        {
            EnumModelBinder modelItem = new EnumModelBinder();
            modelItem.Value = Convert.ToInt32(value);
            modelItem.Text = value.GetDisplayName();
            return modelItem;
        }

        public static List<T> GetEnumList<T>() where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            return Enum.GetValues(enumType).Cast<T>().ToList();
        }

        public static T GetEnum<T>(int integer) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            return (T)Enum.ToObject(typeof(T), integer);
        }

        public static DateTime TrimMinutes(this DateTime date)
        {
            return new DateTime(
                date.Year,
                date.Month,
                date.Day,
                date.Hour,
                0,
                0);
        }
    }
}
