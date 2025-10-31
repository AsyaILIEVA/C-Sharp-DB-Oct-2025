namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

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

            result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
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
            StringBuilder sb = new StringBuilder();

            var songs = context
                .Songs                
                .Select(s => new
                {
                    s.Name,
                    PerformersNames = s.SongPerformers
                    .Select(sp => new
                    {
                        sp.Performer.FirstName,
                        sp.Performer.LastName,
                    })
                    .OrderBy(sp => sp.FirstName)
                    .ThenBy(sp => sp.LastName)
                    .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album != null?
                    (s.Album.Producer != null? s.Album.Producer.Name : null) : null,
                    s.Duration,
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .ToArray();

            int songNumber = 1;
            foreach (var song in songs)
            {
                sb
                    .AppendLine($"-Song #{songNumber++}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}");
                foreach (var performer in song.PerformersNames)
                {
                    sb
                        .AppendLine($"---Performer: {performer.FirstName} {performer.LastName}");
                }

                sb
                    .AppendLine($"---AlbumProducer: {song.AlbumProducerName}")
                    .AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
