using System.ComponentModel.DataAnnotations;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class AdminMenu
    {
        public AdminMenu()
        {
        }

        public string controllername { get; set; }
        public string actionname { get; set; }
        public string title { get; set; }
        public string menuicon { get; set; }
        public long parentmenuid { get; set; }
        public long menuid { get; set; }
    }
}
