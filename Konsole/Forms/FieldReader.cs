using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konsole.Forms
{
    public class FieldReader
    {
        private readonly object _object;
        private readonly Type _type;

        public FieldReader(object @object)
        {
            _object = @object;
            _type = @object.GetType();
        }

        private static readonly Type[] NumericTypes = {
            typeof(byte),
            typeof(int),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(decimal),
            typeof(float),
            typeof(int),
            typeof(long)
        };

        private static readonly Type[] SupportedTypes = new[]
        {
            typeof (string),
            typeof (bool),
            typeof (DateTime)
        }.Concat(NumericTypes).ToArray();

        public FieldList ReadFieldList()
        {
            var fields = readFields();
            int width = fields.Any() ? fields.Max(f => f.Caption.Length) : 10;
            var fieldlist = new FieldList(fields.ToArray(), width);
            return fieldlist;
        }

        public static Type NonGenericType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? t.GetGenericArguments()[0] : t;
        }

        public static bool IsNumericType(Type type)
        {
            Type nonGenericType = NonGenericType(type);
            return NumericTypes.Any(t => nonGenericType.Name == t.Name);
        }


        private IEnumerable<Field> readFields()
        {
            var properties = _type.GetProperties();

            var supportedProps = properties
                .Where(f => SupportedTypes.Contains(NonGenericType(f.PropertyType)));

            var fields = supportedProps
                .Select(f => new Field(
                    ParseFieldType(f.PropertyType),
                    f.Name,
                    ToCaption(f.Name),
                    IsNullable(f.PropertyType),
                    f.GetValue(_object)
                ));
            return fields;
        }

        public static FieldType ParseFieldType(Type type)
        {
            if (IsNumericType(type)) return FieldType.Numeric;
            switch (type.Name)
            {
                case "string": return FieldType.String;
            }
            return FieldType.Unsupported;
        }


        private bool IsNullable(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private string ToCaption(string caption)
        {
            var sb = new StringBuilder();
            char prev = 'A';
            foreach (var c in caption)
            {
                // if previous was lower and next is upper, add space and next, otherwise add char
                if (char.IsLower(prev) && char.IsUpper(c) || char.IsUpper(prev) && char.IsUpper(c))
                {
                    sb.Append(' ');
                }
                sb.Append(c);
                prev = c;
            }
            return sb.ToString();
        }
    }
}

