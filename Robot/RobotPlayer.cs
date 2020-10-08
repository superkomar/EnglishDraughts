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
        public EstimationParameters(PriorityTurn generanTurn, int level, CancellationToken token)
        {
            GeneranTurn = generanTurn;
            Level = level;
            Token = token;
        }

        public EstimationParameters(EstimationParameters other, int level)
        {
            Level = level;

            GeneranTurn = other.GeneranTurn;
            Token = other.Token;
        }

        public PriorityTurn GeneranTurn { get; }

        public int Level { get; }

        public CancellationToken Token { get; }
    }

    public class RobotPlayer : IRobotPlayer
    {
        private PlayerSide _playerSide;

        public int TurnTime { get; set; }

        public IGameTurn GetTunr() => _priorityTurnCollection?.GetBestTurn() ?? default;

        public void Init(PlayerSide side)
        {
            _playerSide = side;
        }

        private PriorityTurnCollection _priorityTurnCollection;

        public Task<IGameTurn> MakeTurnAsync(CoreField gameField, CancellationToken token)
        {
            var robotField = new RobotField(gameField);
            var turns = GetTurns(robotField, _playerSide);

            _priorityTurnCollection = new PriorityTurnCollection(turns);

            var tasks = new List<Task>(turns.Count);

            foreach (var turn in _priorityTurnCollection)
            {
                var parameters = new EstimationParameters(turn, 1, token);
                tasks.Add(Task.Run(() => EstimateTurn(robotField, turn, parameters), token));
            }
            
            return Task.WhenAll(tasks).ContinueWith(_ => GetTunr());
        }

        private static Task EstimateTurn(RobotField field, IGameTurn turn, EstimationParameters parameters)
        {
            // Check the turn is correct
            if (!GameFieldUtils.TryMakeTurn(field.Origin, turn, out CoreField newCoreField))
            {
                parameters.GeneranTurn.ClarifyPriority(double.NegativeInfinity, parameters.Level, turn.Side);
                throw new ArgumentException("Invalid turn");
            }

            var newField = (RobotField)newCoreField;

            parameters.GeneranTurn.ClarifyPriority(
                Metrics.CompareByMetrics(field, newField, turn.Side),
                parameters.Level,
                turn.Side);

            EstimationParameters newParameters;

            var newTurns = !turn.IsSimple
                ? GameFieldUtils.FindTurnsForCell(field, turn.Steps.Last(), TurnType.Jump)
                : new List<GameTurn>();

            if (newTurns.Any()) // Check if it's continuous jump
            {
                newParameters = new EstimationParameters(parameters, parameters.Level);
            }
            else // Find turns for the opposite side
            {
                newParameters = new EstimationParameters(parameters, parameters.Level + 1);
                newTurns = GetTurns(newField, turn.Side.ToOpposite());
            }

            if (parameters.Token.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            var tasks = new List<Task>();
            foreach (var newTurn in newTurns)
            {
                tasks.Add(Task.Run(() => EstimateTurn(newField, newTurn, newParameters), parameters.Token));
            }

            return Task.WhenAll(tasks);
        }

        private static List<GameTurn> GetTurns(RobotField gameField, PlayerSide side)
        {
            var simpleMoves = new List<GameTurn>();
            var requiredJumps = new List<GameTurn>();

            void Processor(int cellIdx)
            {
                var cellTurns = GameFieldUtils.FindTurnsForCell(gameField, cellIdx, TurnType.Both);

                foreach (var turn in cellTurns)
                {
                    if (turn.IsSimple) simpleMoves.Add(turn);
                    else requiredJumps.Add(turn);
                }
            }

            gameField.ProcessorCellBySide(side, Processor);

            return requiredJumps.Any() ? requiredJumps : simpleMoves;
        }
    }
}
