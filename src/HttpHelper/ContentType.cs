using System.Linq;
using System.Reflection;

namespace HttpHelper
{
    /// <summary>
    /// Content Type
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// application/json
        /// </summary>
        [ContentType(name: Text.ApplicationJson)]
        ApplicationJson = 1,

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        [ContentType(name: Text.FormUrlEncoded)]
        FormUrlEncoded = 2,

        /// <summary>
        /// None - do not add content type header
        /// </summary>
        [ContentType(name: Text.None)]
        None = 0,
    }

    /// <summary>
    /// Extensions for <see cref="ContentType"/>
    /// </summary>
    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Return string representation of <see cref="ContentType"/> - either the name or the custom name
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string ToAttributeString(this ContentType contentType)
        {
            var contentTypes = typeof(ContentType).GetProperties();

            var contentTypeString = contentType.ToString();

            var attribute = contentTypes.FirstOrDefault(c => c.Name == contentTypeString);

            var customAttributeName = attribute.GetCustomAttribute<ContentTypeAttribute>();

            return customAttributeName == null ? contentTypeString : customAttributeName.Name;
        }
    }
}