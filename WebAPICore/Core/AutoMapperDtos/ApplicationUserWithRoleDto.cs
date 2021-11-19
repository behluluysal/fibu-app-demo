using System.Collections.Generic;


namespace Core.AutoMapperDtos
{
    public class ApplicationUserWithRoleDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IList<RoleDto> Roles { get; set; }
    }
}
