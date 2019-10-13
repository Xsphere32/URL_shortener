using System;
using System.ComponentModel.DataAnnotations;

namespace URL_shortener.Models
{
    public class Url
    {
        [Display(Name="ID")]
        public virtual int ID { get; set; }
        [Display(Name="Длинный URL")]
        public virtual string FullUrl { get; set; }
        [Display(Name="Сокращенный URL")]
        public virtual string ShortUrl { get; set; }
        [Display(Name="Дата создания")]
        public virtual DateTime DateOfCreation { get; set; }
        [Display(Name="Количество переходов")]
        public virtual int PassCount { get; set; }
    }
}
