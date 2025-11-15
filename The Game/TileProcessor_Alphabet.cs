using System;

public partial class TileProcessor
{
    #region === Upper Case ===
    // Uppercase A-Z
    public static TileObject OnA(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
    new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnB(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnC(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnD(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnE(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnF(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.Forest, false, false, new(), ascii);
        return tile;
    }
    public static TileObject OnG(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnH(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnI(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnJ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnK(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnL(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnM(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnN(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnO(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnP(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnQ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnR(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnS(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnT(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnU(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnV(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private TileObject OnW(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnX(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnY(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnZ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Lower Case ===
    // Lowercase a-z
    public static TileObject Ona(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onb(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onc(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ond(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject One(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onf(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ong(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onh(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Oni(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onj(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onl(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onm(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ono(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onp(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onq(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onr(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ons(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ont(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onu(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onv(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onw(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onx(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Ony(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject Onz(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
}
