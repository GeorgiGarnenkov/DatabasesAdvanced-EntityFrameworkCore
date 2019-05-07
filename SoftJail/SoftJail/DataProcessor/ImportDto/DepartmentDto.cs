using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentDto
    {
        [StringLength(25, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public CellDto[] Cells { get; set; }
    }
}