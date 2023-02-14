using UnityEngine;

public class Void : Wasteland
{
    private static int voidsDestroyed;
    
    public Void(Transform parentTransform, Vector3 pos) : base(parentTransform, pos) { }

    public override string GetDescription()
    {
        return $"Destroyed - All voids are worth +1, forever (currently {voidsDestroyed})";
    }

    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (this != aboutToBeDestroyed)
        {
            //If we're not referencing this object, don't do anything
            return;
        }
        
        voidsDestroyed++;
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(voidsDestroyed);
    }
}