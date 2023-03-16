namespace Palette
{
    using System.IO;
    using System.Text.Json;

    public static class Repository
    {
        public static PaletteInfo Read(FileInfo file)
        {
            return JsonSerializer.Deserialize<PaletteInfo>(File.ReadAllText(file.FullName));
        }

        public static void Save(PaletteInfo palette, FileInfo file)
        {
            File.WriteAllText(file.FullName, JsonSerializer.Serialize(palette));
        }
    }
}
