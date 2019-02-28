using System.Xml.Serialization;

namespace DZzzz.Yandex.Music.Api.Model
{
    [XmlRoot(ElementName = "download-info")]
    public class YandexDownloadInfo
    {
        [XmlElement(ElementName = "host")]
        public string Host { get; set; }

        [XmlElement(ElementName = "path")]
        public string Path { get; set; }

        [XmlElement(ElementName = "s")]
        public string S { get; set; }

        [XmlElement(ElementName = "ts")]
        public string Ts { get; set; }
    }
}