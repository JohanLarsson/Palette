namespace Palette
{
    using System;
    using System.IO;
    using Gu.Persist.NewtonsoftJson;
    using Newtonsoft.Json;

    public class Repository
    {
        private static readonly DirectoryInfo Directory = new DirectoryInfo("./Settings");
        private readonly DataRepository repository;

        public Repository()
        {
            var settings = CreateDefaultSettings();
            this.repository = new DataRepository(settings);
        }

        public PaletteInfo Read(FileInfo file)
        {
            return this.repository.Read<PaletteInfo>(file);
        }

        public bool CanSave(PaletteInfo palette, FileInfo file) => this.repository.IsDirty(file, palette);

        public void Save(PaletteInfo palette, FileInfo file)
        {
            this.repository.Save(file, palette);
        }

        private static DataRepositorySettings CreateDefaultSettings()
        {
            return new DataRepositorySettings(
                directory: Directory.FullName,
                jsonSerializerSettings: new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters =
                    {
                        ColourInfoConverter.Default
                    }
                },
                isTrackingDirty: true,
                saveNullDeletesFile: true,
                backupSettings: null,
                extension: ".palette");
        }

        private class ColourInfoConverter : JsonConverter
        {
            public static readonly ColourInfoConverter Default = new ColourInfoConverter();

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var info = (ColuorInfo)value;
                serializer.Serialize(
                    writer,
                    new ColourDto
                    {
                        Name = info.Name,
                        Colour = info.Hex,
                    });
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var dto = serializer.Deserialize<ColourDto>(reader);
                return new ColuorInfo { Name = dto.Name, Hex = dto.Colour };
            }

            public override bool CanConvert(Type objectType) => objectType == typeof(ColuorInfo);

            internal class ColourDto
            {
                public string Name { get; set; }

                public string Colour { get; set; }
            }
        }
    }
}