using System.Xml.Serialization;

namespace ProductShop.App.Dto.Export
{
    [XmlRoot("users")]
    public class UsersExportDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("user")]
        public UserDto[] Users { get; set; }
    }

}