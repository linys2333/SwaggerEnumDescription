using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using SwaggerEnum.Extensions;

namespace SwaggerEnum
{
    public class SwaggerEnumDescription : IDocumentFilter
    {
        private readonly bool m_IsAsString;
        private readonly bool m_IsCamelString;

        public SwaggerEnumDescription(bool isAsString = false, bool isCamelString = true)
        {
            m_IsAsString = isAsString;
            m_IsCamelString = isAsString && isCamelString;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var definition in swaggerDoc.Definitions)
            {
                DescribeSchema(definition.Value);

                if (definition.Value.Properties?.Any() ?? false)
                {
                    foreach (var property in definition.Value.Properties)
                    {
                        DescribeSchema(property.Value);
                    }
                }
            }

            foreach (var path in swaggerDoc.Paths)
            {
                var items = new[] { path.Value.Get, path.Value.Post, path.Value.Put, path.Value.Delete, path.Value.Patch };
                foreach (var parameter in items.Where(i => i != null).SelectMany(i => i.Parameters))
                {
                    switch (parameter)
                    {
                        case NonBodyParameter _parameter:
                            DescribeSchema(_parameter);
                            break;
                        case BodyParameter _parameter:
                            DescribeSchema(_parameter.Schema);
                            break;
                    }
                }
            }
        }

        private void DescribeSchema(dynamic schema)
        {
            if (!(schema is Schema || schema is NonBodyParameter))
            {
                return;
            }

            IEnumerable<object> enums = schema.Enum ?? schema.Items?.Enum;
            if (enums == null || enums.GetType().GetElementType() != typeof(object))
            {
                return;
            }

            var enumDict = DescribeEnums(enums);
            if (!enumDict.Any())
            {
                return;
            }

            schema.Description += Environment.NewLine + string.Join("，", enumDict.Select(d => $"{d.Key}-{d.Value}"));

            if (m_IsAsString)
            {
                var newEnums = enumDict.Select(d => (object)(m_IsCamelString ? d.Key.ToCamelString() : d.Key)).ToList();
                switch (schema.Type)
                {
                    case "integer":
                        schema.Format = null;
                        schema.Type = "string";
                        schema.Enum = newEnums;
                        break;
                    case "array":
                        schema.Items.Format = null;
                        schema.Items.Type = "string";
                        schema.Items.Enum = newEnums;
                        break;
                }
            }
        }

        private IEnumerable<KeyValuePair<string, string>> DescribeEnums(IEnumerable<object> enums)
        {
            if (!enums?.Any() ?? true)
            {
                yield break;
            }

            foreach (var enumItem in enums)
            {
                if (enumItem is Enum _enumItem)
                {
                    var key = m_IsAsString
                        ? m_IsCamelString ? _enumItem.ToString().ToCamelString() : _enumItem.ToString()
                        : Convert.ToInt32(_enumItem).ToString();
                    var value = _enumItem.GetDisplayName();

                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }
    }
}