using Newtonsoft.Json;
using System.IO;

namespace Huevy.Lib.IO
{
    public abstract class AssemblyConfigRepository<T> where T : class, new()
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            //DefaultValueHandling = DefaultValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new InternalResolver()
        };

        public static T Load()
        {
            var path = GetPath();
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(text))
                    return JsonConvert.DeserializeObject<T>(text, Settings);
            }
            return new T();
        }

        public void Save()
        {
            var serialized = JsonConvert.SerializeObject(this, Settings);

            var path = GetPath();
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, serialized);
        }

        private static string GetPath()
        {
            var fullPath = typeof(T).Assembly.Location;
            return Path.ChangeExtension(fullPath, "jconfig");
        }
    }
}
