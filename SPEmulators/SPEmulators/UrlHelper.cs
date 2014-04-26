namespace SPEmulators
{
    using System;

    internal static class UrlHelper
    {
        public static string Construct(string parentWebUrl, string webName)
        {
            if (webName != string.Empty)
            {
                var baseUri = new Uri(parentWebUrl.TrimEnd('/') + '/');
                return new Uri(baseUri, webName).AbsoluteUri;
            }
            else
            {
                return parentWebUrl;
            }
        }
        public static string ConstructRelative(string parentRelative, string webName)
        {
            return parentRelative.TrimEnd('/') + '/' + webName;
        }
    }
}
