using System.Collections.Generic;

namespace Core.AutoMapperDtos
{
    public class ProductWithTagDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IList<TagDto> Tags { get; set; }
    }
}
