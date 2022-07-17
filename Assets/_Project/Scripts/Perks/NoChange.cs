
public class NoChange : Perk
{

    public override void RunPerk()
    {
        usedPerk = true;
    }

    public override void ResetPerks()
    {
        usedPerk = false;
    }
    
    
}
