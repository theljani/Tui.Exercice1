using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TUI.Flights.Common.Entities.Base
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }
        = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; }
        = DateTime.UtcNow;
    }
}
