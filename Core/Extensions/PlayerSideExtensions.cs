using System;

using Core.Enums;

namespace Core.Extensions
{
    public static class PlayerSideExtensionsExtensions
    {
        public static PlayerSide ToOpposite(this PlayerSide side) =>
            side switch
            {
                PlayerSide.White => PlayerSide.Black,
                PlayerSide.Black => PlayerSide.White,
                _ => throw new NotImplementedException()
            };
    }
}
