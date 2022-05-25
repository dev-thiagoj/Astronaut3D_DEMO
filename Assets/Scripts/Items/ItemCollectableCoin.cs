using Itens;

public class ItemCollectableCoin : ItemCollectableBase
{
    protected override void OnCollect()
    {
        base.OnCollect();

        if (!ItemManager.Instance) return;
    }
}
