using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Models;
using Core.Utils;

using NLog;

using Robot.Extensions;
using Robot.Interfaces;
using Robot.Properties;
using Robot.Utils;

namespace Robot.Models
{
    internal class RobotPlayer : IRobotPlayer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private List<PriorityTurn> _priorityTurns;
        
        public int TurnTime { get; set; }

        public GameTurn GetTunr()
        {
            if (_priorityTurns == null || !_priorityTurns.Any())
            {
                return default;
            }

            var result = _priorityTurns.First();

            foreach (var turn in _priorityTurns)
            {
                if (result.Priority < turn.Priority)
                {
                    result = turn;
                }

                Logger.Debug("Priority turn: {0}", turn.ToString());
            }

            Logger.Debug("Final Result: {0}", result.ToString());

            return result.Origin;
        }

        public async Task<GameTurn> MakeTurnAsync(GameField gameField, PlayerSide side, CancellationToken token)
        {
            var robotField = new CachedField(gameField);
            var turns = robotField.GetTurnsBySide(side);

            _priorityTurns = turns.Select(x => new PriorityTurn(x)).ToList();

            if (_priorityTurns.Count == 1)
            {
                await Task.Delay(Settings.Default.MinAwaitTimeMs);
                return _priorityTurns.First();
            }

            var tasks = new List<Task>();

            foreach (var turn in _priorityTurns)
            {
                tasks.Add(Task.Run(() =>
                    EstimateTurn(robotField, turn, new EstimationParameters(turn, 1, token))));
            }

            await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
                
            return GetTunr();
        }

        private Task EstimateTurn(CachedField oldField, GameTurn turn, EstimationParameters parameters)
        {
            if (parameters.Token.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            // Check the turn is correct
            if (!GameFieldUtils.TryCreateField(oldField.Origin, turn, out GameField newCoreField))
            { 
                Logger.Error("Invalid turn: {0}", turn);
                parameters.TargetTurn.ClarifyPriority(double.NegativeInfinity);
                throw new ArgumentException($"Invalid turn: {turn}");
            }

            var newField = new CachedField(newCoreField);

            var isOppositeTurn = parameters.TargetTurn.Side != turn.Side;

            var newTurns = newField.GetTurnsBySide(turn.Side.ToOpposite());

            var isGameEnded = false;
            var curTurnPriority = 0.0;

            if (newTurns.Any())
            {
                curTurnPriority =
                    MetricsProcessor.CompareWithMetrics(oldField, newField, turn.Side)
                    * (isOppositeTurn ? -1 : 1)
                    * (1 / (double) parameters.Depth);
            }
            else
            {
                isGameEnded = true;
                curTurnPriority = (1 / parameters.Depth) *
                    (GameRules.HasPlayerWon(newField, parameters.TargetTurn.Side)
                    ? Settings.Default.VictoryCost
                    : Settings.Default.LosingCost);
            }

            parameters.TargetTurn.ClarifyPriority(curTurnPriority);

            if (parameters.Token.IsCancellationRequested || isGameEnded)
            {
                return Task.CompletedTask;
            }

            var tasks = new List<Task>();
            var newParameters = new EstimationParameters(parameters, parameters.Depth + 1);

            foreach (var newTurn in newTurns) 
            {
                tasks.Add(Task.Run(() => EstimateTurn(newField, newTurn, newParameters)));
            }

            return Task.WhenAll(tasks);
        }
    }
}
