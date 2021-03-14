using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.DataAccess.Postgre.Entities
{
    [Table("shots", Schema = StorageConstants.Schema)]
    public class ShotEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("game_id")]
        public long GameId { get; set; }

        [Column("ship_id")]
        public long? ShipId { get; set; }

        [Required]
        [Column("x")]
        public int X { get; set; }

        [Required]
        [Column("y")]
        public int Y { get; set; }

        [ForeignKey(nameof(GameId))]
        public GameEntity Game { get; set; }

        [ForeignKey(nameof(ShipId))]
        public ShipEntity Ship { get; set; }
    }
}
