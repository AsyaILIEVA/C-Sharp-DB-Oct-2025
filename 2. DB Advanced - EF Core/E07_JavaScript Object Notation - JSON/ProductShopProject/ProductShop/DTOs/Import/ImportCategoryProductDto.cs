using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace ProductShop.DTOs.Import
{
    public class ImportCategoryProductDto
    {
        [Required]
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [Required]
        [JsonProperty("productId")]
        public int ProductId { get; set; }
    }
}
