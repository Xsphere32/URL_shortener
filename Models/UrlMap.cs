using FluentNHibernate.Mapping;

namespace URL_shortener.Models
{
    public class UrlMap : ClassMap<Url>
    {
        public UrlMap()
        {
            Table("Url");
            Id(x => x.ID).GeneratedBy.Identity();
            Map(x => x.FullUrl);
            Map(x => x.ShortUrl);
            Map(x => x.DateOfCreation);
            Map(x => x.PassCount);
            DynamicUpdate();
        }
    }
}
