using System;

namespace HttpHelper
{
    internal class ContentTypeAttribute : Attribute
    {
        internal string Name { get; }

        internal ContentTypeAttribute(string name)
        {
            this.Name = name;
        }
    }
}