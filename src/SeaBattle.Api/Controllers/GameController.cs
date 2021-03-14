using Microsoft.AspNetCore.Mvc;
using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Application.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SeaBattle.Api.Controllers
{
    /// <summary>
    /// Provides methods for management game.
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class GameController : ControllerBase
    {
        private readonly ISeeBattleGameService _seeBattleGameService;
        private readonly IApplicationMapper _applicationMapper;
        private readonly IContractMapper _contractMapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GameController"/> class.
        /// </summary>
        public GameController(ISeeBattleGameService seeBattleGameService, IApplicationMapper applicationMapper, IContractMapper contractMapper)
        {
            //TODO Transfer mappers to ready-made libraries like AatoMapper
            _seeBattleGameService = seeBattleGameService.NotNull(nameof(seeBattleGameService));
            _applicationMapper = applicationMapper.NotNull(nameof(applicationMapper));
            _contractMapper = contractMapper.NotNull(nameof(contractMapper));
        }

        /// <summary>
        /// Creates board for new game.
        /// </summary>
        /// <response code="200">Play board created.</response>
        /// <response code="400">The request data is not correct.</response>
        [HttpPost("/create-matrix")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateMatrix(MatrixCreationRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));
            GameCreationModel model = _applicationMapper.Map(request);
            await _seeBattleGameService.CreateGame(model);

            return Ok();
        }

        /// <summary>
        /// Places the player's ships on the board.
        /// </summary>
        /// <response code="200">Ships are placed successfully.</response>
        /// <response code="400">The request data is not correct.</response>
        [HttpPost("/ship")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Ship([FromBody] ShipRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));
            ShipsCreationModel model = _applicationMapper.Map(request);
            await _seeBattleGameService.AddShips(model);

            return Ok();
        }

        /// <summary>
        /// Fires a shot at the coordinates specified in the request.
        /// </summary>
        /// <response code="200">The shot was executed successfully.</response>
        /// <response code="400">The request data is not correct.</response>
        [HttpPost("/shot")]
        [ProducesResponseType(typeof(ShotResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Shot([FromBody] ShotRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));
            ShotModel model = _applicationMapper.Map(request);
            ShotResultModel result = await _seeBattleGameService.Shot(model);
            ShotResponse response = _contractMapper.Map(result);

            return new JsonResult(response);
        }

        /// <summary>
        /// Ends the current game and clears the data.
        /// </summary>
        /// <response code="200">Game completed successfully.</response>
        [HttpPost("/clear")]
        public async Task<IActionResult> Clear()
        {
            await _seeBattleGameService.FinishGame();

            return Ok();
        }

        /// <summary>
        /// Returns statistics for the current game.
        /// </summary>
        /// <response code="200">Statistics available.</response>
        /// <response code="400">No statistics available.</response>
        [HttpGet("/state")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetState()
        {
            GameStatsModel stats = await _seeBattleGameService.GetGameStats();
            StatsResponse response = _contractMapper.Map(stats);

            return new JsonResult(response);
        }
    }
}
