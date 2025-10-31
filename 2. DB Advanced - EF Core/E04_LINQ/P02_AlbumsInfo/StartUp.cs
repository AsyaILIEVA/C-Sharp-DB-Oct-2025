namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportAlbumsInfo(context, 9);
            Console.WriteLine( result);
        }

        //P02
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var producerAlbums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer != null ?
                        a.Producer.Name : string.Empty,
                    AlbumSongs = a.Songs
                        .Select(s => new
                        {
                            s.Name,
                            s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.Name)
                        .ThenBy(s => s.WriterName)
                        .ToArray(),
                    TotalPrice = a.Price,
                })
                .ToArray()
                .OrderByDescending(a => a.TotalPrice)
                .ToArray();

            foreach (var album in producerAlbums)
            {
                sb
                    .AppendLine($"-AlbumName: {album.Name}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine($"-Songs:");

                int songNumber = 1;
                foreach (var song in album.AlbumSongs)
                {
                    sb
                        .AppendLine($"---#{songNumber++}")
                        .AppendLine($"---SongName: {song.Name}")
                        .AppendLine($"---Price: {song.Price.ToString("f2")}")
                        .AppendLine($"---Writer: {song.WriterName}");
                }
                sb.AppendLine($"-AlbumPrice: {album.TotalPrice.ToString("f2")}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
