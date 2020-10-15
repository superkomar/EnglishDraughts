using Core.Enums;

using Wpf.ViewModels.CustomTypes;

namespace Wpf.Interfaces
{
    internal interface ITurnsConstructor
    {
        bool DoJumpsContinue { get; }

        PlayerSide Side { get; }

        bool CheckTurnStart(int cellIdx);

        void Clear();

        TurnsConstructor.Result TryMakeTurn(int start, int end);
    }
}
