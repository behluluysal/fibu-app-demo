using System.Collections.Generic;

namespace Core.AutoMapperDtos
{
    public class RoleWithClaimDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<ClaimDto> Claims { get; set; }
    }
}
