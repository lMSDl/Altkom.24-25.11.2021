using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public DateTime Created { get; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get; set; }
    }
}
