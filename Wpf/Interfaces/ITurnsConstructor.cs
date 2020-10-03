using Core.Enums;

using Wpf.ViewModels.CustomTypes;

namespace Wpf.Interfaces
{
    internal interface ITurnsConstructor
    {
        bool IsJumpsContinue { get; }

        PlayerSide Side { get; }

        bool CheckTurnStartCell(int cellIdx);

        void Clear();

        TurnsConstructor.Result TryMakeTurn(int start, int end);
    }
}
