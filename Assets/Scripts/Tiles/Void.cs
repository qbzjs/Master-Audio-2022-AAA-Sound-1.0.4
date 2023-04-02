using UnityEngine;

public class Void : Wasteland
{
    private static int voidsDestroyed;
    
    public Void(Transform parentTransform, Vector3 pos) : base(parentTransform, pos) { }

    public override Tag[] GetTags()
    {
        return new[] {Tag.Darkness};
    }
    
    public override string GetDescription()
    {
        return $"Score is the number of <b>Voids</b> that have been <b>Destroyed</b>";
    }

    public override void WhenAnyDestroyed(int x, int y, ITile aboutToBeDestroyed)
    {
        if (this != aboutToBeDestroyed)
        {
            //If we're not referencing this object, don't do anything
            return;
        }
        voidsDestroyed++;
        TweenManager.Instance.Callout($"Voids now worth {voidsDestroyed}!", Position());
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(voidsDestroyed);
    }
}