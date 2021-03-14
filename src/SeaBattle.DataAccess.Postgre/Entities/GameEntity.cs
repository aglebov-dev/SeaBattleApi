using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.DataAccess.Postgre.Entities
{
    [Table("games", Schema = StorageConstants.Schema)]
    public class GameEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("size")]
        public int Size { get; set; }

        [Column("init")]
        public bool Init { get; set; }

        [Column("ended")]
        public bool Ended { get; set; }

        [Column("finished")]
        public bool Finished { get; set; }

        [InverseProperty(nameof(ShotEntity.Game))]
        public ICollection<ShotEntity> Shots { get; set; }

        [InverseProperty(nameof(ShipEntity.Game))]
        public ICollection<ShipEntity> Ships { get; set; }
    }
}
