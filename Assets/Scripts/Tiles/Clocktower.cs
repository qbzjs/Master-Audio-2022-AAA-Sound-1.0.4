using UnityEngine;

public class Clocktower : Wasteland
{
    public Clocktower(Transform parentTransform, Vector3 pos) : base(parentTransform, pos) { }

    public override void Observe(DefaultEvent e)
    {
        if (xPos != e.xPos || yPos != e.yPos)
        {
            //If we're not referencing this object, don't do anything
            return;
        }
        
        switch (e)
        {
            case GraveyardEvent: //being removed
                GameManager.Instance.Turns--;
                break;
            case PlacedEvent: //being placed
                GameManager.Instance.Turns++;
                break;
        }
    }

    protected override Score CalculateBaseScore()
    {
        return new Score(0);
    }
}