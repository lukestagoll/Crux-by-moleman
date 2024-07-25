public abstract class SkillBase
{
    public int Level { get; private set; }

    public SkillBase(int level)
    {
        Level = level;
    }

    public abstract void Activate();
    public abstract void Deactivate();
}