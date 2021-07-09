using System.Text.Json;

namespace PlaylisterCli.Utilities
{
    public static class JsonUtility
    {
        private static readonly JsonSerializerOptions Options;

        static JsonUtility()
        {
            Options = new JsonSerializerOptions {WriteIndented = true};
        }

        /// <summary>
        /// Serialize object to pretty-printed JSON string.
        /// </summary>
        /// <param name="t">Object you want converted to JSON</param>
        /// <typeparam name="T">Type of <paramref name="t"/></typeparam>
        /// <returns>String representation of the object as pretty-printed JSON</returns>
        public static string PrettyPrint<T>(T t) =>
            JsonSerializer.Serialize(t, Options);
    }
}
