using System.ComponentModel.DataAnnotations;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class UserCoockiesModel
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }        
        public string UserName { get; set; } = null!;
        public long RegistrationId { get; set; }
        public int UserType { get; set; }
        public string? DisplayName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public int beneficiarytype { get; set; }
        public long postid { get; set; }
    }
}
