using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Robot.Extensions;
using Robot.Interfaces;
using Robot.Models;

namespace Robot
{
    using CoreField = Core.Models.GameField;
    using RobotField = Robot.Models.FieldWrapper;

    internal readonly struct EstimationParameters
    {
        public EstimationParameters(PriorityTurn generanTurn, int depth, CancellationToken token)
        {
            GeneralTurn = generanTurn;
            Depth = depth;
            Token = token;
        }

        public EstimationParameters(EstimationParameters other, int level)
        {
            Depth = level;

            GeneralTurn = other.GeneralTurn;
            Token = other.Token;
        }

        public PriorityTurn GeneralTurn { get; }

        public int Depth { get; }

        public CancellationToken Token { get; }
    }

    public class RobotPlayer : IRobotPlayer
    {
        private PlayerSide _playerSide;
        private List<PriorityTurn> _priorityTurns;
        
        public int TurnTime { get; set; }

        public IGameTurn GetTunr()
        {
            if (_priorityTurns == null || !_priorityTurns.Any())
            {
                return default;
            }

            var result = _priorityTurns.First();

            foreach (var turn in _priorityTurns.Skip(1))
            {
                if (result.Priority < turn.Priority)
                {
                    result = turn;
                }
            }

            Console.WriteLine(string.Format("== GetTurn == {0}", result.ToString()));

            return result;
        }

        public void Init(PlayerSide side)
        {
            _playerSide = side;
        }

        public async Task<IGameTurn> MakeTurnAsync(CoreField gameField, CancellationToken token)
        {
            var robotField = new RobotField(gameField);
            var turns = robotField.GetTurnsBySide(_playerSide);

            _priorityTurns = new List<PriorityTurn>(turns.Select(x => new PriorityTurn(x)));

            var tasks = new List<Task>();

            foreach (var turn in _priorityTurns)
            {
                var localTurn = turn;
                var parameters = new EstimationParameters(localTurn, 1, token);
                tasks.Add(Task.Run(() => EstimateTurn(robotField, turn, parameters), token));
                //tasks.Add(EstimateTurn(robotField, localTurn, parameters));
            }

            await Task.WhenAll(tasks);
                
            return GetTunr();
        }

        private static Task EstimateTurn(RobotField field, IGameTurn turn, EstimationParameters parameters)
        {
            Console.WriteLine(string.Format(
                "turn: {0} depth: {1}",
                parameters.GeneralTurn.ToString(),
                parameters.Depth));

            // Check the turn is correct
            if (!GameFieldUtils.TryMakeTurn(field.Origin, turn, out CoreField newCoreField))
            {
                parameters.GeneralTurn.ClarifyPriority(double.NegativeInfinity, parameters.Depth, false);
                throw new ArgumentException($"Invalid turn: {turn}");
            }

            var newField = (RobotField) newCoreField;

            parameters.GeneralTurn.ClarifyPriority(
                MetricsProcessor.CompareByMetrics(field, newField, turn.Side),
                parameters.Depth,
                parameters.GeneralTurn.Side == turn.Side);

            var newParameters = new EstimationParameters(parameters, parameters.Depth + 1);
            var newTurns = newField.GetTurnsBySide(turn.Side.ToOpposite());

            if (parameters.Token.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            var tasks = new List<Task>();
            foreach (var newTurn in newTurns)
            {
                //tasks.Add(Task.Run(() => EstimateTurn(newField, newTurn, newParameters), parameters.Token));
                tasks.Add(EstimateTurn(newField, newTurn, newParameters));
            }

            return Task.WhenAll(tasks);
        }
    }
}
