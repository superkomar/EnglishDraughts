using System;

using Core.Interfaces;

namespace Wpf.CustomTypes
{
    public class LaunchStrategies
    {
        public enum PlayerType
        {
            Human,
            Robot
        }

        public static IPlayerLauncher GetLauncher(IGamePlayer player, PlayerType type) =>
            type switch
            {
                PlayerType.Robot => new RobotPlayer(player),
                PlayerType.Human => new HumanPlayer(player),
                _ => throw new NotImplementedException()
            };
    }
}
