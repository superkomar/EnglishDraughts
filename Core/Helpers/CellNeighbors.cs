namespace Core.Helpers
{
    public readonly struct CellNeighbors
    {
        public CellNeighbors(int leftTop, int leftBot, int rightTop, int rightBot)
        {
            LeftTop = leftTop;
            LeftBot = leftBot;
            RightTop = rightTop;
            RightBot = rightBot;
        }

        public int LeftBot { get; }

        public int LeftTop { get; }

        public int RightBot { get; }

        public int RightTop { get; }

        public bool IsNeighbor(int cellId) =>
            cellId != -1 && (LeftBot == cellId || LeftTop == cellId || RightBot == cellId || RightTop == cellId);

        public int[] ToArray() => new int[] { LeftTop, RightTop, LeftBot, RightBot };
    }
}
