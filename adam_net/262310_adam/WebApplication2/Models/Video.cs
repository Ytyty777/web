using System;
using WebApplication2.Models; // żeby mógł widzieć klasę Profile
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Video
    { //[Required] dodane jako walidacja danych 
        [Required] public int Id { get; set; }
        [Required] public string vID { get; set; } = string.Empty;
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string cID { get; set; } = string.Empty;
        [Required] public DateTime data_m { get; set; }
        [Required] public bool favo { get; set; }
    }
}
