using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    [Serializable]
    public abstract record EmployeeCreationDto {

        [Required(ErrorMessage = "Name is a required field")]
        [MaxLength(30, ErrorMessage = "Length of name cannot be greater than 30")]
        public string Name { get; init; }
        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; init; }
        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string Position { get; init; }
    };
}
