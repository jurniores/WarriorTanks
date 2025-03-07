using System;
using MemoryPack;

[MemoryPackable]
public partial class EntityList
{
    public int peerId;
    public int identityId;
    public int team;
    public string nameTank;
}

[Serializable]
public struct Levels
{
    public int hpTotal, dano, bulletTotal, bulletPent;
    public float cowntDown;
}
public struct JoysTick
{
    public bool w, a, s, d;

    public JoysTick(bool a = false, bool s = false, bool w = false, bool d = false) : this()
    {
        this.a = a;
        this.s = s;
        this.d = d;
        this.w = w;
    }
}