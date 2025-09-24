using SadRogue.Primitives.GridViews;


namespace LuckNGold.Tests;

class GridViewTests : SimpleSurface
{
    const int X = 10;
    const int Y = 10;
    readonly ArrayView<Tile> _tiles = new(X, Y);

    public GridViewTests()
    {
        var area = _tiles.Bounds();
        var floor = area.Expand(-1, -1);
        foreach (var point in floor.Positions())
            _tiles[point] = new Tile(true, false);

        //var view = new LambdaGridView<bool>(X, Y, CheckWalkable);
        //var view = new LambdaTranslationGridView<Tile, bool>(_tiles, CheckWalkable);
        var view = new TerrainWalkabilityView(_tiles);

        DisplayGrid(view);
    }

    void DisplayGrid(IGridView<bool> gridView)
    {
        var text = gridView.ExtendToString(elementStringifier: Stringifier);
        string[] result = text.Split('\n');
        for (int i = 0; i < result.Length; i++)
            Surface.Print(2, i + 2, result[i]);
    }
    record struct Tile(bool IsWalkable, bool IsTransparent);

    class TerrainWalkabilityView(IGridView<Tile> baseGrid) : 
        TranslationGridView<Tile, bool>(baseGrid)
    {
        protected override bool TranslateGet(Tile tile) => tile.IsWalkable;
    }

    string Stringifier(bool b) => b ? "." : "#";

    bool CheckWalkable(Point p) => _tiles[p].IsWalkable;

    bool CheckWalkable(Tile t) => t.IsWalkable;

}