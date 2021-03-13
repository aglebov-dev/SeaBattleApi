using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.DataAccess.Postgre.Entities
{
    [Table("ships", Schema = StorageConstants.Schema)]
    public class ShipEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("game_id")]
        public long GameId { get; set; }

        [Required]
        [Column("x_start")]
        public int XStart { get; set; }

        [Required]
        [Column("x_end")]
        public int XEnd { get; set; }

        [Required]
        [Column("y_start")]
        public int YStart { get; set; }

        [Required]
        [Column("y_end")]
        public int YEnd { get; set; }

        [ForeignKey(nameof(GameId))]
        public GameEntity Game { get; set; }

        [InverseProperty(nameof(ShotEntity.Ship))]
        public ICollection<ShotEntity> Shots { get; set; }
    }
}
