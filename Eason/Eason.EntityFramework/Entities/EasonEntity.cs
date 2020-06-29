using Eason.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eason.EntityFramework.Entities
{
    public class EasonEntity : Entity<long>
    {

        [Required]
        public override long id { get; set; }
        public long creatorId { get; set; }
        public string creatorName { get; set; }
        public DateTime creationTime { get; set; }
        public byte status { get; set; }
    }
}
