using System.Xml.Serialization;

namespace ProductShop.App.Dto.Export
{
    [XmlType("sold-products")]
    public class SoldProductsExportDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("product")]
        public ProductDto[] ProductDtos { get; set; }
    }
}