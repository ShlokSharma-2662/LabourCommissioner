using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ApproveActionModel
    {

        [Required(ErrorMessage = "To Role Field is Required")]
        int ToRole { get; set; }
        string Remarks { get; set; }

    }
}
