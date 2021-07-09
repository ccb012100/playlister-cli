using System;
using System.Globalization;

namespace PlaylisterCli.Models.Data
{
    public record Album
    {
        public string AlbumType { get; init; }

        public string Id { get; init; }

        public string Name { get; init; }

        /// <summary>
        /// The date the album was first released, for example “1981-12-15”. Depending on the precision, it might be shown as “1981” or “1981-12”.
        /// </summary>
        public string ReleaseDate { get; init; }

        public int TotalTracks { get; init; }

        public DateTime DateOfRelease
        {
            get
            {
                // Depending on precision, the ReleaseDate string could be in the format "yyyy", "yyyy-MM" or "yyyy-MM-DD"
                if (DateTime.TryParseExact(ReleaseDate, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime yyyy))
                {
                    return yyyy;
                }

                if (DateTime.TryParse(ReleaseDate, out DateTime dt))
                {
                    return dt;
                }

                throw new InvalidOperationException($"Could not parse ReleaseDate value {ReleaseDate}");
            }
        }
    }
}
